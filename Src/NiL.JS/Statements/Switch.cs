// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.Switch
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NiL.JS.Statements
{
  public sealed class Switch : CodeNode
  {
    private FunctionDefinition[] functions;
    private CodeNode[] lines;
    private SwitchCase[] cases;
    private CodeNode image;

    public FunctionDefinition[] Functions => this.functions;

    public CodeNode[] Body => this.lines;

    public SwitchCase[] Cases => this.cases;

    public CodeNode Image => this.image;

    internal Switch(CodeNode[] body) => this.lines = body;

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      int index1 = index;
      if (!Parser.Validate(state.Code, "switch (", ref index1) && !Parser.Validate(state.Code, "switch(", ref index1))
        return (CodeNode) null;
      while (Tools.IsWhiteSpace(state.Code[index1]))
        ++index1;
      List<CodeNode> codeNodeList = new List<CodeNode>();
      List<FunctionDefinition> functionDefinitionList = new List<FunctionDefinition>();
      List<SwitchCase> switchCaseList = new List<SwitchCase>();
      CodeNode codeNode1 = (CodeNode) null;
      switchCaseList.Add(new SwitchCase()
      {
        index = int.MaxValue
      });
      state.AllowBreak.Push(true);
      int count = state.Variables.Count;
      VariableDescriptor[] variableDescriptorArray = (VariableDescriptor[]) null;
      ++state.LexicalScopeLevel;
      try
      {
        CodeNode codeNode2 = ExpressionTree.Parse(state, ref index1);
        if (state.Code[index1] != ')')
          ExceptionHelper.Throw((Error) new SyntaxError("Expected \")\" at + " + CodeCoordinates.FromTextPosition(state.Code, index1, 0)?.ToString()));
        do
        {
          ++index1;
        }
        while (Tools.IsWhiteSpace(state.Code[index1]));
        if (state.Code[index1] != '{')
          ExceptionHelper.Throw((Error) new SyntaxError("Expected \"{\" at + " + CodeCoordinates.FromTextPosition(state.Code, index1, 0)?.ToString()));
        do
        {
          ++index1;
        }
        while (Tools.IsWhiteSpace(state.Code[index1]));
        while (state.Code[index1] != '}')
        {
label_11:
          if (Parser.Validate(state.Code, "case", index1) && Parser.IsIdentifierTerminator(state.Code[index1 + 4]))
          {
            index1 += 4;
            while (Tools.IsWhiteSpace(state.Code[index1]))
              ++index1;
            CodeNode codeNode3 = ExpressionTree.Parse(state, ref index1);
            if (state.Code[index1] != ':')
              ExceptionHelper.Throw((Error) new SyntaxError("Expected \":\" at + " + CodeCoordinates.FromTextPosition(state.Code, index1, 0)?.ToString()));
            ++index1;
            switchCaseList.Add(new SwitchCase()
            {
              index = codeNodeList.Count,
              statement = codeNode3
            });
          }
          else if (Parser.Validate(state.Code, "default", index1) && Parser.IsIdentifierTerminator(state.Code[index1 + 7]))
          {
            index1 += 7;
            while (Tools.IsWhiteSpace(state.Code[index1]))
              ++index1;
            if (switchCaseList[0].index != int.MaxValue)
              ExceptionHelper.Throw((Error) new SyntaxError("Duplicate default case in switch at " + CodeCoordinates.FromTextPosition(state.Code, index1, 0)?.ToString()));
            if (state.Code[index1] != ':')
              ExceptionHelper.Throw((Error) new SyntaxError("Expected \":\" at + " + CodeCoordinates.FromTextPosition(state.Code, index1, 0)?.ToString()));
            ++index1;
            switchCaseList[0].index = codeNodeList.Count;
          }
          else
          {
            if (switchCaseList.Count == 1 && switchCaseList[0].index == int.MaxValue)
              ExceptionHelper.Throw((Error) new SyntaxError("Switch statement must contain cases. " + CodeCoordinates.FromTextPosition(state.Code, index, 0)?.ToString()));
            CodeNode codeNode4 = Parser.Parse(state, ref index1, CodeFragmentType.Statement);
            if (codeNode4 != null)
            {
              codeNodeList.Add(codeNode4);
              while (Tools.IsWhiteSpace(state.Code[index1]) || state.Code[index1] == ';')
                ++index1;
              continue;
            }
            continue;
          }
          while (Tools.IsWhiteSpace(state.Code[index1]) || state.Code[index1] == ';')
            ++index1;
          goto label_11;
        }
        state.AllowBreak.Pop();
        ++index1;
        int num = index;
        index = index1;
        Switch @switch = new Switch(codeNodeList.ToArray());
        @switch.functions = functionDefinitionList.ToArray();
        @switch.cases = switchCaseList.ToArray();
        @switch.image = codeNode2;
        @switch.Position = num;
        @switch.Length = index - num;
        codeNode1 = (CodeNode) @switch;
        variableDescriptorArray = CodeBlock.extractVariables(state, count);
      }
      finally
      {
        --state.LexicalScopeLevel;
      }
      CodeBlock codeBlock = new CodeBlock(new CodeNode[1]
      {
        codeNode1
      });
      codeBlock._variables = variableDescriptorArray;
      codeBlock.Position = codeNode1.Position;
      codeBlock.Length = codeNode1.Length;
      return (CodeNode) codeBlock;
    }

    public override JSValue Evaluate(Context context)
    {
      int index1 = 1;
      int index2 = this.cases[0].index;
      JSValue first;
      if (context._executionMode >= ExecutionMode.Resume)
      {
        Switch.SuspendData suspendData = context.SuspendData[(CodeNode) this] as Switch.SuspendData;
        first = suspendData.imageValue != null ? suspendData.imageValue : this.image.Evaluate(context);
        index1 = suspendData.caseIndex;
        index2 = suspendData.lineIndex;
      }
      else
      {
        if (context._debugging)
          context.raiseDebugger(this.image);
        first = this.image.Evaluate(context);
      }
      if (context._executionMode == ExecutionMode.Suspend)
      {
        context.SuspendData[(CodeNode) this] = (object) new Switch.SuspendData()
        {
          caseIndex = 1
        };
        return (JSValue) null;
      }
      for (; index1 < this.cases.Length; ++index1)
      {
        if (context._debugging)
          context.raiseDebugger(this.cases[index1].statement);
        JSValue second = this.cases[index1].statement.Evaluate(context);
        if (context._executionMode == ExecutionMode.Suspend)
        {
          context.SuspendData[(CodeNode) this] = (object) new Switch.SuspendData()
          {
            caseIndex = index1,
            imageValue = first
          };
          return (JSValue) null;
        }
        if (StrictEqual.Check(first, second))
        {
          index2 = this.cases[index1].index;
          index1 = this.cases.Length;
          break;
        }
      }
      for (; index2 < this.lines.Length; ++index2)
      {
        if (this.lines[index2] != null)
        {
          context._lastResult = this.lines[index2].Evaluate(context) ?? context._lastResult;
          if (context._executionMode != ExecutionMode.Regular)
          {
            if (context._executionMode == ExecutionMode.Break && context._executionInfo == null)
              context._executionMode = ExecutionMode.Regular;
            else if (context._executionMode == ExecutionMode.Suspend)
              context.SuspendData[(CodeNode) this] = (object) new Switch.SuspendData()
              {
                caseIndex = index1,
                imageValue = first,
                lineIndex = index2
              };
            return (JSValue) null;
          }
        }
      }
      return (JSValue) null;
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
      if (expressionDepth < 1)
        throw new InvalidOperationException();
      Parser.Build<CodeNode>(ref this.image, 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);
      for (int index = 0; index < this.lines.Length; ++index)
        Parser.Build<CodeNode>(ref this.lines[index], 1, variables, codeContext | CodeContext.Conditional, message, stats, opts);
      for (int index = 0; this.functions != null && index < this.functions.Length; ++index)
      {
        CodeNode function = (CodeNode) this.functions[index];
        Parser.Build<CodeNode>(ref function, 1, variables, codeContext, message, stats, opts);
      }
      this.functions = (FunctionDefinition[]) null;
      for (int index = 1; index < this.cases.Length; ++index)
        Parser.Build<CodeNode>(ref this.cases[index].statement, 2, variables, codeContext, message, stats, opts);
      return false;
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
      List<CodeNode> codeNodeList = new List<CodeNode>()
      {
        this.image
      };
      codeNodeList.AddRange((IEnumerable<CodeNode>) this.lines);
      if (this.functions != null && this.functions.Length != 0)
        codeNodeList.AddRange((IEnumerable<CodeNode>) this.functions);
      if (this.cases.Length != 0)
        codeNodeList.AddRange(((IEnumerable<SwitchCase>) this.cases).Where<SwitchCase>((Func<SwitchCase, bool>) (c => c != null)).Select<SwitchCase, CodeNode>((Func<SwitchCase, CodeNode>) (c => c.statement)));
      codeNodeList.RemoveAll((Predicate<CodeNode>) (x => x == null));
      return codeNodeList.ToArray();
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      this.image.Optimize(ref this.image, owner, message, opts, stats);
      for (int index = 1; index < this.cases.Length; ++index)
        this.cases[index].statement.Optimize(ref this.cases[index].statement, owner, message, opts, stats);
      int length = this.lines.Length;
      while (length-- > 0)
      {
        if (this.lines[length] != null)
        {
          CodeNode line = this.lines[length];
          line.Optimize(ref line, owner, message, opts, stats);
          this.lines[length] = line;
        }
      }
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString()
    {
      string str1 = "switch (" + this.image?.ToString() + ") {" + Environment.NewLine;
      string newLine = Environment.NewLine;
      string newValue = Environment.NewLine + "  ";
      int length = this.lines.Length;
      while (length-- > 0)
      {
        for (int index = 0; index < this.cases.Length; ++index)
        {
          if (this.cases[index] != null && this.cases[index].index == length)
            str1 = str1 + "case " + this.cases[index].statement?.ToString() + ":" + Environment.NewLine;
        }
        string str2 = this.lines[length].ToString().Replace(newLine, newValue);
        str1 = str1 + "  " + str2 + (str2[str2.Length - 1] != '}' ? ";" + Environment.NewLine : Environment.NewLine);
      }
      if (this.functions != null)
      {
        for (int index = 0; index < this.functions.Length; ++index)
        {
          string str3 = this.functions[index].ToString().Replace(newLine, newValue);
          str1 = str1 + "  " + str3 + Environment.NewLine;
        }
      }
      return str1 + "}";
    }

    public override void Decompose(ref CodeNode self)
    {
      for (int index = 0; index < this.cases.Length; ++index)
      {
        if (this.cases[index].statement != null)
          this.cases[index].statement.Decompose(ref this.cases[index].statement);
      }
      for (int index = 0; index < this.lines.Length; ++index)
        this.lines[index].Decompose(ref this.lines[index]);
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      this.image.RebuildScope(functionInfo, transferedVariables, scopeBias);
      for (int index = 0; index < this.cases.Length; ++index)
      {
        if (this.cases[index].statement != null)
          this.cases[index].statement.RebuildScope(functionInfo, transferedVariables, scopeBias);
      }
      for (int index = 0; index < this.lines.Length; ++index)
        this.lines[index]?.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    private sealed class SuspendData
    {
      public JSValue imageValue;
      public int caseIndex;
      public int lineIndex;
    }
  }
}
