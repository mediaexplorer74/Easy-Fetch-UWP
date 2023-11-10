// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Empty
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class Empty : Expression
  {
    private static readonly Empty _instance = new Empty();

    public static Empty Instance => Empty._instance;

    internal override bool ResultInTempContainer => false;

    protected internal override PredictedType ResultType => PredictedType.Undefined;

    public Empty()
      : base((Expression) null, (Expression) null, false)
    {
    }

    public Empty(int position)
      : base((Expression) null, (Expression) null, false)
    {
      this.Position = position;
      this.Length = 0;
    }

    public override JSValue Evaluate(Context context) => (JSValue) null;

    protected internal override CodeNode[] GetChildrenImpl() => (CodeNode[]) null;

    public override bool Build(
      ref CodeNode _this,
      int expressionDepth,
      Dictionary<string, VariableDescriptor> variables,
      CodeContext codeContext,
      InternalCompilerMessageCallback message,
      FunctionInfo stats,
      Options opts)
    {
      if (expressionDepth < 2)
      {
        _this = (CodeNode) null;
        this.Eliminated = true;
      }
      return false;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "";
  }
}
