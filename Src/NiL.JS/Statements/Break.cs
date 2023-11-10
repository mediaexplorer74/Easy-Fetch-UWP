// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.Break
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using System.Collections.Generic;

namespace NiL.JS.Statements
{
  public sealed class Break : CodeNode
  {
    private JSValue label;

    public JSValue Label => this.label;

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      int index1 = index;
      if (!Parser.Validate(state.Code, "break", ref index1) || !Parser.IsIdentifierTerminator(state.Code[index1]))
        return (CodeNode) null;
      while (Tools.IsWhiteSpace(state.Code[index1]) && !Tools.IsLineTerminator(state.Code[index1]))
        ++index1;
      int startIndex = index1;
      JSValue jsValue = (JSValue) null;
      if (Parser.ValidateName(state.Code, ref index1, state.Strict))
      {
        jsValue = (JSValue) Tools.Unescape(state.Code.Substring(startIndex, index1 - startIndex), state.Strict);
        if (!state.Labels.Contains(jsValue._oValue.ToString()))
          ExceptionHelper.Throw((Error) new SyntaxError("Try to break to undefined label."));
      }
      else if (!state.AllowBreak.Peek())
        ExceptionHelper.Throw((Error) new SyntaxError("Invalid use of break statement"));
      int num = index;
      index = index1;
      ++state.BreaksCount;
      Break @break = new Break();
      @break.label = jsValue;
      @break.Position = num;
      @break.Length = index - num;
      return (CodeNode) @break;
    }

    public override JSValue Evaluate(Context context)
    {
      context._executionMode = ExecutionMode.Break;
      context._executionInfo = this.label;
      return (JSValue) null;
    }

    protected internal override CodeNode[] GetChildrenImpl() => (CodeNode[]) null;

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override void Decompose(ref CodeNode self)
    {
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
    }

    public override string ToString() => "break" + (this.label != null ? " " + this.label?.ToString() : "");
  }
}
