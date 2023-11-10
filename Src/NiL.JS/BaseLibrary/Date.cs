// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.Date
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace NiL.JS.BaseLibrary
{
  public sealed class Date
  {
    private const long _timeAccuracy = 10000;
    private const long _unixTimeBase = 62135596800000;
    private const long _minuteMillisecond = 60000;
    private const long _hourMilliseconds = 3600000;
    private const long _dayMilliseconds = 86400000;
    private const long _weekMilliseconds = 604800000;
    private const long _400yearsMilliseconds = 12622780800000;
    private const long _100yearsMilliseconds = 3155673600000;
    private const long _4yearsMilliseconds = 126230400000;
    private const long _yearMilliseconds = 31536000000;
    private static readonly long[][] timeToMonthLengths = new long[13][]
    {
      new long[2],
      new long[2]{ 2678400000L, 2678400000L },
      new long[2]{ 5097600000L, 5184000000L },
      new long[2]{ 7776000000L, 7862400000L },
      new long[2]{ 10368000000L, 10454400000L },
      new long[2]{ 13046400000L, 13132800000L },
      new long[2]{ 15638400000L, 15724800000L },
      new long[2]{ 18316800000L, 18403200000L },
      new long[2]{ 20995200000L, 21081600000L },
      new long[2]{ 23587200000L, 23673600000L },
      new long[2]{ 26265600000L, 26352000000L },
      new long[2]{ 28857600000L, 28944000000L },
      new long[2]{ 31536000000L, 31622400000L }
    };
    private static readonly string[] daysOfWeek = new string[7]
    {
      "Mon",
      "Tue",
      "Wed",
      "Thu",
      "Fri",
      "Sat",
      "Sun"
    };
    private static readonly string[] months = new string[12]
    {
      "Jan",
      "Feb",
      "Mar",
      "Apr",
      "May",
      "Jun",
      "Jul",
      "Aug",
      "Sep",
      "Oct",
      "Nov",
      "Dec"
    };
    private long _time;
    private long _timeZoneOffset;
    private bool _error;

    [Obsolete("Use GlobalContext.CurrentTimeZone instead")]
    [Hidden]
    public static TimeZoneInfo CurrentTimeZone => Context.CurrentGlobalContext.CurrentTimeZone;

    [DoNotEnumerate]
    public Date()
    {
      this._time = DateTime.Now.Ticks / 10000L;
      this._timeZoneOffset = Context.CurrentGlobalContext.CurrentTimeZone.GetUtcOffset(DateTime.Now).Ticks / 10000L;
      this._time -= this._timeZoneOffset;
    }

    [DoNotEnumerate]
    public Date(DateTime dateTime)
    {
      this._time = dateTime.Ticks / 10000L;
      this._timeZoneOffset = Context.CurrentGlobalContext.CurrentTimeZone.GetUtcOffset(dateTime).Ticks / 10000L;
      if (dateTime.Kind == DateTimeKind.Utc)
        return;
      this._time -= this._timeZoneOffset;
    }

    [DoNotEnumerate]
    [ArgumentsCount(7)]
    public Date(Arguments args)
    {
      if (args._iValue == 1)
      {
        JSValue valueValueString = args[0];
        if (valueValueString._valueType >= JSValueType.Object)
          valueValueString = valueValueString.ToPrimitiveValue_Value_String();
        switch (valueValueString._valueType)
        {
          case JSValueType.Boolean:
          case JSValueType.Integer:
          case JSValueType.Double:
            double d = Tools.JSObjectToDouble(valueValueString);
            if (double.IsNaN(d) || double.IsInfinity(d))
            {
              this._error = true;
              break;
            }
            this._time = (long) d + 62135596800000L;
            this._timeZoneOffset = Date.getTimeZoneOffset(this._time);
            break;
          case JSValueType.String:
            this._error = !Date.tryParse(valueValueString.ToString(), out this._time, out this._timeZoneOffset);
            break;
        }
      }
      else
      {
        for (int index = 0; index < 9 && !this._error; ++index)
        {
          if (args[index].Exists && !args[index].Defined)
          {
            this._error = true;
            return;
          }
        }
        long int64_1 = Tools.JSObjectToInt64(args[0], 1L, true);
        long int64_2 = Tools.JSObjectToInt64(args[1], 0L, true);
        long int64_3 = Tools.JSObjectToInt64(args[2], 1L, true);
        long int64_4 = Tools.JSObjectToInt64(args[3], 0L, true);
        long int64_5 = Tools.JSObjectToInt64(args[4], 0L, true);
        long int64_6 = Tools.JSObjectToInt64(args[5], 0L, true);
        long int64_7 = Tools.JSObjectToInt64(args[6], 0L, true);
        if (int64_1 == long.MaxValue || int64_1 == long.MinValue)
          this._error = true;
        else if (int64_1 > 9999999L || int64_1 < -9999999L)
          this._error = true;
        else if (int64_2 == long.MaxValue || int64_2 == long.MinValue)
          this._error = true;
        else if (int64_3 == long.MaxValue || int64_3 == long.MinValue)
          this._error = true;
        else if (int64_4 == long.MaxValue || int64_4 == long.MinValue)
          this._error = true;
        else if (int64_5 == long.MaxValue || int64_5 == long.MinValue)
          this._error = true;
        else if (int64_6 == long.MaxValue || int64_6 == long.MinValue)
          this._error = true;
        else if (int64_7 == long.MaxValue || int64_7 == long.MinValue)
        {
          this._error = true;
        }
        else
        {
          for (int index = 7; index < System.Math.Min(8, args._iValue); ++index)
          {
            switch (Tools.JSObjectToInt64(args[index], 0L, true))
            {
              case long.MinValue:
              case long.MaxValue:
                this._error = true;
                return;
              default:
                continue;
            }
          }
          if (int64_1 < 100L)
            int64_1 += 1900L;
          this._time = Date.dateToMilliseconds(int64_1, int64_2, int64_3, int64_4, int64_5, int64_6, int64_7);
          this._timeZoneOffset = Date.getTimeZoneOffset(this._time);
          this._time -= this._timeZoneOffset;
          if (this._time - 62135596800000L <= 8640000000000000L)
            return;
          this._error = true;
        }
      }
    }

    private static long getTimeZoneOffset(long time) => Context.CurrentGlobalContext.CurrentTimeZone.GetUtcOffset(new DateTime(System.Math.Min(System.Math.Max(time * 10000L, DateTime.MinValue.Ticks), DateTime.MaxValue.Ticks), DateTimeKind.Utc)).Ticks / 10000L;

    private void offsetTimeValue(
      JSValue value,
      long amort,
      long mul,
      bool correctTimeWithTimezone = true)
    {
      if (value == null || !value.Defined || value._valueType == JSValueType.Double && (double.IsNaN(value._dValue) || double.IsInfinity(value._dValue)))
      {
        this._error = true;
        this._time = 0L;
      }
      else
      {
        this._time += (-amort + Tools.JSObjectToInt64(value)) * mul;
        this._error = Date.isIncorrectTimeRange(this._time);
        long timeZoneOffset = this._timeZoneOffset;
        this._timeZoneOffset = Date.getTimeZoneOffset(this._time);
        if (!correctTimeWithTimezone)
          return;
        this._time -= this._timeZoneOffset - timeZoneOffset;
      }
    }

    [DoNotEnumerate]
    public JSValue valueOf() => this.getTime();

    [DoNotEnumerate]
    public JSValue getTime() => this._error ? (JSValue) double.NaN : (JSValue) (this._time - 62135596800000L);

    [DoNotEnumerate]
    public static JSValue now() => (JSValue) (DateTime.Now.Ticks / 10000L - Context.CurrentGlobalContext.CurrentTimeZone.GetUtcOffset(DateTime.Now).Ticks / 10000L - 62135596800000L);

    [DoNotEnumerate]
    public JSValue getTimezoneOffset() => this._error ? Number.NaN : (JSValue) (int) (-this._timeZoneOffset / 60000L);

    [DoNotEnumerate]
    public JSValue getYear()
    {
      JSValue fullYear = this.getFullYear();
      if (fullYear._valueType == JSValueType.Integer)
        fullYear._iValue -= 1900;
      else if (fullYear._valueType == JSValueType.Double)
        fullYear._dValue -= 1900.0;
      return fullYear;
    }

    [DoNotEnumerate]
    public JSValue getFullYear() => this._error ? Number.NaN : (JSValue) this.getYearImpl(true);

    private int getYearImpl(bool withTzo)
    {
      long time = this._time;
      if (withTzo)
        time += this._timeZoneOffset;
      long num1 = time / 12622780800000L * 400L;
      long num2 = time % 12622780800000L;
      long num3 = System.Math.Min(3L, num2 / 3155673600000L) * 100L;
      long num4 = num1 + num3;
      long num5 = num2 - System.Math.Min(3L, num2 / 3155673600000L) * 3155673600000L;
      long num6 = num5 / 126230400000L * 4L;
      return (int) (num4 + num6 + (System.Math.Min(3L, num5 % 126230400000L / 31536000000L) + 1L));
    }

    [DoNotEnumerate]
    public JSValue getUTCFullYear() => this._error ? Number.NaN : (JSValue) this.getYearImpl(false);

    [DoNotEnumerate]
    public JSValue getMonth() => this._error ? Number.NaN : (JSValue) this.getMonthImpl(true);

    private int getMonthImpl(bool withTzo)
    {
      long time = this._time;
      if (withTzo)
        time += this._timeZoneOffset;
      while (time < 0L)
        time += 88359465600000L;
      long num1 = time / 12622780800000L * 400L;
      long num2 = time % 12622780800000L;
      long num3 = num1 + System.Math.Min(3L, num2 / 3155673600000L) * 100L;
      long num4 = num2 - System.Math.Min(3L, num2 / 3155673600000L) * 3155673600000L;
      long num5 = num3 + num4 / 126230400000L * 4L;
      long num6 = num4 % 126230400000L;
      long num7 = num5 + (System.Math.Min(3L, num6 / 31536000000L) + 1L);
      int index1 = num7 % 4L == 0L && num7 % 100L != 0L || num7 % 400L == 0L ? 1 : 0;
      long num8 = num6 - System.Math.Min(3L, num6 / 31536000000L) * 31536000000L;
      int index2 = 0;
      while (Date.timeToMonthLengths[index2][index1] <= num8)
        ++index2;
      return index2 - 1;
    }

    [DoNotEnumerate]
    public JSValue getUTCMonth() => this.getMonth();

    [DoNotEnumerate]
    public JSValue getDate() => this._error ? Number.NaN : (JSValue) this.getDateImpl(true);

    private int getDateImpl(bool withTzo)
    {
      long time = this._time;
      if (withTzo)
        time += this._timeZoneOffset;
      if (time < 0L)
        time += (1L - time / 88359465600000L) * 88359465600000L;
      long num1 = time / 12622780800000L * 400L;
      long num2 = time % 12622780800000L;
      long num3 = num1 + System.Math.Min(3L, num2 / 3155673600000L) * 100L;
      long num4 = num2 - System.Math.Min(3L, num2 / 3155673600000L) * 3155673600000L;
      long num5 = num3 + num4 / 126230400000L * 4L;
      long num6 = num4 % 126230400000L;
      long num7 = num5 + (System.Math.Min(3L, num6 / 31536000000L) + 1L);
      int index1 = num7 % 4L == 0L && num7 % 100L != 0L || num7 % 400L == 0L ? 1 : 0;
      long num8 = num6 - System.Math.Min(3L, num6 / 31536000000L) * 31536000000L;
      int index2 = 0;
      while (Date.timeToMonthLengths[index2][index1] <= num8)
        ++index2;
      if (index2 > 0)
        num8 -= Date.timeToMonthLengths[index2 - 1][index1];
      return (int) (num8 / 86400000L + 1L);
    }

    [DoNotEnumerate]
    public JSValue getUTCDate() => (JSValue) this.getDateImpl(false);

    [DoNotEnumerate]
    public JSValue getDay() => (JSValue) this.getDayImpl(true);

    [DoNotEnumerate]
    public JSValue getUTCDay() => (JSValue) this.getDayImpl(false);

    private int getDayImpl(bool withTzo) => (int) ((System.Math.Abs(this._time + (withTzo ? this._timeZoneOffset : 0L)) / 86400000L + 1L) % 7L);

    [DoNotEnumerate]
    public JSValue getHours() => this._error ? Number.NaN : (JSValue) this.getHoursImpl(true);

    private int getHoursImpl(bool withTzo) => (int) (System.Math.Abs(this._time + (withTzo ? this._timeZoneOffset : 0L)) % 86400000L / 3600000L);

    [DoNotEnumerate]
    public JSValue getUTCHours() => this._error ? Number.NaN : (JSValue) this.getHoursImpl(false);

    [DoNotEnumerate]
    public JSValue getMinutes() => this._error ? Number.NaN : (JSValue) this.getMinutesImpl(true);

    private int getMinutesImpl(bool withTzo) => (int) (System.Math.Abs(this._time + (withTzo ? this._timeZoneOffset : 0L)) % 3600000L / 60000L);

    [DoNotEnumerate]
    public JSValue getUTCMinutes() => this._error ? Number.NaN : (JSValue) this.getMinutesImpl(false);

    [DoNotEnumerate]
    public JSValue getSeconds() => this._error ? Number.NaN : (JSValue) this.getSecondsImpl();

    private int getSecondsImpl() => (int) (System.Math.Abs(this._time) % 60000L / 1000L);

    [DoNotEnumerate]
    public JSValue getUTCSeconds() => this.getSeconds();

    [DoNotEnumerate]
    public JSValue getMilliseconds() => this._error ? Number.NaN : (JSValue) this.getMillisecondsImpl();

    private int getMillisecondsImpl() => (int) (System.Math.Abs(this._time) % 60000L % 1000L);

    [DoNotEnumerate]
    public JSValue getUTCMilliseconds() => this._error ? Number.NaN : (JSValue) this.getMillisecondsImpl();

    [DoNotEnumerate]
    public JSValue setTime(JSValue time)
    {
      if (time == null || !time.Defined || time._valueType == JSValueType.Double && (double.IsNaN(time._dValue) || double.IsInfinity(time._dValue)))
      {
        this._error = true;
        this._time = 0L;
        this._timeZoneOffset = 0L;
      }
      else
        this.offsetTimeValue(time, this._time - 62135596800000L, 1L, false);
      return this.valueOf();
    }

    [DoNotEnumerate]
    public JSValue setMilliseconds(JSValue milliseconds)
    {
      this.offsetTimeValue(milliseconds, (long) this.getMillisecondsImpl(), 1L);
      return this.valueOf();
    }

    [DoNotEnumerate]
    public JSValue setUTCMilliseconds(JSValue milliseconds) => this.setMilliseconds(milliseconds);

    [DoNotEnumerate]
    public JSValue setSeconds(JSValue seconds, JSValue milliseconds)
    {
      if (seconds != null && seconds.Exists)
        this.offsetTimeValue(seconds, (long) this.getSecondsImpl(), 1000L);
      if (!this._error && milliseconds != null && milliseconds.Exists)
        this.setMilliseconds(milliseconds);
      return this.valueOf();
    }

    [DoNotEnumerate]
    public JSValue setUTCSeconds(JSValue seconds, JSValue milliseconds) => this.setSeconds(seconds, milliseconds);

    [DoNotEnumerate]
    public JSValue setMinutes(JSValue minutes, JSValue seconds, JSValue milliseconds)
    {
      if (minutes != null && minutes.Exists)
        this.offsetTimeValue(minutes, (long) this.getMinutesImpl(true), 60000L);
      if (!this._error)
        this.setSeconds(seconds, milliseconds);
      return this.valueOf();
    }

    [DoNotEnumerate]
    public JSValue setUTCMinutes(JSValue minutes, JSValue seconds, JSValue milliseconds)
    {
      if (minutes != null && minutes.Exists)
        this.offsetTimeValue(minutes, (long) this.getMinutesImpl(false), 60000L);
      if (!this._error)
        this.setUTCSeconds(seconds, milliseconds);
      return this.valueOf();
    }

    [DoNotEnumerate]
    public JSValue setHours(JSValue hours, JSValue minutes, JSValue seconds, JSValue milliseconds)
    {
      if (hours != null && hours.Exists)
        this.offsetTimeValue(hours, (long) this.getHoursImpl(true), 3600000L);
      this.setMinutes(minutes, seconds, milliseconds);
      return this.valueOf();
    }

    [DoNotEnumerate]
    public JSValue setUTCHours(
      JSValue hours,
      JSValue minutes,
      JSValue seconds,
      JSValue milliseconds)
    {
      if (hours != null && hours.Exists)
        this.offsetTimeValue(hours, (long) this.getHoursImpl(false), 3600000L);
      this.setUTCMinutes(minutes, seconds, milliseconds);
      return this.valueOf();
    }

    [DoNotEnumerate]
    public JSValue setDate(JSValue days)
    {
      if (days != null && days.Exists)
        this.offsetTimeValue(days, (long) this.getDateImpl(true), 86400000L);
      return this.valueOf();
    }

    [DoNotEnumerate]
    public JSValue setUTCDate(JSValue days)
    {
      if (days != null && days.Exists)
        this.offsetTimeValue(days, (long) this.getDateImpl(false), 86400000L);
      return this.valueOf();
    }

    [DoNotEnumerate]
    public JSValue setMonth(JSValue month, JSValue day)
    {
      if (month != null && month.Exists)
      {
        if (!month.Defined || month._valueType == JSValueType.Double && (double.IsNaN(month._dValue) || double.IsInfinity(month._dValue)))
        {
          this._error = true;
          this._time = 0L;
          return Number.NaN;
        }
        long int64 = Tools.JSObjectToInt64(month);
        if (int64 < 0L || int64 > 12L)
        {
          this._time -= Date.timeToMonthLengths[this.getMonthImpl(true)][Date.isLeap(this.getYearImpl(true)) ? 1 : 0];
          this._time = Date.dateToMilliseconds((long) this.getYearImpl(true), int64, (long) this.getDateImpl(true), (long) this.getHoursImpl(true), (long) this.getMinutesImpl(true), (long) this.getSecondsImpl(), (long) this.getMillisecondsImpl());
        }
        else
          this._time = this._time - Date.timeToMonthLengths[this.getMonthImpl(true)][Date.isLeap(this.getYearImpl(true)) ? 1 : 0] + Date.timeToMonthLengths[int64][Date.isLeap(this.getYearImpl(true)) ? 1 : 0];
      }
      if (day != null)
        this.setDate(day);
      return this.valueOf();
    }

    [DoNotEnumerate]
    public JSValue setUTCMonth(JSValue monthO, JSValue day) => this.setMonth(monthO, day);

    [DoNotEnumerate]
    public JSValue setYear(JSValue year)
    {
      this._time = Date.dateToMilliseconds(Tools.JSObjectToInt64(year) + 1900L, (long) this.getMonthImpl(true), (long) this.getDateImpl(true), (long) this.getHoursImpl(true), (long) this.getMinutesImpl(true), (long) this.getSecondsImpl(), (long) this.getMillisecondsImpl());
      return year;
    }

    [DoNotEnumerate]
    public JSValue setUTCYear(JSValue year)
    {
      this._time = Date.dateToMilliseconds(Tools.JSObjectToInt64(year) + 1900L, (long) this.getMonthImpl(false), (long) this.getDateImpl(false), (long) this.getHoursImpl(false), (long) this.getMinutesImpl(false), (long) this.getSecondsImpl(), (long) this.getMillisecondsImpl());
      return year;
    }

    [DoNotEnumerate]
    public JSValue setFullYear(JSValue year, JSValue month, JSValue day)
    {
      if (year != null && year.Exists)
      {
        if (!year.Defined || year._valueType == JSValueType.Double && (double.IsNaN(year._dValue) || double.IsInfinity(year._dValue)))
        {
          this._error = true;
          this._time = 0L;
          return Number.NaN;
        }
        this._time = Date.dateToMilliseconds(Tools.JSObjectToInt64(year), (long) this.getMonthImpl(false), (long) this.getDateImpl(false), (long) this.getHoursImpl(false), (long) this.getMinutesImpl(false), (long) this.getSecondsImpl(), (long) this.getMillisecondsImpl());
        this._error = this._time < 5992660800000L;
      }
      if (!this._error)
        this.setMonth(month, day);
      return this.valueOf();
    }

    [DoNotEnumerate]
    public JSValue setUTCFullYear(JSValue year, JSValue month, JSValue day) => this.setFullYear(year, month, day);

    [DoNotEnumerate]
    [CLSCompliant(false)]
    public JSValue toString() => (JSValue) this.ToString();

    [Hidden]
    public DateTime ToDateTime()
    {
      DateTime dateTime = new DateTime(this._time * 10000L, DateTimeKind.Utc);
      dateTime = dateTime.ToLocalTime();
      return dateTime;
    }

    [DoNotEnumerate]
    public JSValue toLocaleString() => (JSValue) (this.stringifyDate(true, false) + " " + this.stringifyTime(true, false));

    [DoNotEnumerate]
    public JSValue toLocaleTimeString() => (JSValue) this.stringifyTime(true, false);

    [DoNotEnumerate]
    public JSValue toISOString() => this.toIsoString();

    private JSValue toIsoString()
    {
      if (this._error || Date.isIncorrectTimeRange(this._time))
        ExceptionHelper.Throw((Error) new RangeError("Invalid time value"));
      return (JSValue) (this.getYearImpl(false).ToString() + "-" + (this.getMonthImpl(false) + 1).ToString("00") + "-" + this.getDateImpl(false).ToString("00") + "T" + this.getHoursImpl(false).ToString("00") + ":" + this.getMinutesImpl(false).ToString("00") + ":" + this.getSecondsImpl().ToString("00") + "." + ((double) this.getMillisecondsImpl() / 1000.0).ToString(".000", (IFormatProvider) CultureInfo.InvariantCulture).Substring(1) + "Z");
    }

    private static bool isIncorrectTimeRange(long time) => time > 8702135604000000L || time <= -8577864403199999L;

    private string stringify(bool withTzo, bool rfc1123) => this._error ? "Invalid date" : this.stringifyDate(withTzo, rfc1123) + " " + this.stringifyTime(withTzo, rfc1123);

    private string stringifyDate(bool withTzo, bool rfc1123)
    {
      if (withTzo & rfc1123)
        throw new ArgumentException();
      if (this._error)
        return "Invalid date";
      return Date.daysOfWeek[(this.getDayImpl(withTzo) + 6) % 7] + (rfc1123 ? ", " : " ") + Date.months[this.getMonthImpl(withTzo)] + " " + this.getDateImpl(withTzo).ToString("00") + " " + this.getYearImpl(withTzo).ToString();
    }

    private string stringifyTime(bool withTzo, bool rfc1123)
    {
      if (withTzo & rfc1123)
        throw new ArgumentException();
      if (this._error)
        return "Invalid date";
      TimeSpan offset = new TimeSpan(this._timeZoneOffset * 10000L);
      string str1 = Context.CurrentGlobalContext.CurrentTimeZone.IsDaylightSavingTime(new DateTimeOffset(this._time * 10000L, offset)) ? Context.CurrentGlobalContext.CurrentTimeZone.DaylightName : Context.CurrentGlobalContext.CurrentTimeZone.StandardName;
      string[] strArray1 = new string[5]
      {
        this.getHoursImpl(withTzo).ToString("00:"),
        null,
        null,
        null,
        null
      };
      int num = this.getMinutesImpl(withTzo);
      strArray1[1] = num.ToString("00:");
      num = this.getSecondsImpl();
      strArray1[2] = num.ToString("00");
      strArray1[3] = " GMT";
      string str2;
      if (!withTzo)
      {
        str2 = "";
      }
      else
      {
        string[] strArray2 = new string[5]
        {
          offset.Ticks > 0L ? "+" : "",
          null,
          null,
          null,
          null
        };
        num = offset.Hours * 100 + offset.Minutes;
        strArray2[1] = num.ToString("0000");
        strArray2[2] = " (";
        strArray2[3] = str1;
        strArray2[4] = ")";
        str2 = string.Concat(strArray2);
      }
      strArray1[4] = str2;
      return string.Concat(strArray1);
    }

    [Hidden]
    public override string ToString() => this.stringify(true, false);

    [DoNotEnumerate]
    [ArgumentsCount(1)]
    public JSValue toJSON() => this.toISOString();

    [DoNotEnumerate]
    public JSValue toUTCString() => (JSValue) this.stringify(false, true);

    [DoNotEnumerate]
    public JSValue toGMTString() => (JSValue) this.stringify(false, true);

    [DoNotEnumerate]
    public JSValue toTimeString() => (JSValue) this.stringifyTime(true, false);

    [DoNotEnumerate]
    public JSValue toDateString() => (JSValue) this.stringifyDate(true, false);

    [DoNotEnumerate]
    public JSValue toLocaleDateString() => (JSValue) this.stringifyDate(true, false);

    [DoNotEnumerate]
    public static JSValue parse(string dateTime)
    {
      long time = 0;
      long tzo = 0;
      return Date.tryParse(dateTime, out time, out tzo) ? (JSValue) (time - 62135596800000L) : (JSValue) double.NaN;
    }

    [DoNotEnumerate]
    [ArgumentsCount(7)]
    public static JSValue UTC(Arguments dateTime)
    {
      try
      {
        return (JSValue) (Date.dateToMilliseconds(Tools.JSObjectToInt64(dateTime[0], 1L), Tools.JSObjectToInt64(dateTime[1]), Tools.JSObjectToInt64(dateTime[2], 1L), Tools.JSObjectToInt64(dateTime[3]), Tools.JSObjectToInt64(dateTime[4]), Tools.JSObjectToInt64(dateTime[5]), Tools.JSObjectToInt64(dateTime[6])) - 62135596800000L);
      }
      catch
      {
        return (JSValue) double.NaN;
      }
    }

    private static long dateToMilliseconds(
      long year,
      long month,
      long day,
      long hour,
      long minute,
      long second,
      long millisecond)
    {
      for (; month < 0L; month += 12L)
        --year;
      year += month / 12L;
      month %= 12L;
      int index = year % 4L == 0L && year % 100L != 0L || year % 400L == 0L ? 1 : 0;
      --year;
      --day;
      long num1 = year / 400L * 12622780800000L;
      year %= 400L;
      long num2 = year / 100L * 3155673600000L;
      long num3 = num1 + num2;
      year %= 100L;
      long num4 = year / 4L * 126230400000L;
      long num5 = num3 + num4;
      year %= 4L;
      long num6 = 365L * year * 86400000L;
      return num5 + num6 + Date.timeToMonthLengths[month][index] + day * 86400000L + hour * 3600000L + minute * 60000L + second * 1000L + millisecond;
    }

    private static IEnumerable<string> tokensOf(string source)
    {
      int position = 0;
      int startIndex = 0;
      bool allowSlash = true;
      while (position < source.Length)
      {
        if (source[position] == '(' && (startIndex == position || source.IndexOf(':', startIndex, position - startIndex) == -1))
        {
          if (startIndex != position)
            yield return source.Substring(startIndex, position - startIndex);
          int num = 1;
          ++position;
          while (num > 0 && position < source.Length)
          {
            switch (source[position++])
            {
              case '(':
                ++num;
                continue;
              case ')':
                --num;
                continue;
              default:
                continue;
            }
          }
          startIndex = position;
        }
        else if (!Tools.IsWhiteSpace(source[position]) && (source[position] != '/' || !allowSlash))
        {
          ++position;
        }
        else
        {
          allowSlash &= source[position] == '/';
          if (startIndex != position)
          {
            yield return source.Substring(startIndex, position - startIndex);
            startIndex = position;
          }
          else
            startIndex = ++position;
        }
      }
      if (startIndex != position)
        yield return source.Substring(startIndex);
    }

    private static int indexOf(IList<string> list, string value, bool ignoreCase)
    {
      for (int index = 0; index < list.Count; ++index)
      {
        if (string.Compare(list[index], value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == 0)
          return index;
      }
      return -1;
    }

    private static bool parseSelf(string timeStr, out long time, out long timeZoneOffset)
    {
      timeStr = timeStr.Trim(Tools.TrimChars);
      if (string.IsNullOrEmpty(timeStr))
      {
        time = 0L;
        timeZoneOffset = 0L;
        return false;
      }
      time = 0L;
      timeZoneOffset = 0L;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      bool flag6 = false;
      int num1 = 0;
      int year = 0;
      int day = 1;
      string[] strArray = (string[]) null;
      int num2 = 0;
      int num3 = 0;
      int result1 = 0;
      bool flag7 = false;
      foreach (string str in Date.tokensOf(timeStr))
      {
        if (Date.indexOf((IList<string>) Date.daysOfWeek, str, true) == -1)
        {
          int result2 = Date.indexOf((IList<string>) Date.months, str, true);
          if (result2 != -1)
          {
            if (flag2)
            {
              if (flag1 || flag3 & flag4)
                return false;
              if (!flag3)
              {
                day = num1;
                flag3 = true;
              }
              else
              {
                if (flag4)
                  return false;
                year = num1;
                flag4 = true;
              }
            }
            flag1 = true;
            flag2 = true;
            num1 = result2 + 1;
          }
          else if (int.TryParse(str, out result2))
          {
            if (!flag2 && result2 <= 12 && result2 > 0)
            {
              num1 = result2;
              flag2 = true;
            }
            else if (!flag3 && result2 > 0 && result2 <= 31)
            {
              day = result2;
              flag3 = true;
            }
            else
            {
              if (flag4 || flag3 | flag2 && (!flag3 || !flag2))
                return false;
              year = result2;
              flag4 = true;
            }
          }
          else if (str.IndexOf(':') != -1)
          {
            if (strArray != null)
              return false;
            strArray = str.Split(':');
          }
          else if (str.StartsWith("gmt", StringComparison.OrdinalIgnoreCase) || str.StartsWith("ut", StringComparison.OrdinalIgnoreCase) || str.StartsWith("utc", StringComparison.OrdinalIgnoreCase) || str.StartsWith("pst", StringComparison.OrdinalIgnoreCase) || str.StartsWith("pdt", StringComparison.OrdinalIgnoreCase))
          {
            if (flag5)
              return false;
            if (str.Length > 3)
            {
              if (flag6 || !int.TryParse(str.Substring(3), out result1))
                return false;
              num3 += result1 % 100;
              num2 += result1 / 100;
            }
            if (str.StartsWith("pst", StringComparison.OrdinalIgnoreCase))
              num2 -= 8;
            if (str.StartsWith("pdt", StringComparison.OrdinalIgnoreCase))
              num2 -= 7;
            flag5 = true;
          }
          else if (!flag6 && (str[0] == '+' || str[0] == '-') && int.TryParse(str.Substring(3), out result1))
          {
            num3 += result1 % 100;
            num2 += result1 / 100;
            flag6 = true;
            flag5 = true;
          }
          else if (string.Compare("am", str, StringComparison.OrdinalIgnoreCase) != 0)
          {
            if (string.Compare("pm", str, StringComparison.OrdinalIgnoreCase) != 0)
              return false;
            flag7 = true;
          }
        }
      }
      try
      {
        if (!flag3 && !flag2 && !flag4 && strArray == null || flag3 | flag2 | flag4 && (!flag3 || !flag2 || !flag4))
          return false;
        if (!flag4)
          year = DateTime.Now.Year;
        else if (year < 100)
          year += DateTime.Now.Year / 100 * 100;
        time = Date.dateToMilliseconds((long) year, (long) (num1 - 1), (long) day, strArray == null || strArray.Length == 0 ? (long) -num2 : (long) double.Parse(strArray[0]) - (long) num2, strArray == null || strArray.Length <= 1 ? (long) -num3 : (long) double.Parse(strArray[1]) - (long) num3, strArray == null || strArray.Length <= 2 ? 0L : (long) double.Parse(strArray[2]), strArray == null || strArray.Length <= 3 ? 0L : (long) double.Parse(strArray[3]));
        if (flag7)
          time += 43200000L;
        timeZoneOffset = Context.CurrentGlobalContext.CurrentTimeZone.GetUtcOffset(new DateTime(time * 10000L)).Ticks / 10000L;
        if (!flag5)
          time -= timeZoneOffset;
      }
      catch
      {
        return false;
      }
      return true;
    }

    private static bool parseIso8601(string timeStr, out long time, out long timeZoneOffset)
    {
      time = 0L;
      timeZoneOffset = 0L;
      int year = 0;
      int num1 = int.MinValue;
      int day = int.MinValue;
      int hour = 0;
      int minute = 0;
      int second = 0;
      int millisecond = 0;
      bool flag1 = false;
      int num2 = 0;
      bool flag2 = false;
      int index1 = 0;
      int index2 = 0;
      while (index1 < "YYYY|-MM|-DD|T|HH|:MM|:SS|.S*".Length)
      {
        if (timeStr.Length <= index2)
        {
          if ("YYYY|-MM|-DD|T|HH|:MM|:SS|.S*"[index1] != '|')
            return false;
          break;
        }
        switch (char.ToLowerInvariant("YYYY|-MM|-DD|T|HH|:MM|:SS|.S*"[index1]))
        {
          case ' ':
            if ((int) "YYYY|-MM|-DD|T|HH|:MM|:SS|.S*"[index1] != (int) timeStr[index2])
              return false;
            while (index2 < timeStr.Length && Tools.IsWhiteSpace(timeStr[index2]))
              ++index2;
            --index2;
            break;
          case '*':
            index1 -= 2;
            --index2;
            flag2 = true;
            break;
          case '-':
            if ((int) "YYYY|-MM|-DD|T|HH|:MM|:SS|.S*"[index1] != (int) timeStr[index2])
              return false;
            break;
          case '.':
            if ('.' != timeStr[index2])
            {
              if (char.ToLowerInvariant(timeStr[index2]) != 'z')
                return false;
              index2 = timeStr.Length;
              index1 = "YYYY|-MM|-DD|T|HH|:MM|:SS|.S*".Length;
              break;
            }
            if (num2 != 1)
              return false;
            ++num2;
            break;
          case '/':
            if (num2 != 0 || (int) "YYYY|-MM|-DD|T|HH|:MM|:SS|.S*"[index1] != (int) timeStr[index2])
              return false;
            break;
          case ':':
            if (num2 != 1 || (int) "YYYY|-MM|-DD|T|HH|:MM|:SS|.S*"[index1] != (int) timeStr[index2])
              return false;
            break;
          case 'd':
            if (num2 != 0 || !Tools.IsDigit(timeStr[index2]))
              return false;
            if (day == int.MinValue)
              day = 0;
            day = day * 10 + (int) timeStr[index2] - 48;
            break;
          case 'h':
            if (num2 != 1 || !Tools.IsDigit(timeStr[index2]))
              return false;
            hour = hour * 10 + (int) timeStr[index2] - 48;
            break;
          case 'm':
            if (!Tools.IsDigit(timeStr[index2]))
              return false;
            switch (num2)
            {
              case 0:
                if (num1 == int.MinValue)
                  num1 = 0;
                num1 = num1 * 10 + (int) timeStr[index2] - 48;
                break;
              case 1:
                minute = minute * 10 + (int) timeStr[index2] - 48;
                break;
              default:
                return false;
            }
            break;
          case 's':
            if (num2 < 1)
              return false;
            if (!Tools.IsDigit(timeStr[index2]))
            {
              if (!flag2)
                return false;
              flag2 = false;
              ++index1;
              --index2;
              break;
            }
            if (num2 == 1)
            {
              second = second * 10 + (int) timeStr[index2] - 48;
              break;
            }
            millisecond = millisecond * 10 + (int) timeStr[index2] - 48;
            break;
          case 't':
            if (num2 != 0 || char.ToLowerInvariant(timeStr[index2]) != 't' && !char.IsWhiteSpace(timeStr[index2]))
              return false;
            flag1 = char.ToLowerInvariant(timeStr[index2]) == 't';
            ++num2;
            break;
          case 'y':
            if (num2 != 0 || !Tools.IsDigit(timeStr[index2]))
              return false;
            year = year * 10 + (int) timeStr[index2] - 48;
            break;
          case '|':
            --index2;
            break;
          default:
            return false;
        }
        ++index1;
        ++index2;
      }
      if (index2 < timeStr.Length && char.ToLowerInvariant(timeStr[index2]) != 'z')
        return false;
      if (num1 == int.MinValue)
        num1 = 1;
      if (day == int.MinValue)
        day = 1;
      if (year < 100)
        year += DateTime.Now.Year / 100 * 100;
      time = Date.dateToMilliseconds((long) year, (long) (num1 - 1), (long) day, (long) hour, (long) minute, (long) second, (long) millisecond);
      if (flag1)
      {
        timeZoneOffset = Context.CurrentGlobalContext.CurrentTimeZone.GetUtcOffset(new DateTime(time * 10000L)).Ticks / 10000L;
        if (index2 >= timeStr.Length)
          time -= timeZoneOffset;
      }
      return true;
    }

    private static bool parseDateTime(string timeString, out long time, out long tzo)
    {
      try
      {
        DateTime dateTime = DateTime.Parse(timeString);
        time = dateTime.Ticks / 10000L;
        tzo = Context.CurrentGlobalContext.CurrentTimeZone.GetUtcOffset(dateTime).Ticks / 10000L;
        if (dateTime.Kind == DateTimeKind.Local)
          time += tzo;
        return true;
      }
      catch (FormatException ex)
      {
        time = 0L;
        tzo = 0L;
        return false;
      }
    }

    private static bool tryParse(string timeString, out long time, out long tzo) => Date.parseIso8601(timeString, out time, out tzo) || Date.parseSelf(timeString, out time, out tzo) || Date.parseDateTime(timeString, out time, out tzo);

    private static bool isLeap(int year) => year % 4 == 0 && year % 100 != 0 || year % 400 == 0;

    [Hidden]
    public override bool Equals(object obj) => base.Equals(obj);

    [Hidden]
    public override int GetHashCode() => base.GetHashCode();
  }
}
