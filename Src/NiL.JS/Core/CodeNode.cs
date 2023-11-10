// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.CodeNode
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Expressions;
using System;
using System.Collections.Generic;

namespace NiL.JS.Core
{
  public abstract class CodeNode
  {
    private static readonly CodeNode[] emptyCodeNodeArray = new CodeNode[0];
    private CodeNode[] children;

    public virtual bool Eliminated { get; internal set; }

    public virtual int Position { get; internal set; }

    public virtual int Length { get; internal set; }

    public int EndPosition => this.Position + this.Length;

    public CodeNode[] Children => this.children ?? (this.children = this.GetChildrenImpl() ?? CodeNode.emptyCodeNodeArray);

    protected internal virtual CodeNode[] GetChildrenImpl() => new CodeNode[0];

    protected internal virtual JSValue EvaluateForWrite(Context context)
    {
      ExceptionHelper.ThrowReferenceError(Strings.InvalidLefthandSideInAssignment);
      return (JSValue) null;
    }

    public abstract JSValue Evaluate(Context context);

    public virtual bool Build(
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

    public virtual void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
    }

    internal virtual System.Linq.Expressions.Expression TryCompile(
      bool selfCompile,
      bool forAssign,
      Type expectedType,
      List<CodeNode> dynamicValues)
    {
      return (System.Linq.Expressions.Expression) null;
    }

    public abstract void Decompose(ref CodeNode self);

    public abstract void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias);

    public virtual T Visit<T>(Visitor<T> visitor) => default (T);
  }
}
