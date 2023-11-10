// Decompiled with JetBrains decompiler
// Type: NiL.JS.Extensions.JSValueExtensions
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using System;
using System.Reflection;

namespace NiL.JS.Extensions
{
  public static class JSValueExtensions
  {
    public static bool Is(this JSValue self, JSValueType type) => self != null && self._valueType == type;

    public static bool Is<T>(this JSValue self)
    {
      if (self == null)
        return false;
      switch (typeof (T).GetTypeCode())
      {
        case TypeCode.Object:
          return self.Value is T;
        case TypeCode.Boolean:
          return self.Is(JSValueType.Boolean);
        case TypeCode.Char:
          return self != null && self._valueType == JSValueType.Object && self._oValue is char;
        case TypeCode.SByte:
          return self.Is(JSValueType.Integer) && (self._iValue & -256) == 0;
        case TypeCode.Byte:
          return self.Is(JSValueType.Integer) && (self._iValue & -256) == 0;
        case TypeCode.Int16:
          return self.Is(JSValueType.Integer) && (self._iValue & -65536) == 0;
        case TypeCode.UInt16:
          return self.Is(JSValueType.Integer) && (self._iValue & -65536) == 0;
        case TypeCode.Int32:
          return self.Is(JSValueType.Integer);
        case TypeCode.UInt32:
          return self.Is(JSValueType.Integer);
        case TypeCode.Int64:
          if (self.Is(JSValueType.Integer))
            return true;
          return self.Is(JSValueType.Double) && self._dValue == (double) (long) self._dValue;
        case TypeCode.UInt64:
          if (self.Is(JSValueType.Integer) && self._iValue >= 0)
            return true;
          return self.Is(JSValueType.Double) && self._dValue == (double) (ulong) self._dValue;
        case TypeCode.Single:
          return self.Is(JSValueType.Double) && self._dValue == self._dValue;
        case TypeCode.Double:
          return self.Is(JSValueType.Double);
        case TypeCode.Decimal:
          return false;
        case TypeCode.String:
          return self.Is(JSValueType.String);
        default:
          return false;
      }
    }

    public static T As<T>(this JSValue self) => typeof (T).GetTypeCode() == TypeCode.Double ? self.GetDefinedOr<T>((T) (ValueType) double.NaN) : self.GetDefinedOr<T>(default (T));

    public static T GetDefinedOr<T>(this JSValue self, T defaultValue)
    {
      if (!self.Defined)
        return defaultValue;
      switch (typeof (T).GetTypeCode())
      {
        case TypeCode.Object:
          if (self.Value is Function && TypeExtensions.IsAssignableFrom(typeof (Delegate), typeof (T)))
            return ((Function) self.Value).MakeDelegate<T>();
          if (TypeExtensions.IsAssignableFrom(typeof (T), self.GetType()))
            return (T) self;
          try
          {
            return (T) (Tools.convertJStoObj(self, typeof (T), true) ?? self.Value);
          }
          catch (InvalidCastException ex)
          {
            return default (T);
          }
        case TypeCode.Boolean:
          return (T) (ValueType) (bool) self;
        case TypeCode.Char:
          if (self._valueType == JSValueType.Object && self._oValue is char)
            return (T) self._oValue;
          break;
        case TypeCode.SByte:
          return (T) (ValueType) (sbyte) Tools.JSObjectToInt32(self);
        case TypeCode.Byte:
          return (T) (ValueType) (byte) Tools.JSObjectToInt32(self);
        case TypeCode.Int16:
          return (T) (ValueType) (short) Tools.JSObjectToInt32(self);
        case TypeCode.UInt16:
          return (T) (ValueType) (ushort) Tools.JSObjectToInt32(self);
        case TypeCode.Int32:
          return (T) (ValueType) Tools.JSObjectToInt32(self);
        case TypeCode.UInt32:
          return (T) (ValueType) (uint) Tools.JSObjectToInt32(self);
        case TypeCode.Int64:
          return (T) (ValueType) Tools.JSObjectToInt64(self);
        case TypeCode.UInt64:
          return (T) (ValueType) (ulong) Tools.JSObjectToInt64(self);
        case TypeCode.Single:
          return (T) (ValueType) (float) Tools.JSObjectToDouble(self);
        case TypeCode.Double:
          return (T) (ValueType) Tools.JSObjectToDouble(self);
        case TypeCode.Decimal:
          double d = Tools.JSObjectToDouble(self);
          if (double.IsNaN(d))
            d = 0.0;
          return (T) (ValueType) (Decimal) d;
        case TypeCode.String:
          return (T) self.ToString();
      }
      throw new InvalidCastException();
    }

    public static bool IsNaN(this JSValue self) => self != null && self._valueType == JSValueType.Double && double.IsNaN(self._dValue);

    public static bool IsUndefined(this JSValue self) => self != null && self._valueType <= JSValueType.Undefined;

    public static bool IsNumber(this JSValue self) => self._valueType == JSValueType.Integer || self._valueType == JSValueType.Double;

    public static object ConvertToType(this JSValue value, Type targetType) => Tools.convertJStoObj(value, targetType, true);

    public static void Assign(this JSValue target, object value) => target.Assign(JSValue.Marshal(value));
  }
}
