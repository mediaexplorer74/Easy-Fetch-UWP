// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.FunctionDefinition
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Core.Functions;
using NiL.JS.Statements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NiL.JS.Expressions
{
  public sealed class FunctionDefinition : EntityDefinition
  {
    internal int parametersStored;
    internal int recursionDepth;
    internal readonly FunctionInfo _functionInfo;
    internal ParameterDescriptor[] parameters;
    internal CodeBlock _body;
    internal FunctionKind kind;

    public CodeBlock Body => this._body;

    public ReadOnlyCollection<ParameterDescriptor> Parameters => new ReadOnlyCollection<ParameterDescriptor>((IList<ParameterDescriptor>) this.parameters);

    protected internal override bool NeedDecompose => this._functionInfo.NeedDecompose;

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => false;

    protected internal override PredictedType ResultType => PredictedType.Function;

    public override bool Hoist => this.kind != FunctionKind.Arrow && this.kind != FunctionKind.AsyncArrow;

    public FunctionKind Kind => this.kind;

    public bool Strict
    {
      get
      {
        CodeBlock body = this._body;
        return body != null && body.Strict;
      }
      internal set
      {
        if (this._body == null)
          return;
        this._body._strict = value;
      }
    }

    private FunctionDefinition(string name)
      : base(name)
    {
      this._functionInfo = new FunctionInfo();
    }

    internal FunctionDefinition()
      : this("anonymous")
    {
      this.parameters = new ParameterDescriptor[0];
      this._body = new CodeBlock(new CodeNode[0])
      {
        _strict = true,
        _variables = new VariableDescriptor[0]
      };
    }

    internal static ParseDelegate ParseFunction(FunctionKind kind) => (ParseDelegate) ((ParseInfo info, ref int index) => (CodeNode) FunctionDefinition.Parse(info, ref index, kind));

    internal static CodeNode ParseFunction(ParseInfo state, ref int index) => (CodeNode) FunctionDefinition.Parse(state, ref index, FunctionKind.Function);

    internal static Expression Parse(ParseInfo state, ref int index, FunctionKind kind)
    {
      string code = state.Code;
      int num1 = index;
      switch (kind)
      {
        case FunctionKind.Function:
          if (!Parser.Validate(code, "function", ref num1))
            return (Expression) null;
          if (code[num1] == '*')
          {
            kind = FunctionKind.Generator;
            ++num1;
            goto case FunctionKind.AnonymousFunction;
          }
          else
          {
            if (code[num1] != '(' && !Tools.IsWhiteSpace(code[num1]))
              return (Expression) null;
            goto case FunctionKind.AnonymousFunction;
          }
        case FunctionKind.Getter:
          if (!Parser.Validate(code, "get ", ref num1))
            return (Expression) null;
          goto case FunctionKind.AnonymousFunction;
        case FunctionKind.Setter:
          if (!Parser.Validate(code, "set ", ref num1))
            return (Expression) null;
          goto case FunctionKind.AnonymousFunction;
        case FunctionKind.AnonymousFunction:
        case FunctionKind.AnonymousGenerator:
        case FunctionKind.Arrow:
        case FunctionKind.AsyncAnonymousFunction:
        case FunctionKind.AsyncMethod:
          Tools.SkipSpaces(state.Code, ref num1);
          List<ParameterDescriptor> parameterDescriptorList = new List<ParameterDescriptor>();
          CodeBlock codeBlock = (CodeBlock) null;
          string name = (string) null;
          bool flag1 = false;
          int num2 = 0;
          bool flag2 = false;
          if (kind != FunctionKind.Arrow)
          {
            if (code[num1] != '(')
            {
              num2 = num1;
              if (Parser.ValidateName(code, ref num1, false, true, state.Strict))
                name = Tools.Unescape(code.Substring(num2, num1 - num2), state.Strict);
              else if ((kind == FunctionKind.Getter || kind == FunctionKind.Setter) && Parser.ValidateString(code, ref num1, false))
                name = Tools.Unescape(code.Substring(num2 + 1, num1 - num2 - 2), state.Strict);
              else if ((kind == FunctionKind.Getter || kind == FunctionKind.Setter) && Parser.ValidateNumber(code, ref num1))
                name = Tools.Unescape(code.Substring(num2, num1 - num2), state.Strict);
              else
                ExceptionHelper.ThrowSyntaxError("Invalid function name", code, num2, num1 - num2);
              Tools.SkipSpaces(code, ref num1);
              if (code[num1] != '(')
                ExceptionHelper.ThrowUnknownToken(code, num1);
            }
            else
            {
              switch (kind)
              {
                case FunctionKind.Getter:
                case FunctionKind.Setter:
                  ExceptionHelper.ThrowSyntaxError("Getter and Setter must have name", code, index);
                  break;
                case FunctionKind.Method:
                case FunctionKind.MethodGenerator:
                case FunctionKind.AsyncMethod:
                  ExceptionHelper.ThrowSyntaxError("Method must have name", code, index);
                  break;
              }
            }
            ++num1;
          }
          else if (code[num1] != '(')
            flag1 = true;
          else
            ++num1;
          Tools.SkipSpaces(code, ref num1);
          if (code[num1] == ',')
            ExceptionHelper.ThrowSyntaxError(Strings.UnexpectedToken, code, num1);
          while (code[num1] != ')')
          {
            if (parameterDescriptorList.Count == (int) byte.MaxValue || kind == FunctionKind.Setter && parameterDescriptorList.Count == 1 || kind == FunctionKind.Getter)
              ExceptionHelper.ThrowSyntaxError(string.Format(Strings.TooManyArgumentsForFunction, (object) name), code, index);
            bool rest = Parser.Validate(code, "...", ref num1);
            Expression definition = (Expression) null;
            int startIndex = num1;
            if (!Parser.ValidateName(code, ref num1, state.Strict))
            {
              if (code[num1] == '{')
                definition = (Expression) ObjectDefinition.Parse(state, ref num1);
              else if (code[num1] == '[')
                definition = (Expression) ArrayDefinition.Parse(state, ref num1);
              if (definition == null)
                ExceptionHelper.ThrowUnknownToken(code, num2);
              flag2 = true;
            }
            ParameterReference parameterReference = new ParameterReference(Tools.Unescape(code.Substring(startIndex, num1 - startIndex), state.Strict), rest, state.LexicalScopeLevel + 1);
            parameterReference.Position = startIndex;
            parameterReference.Length = num1 - startIndex;
            ParameterDescriptor descriptor = parameterReference.Descriptor as ParameterDescriptor;
            if (definition != null)
              descriptor.Destructor = new ObjectDesctructor(definition);
            parameterDescriptorList.Add(descriptor);
            Tools.SkipSpaces(state.Code, ref num1);
            if (flag1)
            {
              --num1;
              break;
            }
            if (code[num1] == '=')
            {
              if (rest)
                ExceptionHelper.ThrowSyntaxError("Rest parameters can not have an initializer", code, num1);
              do
              {
                ++num1;
              }
              while (Tools.IsWhiteSpace(code[num1]));
              descriptor.initializer = ExpressionTree.Parse(state, ref num1, false, false, false, true, false);
            }
            if (code[num1] == ',')
            {
              if (rest)
                ExceptionHelper.ThrowSyntaxError("Rest parameters must be the last in parameters list", code, num1);
              do
              {
                ++num1;
              }
              while (Tools.IsWhiteSpace(code[num1]));
            }
          }
          if (kind == FunctionKind.Setter && parameterDescriptorList.Count != 1)
            ExceptionHelper.ThrowSyntaxError("Setter must has only one argument", code, index);
          ++num1;
          Tools.SkipSpaces(code, ref num1);
          if (kind == FunctionKind.Arrow || kind == FunctionKind.AsyncArrow)
          {
            if (!Parser.Validate(code, "=>", ref num1))
              ExceptionHelper.ThrowSyntaxError("Expected \"=>\"", code, num1);
            Tools.SkipSpaces(code, ref num1);
          }
          if (code[num1] != '{')
          {
            int functionScopeLevel = state.FunctionScopeLevel;
            state.FunctionScopeLevel = ++state.LexicalScopeLevel;
            try
            {
              if (kind == FunctionKind.Arrow || kind == FunctionKind.AsyncArrow)
              {
                codeBlock = new CodeBlock(new CodeNode[1]
                {
                  (CodeNode) new Return(ExpressionTree.Parse(state, ref num1, false, false, false, true, false))
                })
                {
                  _variables = new VariableDescriptor[0]
                };
                codeBlock.Position = codeBlock._lines[0].Position;
                codeBlock.Length = codeBlock._lines[0].Length;
              }
              else
                ExceptionHelper.ThrowUnknownToken(code, num1);
            }
            finally
            {
              state.FunctionScopeLevel = functionScopeLevel;
              --state.LexicalScopeLevel;
            }
          }
          else
          {
            using (state.WithNewLabelsScope())
            {
              using (state.WithCodeContext())
              {
                switch (kind)
                {
                  case FunctionKind.AnonymousGenerator:
                  case FunctionKind.Generator:
                  case FunctionKind.MethodGenerator:
                    state.CodeContext |= CodeContext.InGenerator;
                    break;
                  case FunctionKind.AsyncFunction:
                  case FunctionKind.AsyncAnonymousFunction:
                  case FunctionKind.AsyncArrow:
                  case FunctionKind.AsyncMethod:
                    state.CodeContext |= CodeContext.InAsync;
                    break;
                }
                state.CodeContext |= CodeContext.InFunction;
                state.CodeContext |= CodeContext.AllowDirectives;
                state.CodeContext &= ~(CodeContext.Conditional | CodeContext.InEval | CodeContext.InExpression);
                ++state.AllowReturn;
                try
                {
                  state.AllowBreak.Push(false);
                  state.AllowContinue.Push(false);
                  codeBlock = CodeBlock.Parse(state, ref num1) as CodeBlock;
                  if (flag2)
                  {
                    List<VariableDescriptor> variableDescriptorList = new List<VariableDescriptor>();
                    List<Expression> expressionList = new List<Expression>();
                    for (int index1 = 0; index1 < parameterDescriptorList.Count; ++index1)
                    {
                      if (parameterDescriptorList[index1].Destructor != null)
                      {
                        IList<Variable> targetVariables = parameterDescriptorList[index1].Destructor.GetTargetVariables();
                        for (int index2 = 0; index2 < targetVariables.Count; ++index2)
                          variableDescriptorList.Add(new VariableDescriptor(targetVariables[index2].Name, state.FunctionScopeLevel));
                        expressionList.Add((Expression) new Assignment((Expression) parameterDescriptorList[index1].Destructor, (Expression) parameterDescriptorList[index1].references[0]));
                      }
                    }
                    CodeNode[] destinationArray = new CodeNode[codeBlock._lines.Length + 1];
                    System.Array.Copy((System.Array) codeBlock._lines, 0, (System.Array) destinationArray, 1, codeBlock._lines.Length);
                    destinationArray[0] = (CodeNode) new VariableDefinition(variableDescriptorList.ToArray(), expressionList.ToArray(), VariableKind.AutoGeneratedParameters);
                    codeBlock._lines = destinationArray;
                  }
                }
                finally
                {
                  state.AllowBreak.Pop();
                  state.AllowContinue.Pop();
                  --state.AllowReturn;
                }
                if (kind == FunctionKind.Function)
                {
                  if (string.IsNullOrEmpty(name))
                    kind = FunctionKind.AnonymousFunction;
                }
              }
            }
          }
          if (codeBlock._strict || parameterDescriptorList.Count > 0 && parameterDescriptorList[parameterDescriptorList.Count - 1].IsRest || kind == FunctionKind.Arrow)
          {
            int count1 = parameterDescriptorList.Count;
            while (count1-- > 1)
            {
              int index3 = count1;
              while (index3-- > 0)
              {
                if (parameterDescriptorList[count1].Name == parameterDescriptorList[index3].Name)
                  ExceptionHelper.ThrowSyntaxError("Duplicate names of function parameters not allowed in strict mode", code, index);
              }
            }
            if (name == "arguments" || name == "eval")
              ExceptionHelper.ThrowSyntaxError("Functions name can not be \"arguments\" or \"eval\" in strict mode at", code, index);
            int count2 = parameterDescriptorList.Count;
            while (count2-- > 0)
            {
              if (parameterDescriptorList[count2].Name == "arguments" || parameterDescriptorList[count2].Name == "eval")
                ExceptionHelper.ThrowSyntaxError("Parameters name cannot be \"arguments\" or \"eval\" in strict mode at", code, parameterDescriptorList[count2].references[0].Position, parameterDescriptorList[count2].references[0].Length);
            }
          }
          FunctionDefinition functionDefinition = new FunctionDefinition(name);
          functionDefinition.parameters = parameterDescriptorList.ToArray();
          functionDefinition._body = codeBlock;
          functionDefinition.kind = kind;
          functionDefinition.Position = index;
          functionDefinition.Length = num1 - index;
          FunctionDefinition first = functionDefinition;
          if (!string.IsNullOrEmpty(name))
          {
            first.Reference.ScopeLevel = state.LexicalScopeLevel;
            first.Reference.Position = num2;
            first.Reference.Length = name.Length;
            first.reference._descriptor.definitionScopeLevel = first.reference.ScopeLevel;
          }
          if (parameterDescriptorList.Count != 0)
          {
            int length = codeBlock._variables.Length + parameterDescriptorList.Count;
            for (int index4 = 0; index4 < codeBlock._variables.Length; ++index4)
            {
              for (int index5 = 0; index5 < parameterDescriptorList.Count; ++index5)
              {
                if (codeBlock._variables[index4].name == parameterDescriptorList[index5].name)
                {
                  --length;
                  break;
                }
              }
            }
            VariableDescriptor[] variableDescriptorArray = new VariableDescriptor[length];
            for (int index6 = 0; index6 < parameterDescriptorList.Count; ++index6)
            {
              variableDescriptorArray[index6] = (VariableDescriptor) parameterDescriptorList[parameterDescriptorList.Count - index6 - 1];
              for (int index7 = 0; index7 < codeBlock._variables.Length; ++index7)
              {
                if (codeBlock._variables[index7] != null && codeBlock._variables[index7].name == parameterDescriptorList[index6].name)
                {
                  if (codeBlock._variables[index7].initializer != null)
                    variableDescriptorArray[index6] = codeBlock._variables[index7];
                  else
                    codeBlock._variables[index7].lexicalScope = false;
                  codeBlock._variables[index7] = (VariableDescriptor) null;
                  break;
                }
              }
            }
            int index8 = 0;
            int count = parameterDescriptorList.Count;
            for (; index8 < codeBlock._variables.Length; ++index8)
            {
              if (codeBlock._variables[index8] != null)
                variableDescriptorArray[count++] = codeBlock._variables[index8];
            }
            codeBlock._variables = variableDescriptorArray;
          }
          if ((state.CodeContext & CodeContext.InExpression) == CodeContext.None && kind == FunctionKind.Function)
          {
            int num3 = num1;
            while (num1 < code.Length && Tools.IsWhiteSpace(code[num1]) && !Tools.IsLineTerminator(code[num1]))
              ++num1;
            if (num1 < code.Length && code[num1] == '(')
            {
              List<Expression> expressionList = new List<Expression>();
              int index9 = num1 + 1;
              while (true)
              {
                while (Tools.IsWhiteSpace(code[index9]))
                  ++index9;
                if (code[index9] != ')')
                {
                  if (code[index9] == ',')
                  {
                    do
                    {
                      ++index9;
                    }
                    while (Tools.IsWhiteSpace(code[index9]));
                  }
                  expressionList.Add(ExpressionTree.Parse(state, ref index9, false, false, false, true, false));
                }
                else
                  break;
              }
              ++index9;
              index = index9;
              while (index9 < code.Length && Tools.IsWhiteSpace(code[index9]))
                ++index9;
              if (index9 < code.Length && code[index9] == ';')
                ExceptionHelper.Throw((Error) new SyntaxError("Expression can not start with word \"function\""));
              return (Expression) new Call((Expression) first, expressionList.ToArray());
            }
            num1 = num3;
          }
          if ((state.CodeContext & CodeContext.InExpression) == CodeContext.None && (kind != FunctionKind.Arrow || (state.CodeContext & CodeContext.InEval) == CodeContext.None) && ((state.CodeContext & CodeContext.InExport) == CodeContext.None || !string.IsNullOrEmpty(name)))
          {
            if (string.IsNullOrEmpty(name))
              ExceptionHelper.ThrowSyntaxError("Function must has name", state.Code, index);
            if (kind != FunctionKind.Arrow && kind != FunctionKind.Method)
              state.Variables.Add(first.reference._descriptor);
          }
          index = num1;
          return (Expression) first;
        case FunctionKind.Method:
        case FunctionKind.MethodGenerator:
          if (code[num1] == '*')
          {
            kind = FunctionKind.MethodGenerator;
            ++num1;
            goto case FunctionKind.AnonymousFunction;
          }
          else
          {
            if (kind == FunctionKind.MethodGenerator)
              throw new ArgumentException("mode");
            goto case FunctionKind.AnonymousFunction;
          }
        case FunctionKind.AsyncFunction:
          if (!Parser.Validate(code, "async", ref num1))
            return (Expression) null;
          Tools.SkipSpaces(code, ref num1);
          if (!Parser.Validate(code, "function", ref num1))
            return (Expression) null;
          goto case FunctionKind.AnonymousFunction;
        case FunctionKind.AsyncArrow:
          if (!Parser.Validate(code, "async", ref num1))
            return (Expression) null;
          goto case FunctionKind.AnonymousFunction;
        default:
          throw new NotImplementedException(kind.ToString());
      }
    }

    public override JSValue Evaluate(Context context) => (JSValue) this.MakeFunction(context);

    protected internal override CodeNode[] GetChildrenImpl()
    {
      CodeNode[] childrenImpl = new CodeNode[1 + this.parameters.Length + (this.Reference != null ? 1 : 0)];
      for (int index = 0; index < this.parameters.Length; ++index)
        childrenImpl[index] = (CodeNode) this.parameters[index].references[0];
      childrenImpl[this.parameters.Length] = (CodeNode) this._body;
      if (this.Reference != null)
        childrenImpl[childrenImpl.Length - 1] = (CodeNode) this.Reference;
      return childrenImpl;
    }

    public Function MakeFunction(Module script) => this.MakeFunction(script.Context);

    public Function MakeFunction(Context context)
    {
      if (this.kind == FunctionKind.Generator || this.kind == FunctionKind.MethodGenerator || this.kind == FunctionKind.AnonymousGenerator)
        return (Function) new GeneratorFunction(context, this);
      if (this.kind == FunctionKind.AsyncFunction || this.kind == FunctionKind.AsyncAnonymousFunction || this.kind == FunctionKind.AsyncArrow || this.kind == FunctionKind.AsyncMethod)
        return (Function) new AsyncFunction(context, this);
      if (this._body != null)
      {
        if (this._body._lines.Length == 0)
          return (Function) new ConstantFunction(JSValue.notExists, this);
        if (this._body._lines.Length == 1 && this._body._lines[0] is Return line && (line.Value == null || line.Value.ContextIndependent))
          return (Function) new ConstantFunction(line.Value?.Evaluate((Context) null) ?? JSValue.undefined, this);
      }
      return !this._functionInfo.ContainsArguments && !this._functionInfo.ContainsRestParameters && !this._functionInfo.ContainsEval && !this._functionInfo.ContainsWith && !this._functionInfo.ContainsDebugger ? (Function) new SimpleFunction(context, this) : new Function(context, this);
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
      if (this._body.built)
        return false;
      if (stats != null)
        stats.ContainsInnerEntities = true;
      this._codeContext = codeContext;
      if ((codeContext & CodeContext.InLoop) != CodeContext.None && message != null)
        message(MessageLevel.Warning, this.Position, this.EndPosition - this.Position, Strings.FunctionInLoop);
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      foreach (KeyValuePair<string, VariableDescriptor> variable in variables)
        dictionary[variable.Key] = variable.Value.references.Count;
      VariableDescriptor variableDescriptor = (VariableDescriptor) null;
      if (!string.IsNullOrEmpty(this._name) && (this.kind == FunctionKind.Function || this.kind == FunctionKind.Generator))
      {
        variables.TryGetValue(this._name, out variableDescriptor);
        variables[this._name] = this.reference._descriptor;
      }
      this._functionInfo.ContainsRestParameters = this.parameters.Length != 0 && this.parameters[this.parameters.Length - 1].IsRest;
      CodeNode body = (CodeNode) this._body;
      body.Build(ref body, 0, variables, codeContext & ~(CodeContext.Conditional | CodeContext.InEval | CodeContext.InExpression) | CodeContext.InFunction, message, this._functionInfo, opts);
      this._body = body as CodeBlock;
      if (message != null)
      {
        int length = this.parameters.Length;
        while (length-- > 0 && this.parameters[length].ReferenceCount == 1)
          message(MessageLevel.Recomendation, this.parameters[length].references[0].Position, 0, "Unused parameter \"" + this.parameters[length].name + "\"");
      }
      this._body._suppressScopeIsolation = SuppressScopeIsolationMode.Suppress;
      this.checkUsings();
      if (stats != null)
      {
        stats.ContainsDebugger |= this._functionInfo.ContainsDebugger;
        stats.ContainsEval |= this._functionInfo.ContainsEval;
        stats.ContainsInnerEntities = true;
        stats.ContainsTry |= this._functionInfo.ContainsTry;
        stats.ContainsWith |= this._functionInfo.ContainsWith;
        stats.NeedDecompose |= this._functionInfo.NeedDecompose;
        stats.UseCall |= this._functionInfo.UseCall;
        stats.UseGetMember |= this._functionInfo.UseGetMember;
        stats.ContainsThis |= this._functionInfo.ContainsThis;
      }
      if (variableDescriptor != null)
        variables[variableDescriptor.name] = variableDescriptor;
      else if (!string.IsNullOrEmpty(this._name) && (this.kind == FunctionKind.Function || this.kind == FunctionKind.Generator))
        variables.Remove(this._name);
      foreach (KeyValuePair<string, VariableDescriptor> variable in variables)
      {
        int num = 0;
        if (!dictionary.TryGetValue(variable.Key, out num) || num != variable.Value.references.Count)
        {
          variable.Value.captured = true;
          if ((codeContext & CodeContext.InWith) != CodeContext.None)
          {
            for (int index = num; index < variable.Value.references.Count; ++index)
              variable.Value.references[index].ScopeLevel = -System.Math.Abs(variable.Value.references[index].ScopeLevel);
          }
        }
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
      CodeNode body = (CodeNode) this._body;
      SuppressScopeIsolationMode suppressScopeIsolation = this._body._suppressScopeIsolation;
      this._body._suppressScopeIsolation = SuppressScopeIsolationMode.DoNotSuppress;
      this._body.Optimize(ref body, this, message, opts, this._functionInfo);
      this._body._suppressScopeIsolation = suppressScopeIsolation;
      if (this._functionInfo.Returns.Count > 0)
      {
        this._functionInfo.ResultType = this._functionInfo.Returns[0].ResultType;
        for (int index = 1; index < this._functionInfo.Returns.Count; ++index)
        {
          if (this._functionInfo.ResultType != this._functionInfo.Returns[index].ResultType)
          {
            this._functionInfo.ResultType = PredictedType.Ambiguous;
            if (message == null || this._functionInfo.ResultType < PredictedType.Undefined || this._functionInfo.Returns[index].ResultType < PredictedType.Undefined)
              break;
            message(MessageLevel.Warning, this.parameters[index].references[0].Position, 0, "Type of return value is ambiguous");
            break;
          }
        }
      }
      else
        this._functionInfo.ResultType = PredictedType.Undefined;
    }

    private void checkUsings()
    {
      if (this._body == null || this._body._lines == null || this._body._lines.Length == 0 || this._body._variables == null)
        return;
      bool containsInnerEntities = this._functionInfo.ContainsInnerEntities;
      if (!containsInnerEntities)
      {
        for (int index = 0; !containsInnerEntities && index < this._body._variables.Length; ++index)
          containsInnerEntities |= this._body._variables[index].initializer != null;
        this._functionInfo.ContainsInnerEntities = containsInnerEntities;
      }
      for (int index = 0; index < this._body._variables.Length; ++index)
        this._functionInfo.ContainsArguments |= this._body._variables[index].name == "arguments";
    }

    internal override System.Linq.Expressions.Expression TryCompile(
      bool selfCompile,
      bool forAssign,
      Type expectedType,
      List<CodeNode> dynamicValues)
    {
      this._body.TryCompile(true, false, (Type) null, new List<CodeNode>());
      return (System.Linq.Expressions.Expression) null;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override void Decompose(ref Expression self, IList<CodeNode> result)
    {
      CodeNode body = (CodeNode) this._body;
      body.Decompose(ref body);
      this._body = (CodeBlock) body;
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      base.RebuildScope(functionInfo, (Dictionary<string, VariableDescriptor>) null, scopeBias);
      Dictionary<string, VariableDescriptor> transferedVariables1 = this._functionInfo.WithLexicalEnvironment ? (Dictionary<string, VariableDescriptor>) null : new Dictionary<string, VariableDescriptor>();
      this._body.RebuildScope(this._functionInfo, transferedVariables1, scopeBias + (this._body._variables == null || this._body._variables.Length == 0 || !this._functionInfo.WithLexicalEnvironment ? 1 : 0));
      if (transferedVariables1 == null)
        return;
      CodeBlock body = this._body;
      if (body == null)
        return;
      body._variables = transferedVariables1.Values.Where<VariableDescriptor>((Func<VariableDescriptor, bool>) (x => !(x is ParameterDescriptor))).ToArray<VariableDescriptor>();
    }

    public override string ToString() => this.ToString(false);

    internal string ToString(bool headerOnly)
    {
      StringBuilder stringBuilder = new StringBuilder();
      switch (this.kind)
      {
        case FunctionKind.Getter:
          stringBuilder.Append("get ");
          goto case FunctionKind.Method;
        case FunctionKind.Setter:
          stringBuilder.Append("set ");
          goto case FunctionKind.Method;
        case FunctionKind.Generator:
          stringBuilder.Append("functions* ");
          goto case FunctionKind.Method;
        case FunctionKind.Method:
        case FunctionKind.Arrow:
          stringBuilder.Append(this._name).Append("(");
          if (this.parameters != null)
          {
            int index = 0;
            while (index < this.parameters.Length)
              stringBuilder.Append((object) this.parameters[index]).Append(++index < this.parameters.Length ? "," : "");
          }
          stringBuilder.Append(")");
          if (!headerOnly)
          {
            stringBuilder.Append(" ");
            if (this.kind == FunctionKind.Arrow)
              stringBuilder.Append("=> ");
            if (this.kind == FunctionKind.Arrow && this._body._lines.Length == 1 && this._body.Position == this._body._lines[0].Position)
              stringBuilder.Append(this._body._lines[0].Children[0].ToString());
            else
              stringBuilder.Append((object) this._body ?? (object) "{ [native code] }");
          }
          return stringBuilder.ToString();
        case FunctionKind.AsyncFunction:
          stringBuilder.Append("async ");
          goto default;
        case FunctionKind.AsyncMethod:
          stringBuilder.Append("async ");
          goto case FunctionKind.Method;
        default:
          stringBuilder.Append("function ");
          goto case FunctionKind.Method;
      }
    }
  }
}
