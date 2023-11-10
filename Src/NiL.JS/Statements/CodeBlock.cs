// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.CodeBlock
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NiL.JS.Statements
{
  public sealed class CodeBlock : CodeNode
  {
    internal static readonly VariableDescriptor[] emptyVariables = new VariableDescriptor[0];
    private string code;
    internal VariableDescriptor[] _variables;
    internal CodeNode[] _lines;
    internal bool _strict;
    internal bool built;
    internal SuppressScopeIsolationMode _suppressScopeIsolation;

    public VariableDescriptor[] Variables => this._variables;

    public CodeNode[] Body => this._lines;

    public bool Strict => this._strict;

    public string Code
    {
      get
      {
        string str = this.ToString();
        return str.Substring(1, str.Length - 2);
      }
    }

    public override int Length
    {
      get => base.Length >= 0 ? base.Length : -base.Length;
      internal set => base.Length = value;
    }

    public CodeBlock(CodeNode[] body)
    {
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      this.code = "";
      this._lines = body;
      this._variables = (VariableDescriptor[]) null;
      this._strict = false;
    }

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      int num1 = index;
      bool flag1 = num1 == 0 && state.AllowDirectives;
      if (!flag1)
      {
        if (state.Code[num1] != '{')
          throw new ArgumentException("code (" + num1.ToString() + ")");
        ++num1;
      }
      Tools.SkipSpaces(state.Code, ref num1);
      List<CodeNode> codeNodeList = new List<CodeNode>();
      int functionScopeLevel = state.FunctionScopeLevel;
      ++state.LexicalScopeLevel;
      if (state.AllowDirectives)
        state.FunctionScopeLevel = state.LexicalScopeLevel;
      int count1 = state.Variables.Count;
      VariableDescriptor[] variableDescriptorArray = (VariableDescriptor[]) null;
      state.LabelsCount = 0;
      bool allowDirectives = state.AllowDirectives;
      using (state.WithCodeContext())
      {
        state.CodeContext &= ~CodeContext.AllowDirectives;
        try
        {
          if (allowDirectives)
          {
            int num2 = num1;
            bool flag2 = false;
label_10:
            int num3;
            while (true)
            {
              num3 = num1;
              if (num1 < state.Code.Length)
              {
                if (Parser.ValidateValue(state.Code, ref num1))
                {
                  while (num1 < state.Code.Length && Tools.IsWhiteSpace(state.Code[num1]))
                    ++num1;
                  if (num1 >= state.Code.Length || !Parser.IsOperator(state.Code[num1]) && !Parser.Validate(state.Code, "instanceof", num1) && !Parser.Validate(state.Code, "in", num1))
                  {
                    int index1 = num3;
                    if (Parser.ValidateString(state.Code, ref index1, true))
                    {
                      if (state.Code.Substring(num3 + 1, index1 - num3 - 2) == "use strict")
                        state.CodeContext |= CodeContext.Strict;
                      flag2 = true;
                    }
                    else
                      goto label_20;
                  }
                  else
                    break;
                }
                else
                  goto label_21;
              }
              else
                goto label_23;
            }
            num1 = num3;
            goto label_23;
label_20:
            num1 = num3;
            goto label_23;
label_21:
            if (state.Code[num1] == ';' && flag2)
            {
              do
              {
                ++num1;
              }
              while (num1 < state.Code.Length && Tools.IsWhiteSpace(state.Code[num1]));
              goto label_10;
            }
label_23:
            num1 = num2;
          }
          int count2 = codeNodeList.Count;
          while (count2-- > 0)
            (codeNodeList[count2] as Constant).value._oValue = (object) Tools.Unescape((codeNodeList[count2] as Constant).value._oValue.ToString(), state.Strict);
          bool flag3 = false;
          while (true)
          {
            CodeNode codeNode;
            do
            {
              if (!flag1 || num1 >= state.Code.Length)
                goto label_39;
label_28:
              codeNode = Parser.Parse(state, ref num1, CodeFragmentType.Statement);
              if (codeNode == null)
                continue;
              goto label_37;
label_39:
              if (!flag1)
              {
                if (state.Code[num1] != '}')
                  goto label_28;
                else
                  goto label_44;
              }
              else
                goto label_44;
            }
            while (num1 >= state.Code.Length);
            if (flag1 && state.Code[num1] == '}')
              ExceptionHelper.Throw((Error) new SyntaxError("Unexpected symbol \"}\" at " + CodeCoordinates.FromTextPosition(state.Code, num1, 0)?.ToString()));
            if (state.Code[num1] == ';' || state.Code[num1] == ',')
            {
              if (state.Message != null && !flag3)
                state.Message(MessageLevel.Warning, num1, 1, "Unnecessary semicolon.");
              ++num1;
            }
            flag3 = false;
            continue;
label_37:
            flag3 = !(codeNode is EntityDefinition);
            codeNodeList.Add(codeNode);
          }
        }
        finally
        {
          if (count1 != state.Variables.Count)
            variableDescriptorArray = CodeBlock.extractVariables(state, count1);
          state.FunctionScopeLevel = functionScopeLevel;
          --state.LexicalScopeLevel;
        }
label_44:
        if (!flag1)
          ++num1;
        int num4 = index;
        index = num1;
        CodeBlock codeBlock = new CodeBlock(codeNodeList.ToArray());
        codeBlock._strict = state.Strict;
        codeBlock._variables = variableDescriptorArray ?? CodeBlock.emptyVariables;
        codeBlock.Position = num4;
        codeBlock.code = state.SourceCode;
        codeBlock.Length = num1 - num4;
        return (CodeNode) codeBlock;
      }
    }

    internal static VariableDescriptor[] extractVariables(ParseInfo state, int oldVariablesCount)
    {
      VariableDescriptor[] variables = CodeBlock.emptyVariables;
      int count = 0;
      for (int index = oldVariablesCount; index < state.Variables.Count; ++index)
      {
        if (state.Variables[index].definitionScopeLevel == state.LexicalScopeLevel)
          ++count;
      }
      if (count > 0)
      {
        variables = new VariableDescriptor[count];
        HashSet<string> stringSet = (HashSet<string>) null;
        if (state.LexicalScopeLevel != state.FunctionScopeLevel)
          stringSet = new HashSet<string>();
        int num = oldVariablesCount;
        int index = 0;
        for (; num < state.Variables.Count; ++num)
        {
          if (state.Variables[num].definitionScopeLevel == state.LexicalScopeLevel)
          {
            variables[index] = state.Variables[num];
            if (stringSet != null)
            {
              if (stringSet.Contains(variables[index].name) && variables[index].lexicalScope)
                ExceptionHelper.ThrowSyntaxError("Variable \"" + variables[index].name + "\" already has been defined", state.Code, num);
              stringSet.Add(variables[index].name);
            }
            ++index;
          }
          else if (index != 0)
            state.Variables[num - index] = state.Variables[num];
        }
        state.Variables.RemoveRange(state.Variables.Count - count, count);
      }
      return variables;
    }

    public override JSValue Evaluate(Context context)
    {
      int i = 0;
      bool clearSuspendData = false;
      if (context._executionMode >= ExecutionMode.Resume)
      {
        CodeBlock.SuspendData suspendData = context.SuspendData[(CodeNode) this] as CodeBlock.SuspendData;
        suspendData.Context._executionMode = context._executionMode;
        suspendData.Context._executionInfo = context._executionInfo;
        context = suspendData.Context;
        i = suspendData.LineIndex;
        clearSuspendData = true;
      }
      else
      {
        if (this._suppressScopeIsolation != SuppressScopeIsolationMode.Suppress)
          context = new Context(context, false, context._owner)
          {
            SuspendData = context.SuspendData,
            _definedVariables = this._variables,
            _thisBind = context._thisBind,
            _strict = context._strict,
            _executionInfo = context._executionInfo,
            _executionMode = context._executionMode
          };
        if (this._variables != null && this._variables.Length != 0)
          this.initVariables(context);
      }
      if (this._suppressScopeIsolation != SuppressScopeIsolationMode.Suppress)
        this.evaluateWithScope(context, i, clearSuspendData);
      else
        this.evaluateLines(context, i, clearSuspendData);
      return (JSValue) null;
    }

    private void evaluateWithScope(Context context, int i, bool clearSuspendData)
    {
      bool flag = this._suppressScopeIsolation != SuppressScopeIsolationMode.Suppress && context.Activate();
      try
      {
        this.evaluateLines(context, i, clearSuspendData);
      }
      finally
      {
        if (this._suppressScopeIsolation != SuppressScopeIsolationMode.Suppress)
        {
          if (flag)
            context.Deactivate();
          context._parent._lastResult = context._lastResult;
          context._parent._executionInfo = context._executionInfo;
          context._parent._executionMode = context._executionMode;
          if (this._variables.Length != 0)
            this.clearVariablesCache();
        }
      }
    }

    private void evaluateLines(Context context, int i, bool clearSuspendData)
    {
      for (CodeNode[] lines = this._lines; i < lines.Length; ++i)
      {
        if (context._debugging)
          context.raiseDebugger(this._lines[i]);
        JSValue jsValue = lines[i].Evaluate(context);
        if (jsValue != null)
          context._lastResult = jsValue;
        if (context._executionMode != ExecutionMode.Regular)
        {
          if (context._executionMode != ExecutionMode.Suspend)
            break;
          context.SuspendData[(CodeNode) this] = (object) new CodeBlock.SuspendData()
          {
            Context = context,
            LineIndex = i
          };
          break;
        }
        if (clearSuspendData)
          context.SuspendData.Clear();
      }
    }

    internal void clearVariablesCache()
    {
      for (int index = 0; index < this._variables.Length; ++index)
      {
        this._variables[index].cacheContext = (Context) null;
        this._variables[index].cacheRes = (JSValue) null;
      }
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
      List<CodeNode> codeNodeList = new List<CodeNode>();
      for (int index = 0; index < this._lines.Length; ++index)
      {
        CodeNode line = this._lines[index];
        if (line != null)
          codeNodeList.Add(line);
        else
          break;
      }
      if (this._variables != null)
        codeNodeList.AddRange((IEnumerable<CodeNode>) ((IEnumerable<VariableDescriptor>) this._variables).Where<VariableDescriptor>((Func<VariableDescriptor, bool>) (v =>
        {
          if (v.initializer == null)
            return false;
          return !(v.initializer is FunctionDefinition) || (v.initializer as FunctionDefinition)._body != this;
        })).Select<VariableDescriptor, NiL.JS.Expressions.Expression>((Func<VariableDescriptor, NiL.JS.Expressions.Expression>) (v => v.initializer)));
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
      if (this.built)
        return false;
      this.built = true;
      List<VariableDescriptor> variableDescriptorList = (List<VariableDescriptor>) null;
      if (this._variables != null && this._variables.Length != 0)
      {
        for (int index = 0; index < this._variables.Length; ++index)
        {
          VariableDescriptor variableDescriptor = (VariableDescriptor) null;
          if (variables.TryGetValue(this._variables[index].name, out variableDescriptor) && variableDescriptor.definitionScopeLevel < this._variables[index].definitionScopeLevel)
          {
            if (variableDescriptorList == null)
              variableDescriptorList = new List<VariableDescriptor>();
            variableDescriptorList.Add(variableDescriptor);
          }
          variables[this._variables[index].name] = this._variables[index];
          this._variables[index].owner = (CodeNode) this;
        }
        for (int index = 0; index < this._variables.Length; ++index)
          Parser.Build(ref this._variables[index].initializer, System.Math.Max(2, expressionDepth), variables, (codeContext | (this._strict ? CodeContext.Strict : CodeContext.None)) & ~CodeContext.InExpression, message, stats, opts);
      }
      int num1 = 0;
      for (int index = 0; index < this._lines.Length; ++index)
      {
        if (this._lines[index] is EntityDefinition line && line.Hoist)
          this._lines[index] = (CodeNode) null;
        else
          num1 = index;
      }
      bool flag = false;
      for (int index = 0; index < this._lines.Length; ++index)
      {
        if (this._lines[index] != null)
        {
          if (this._lines[index] is Empty)
          {
            this._lines[index] = (CodeNode) null;
          }
          else
          {
            if (flag && message != null)
              message(MessageLevel.CriticalWarning, this._lines[index].Position, this._lines[index].Length, "Unreachable code detected.");
            CodeNode line = this._lines[index];
            Parser.Build<CodeNode>(ref line, (codeContext & CodeContext.InEval) != CodeContext.None ? 2 : System.Math.Max(1, expressionDepth), variables, codeContext | (this._strict ? CodeContext.Strict : CodeContext.None), message, stats, opts | (flag || index != num1 ? Options.None : Options.SuppressUselessExpressionsElimination | Options.SuppressUselessStatementsElimination));
            this._lines[index] = !(line is Empty) ? line : (CodeNode) null;
            int num2 = flag ? 1 : 0;
            int num3;
            switch (line)
            {
              case Return _:
              case Break _:
              case Continue _:
                num3 = 1;
                break;
              default:
                num3 = line is Throw ? 1 : 0;
                break;
            }
            flag = (num2 | num3) != 0;
          }
        }
      }
      int length = this._lines.Length;
      int index1 = this._lines.Length - 1;
      while (length-- > 0)
      {
        if (this._lines[length] != null && this._lines[index1] == null)
        {
          this._lines[index1] = this._lines[length];
          this._lines[length] = (CodeNode) null;
        }
        if (this._lines[index1] != null)
          --index1;
      }
      if (expressionDepth > 0 && (this._variables == null || this._variables.Length == 0))
      {
        if (this._lines.Length == 0)
          _this = (CodeNode) Empty.Instance;
      }
      else if (message != null)
      {
        for (int index2 = 0; index2 < this._variables.Length && this._variables[index2].ReferenceCount == 1 && !(this._variables[index2].references[0] is ParameterReference); ++index2)
          message(MessageLevel.Recomendation, this._variables[index2].references[0].Position, 0, "Unused variable \"" + this._variables[index2].name + "\"");
      }
      if (index1 >= 0 && this == _this)
      {
        CodeNode[] codeNodeArray = new CodeNode[this._lines.Length - index1 - 1];
        int num4 = 0;
        while (++index1 < this._lines.Length)
          codeNodeArray[num4++] = this._lines[index1];
        this._lines = codeNodeArray;
      }
      if (this._variables != null && this._variables.Length != 0)
      {
        for (int index3 = 0; index3 < this._variables.Length; ++index3)
          variables.Remove(this._variables[index3].name);
      }
      if (variableDescriptorList != null)
      {
        for (int index4 = 0; index4 < variableDescriptorList.Count; ++index4)
          variables[variableDescriptorList[index4].name] = variableDescriptorList[index4];
      }
      return false;
    }

    internal void Optimize(
      ref CodeBlock self,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      CodeNode _this = (CodeNode) self;
      this.Optimize(ref _this, owner, message, opts, stats);
      self = (CodeBlock) _this;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      if (this._variables != null)
      {
        for (int index = 0; index < this._variables.Length; ++index)
        {
          if (this._variables[index].initializer != null)
          {
            CodeNode initializer = (CodeNode) this._variables[index].initializer;
            initializer.Optimize(ref initializer, owner, message, opts, stats);
          }
        }
      }
      for (int index = 0; index < this._lines.Length; ++index)
      {
        CodeNode line = this._lines[index];
        line.Optimize(ref line, owner, message, opts | (index == this._lines.Length - 1 ? Options.SuppressUselessExpressionsElimination | Options.SuppressUselessStatementsElimination : Options.None), stats);
        this._lines[index] = line;
      }
      if (this._variables != null)
      {
        for (int index = 0; index < this._variables.Length; ++index)
        {
          if (this._variables[index].initializer != null)
          {
            CodeNode initializer = (CodeNode) this._variables[index].initializer;
            initializer.Optimize(ref initializer, owner, message, opts, stats);
          }
        }
      }
      if (this._lines.Length != 1 || this._suppressScopeIsolation != SuppressScopeIsolationMode.Suppress || this._variables.Length != 0)
        return;
      _this = this._lines[0];
    }

    public override void Decompose(ref CodeNode self)
    {
      if (this._variables != null)
      {
        for (int index = 0; index < this._variables.Length; ++index)
        {
          if (this._variables[index].initializer != null)
            this._variables[index].initializer.Decompose(ref this._variables[index].initializer);
        }
      }
      for (int index = 0; index < this._lines.Length; ++index)
      {
        CodeNode line = this._lines[index];
        line.Decompose(ref line);
        this._lines[index] = line;
      }
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      if (this._variables != null)
      {
        VariableDescriptor[] variables = this._variables;
        if (this._variables.Length != 0 && !functionInfo.WithLexicalEnvironment)
        {
          for (int index = 0; index < this._variables.Length; ++index)
          {
            if (!transferedVariables.TryGetValue(this._variables[index].name, out VariableDescriptor _) || this._variables[index].initializer != null)
              transferedVariables[this._variables[index].name] = this._variables[index];
          }
          this._variables = CodeBlock.emptyVariables;
        }
        if (this._variables.Length == 0)
        {
          if (this._suppressScopeIsolation == SuppressScopeIsolationMode.Auto)
            this._suppressScopeIsolation = SuppressScopeIsolationMode.Suppress;
          --scopeBias;
        }
        for (int index = 0; index < variables.Length; ++index)
        {
          if (variables[index].definitionScopeLevel != -1)
          {
            variables[index].definitionScopeLevel -= variables[index].scopeBias;
            variables[index].definitionScopeLevel += scopeBias;
          }
          variables[index].scopeBias = scopeBias;
          variables[index].initializer?.RebuildScope(functionInfo, transferedVariables, scopeBias);
        }
      }
      else
        this._suppressScopeIsolation = SuppressScopeIsolationMode.Suppress;
      if (transferedVariables == null)
      {
        for (int index = 0; index < this._lines.Length; ++index)
          this._lines[index].RebuildScope(functionInfo, (Dictionary<string, VariableDescriptor>) null, scopeBias);
      }
      else
      {
        bool flag;
        do
        {
          flag = false;
          for (int index = 0; index < this._lines.Length; ++index)
          {
            int count = transferedVariables.Count;
            this._lines[index].RebuildScope(functionInfo, transferedVariables, scopeBias);
            if (transferedVariables.Count > count && index > 0)
              flag = true;
          }
        }
        while (flag);
      }
    }

    internal void initVariables(Context context)
    {
      FunctionInfo functionInfo = context._owner?._functionDefinition?._functionInfo;
      bool flag1 = functionInfo == null || functionInfo.ContainsEval || functionInfo.ContainsWith || functionInfo.NeedDecompose || functionInfo.ContainsDebugger;
      for (int index = 0; index < this._variables.Length; ++index)
      {
        VariableDescriptor variable = this._variables[index];
        if (variable.cacheContext != null)
        {
          if (variable.cacheContext._variables == null)
            variable.cacheContext._variables = JSObject.getFieldsContainer();
          variable.cacheContext._variables[variable.name] = variable.cacheRes;
        }
        if (!variable.lexicalScope)
        {
          bool flag2 = functionInfo != null && string.CompareOrdinal(variable.name, "arguments") == 0;
          if (!flag2 || variable.initializer != null)
          {
            JSValue jsValue = new JSValue()
            {
              _valueType = JSValueType.Undefined,
              _attributes = JSValueAttributesInternal.DoNotDelete
            };
            variable.cacheRes = jsValue;
            variable.cacheContext = context;
            if (((variable.definitionScopeLevel < 0 ? 1 : (variable.captured ? 1 : 0)) | (flag1 ? 1 : 0)) != 0)
              (context._variables ?? (context._variables = JSObject.getFieldsContainer()))[variable.name] = jsValue;
            if (variable.initializer != null)
              jsValue.Assign(variable.initializer.Evaluate(context));
            if (variable.isReadOnly)
              jsValue._attributes |= JSValueAttributesInternal.ReadOnly;
            if (flag2)
              context._arguments = jsValue;
          }
        }
      }
    }

    internal override System.Linq.Expressions.Expression TryCompile(
      bool selfCompile,
      bool forAssign,
      Type expectedType,
      List<CodeNode> dynamicValues)
    {
      int length = this._variables.Length;
      while (length-- > 0)
      {
        if (this._variables[length].initializer != null)
          this._variables[length].initializer.TryCompile(true, false, (Type) null, dynamicValues);
      }
      for (int index = 0; index < this._lines.Length; ++index)
        this._lines[index].TryCompile(true, false, (Type) null, dynamicValues);
      return (System.Linq.Expressions.Expression) null;
    }

    public override string ToString() => this.ToString(false);

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public string ToString(bool linewiseStringify)
    {
      if (linewiseStringify || string.IsNullOrEmpty(this.code))
      {
        if (this._lines == null || this._lines.Length == 0)
          return "{ }";
        StringBuilder stringBuilder = new StringBuilder().Append("{").Append(Environment.NewLine);
        string newLine = Environment.NewLine;
        string newValue = Environment.NewLine + "  ";
        for (int index = 0; index < this._lines.Length; ++index)
        {
          string str1 = this._lines[index].ToString();
          if (str1[0] == '(')
            str1 = str1.Substring(1, str1.Length - 2);
          string str2 = str1.Replace(newLine, newValue);
          stringBuilder.Append("  ").Append(str2).Append(str2[str2.Length - 1] != '}' ? ";" : "").Append(Environment.NewLine);
        }
        return stringBuilder.Append("}").ToString();
      }
      if (base.Length > 0)
      {
        this.Length = -base.Length;
        if (this.Position > 0)
          this.code = this.code.Substring(this.Position + 1, this.Length - 2);
      }
      return "{" + this.code + "}";
    }

    private sealed class SuspendData
    {
      public int LineIndex;
      public Context Context;
    }
  }
}
