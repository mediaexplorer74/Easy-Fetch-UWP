// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Assignment
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using System;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public class Assignment : Expression
  {
    private Arguments _setterArgs;
    private bool _saveResult;

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => false;

    protected internal override bool LValueModifier => true;

    public bool Force { get; internal set; }

    public Assignment(Expression left, Expression right)
      : base(left, right, false)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue forWrite = this._left.EvaluateForWrite(context);
      JSValue objectSource = context._objectSource;
      JSValue jsValue = this._right.Evaluate(context);
      if (forWrite._valueType == JSValueType.Property)
        return this.setProperty(context, objectSource, forWrite, jsValue);
      if ((forWrite._attributes & JSValueAttributesInternal.ReadOnly) != JSValueAttributesInternal.None)
      {
        if (this.Force)
        {
          JSValueAttributesInternal attributes = forWrite._attributes;
          forWrite._attributes = attributes & ~JSValueAttributesInternal.ReadOnly;
          forWrite.Assign(jsValue);
          forWrite._attributes = attributes;
        }
        else if (context._strict)
          this.throwReadOnlyError(context);
      }
      else
        forWrite.Assign(jsValue);
      return jsValue;
    }

    protected void throwReadOnlyError(Context context) => ExceptionHelper.ThrowTypeError(string.Format(Strings.CannotAssignReadOnly, (object) this._left), (CodeNode) this, context);

    protected JSValue setProperty(
      Context context,
      JSValue fieldSource,
      JSValue field,
      JSValue value)
    {
      lock (this)
      {
        Arguments arguments = this._setterArgs;
        this._setterArgs = (Arguments) null;
        if (arguments == null)
          arguments = new Arguments();
        JSValue jsValue = value;
        if (this._saveResult)
        {
          if (this._tempContainer == null)
            this._tempContainer = new JSValue();
          this._tempContainer.Assign(value);
          jsValue = this._tempContainer;
          this._tempContainer = (JSValue) null;
        }
        arguments.Reset();
        arguments.Add(jsValue);
        Function setter = (field._oValue as NiL.JS.Core.PropertyPair).setter;
        if (setter != null)
          setter.Call(fieldSource, arguments);
        else if (context._strict)
          this.throwReadOnlyError(context);
        if (this._saveResult)
          this._tempContainer = jsValue;
        this._setterArgs = arguments;
        return jsValue;
      }
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
      base.Build(ref _this, expressionDepth, variables, codeContext, message, stats, opts);
      if (!(this._left is VariableReference variableReference1))
        variableReference1 = this._left is AssignmentOperatorCache ? (this._left as AssignmentOperatorCache).Source as VariableReference : (VariableReference) null;
      VariableReference variableReference2 = variableReference1;
      if (variableReference2 != null)
      {
        List<Expression> expressionList = variableReference2.Descriptor.assignments ?? (variableReference2.Descriptor.assignments = new List<Expression>());
        if (expressionList.IndexOf((Expression) this) == -1)
          expressionList.Add((Expression) this);
        if (this._right is Constant)
        {
          PredictedType resultType = this._right.ResultType;
          if (variableReference2._descriptor.lastPredictedType == PredictedType.Unknown)
            variableReference2._descriptor.lastPredictedType = resultType;
        }
      }
      if (this._left is Property left)
      {
        ref CodeNode local = ref _this;
        SetProperty setProperty = new SetProperty(left._left, left._right, this._right);
        setProperty.Position = this.Position;
        setProperty.Length = this.Length;
        local = (CodeNode) setProperty;
      }
      if ((codeContext & (CodeContext.InEval | CodeContext.InExpression)) != CodeContext.None)
        this._saveResult = true;
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      this.baseOptimize(ref _this, owner, message, opts, stats);
      if (this._left is VariableReference left1)
      {
        if (left1._descriptor.IsDefined)
        {
          PredictedType resultType = this._right.ResultType;
          if (left1._descriptor.lastPredictedType == PredictedType.Unknown)
            left1._descriptor.lastPredictedType = resultType;
          else if (left1._descriptor.lastPredictedType != resultType)
          {
            if (Tools.IsEqual((Enum) left1._descriptor.lastPredictedType, (Enum) resultType, (Enum) PredictedType.Group))
            {
              left1._descriptor.lastPredictedType = resultType & PredictedType.Group;
            }
            else
            {
              if (message != null && resultType >= PredictedType.Undefined && left1._descriptor.lastPredictedType >= PredictedType.Undefined)
                message(MessageLevel.Warning, this.Position, this.Length, "Variable \"" + left1.Name + "\" has ambiguous type. It can be make impossible some optimizations and cause errors.");
              left1._descriptor.lastPredictedType = PredictedType.Ambiguous;
            }
          }
        }
        else if (message != null)
          message(MessageLevel.CriticalWarning, this.Position, this.Length, "Assign to undefined variable \"" + left1.Name + "\". It will declare a global variable.");
      }
      if (!(this._left is Variable left2) || !left2._descriptor.IsDefined || (this._codeContext & CodeContext.InWith) != CodeContext.None || left2._descriptor.captured || stats.ContainsEval || stats.ContainsWith || owner == null || (opts & Options.SuppressUselessExpressionsElimination) != Options.None || (this._codeContext & CodeContext.InLoop) != CodeContext.None || !owner._body._strict && left2._descriptor.owner == owner && owner._functionInfo.ContainsArguments)
        return;
      bool flag = true;
      for (int index = 0; flag && index < left2._descriptor.references.Count; ++index)
        flag = ((flag ? 1 : 0) & (left2._descriptor.references[index].Eliminated ? 1 : (left2._descriptor.references[index].Position <= this.Position ? 1 : 0))) != 0;
      if (!flag)
        return;
      if (this._right.ContextIndependent)
      {
        _this.Eliminated = true;
        _this = (CodeNode) Empty.Instance;
      }
      else
      {
        _this = (CodeNode) this._right;
        this._right = (Expression) null;
        this.Eliminated = true;
        this._right = _this as Expression;
      }
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString()
    {
      string str1 = this._left.ToString();
      if (str1[0] == '(')
        str1 = str1.Substring(1, str1.Length - 2);
      string str2 = this._right.ToString();
      if (str2[0] == '(')
        str2 = str2.Substring(1, str2.Length - 2);
      return "(" + str1 + " = " + str2 + ")";
    }
  }
}
