// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.SuspendableExpression
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;

namespace NiL.JS.Expressions
{
  public sealed class SuspendableExpression : Expression
  {
    private Expression _original;
    private CodeNode[] _parts;

    protected internal override bool ContextIndependent => false;

    internal SuspendableExpression(Expression prototype, CodeNode[] parts)
    {
      this._original = prototype;
      this._parts = parts;
    }

    public override JSValue Evaluate(Context context)
    {
      int index = 0;
      if (context._executionMode >= ExecutionMode.Resume)
        index = (int) context.SuspendData[(CodeNode) this];
      for (; index < this._parts.Length; ++index)
      {
        this._parts[index].Evaluate(context);
        if (context._executionMode == ExecutionMode.Suspend)
        {
          context.SuspendData[(CodeNode) this] = (object) index;
          return (JSValue) null;
        }
      }
      JSValue jsValue = this._original.Evaluate(context);
      if (context._executionMode != ExecutionMode.Suspend)
        return jsValue;
      context.SuspendData[(CodeNode) this] = (object) index;
      return (JSValue) null;
    }

    public override string ToString() => this._original.ToString();
  }
}
