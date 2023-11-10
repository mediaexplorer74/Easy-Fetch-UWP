// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.StoreValue
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Expressions;
using System.Collections.Generic;

namespace NiL.JS.Statements
{
  public sealed class StoreValue : CodeNode
  {
    private readonly Expression _source;
    private readonly bool _forWrite;

    public override int Position
    {
      get => this._source.Position;
      internal set => this._source.Position = value;
    }

    public override int Length
    {
      get => this._source.Length;
      internal set => this._source.Length = value;
    }

    public bool ForWrite => this._forWrite;

    public StoreValue(Expression source, bool forWrite)
    {
      this._source = source;
      this._forWrite = forWrite;
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue = this._forWrite ? this._source.EvaluateForWrite(context) : this._source.Evaluate(context);
      if (context._executionMode == ExecutionMode.Suspend)
        return (JSValue) null;
      context.SuspendData[(CodeNode) this._source] = this._forWrite ? (object) jsValue : (object) jsValue.CloneImpl(false);
      return (JSValue) null;
    }

    public override string ToString() => this._source.ToString();

    protected internal override CodeNode[] GetChildrenImpl() => this._source.GetChildrenImpl();

    public override T Visit<T>(Visitor<T> visitor) => this._source.Visit<T>(visitor);

    public override void Decompose(ref CodeNode self)
    {
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      this._source.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }
  }
}
