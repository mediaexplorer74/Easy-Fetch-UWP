// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.ExtractStoredValue
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class ExtractStoredValue : Expression
  {
    protected internal override bool ContextIndependent => false;

    protected internal override bool NeedDecompose => false;

    public ExtractStoredValue(Expression source)
      : base(source, (Expression) null, false)
    {
    }

    protected internal override JSValue EvaluateForWrite(Context context) => this.Evaluate(context);

    public override JSValue Evaluate(Context context) => (JSValue) context.SuspendData[(CodeNode) this._left];

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

    public override string ToString() => this._left.ToString();

    public override void Decompose(ref Expression self, IList<CodeNode> result)
    {
    }
  }
}
