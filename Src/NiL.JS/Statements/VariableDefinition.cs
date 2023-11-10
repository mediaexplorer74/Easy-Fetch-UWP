// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.VariableDefinition
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Expressions;
using System;
using System.Collections.Generic;

namespace NiL.JS.Statements
{
  public sealed class VariableDefinition : CodeNode
  {
    internal readonly VariableDescriptor[] _variables;
    internal Expression[] _initializers;

    public CodeNode[] Initializers => (CodeNode[]) this._initializers;

    public VariableDescriptor[] Variables => this._variables;

    public VariableKind Kind { get; private set; }

    internal VariableDefinition(
      VariableDescriptor[] variables,
      Expression[] initializers,
      VariableKind kind)
    {
      this._initializers = initializers;
      this._variables = variables;
      this.Kind = kind;
    }

    internal static CodeNode Parse(ParseInfo state, ref int index) => VariableDefinition.Parse(state, ref index, false);

    internal static CodeNode Parse(ParseInfo state, ref int index, bool forForLoop)
    {
      int num1 = index;
      Tools.SkipSpaces(state.Code, ref num1);
      VariableKind kind;
      if (Parser.Validate(state.Code, "var ", ref num1))
        kind = VariableKind.FunctionScope;
      else if (Parser.Validate(state.Code, "let ", ref num1))
      {
        kind = VariableKind.LexicalScope;
      }
      else
      {
        if (!Parser.Validate(state.Code, "const ", ref num1))
          return (CodeNode) null;
        kind = VariableKind.ConstantInLexicalScope;
      }
      int definitionScopeLevel = kind <= VariableKind.FunctionScope ? state.FunctionScopeLevel : state.LexicalScopeLevel;
      List<Expression> expressionList = new List<Expression>();
      List<string> stringList = new List<string>();
      int i = num1;
      while (state.Code[num1] != ';' && state.Code[num1] != '}' && !Tools.IsLineTerminator(state.Code[num1]))
      {
        Tools.SkipSpaces(state.Code, ref num1);
        if (state.Code[num1] != '[' && state.Code[num1] != '{' && !Parser.ValidateName(state.Code, num1, state.Strict))
        {
          if (Parser.ValidateName(state.Code, ref num1, false, true, state.Strict))
            ExceptionHelper.ThrowSyntaxError("\"" + Tools.Unescape(state.Code.Substring(i, num1 - i), state.Strict) + "\" is a reserved word, but used as a variable. " + CodeCoordinates.FromTextPosition(state.Code, i, num1 - i)?.ToString());
          ExceptionHelper.ThrowSyntaxError("Invalid variable definition at " + CodeCoordinates.FromTextPosition(state.Code, i, num1 - i)?.ToString());
        }
        Expression expression = ExpressionTree.Parse(state, ref num1, processComma: false, forForLoop: forForLoop);
        if (expression is VariableReference)
        {
          string str = expression.ToString();
          if (state.Strict && (str == "arguments" || str == "eval"))
            ExceptionHelper.ThrowSyntaxError("Varible name cannot be \"arguments\" or \"eval\" in strict mode", state.Code, i, num1 - i);
          stringList.Add(str);
          expressionList.Add(expression);
        }
        else
        {
          bool flag1 = false;
          bool flag2;
          if (expression is ExpressionTree expressionTree)
          {
            if (expressionTree.Type == OperationType.None && expressionTree._right == null)
              expressionTree = expressionTree._left as ExpressionTree;
            flag2 = ((flag1 ? 1 : 0) | (expressionTree == null ? 0 : (expressionTree.Type == OperationType.Assignment ? 1 : 0))) != 0;
            if (flag2)
            {
              if (expressionTree._left is ObjectDesctructor)
              {
                IList<Variable> targetVariables = (expressionTree._left as ObjectDesctructor).GetTargetVariables();
                for (int index1 = 0; index1 < targetVariables.Count; ++index1)
                {
                  stringList.Add(targetVariables[index1].ToString());
                  expressionList.Add((Expression) targetVariables[index1]);
                }
                expressionList.Add((Expression) expressionTree);
              }
              else
              {
                stringList.Add(expressionTree._left.ToString());
                expressionList.Add(expression);
              }
            }
          }
          else
          {
            flag2 = expression is Constant constant && constant.value == JSValue.undefined;
            if (flag2)
            {
              expressionList.Add(expression);
              stringList.Add(constant.value.ToString());
            }
          }
          if (!flag2)
            ExceptionHelper.ThrowSyntaxError("Invalid variable initializer", state.Code, num1);
        }
        i = num1;
        if (num1 < state.Code.Length)
        {
          Tools.SkipSpaces(state.Code, ref i);
          if (i < state.Code.Length && state.Code[i] == ',')
          {
            num1 = i + 1;
            Tools.SkipSpaces(state.Code, ref num1);
          }
          else
            break;
        }
        else
          break;
      }
      if (stringList.Count == 0)
        throw new InvalidOperationException("code (" + num1.ToString() + ")");
      if (!forForLoop && num1 < state.Code.Length && state.Code[num1] == ';')
        ++num1;
      else
        num1 = i;
      VariableDescriptor[] variables = new VariableDescriptor[stringList.Count];
      int index2 = 0;
      int num2 = 0;
      for (; index2 < stringList.Count; ++index2)
      {
        bool flag = false;
        for (int index3 = 0; index3 < state.Variables.Count - index2 + num2; ++index3)
        {
          if (state.Variables[index3].name == stringList[index2] && state.Variables[index3].definitionScopeLevel >= definitionScopeLevel)
          {
            if (state.Variables[index3].lexicalScope || kind > VariableKind.FunctionScope)
              ExceptionHelper.ThrowSyntaxError(string.Format(Strings.IdentifierAlreadyDeclared, (object) stringList[index2]), state.Code, index);
            flag = true;
            variables[index2] = state.Variables[index3];
            ++num2;
            break;
          }
        }
        if (!flag)
        {
          variables[index2] = new VariableDescriptor(stringList[index2], definitionScopeLevel)
          {
            lexicalScope = kind > VariableKind.FunctionScope,
            isReadOnly = kind == VariableKind.ConstantInLexicalScope
          };
          state.Variables.Add(variables[index2]);
        }
      }
      int num3 = index;
      index = num1;
      VariableDefinition variableDefinition = new VariableDefinition(variables, expressionList.ToArray(), kind);
      variableDefinition.Position = num3;
      variableDefinition.Length = index - num3;
      return (CodeNode) variableDefinition;
    }

