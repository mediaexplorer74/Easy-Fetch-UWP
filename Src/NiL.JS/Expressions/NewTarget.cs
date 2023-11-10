// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.NewTarget
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class NewTarget : Expression
  {
    protected internal override bool ContextIndependent => false;

    protected internal override bool NeedDecompose => false;

    protected internal override bool LValueModifier => false;

    public override JSValue Evaluate(Context context)
    {
      if (context._thisBind == null || (context._thisBind._attributes & JSValueAttributesInternal.ConstructingObject) == JSValueAttributesInternal.None)
        return JSValue.undefined;
      List<Context> currectContextStack = Context.GetCurrectContextStack();
      for (int index = 2; currectContextStack.Count >= index && currectContextStack[currectContextStack.Count - index]._thisBind == context._thisBind; ++index)
        context = currectContextStack[currectContextStack.Count - index];
      return (JSValue) context._owner;
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

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit((Expression) this);

    public override string ToString() => "new.target";
  }
}
