// Decompiled with JetBrains decompiler
// Type: NiL.JS.Script
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Expressions;
using NiL.JS.Statements;
using System;
using System.Collections.Generic;

namespace NiL.JS
{
  public sealed class Script
  {
    public string Code { get; private set; }

    public CodeBlock Root { get; private set; }

    private Script()
    {
    }

    public static Script Parse(
      string code,
      CompilerMessageCallback messageCallback = null,
      Options options = Options.None)
    {
      if (code == null)
        throw new ArgumentNullException();
      if (code == "")
        return new Script()
        {
          Root = new CodeBlock(new CodeNode[0]),
          Code = ""
        };
      InternalCompilerMessageCallback message1 = messageCallback != null ? (InternalCompilerMessageCallback) ((level, position, length, message) => messageCallback(level, CodeCoordinates.FromTextPosition(code, position, length), message)) : (InternalCompilerMessageCallback) null;
      int index1 = 0;
      CodeBlock self = (CodeBlock) CodeBlock.Parse(new ParseInfo(Parser.RemoveComments(code, 0), code, message1), ref index1);
      FunctionInfo functionInfo = new FunctionInfo();
      Parser.Build<CodeBlock>(ref self, 0, new Dictionary<string, VariableDescriptor>(), CodeContext.None, message1, functionInfo, options);
      CodeBlock codeBlock = self;
      codeBlock._suppressScopeIsolation = SuppressScopeIsolationMode.Suppress;
      for (int index2 = 0; index2 < codeBlock._variables.Length; ++index2)
        codeBlock._variables[index2].captured = true;
      Dictionary<string, VariableDescriptor> transferedVariables = functionInfo.WithLexicalEnvironment ? (Dictionary<string, VariableDescriptor>) null : new Dictionary<string, VariableDescriptor>();
      codeBlock.RebuildScope(functionInfo, transferedVariables, codeBlock._variables.Length == 0 || !functionInfo.WithLexicalEnvironment ? 1 : 0);
      CodeNode codeNode = (CodeNode) codeBlock;
      codeBlock.Optimize(ref codeNode, (FunctionDefinition) null, message1, options, functionInfo);
      if (transferedVariables != null)
        codeBlock._variables = new List<VariableDescriptor>((IEnumerable<VariableDescriptor>) transferedVariables.Values).ToArray();
      if (functionInfo.NeedDecompose)
        codeBlock.Decompose(ref codeNode);
      return new Script() { Code = code, Root = self };
    }

    public JSValue Evaluate(Context context)
    {
      if (this.Code == "")
        return JSValue.Undefined;
      try
      {
        context.Activate();
        return this.Root.Evaluate(context) ?? context._lastResult ?? JSValue.notExists;
      }
      finally
      {
        for (int index = 0; index < this.Root._variables.Length; ++index)
          this.Root._variables[index].cacheContext = (Context) null;
        context.Deactivate();
      }
    }
  }
}
