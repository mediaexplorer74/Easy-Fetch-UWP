// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.InstanceOf
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class InstanceOf : Expression
  {
    protected internal override PredictedType ResultType => PredictedType.Bool;

    internal override bool ResultInTempContainer => false;

    public InstanceOf(Expression first, Expression second)
      : base(first, second, true)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue1 = this._tempContainer;
      if (jsValue1 == null)
        jsValue1 = new JSValue()
        {
          _attributes = JSValueAttributesInternal.Temporary
        };
      JSValue jsValue2 = jsValue1;
      this._tempContainer = (JSValue) null;
      jsValue2.Assign(this._left.Evaluate(context));
      JSValue jsValue3 = this._right.Evaluate(context);
      this._tempContainer = jsValue2;
      if (jsValue3._valueType != JSValueType.Function)
        ExceptionHelper.Throw((Error) new TypeError("Right-hand value of instanceof is not a function."));
      if (jsValue2._valueType < JSValueType.Object)
        return (JSValue) false;
      JSValue prototype = (jsValue3._oValue as Function).prototype;
      if (prototype._valueType < JSValueType.Object || prototype.IsNull)
        ExceptionHelper.Throw((Error) new TypeError("Property \"prototype\" of function not represent object."));
      if (prototype._oValue != null)
      {
        for (; jsValue2 != null && jsValue2._valueType >= JSValueType.Object && jsValue2._oValue != null; jsValue2 = (JSValue) jsValue2.__proto__)
        {
          if (jsValue2._oValue == prototype._oValue)
            return (JSValue) true;
        }
      }
      return (JSValue) false;
    }

    public override bool Build(
      ref CodeNode _this,
      int expressionDepth,
      Dictionary<string, VariableDescriptor> variables,
      CodeContext codeContext,
      InternalCompilerMessageCallback message,
      FunctionInfo stats,
      Options opts)
    {
      int num = base.Build(ref _this, expressionDepth, variables, codeContext, message, stats, opts) ? 1 : 0;
      if (num != 0)
        return num != 0;
      Constant left = this._left as Constant;
      return num != 0;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "(" + this._left?.ToString() + " instanceof " + this._right?.ToString() + ")";
  }
}
