// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.DeleteProperty
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class DeleteProperty : Expression
  {
    private JSValue cachedMemberName;

    public Expression Source => this._left;

    public Expression PropertyName => this._right;

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => false;

    internal DeleteProperty(Expression obj, Expression fieldName)
      : base(obj, fieldName, true)
    {
      if (!(fieldName is Constant))
        return;
      this.cachedMemberName = fieldName.Evaluate((Context) null);
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue1 = this._left.Evaluate(context);
      JSValue jsValue2;
      if (jsValue1._valueType < JSValueType.Object)
      {
        jsValue2 = jsValue1.CloneImpl(false);
      }
      else
      {
        if (!(jsValue1._oValue is JSValue jsValue3))
          jsValue3 = jsValue1;
        jsValue2 = jsValue3;
      }
      int num = jsValue2.DeleteProperty(this.cachedMemberName ?? this._right.Evaluate(context)) ? 1 : 0;
      context._objectSource = (JSValue) null;
      if (num == 0 && context._strict)
        ExceptionHelper.ThrowTypeError("Cannot delete property \"" + this._left?.ToString() + "\".");
      return (JSValue) (num != 0);
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
      return false;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString()
    {
      string str = this._left.ToString();
      int index = 0;
      Constant right = this._right as Constant;
      return "delete " + (!(this._right is Constant) || right.value.ToString().Length <= 0 || !Parser.ValidateName(right.value.ToString(), ref index, true) ? str + "[" + this._right?.ToString() + "]" : str + "." + right.value?.ToString());
    }
  }
}
