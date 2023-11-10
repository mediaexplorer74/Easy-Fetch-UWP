// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.Boolean
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System;
using System.Runtime.CompilerServices;

namespace NiL.JS.BaseLibrary
{
  public class Boolean : JSObject
  {
    internal const string TrueString = "true";
    internal const string FalseString = "false";
    [Hidden]
    internal static readonly Boolean True;
    [Hidden]
    internal static readonly Boolean False;

    [DoNotEnumerate]
    public Boolean()
    {
      this._valueType = JSValueType.Boolean;
      this._iValue = 0;
      this._attributes |= JSValueAttributesInternal.SystemObject;
    }

    [StrictConversion]
    [DoNotEnumerate]
    public Boolean(Arguments obj)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      this._valueType = JSValueType.Boolean;
      this._iValue = (bool) obj[0] ? 1 : 0;
      this._attributes |= JSValueAttributesInternal.SystemObject;
    }

    [StrictConversion]
    [DoNotEnumerate]
    public Boolean(bool value)
    {
      this._valueType = JSValueType.Boolean;
      this._iValue = value ? 1 : 0;
      this._attributes |= JSValueAttributesInternal.SystemObject;
    }

    [StrictConversion]
    [DoNotEnumerate]
    public Boolean(double value)
    {
      this._valueType = JSValueType.Boolean;
      this._iValue = value == 0.0 || double.IsNaN(value) ? 0 : 1;
      this._attributes |= JSValueAttributesInternal.SystemObject;
    }

    [StrictConversion]
    [DoNotEnumerate]
    public Boolean(int value)
    {
      this._valueType = JSValueType.Boolean;
      this._iValue = value != 0 ? 1 : 0;
      this._attributes |= JSValueAttributesInternal.SystemObject;
    }

    [StrictConversion]
    [DoNotEnumerate]
    public Boolean(string value)
    {
      this._valueType = JSValueType.Boolean;
      this._iValue = !string.IsNullOrEmpty(value) ? 1 : 0;
      this._attributes |= JSValueAttributesInternal.SystemObject;
    }

    [Hidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Boolean(bool value) => !value ? Boolean.False : Boolean.True;

    [Hidden]
    public static implicit operator bool(Boolean value) => value != null && value._iValue != 0;

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(0)]
    public static JSValue toLocaleString(JSValue self)
    {
      if ((object) self.GetType() != (object) typeof (Boolean) && self._valueType != JSValueType.Boolean)
        ExceptionHelper.Throw((Error) new TypeError("Boolean.prototype.toLocaleString called for not boolean."));
      return (JSValue) (self._iValue != 0 ? "true" : "false");
    }

    [DoNotEnumerate]
    [InstanceMember]
    [ArgumentsCount(0)]
    public static JSValue valueOf(JSValue self)
    {
      if ((object) self.GetType() == (object) typeof (Boolean))
        return (JSValue) (self._iValue != 0);
      if (self._valueType != JSValueType.Boolean)
        ExceptionHelper.Throw((Error) new TypeError("Boolean.prototype.valueOf called for not boolean."));
      return self;
    }

    [CLSCompliant(false)]
    [InstanceMember]
    [ArgumentsCount(0)]
    [DoNotEnumerate]
    public static JSValue toString(JSValue self, Arguments args)
    {
      if ((object) self.GetType() != (object) typeof (Boolean) && self._valueType != JSValueType.Boolean)
        ExceptionHelper.Throw((Error) new TypeError("Boolean.prototype.toString called for not boolean."));
      return (JSValue) (self._iValue != 0 ? "true" : "false");
    }

    static Boolean()
    {
      Boolean boolean1 = new Boolean(true);
      boolean1._attributes = JSValueAttributesInternal.SystemObject;
      Boolean.True = boolean1;
      Boolean boolean2 = new Boolean(false);
      boolean2._attributes = JSValueAttributesInternal.SystemObject;
      Boolean.False = boolean2;
    }
  }
}
