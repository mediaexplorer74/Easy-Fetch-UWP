// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.RegExpExpression
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Statements;
using System;

namespace NiL.JS.Expressions
{
  public sealed class RegExpExpression : Expression
  {
    private string pattern;
    private string flags;

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => false;

    protected internal override PredictedType ResultType => PredictedType.Object;

    public RegExpExpression(string pattern, string flags)
    {
      this.pattern = pattern;
      this.flags = flags;
    }

    public static CodeNode Parse(ParseInfo state, ref int position)
    {
      int index = position;
      if (!Parser.ValidateRegex(state.Code, ref index, false))
        return (CodeNode) null;
      string str = state.Code.Substring(position, index - position);
      position = index;
      state.Code = Parser.RemoveComments(state.SourceCode, index);
      int startIndex = str.LastIndexOf('/') + 1;
      string flags = str.Substring(startIndex);
      try
      {
        return (CodeNode) new RegExpExpression(str.Substring(1, startIndex - 2), flags);
      }
      catch (Exception ex)
      {
        if (state.Message != null)
          state.Message(MessageLevel.Error, index - str.Length, str.Length, string.Format(Strings.InvalidRegExp, (object) str));
        return (CodeNode) new ExpressionWrapper((CodeNode) new Throw(ex));
      }
    }

    protected internal override CodeNode[] GetChildrenImpl() => (CodeNode[]) null;

    public override JSValue Evaluate(Context context) => (JSValue) new RegExp(this.pattern, this.flags);

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "/" + this.pattern + "/" + this.flags;
  }
}
