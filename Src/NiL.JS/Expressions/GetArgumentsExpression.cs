// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.GetArgumentsExpression
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;

namespace NiL.JS.Expressions
{
  public sealed class GetArgumentsExpression : Variable
  {
    internal GetArgumentsExpression(int functionDepth)
      : base("arguments", functionDepth)
    {
    }

    protected internal override JSValue EvaluateForWrite(Context context)
    {
      if (context._owner._functionDefinition.kind == FunctionKind.Arrow)
        context = context._parent;
      if (context._arguments == null)
        context._owner.BuildArgumentsObject();
      JSValue forWrite = context._arguments;
      if (forWrite is Arguments)
        context._arguments = forWrite = forWrite.CloneImpl(false);
      if (context._variables != null && context._variables.ContainsKey(this.Name))
        context._variables[this.Name] = forWrite;
      return forWrite;
    }

    public override JSValue Evaluate(Context context)
    {
      if (context._owner._functionDefinition.kind == FunctionKind.Arrow)
        context = context._parent;
      if (context._arguments == null)
        context._owner.BuildArgumentsObject();
      return context._arguments;
    }
  }
}
