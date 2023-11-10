// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.ParseInfo
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;
using System.Collections.Generic;

namespace NiL.JS.Core
{
  public sealed class ParseInfo
  {
    public readonly string SourceCode;
    public readonly Stack<bool> AllowBreak;
    public readonly Stack<bool> AllowContinue;
    public readonly List<VariableDescriptor> Variables;
    public readonly Dictionary<string, JSValue> StringConstants;
    public readonly Dictionary<int, JSValue> IntConstants;
    public readonly Dictionary<double, JSValue> DoubleConstants;
    public readonly InternalCompilerMessageCallback Message;
    public string Code;
    public int LabelsCount;
    public int AllowReturn;
    public int LexicalScopeLevel;
    public int FunctionScopeLevel;
    public CodeContext CodeContext;
    public int BreaksCount;
    public int ContiniesCount;

    public List<string> Labels { get; private set; }

    public bool Strict => (this.CodeContext & CodeContext.Strict) != 0;

    public bool AllowDirectives => (this.CodeContext & CodeContext.AllowDirectives) != 0;

    private ParseInfo(
      string codeWithoutComments,
      List<string> labels,
      Stack<bool> allowBreak,
      Stack<bool> allowContinue,
      Dictionary<string, JSValue> stringConstants,
      Dictionary<int, JSValue> intConstants,
      Dictionary<double, JSValue> doubleConstants,
      List<VariableDescriptor> variables)
    {
      this.Code = codeWithoutComments;
      this.SourceCode = codeWithoutComments;
      this.Labels = labels;
      this.AllowBreak = allowBreak;
      this.AllowContinue = allowContinue;
      this.StringConstants = stringConstants;
      this.IntConstants = intConstants;
      this.DoubleConstants = doubleConstants;
      this.Variables = variables;
    }

    public ParseInfo(
      string codeWithoutComments,
      string sourceCode,
      InternalCompilerMessageCallback message)
    {
      this.Code = codeWithoutComments;
      this.SourceCode = sourceCode;
      this.Message = message;
      this.CodeContext |= CodeContext.AllowDirectives;
      this.Labels = new List<string>();
      this.AllowBreak = new Stack<bool>();
      this.AllowContinue = new Stack<bool>();
      this.StringConstants = new Dictionary<string, JSValue>();
      this.IntConstants = new Dictionary<int, JSValue>();
      this.DoubleConstants = new Dictionary<double, JSValue>();
      this.Variables = new List<VariableDescriptor>();
      this.AllowContinue.Push(false);
      this.AllowBreak.Push(false);
    }

    internal JSValue GetCachedValue(int value)
    {
      if (this.IntConstants.ContainsKey(value))
        return this.IntConstants[value];
      JSValue cachedValue = (JSValue) value;
      this.IntConstants[value] = cachedValue;
      return cachedValue;
    }

    public ParseInfo AlternateCode(string code) => new ParseInfo(code, this.Labels, this.AllowBreak, this.AllowContinue, this.StringConstants, this.IntConstants, this.DoubleConstants, this.Variables);

    public IDisposable WithCodeContext(CodeContext codeContext = CodeContext.None)
    {
      ParseInfo.ContextReseter contextReseter = new ParseInfo.ContextReseter(this, this.CodeContext);
      this.CodeContext |= codeContext;
      return (IDisposable) contextReseter;
    }

    public IDisposable WithNewLabelsScope()
    {
      ParseInfo.LabelsReseter labelsReseter = new ParseInfo.LabelsReseter(this, this.Labels);
      this.Labels = new List<string>();
      return (IDisposable) labelsReseter;
    }

    private class ContextReseter : IDisposable
    {
      private readonly ParseInfo _parseInfo;
      private readonly CodeContext _oldCodeContext;

      public ContextReseter(ParseInfo parseInfo, CodeContext oldCodeContext)
      {
        this._parseInfo = parseInfo;
        this._oldCodeContext = oldCodeContext;
      }

      public void Dispose() => this._parseInfo.CodeContext = this._oldCodeContext;
    }

    private class LabelsReseter : IDisposable
    {
      private readonly ParseInfo _parseInfo;
      private readonly List<string> _oldLabels;

      public LabelsReseter(ParseInfo parseInfo, List<string> oldLabels)
      {
        this._parseInfo = parseInfo;
        this._oldLabels = oldLabels;
      }

      public void Dispose() => this._parseInfo.Labels = this._oldLabels;
    }
  }
}
