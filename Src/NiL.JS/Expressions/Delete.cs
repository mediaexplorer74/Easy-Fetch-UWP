// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Delete
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class Delete : Expression
  {
    protected internal override PredictedType ResultType => PredictedType.Bool;

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => false;

    public Delete(Expression first)
      : base(first, (Expression) null, false)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue = this._left.Evaluate(context);
      if (jsValue._valueType < JSValueType.Undefined)
        return (JSValue) true;
      if ((jsValue._attributes & JSValueAttributesInternal.Argument) != JSValueAttributesInternal.None)
        return (JSValue) false;
      if ((jsValue._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None)
      {
        if ((jsValue._attributes & JSValueAttributesInternal.SystemObject) == JSValueAttributesInternal.None)
        {
          jsValue._valueType = JSValueType.NotExists;
          jsValue._oValue = (object) null;
        }
        return (JSValue) true;
      }
      if (context._strict)
        ExceptionHelper.Throw((Error) new TypeError("Can not delete property \"" + this._left?.ToString() + "\"."));
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
      if (base.Build(ref _this, expressionDepth, variables, codeContext, message, stats, opts))
        return true;
      if (this._left is Variable)
      {
        if ((codeContext & CodeContext.Strict) != CodeContext.None)
          ExceptionHelper.Throw((Error) new SyntaxError("Can not delete variable in strict mode"));
        (this._left as Variable)._suspendThrow = true;
      }
      if (this._left is Property left)
      {
        _this = (CodeNode) new DeleteProperty(left._left, left._right);
        return false;
      }
      if (!(this._left is VariableReference variableReference1))
        variableReference1 = this._left is AssignmentOperatorCache ? (this._left as AssignmentOperatorCache).Source as VariableReference : (VariableReference) null;
      VariableReference variableReference2 = variableReference1;
      if (variableReference2 != null)
      {
        if (variableReference2.Descriptor.IsDefined && message != null)
          message(MessageLevel.Warning, this.Position, this.Length, "Tring to delete defined variable." + ((codeContext & CodeContext.Strict) != CodeContext.None ? " In strict mode it cause exception." : " It is not allowed"));
        (variableReference2.Descriptor.assignments ?? (variableReference2.Descriptor.assignments = new List<Expression>())).Add((Expression) this);
      }
      return false;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "delete " + this._left?.ToString();
  }
}
