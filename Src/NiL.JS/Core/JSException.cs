// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.JSException
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;

namespace NiL.JS.Core
{
  public sealed class JSException : Exception
  {
    public JSValue Error { get; }

    public CodeNode ExceptionMaker { get; }

    public string Code { get; internal set; }

    public CodeCoordinates CodeCoordinates { get; internal set; }

    public JSException(NiL.JS.BaseLibrary.Error data) => this.Error = Context.CurrentGlobalContext.ProxyValue((object) data);

    public JSException(NiL.JS.BaseLibrary.Error data, CodeNode exceptionMaker, string code)
    {
      this.Error = Context.CurrentGlobalContext.ProxyValue((object) data);
      this.ExceptionMaker = exceptionMaker;
      this.Code = code;
      if (code == null)
        return;
      this.CodeCoordinates = CodeCoordinates.FromTextPosition(code, exceptionMaker.Position, exceptionMaker.Length);
    }

    public JSException(JSValue data) => this.Error = data;

    public JSException(JSValue data, Exception innerException)
      : base("External error", innerException)
    {
      this.Error = data;
    }

    public JSException(NiL.JS.BaseLibrary.Error avatar, Exception innerException)
      : base("", innerException)
    {
      this.Error = Context.CurrentGlobalContext.ProxyValue((object) avatar);
    }

    public override string Message
    {
      get
      {
        string str = this.CodeCoordinates != null ? " at " + this.CodeCoordinates?.ToString() : "";
        string message;
        if (this.Error._oValue is NiL.JS.BaseLibrary.Error)
        {
          JSValue property1 = this.Error.GetProperty("name");
          if (property1._valueType == JSValueType.Property)
            property1 = (JSValue) (property1._oValue as PropertyPair).getter.Call(this.Error, (Arguments) null).ToString();
          JSValue property2 = this.Error.GetProperty("message");
          message = property2._valueType != JSValueType.Property ? property1?.ToString() + ": " + property2?.ToString() + str : property1?.ToString() + ": " + (property2._oValue as PropertyPair).getter.Call(this.Error, (Arguments) null)?.ToString() + str;
        }
        else
          message = this.Error.ToString() + str;
        return message;
      }
    }
  }
}
