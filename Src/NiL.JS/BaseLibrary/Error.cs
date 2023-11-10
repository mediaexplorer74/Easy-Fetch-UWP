// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.Error
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System;

namespace NiL.JS.BaseLibrary
{
  public class Error
  {
    [DoNotEnumerate]
    public JSValue message { [Hidden] get; private set; }

    [DoNotEnumerate]
    public JSValue name { [Hidden] get; set; }

    [DoNotEnumerate]
    public Error()
    {
      this.name = (JSValue) this.GetType().Name;
      this.message = (JSValue) "";
    }

    [DoNotEnumerate]
    public Error(Arguments args)
    {
      this.name = (JSValue) this.GetType().Name;
      this.message = (JSValue) args[0].ToString();
    }

    [DoNotEnumerate]
    public Error(string message)
    {
      this.name = (JSValue) this.GetType().Name;
      this.message = (JSValue) message;
    }

    [Hidden]
    public override string ToString()
    {
      string str1;
      if (this.message == null || this.message._valueType <= JSValueType.Undefined || string.IsNullOrEmpty(str1 = this.message.ToString()))
        return this.name.ToString();
      string str2;
      return this.name == null || this.name._valueType <= JSValueType.Undefined || string.IsNullOrEmpty(str2 = this.name.ToString()) ? str1 : str2 + ": " + str1;
    }

    [DoNotEnumerate]
    [CLSCompliant(false)]
    public JSValue toString() => (JSValue) this.ToString();
  }
}
