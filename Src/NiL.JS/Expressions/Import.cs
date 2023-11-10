// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.Import
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NiL.JS.Expressions
{
  public sealed class Import : Expression
  {
    protected internal override PredictedType ResultType => PredictedType.Object;

    internal override bool ResultInTempContainer => false;

    protected internal override bool ContextIndependent => false;

    public Import(Expression importPath)
      : base(importPath, (Expression) null, false)
    {
    }

    public static CodeNode Parse(ParseInfo state, ref int index)
    {
      int position = index;
      if (!Parser.Validate(state.Code, "import", ref position))
        throw new InvalidOperationException("\"import\" expected");
      Tools.SkipSpaces(state.Code, ref position);
      if (!Parser.Validate(state.Code, "(", ref position))
      {
        if ((state.CodeContext & CodeContext.InExpression) == CodeContext.None)
          return (CodeNode) null;
        ExceptionHelper.ThrowSyntaxError("\"(\" expected", state.Code, position);
      }
      Tools.SkipSpaces(state.Code, ref position);
      Expression importPath = (Expression) ExpressionTree.Parse(state, ref position);
      Tools.SkipSpaces(state.Code, ref position);
      if (!Parser.Validate(state.Code, ")", ref position))
        ExceptionHelper.ThrowSyntaxError("\")\" expected", state.Code, position);
      index = position;
      return (CodeNode) new Import(importPath);
    }

    public override JSValue Evaluate(Context context)
    {
      Task<JSValue> task = new Task<JSValue>((Func<JSValue>) (() =>
      {
        Module module = context._module.Import(this.LeftOperand.Evaluate(context).ToString());
        JSObject jsObject = JSObject.CreateObject();
        foreach (KeyValuePair<string, JSValue> export in module.Exports)
        {
          string name = export.Key;
          if (name == string.Empty)
            name = "default";
          jsObject[name] = export.Value;
        }
        return (JSValue) jsObject;
      }));
      task.Start();
      return context.GlobalContext.ProxyValue((object) new Promise(task));
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "Import(" + this.LeftOperand?.ToString() + ")";
  }
}
