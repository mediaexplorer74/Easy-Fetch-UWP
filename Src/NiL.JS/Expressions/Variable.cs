// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Variable
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public class Variable : VariableReference
  {
    internal string _variableName;
    internal bool _suspendThrow;
    internal bool _forceThrow;

    public override string Name => this._variableName;

    protected internal override bool ContextIndependent => false;

    internal Variable(string name, int scopeLevel, bool reserveControl = true, bool allowEscape = true)
    {
      if (!Parser.ValidateName(name, 0, reserveControl, allowEscape, false))
        throw new ArgumentException("Invalid variable name");
      this.ScopeLevel = scopeLevel;
      this._variableName = name;
    }

    protected internal override JSValue EvaluateForWrite(Context context)
    {
      JSValue forWrite = this._descriptor.Get(context, true, this._scopeLevel);
      if (context._strict || this._forceThrow)
      {
        if (forWrite._valueType < JSValueType.Undefined && (!this._suspendThrow || this._forceThrow))
        {
          if ((this._codeContext & CodeContext.InEval) != CodeContext.None)
            ExceptionHelper.ThrowVariableIsNotDefined(this._variableName, (CodeNode) this);
          else
            ExceptionHelper.ThrowVariableIsNotDefined(this._variableName, context, (CodeNode) this);
        }
        if (context._strict && (forWrite._attributes & JSValueAttributesInternal.Argument) != JSValueAttributesInternal.None)
          context._owner.BuildArgumentsObject();
      }
      return forWrite;
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue property = this._descriptor.Get(context, false, this._scopeLevel);
      switch (property._valueType)
      {
        case JSValueType.NotExists:
          if (!this._suspendThrow)
          {
            if ((this._codeContext & CodeContext.InEval) != CodeContext.None)
            {
              ExceptionHelper.ThrowVariableIsNotDefined(this._variableName, (CodeNode) this);
              break;
            }
            ExceptionHelper.ThrowVariableIsNotDefined(this._variableName, context, (CodeNode) this);
            break;
          }
          break;
        case JSValueType.Property:
          return Tools.InvokeGetter(property, context._objectSource);
      }
      return property;
    }

    protected internal override CodeNode[] GetChildrenImpl() => (CodeNode[]) null;

    public override string ToString() => this._variableName;

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

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
      VariableDescriptor variableDescriptor = (VariableDescriptor) null;
      if (!variables.TryGetValue(this._variableName, out variableDescriptor) || variableDescriptor == null)
      {
        variableDescriptor = new VariableDescriptor((VariableReference) this, 1)
        {
          isDefined = false
        };
        variables[this._variableName] = this.Descriptor;
      }
      else
      {
        if (!variableDescriptor.references.Contains((VariableReference) this))
          variableDescriptor.references.Add((VariableReference) this);
        this._descriptor = variableDescriptor;
      }
      if (this._variableName == "this")
      {
        stats.ContainsThis = true;
        variableDescriptor.definitionScopeLevel = -1;
      }
      else if ((codeContext & CodeContext.InWith) != CodeContext.None || stats.ContainsEval && !variableDescriptor.isDefined)
      {
        this.ScopeLevel = -Math.Abs(this.ScopeLevel);
        variableDescriptor.definitionScopeLevel = -Math.Abs(variableDescriptor.definitionScopeLevel);
      }
      this._forceThrow |= variableDescriptor.lexicalScope;
      if (expressionDepth >= 0 && expressionDepth < 2 && variableDescriptor.IsDefined && !variableDescriptor.lexicalScope && (opts & Options.SuppressUselessExpressionsElimination) == Options.None)
      {
        _this = (CodeNode) null;
        this.Eliminated = true;
        if (message != null)
          message(MessageLevel.Warning, this.Position, this.Length, "Unused getting of defined variable was removed. Maybe something missing.");
      }
      else if (this._variableName == "arguments" && (codeContext & CodeContext.InFunction) != CodeContext.None)
      {
        if (stats != null)
          stats.ContainsArguments = true;
        ref CodeNode local = ref _this;
        GetArgumentsExpression argumentsExpression = new GetArgumentsExpression(this.ScopeLevel);
        argumentsExpression._descriptor = this._descriptor;
        local = (CodeNode) argumentsExpression;
      }
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      if ((opts & Options.SuppressConstantPropogation) != Options.None || this._descriptor.captured || !this._descriptor.isDefined || stats.ContainsWith || stats.ContainsEval || this._descriptor.owner == owner && owner._functionInfo.ContainsArguments)
        return;
      List<Expression> assignments = this._descriptor.assignments;
      if (assignments == null || assignments.Count <= 0)
        return;
      CodeNode codeNode = (CodeNode) null;
      int count = assignments.Count;
      while (count-- > 0)
      {
        if (assignments[count]._left == this || assignments[count]._left is AssignmentOperatorCache && assignments[count]._left._left == this)
        {
          codeNode = (CodeNode) null;
          break;
        }
        if (assignments[count].Position > this.Position)
        {
          if ((this._codeContext & CodeContext.InLoop) != CodeContext.None && (assignments[count]._codeContext & CodeContext.InLoop) != CodeContext.None)
          {
            codeNode = (CodeNode) null;
            break;
          }
        }
        else
        {
          if (this._descriptor.isReadOnly && assignments[count] is Assignment assignment && assignment.Force)
          {
            codeNode = (CodeNode) assignments[count];
            break;
          }
          if (codeNode == null || assignments[count].Position > codeNode.Position)
            codeNode = (CodeNode) assignments[count];
        }
      }
      if (!(codeNode is Assignment assignment1) || (assignment1._codeContext & CodeContext.Conditional) != CodeContext.None || !(assignment1._right is Constant))
        return;
      _this = (CodeNode) assignment1._right;
    }
  }
}