    public override JSValue Evaluate(Context context)
    {
      int index1 = 0;
      if (context._executionMode >= ExecutionMode.Resume)
        index1 = (int) context.SuspendData[(CodeNode) this];
      if (context._executionMode == ExecutionMode.Regular)
      {
        for (int index2 = 0; index2 < this._variables.Length; ++index2)
        {
          if (context._executionMode == ExecutionMode.Regular && this.Kind > VariableKind.FunctionScope && this._variables[index2].lexicalScope)
          {
            JSValue jsValue = context.DefineVariable(this._variables[index2].name);
            this._variables[index2].cacheRes = jsValue;
            this._variables[index2].cacheContext = context;
            if (this.Kind == VariableKind.ConstantInLexicalScope)
              jsValue._attributes |= JSValueAttributesInternal.ReadOnly;
          }
        }
      }
      for (; index1 < this._initializers.Length; ++index1)
      {
        this._initializers[index1].Evaluate(context);
        if (context._executionMode == ExecutionMode.Suspend)
        {
          context.SuspendData[(CodeNode) this] = (object) index1;
          return (JSValue) null;
        }
      }
      return JSValue.notExists;
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
      List<CodeNode> codeNodeList = new List<CodeNode>();
      codeNodeList.AddRange((IEnumerable<CodeNode>) this._initializers);
      codeNodeList.RemoveAll((Predicate<CodeNode>) (x => x == null));
      return codeNodeList.ToArray();
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
      if (this.Kind > VariableKind.FunctionScope)
        stats.WithLexicalEnvironment = true;
      int length = 0;
      for (int index = 0; index < this._initializers.Length; ++index)
      {
        Parser.Build(ref this._initializers[index], message != null ? 2 : expressionDepth, variables, codeContext, message, stats, opts);
        if (this._initializers[index] != null)
        {
          ++length;
          if (this.Kind == VariableKind.ConstantInLexicalScope && this._initializers[index] is Assignment initializer)
          {
            initializer.Force = true;
            if (initializer.LeftOperand is ObjectDesctructor leftOperand)
              leftOperand.Force = true;
          }
        }
      }
      if (length < this._initializers.Length)
      {
        if ((opts & Options.SuppressUselessStatementsElimination) == Options.None && length == 0)
        {
          _this = (CodeNode) null;
          this.Eliminated = true;
          return false;
        }
        Expression[] expressionArray = new Expression[length];
        int index = 0;
        int num = 0;
        for (; index < this._initializers.Length; ++index)
        {
          if (this._initializers[index] != null)
            expressionArray[num++] = this._initializers[index];
        }
        this._initializers = expressionArray;
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
      for (int index = 0; index < this._initializers.Length; ++index)
        this._initializers[index].Optimize(ref this._initializers[index], owner, message, opts, stats);
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString()
    {
      if (this.Kind == VariableKind.AutoGeneratedParameters)
        return "";
      string str1 = this.Kind != VariableKind.ConstantInLexicalScope ? (this.Kind != VariableKind.LexicalScope ? "var " : "let ") : "const ";
      for (int index = 0; index < this._initializers.Length; ++index)
      {
        string str2 = this._initializers[index].ToString();
        if (!string.IsNullOrEmpty(str2))
        {
          if (str2[0] == '(')
            str2 = str2.Substring(1, str2.Length - 2);
          if (index > 0)
            str1 += ", ";
          str1 += str2;
        }
      }
      return str1;
    }

    public override void Decompose(ref CodeNode self)
    {
      for (int index = 0; index < this._initializers.Length; ++index)
        this._initializers[index].Decompose(ref this._initializers[index]);
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      for (int index = 0; index < this._initializers.Length; ++index)
        this._initializers[index].RebuildScope(functionInfo, transferedVariables, scopeBias);
    }
  }
}
