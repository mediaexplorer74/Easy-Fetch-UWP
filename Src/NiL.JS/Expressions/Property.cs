// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Property
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class Property : Expression
  {
    private JSValue cachedMemberName;
    private PropertyScope memberScope;

    public CodeNode Source => (CodeNode) this._left;

    public CodeNode FieldName => (CodeNode) this._right;

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => false;

    internal Property(Expression source, Expression fieldName)
      : base(source, fieldName, false)
    {
    }

    protected internal override JSValue EvaluateForWrite(Context context)
    {
      JSValue jsValue1 = this._left.Evaluate(context);
      JSValue jsValue2;
      if (jsValue1._valueType < JSValueType.Object)
      {
        jsValue2 = jsValue1.Clone() as JSValue;
      }
      else
      {
        if (!(jsValue1._oValue is JSValue jsValue3))
          jsValue3 = jsValue1;
        jsValue2 = jsValue3;
      }
      JSValue property = jsValue2.GetProperty(this.cachedMemberName ?? this._right.Evaluate(context), true, this.memberScope);
      context._objectSource = jsValue2;
      if (property._valueType == JSValueType.NotExists)
        property._valueType = JSValueType.NotExistsInObject;
      return property;
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue target = this._left.Evaluate(context);
      if (target._valueType < JSValueType.Object)
        target = target.CloneImpl(false);
      else if (target != target._oValue && target._oValue is JSValue oValue)
        target = oValue;
      JSValue property = target.GetProperty(this.cachedMemberName ?? this._right.Evaluate(context), false, this.memberScope);
      context._objectSource = target;
      if (property == null)
        property = JSValue.undefined;
      if (property._valueType == JSValueType.NotExists)
        property._valueType = JSValueType.NotExistsInObject;
      else if (property._valueType == JSValueType.Property)
        property = Tools.InvokeGetter(property, target);
      return property;
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
      if (stats != null)
        stats.UseGetMember = true;
      base.Build(ref _this, expressionDepth, variables, codeContext, message, stats, opts);
      if (this._right is Constant)
      {
        this.cachedMemberName = this._right.Evaluate((Context) null);
        if (stats != null && this.cachedMemberName.ToString() == "arguments")
          stats.ContainsArguments = true;
      }
      if (this._left is Super)
        this.memberScope = (codeContext & CodeContext.InStaticMember) != CodeContext.None ? PropertyScope.Super : PropertyScope.PrototypeOfSuperclass;
      return false;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString()
    {
      string str = this._left.ToString();
      int index = 0;
      return !(this._right is Constant) || (this._right as Constant).value.ToString().Length <= 0 || !Parser.ValidateName((this._right as Constant).value.ToString(), ref index, false, true, true) ? str + "[" + this._right?.ToString() + "]" : str + "." + (this._right as Constant).value?.ToString();
    }
  }
}
