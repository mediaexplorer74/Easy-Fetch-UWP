// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.SetProperty
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NiL.JS.Expressions
{
  public sealed class SetProperty : Expression
  {
    private JSValue _propertyNameTempContainer;
    private JSValue _sourceTempContainer;
    private JSValue _cachedMemberName;
    private Expression _value;

    public Expression Source => this._left;

    public Expression FieldName => this._right;

    public Expression Value => this._value;

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => true;

    internal SetProperty(Expression obj, Expression fieldName, Expression value)
      : base(obj, fieldName, true)
    {
      if (fieldName is Constant)
        this._cachedMemberName = fieldName.Evaluate((Context) null);
      else
        this._propertyNameTempContainer = new JSValue();
      this._value = value;
      this._sourceTempContainer = new JSValue();
    }

    public override JSValue Evaluate(Context context)
    {
      lock (this)
      {
        JSValue tempContainer = this._tempContainer;
        JSValue nameTempContainer = this._propertyNameTempContainer;
        JSValue sourceTempContainer = this._sourceTempContainer;
        try
        {
          JSValue jsValue1 = this._left.Evaluate(context);
          JSValue jsValue2;
          if (jsValue1._valueType >= JSValueType.Object && jsValue1._oValue != null && jsValue1._oValue != jsValue1 && jsValue1._oValue is JSValue oValue && oValue._valueType >= JSValueType.Object)
          {
            jsValue2 = oValue;
          }
          else
          {
            if (this._sourceTempContainer == null)
              this._sourceTempContainer = new JSValue();
            this._sourceTempContainer.Assign(jsValue1);
            jsValue2 = this._sourceTempContainer;
            this._sourceTempContainer = (JSValue) null;
          }
          JSValue cachedMemberName = this._cachedMemberName;
          if (cachedMemberName == null)
          {
            if (this._propertyNameTempContainer == null)
              this._propertyNameTempContainer = new JSValue();
            cachedMemberName = SetProperty.safeGet(this._propertyNameTempContainer, (CodeNode) this._right, context);
            this._propertyNameTempContainer = (JSValue) null;
          }
          if (this._tempContainer == null)
            this._tempContainer = new JSValue();
          JSValue jsValue3 = SetProperty.safeGet(this._tempContainer, (CodeNode) this._value, context);
          this._tempContainer = (JSValue) null;
          jsValue2.SetProperty(cachedMemberName, jsValue3, context._strict);
          context._objectSource = (JSValue) null;
          return jsValue3;
        }
        finally
        {
          this._tempContainer = tempContainer;
          this._propertyNameTempContainer = nameTempContainer;
          this._sourceTempContainer = sourceTempContainer;
        }
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static JSValue safeGet(JSValue temp, CodeNode source, Context context)
    {
      temp.Assign(source.Evaluate(context));
      return temp;
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
      this._codeContext = codeContext;
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      CodeNode _this1 = (CodeNode) this._value;
      this._value.Optimize(ref _this1, owner, message, opts, stats);
      this._value = _this1 as Expression;
      base.Optimize(ref _this, owner, message, opts, stats);
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      base.RebuildScope(functionInfo, transferedVariables, scopeBias);
      this._value.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    protected internal override CodeNode[] GetChildrenImpl() => new CodeNode[3]
    {
      (CodeNode) this._left,
      (CodeNode) this._right,
      (CodeNode) this._value
    };

    public override string ToString()
    {
      string str = this._left.ToString();
      int index = 0;
      Constant right = this._right as Constant;
      return (!(this._right is Constant) || right.value.ToString().Length <= 0 || !Parser.ValidateName(right.value.ToString(), ref index, true) ? str + "[" + this._right?.ToString() + "]" : str + "." + right.value?.ToString()) + " = " + this._value?.ToString();
    }
  }
}
