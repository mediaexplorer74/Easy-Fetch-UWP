// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.TemplateString
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Statements;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NiL.JS.Expressions
{
  public sealed class TemplateString : Expression
  {
    private Expression[] expressions;
    private string[] strings;

    public IEnumerable<string> Strings => (IEnumerable<string>) this.strings;

    public IEnumerable<Expression> Expressions => (IEnumerable<Expression>) this.expressions;

    public TemplateStringMode Mode { get; private set; }

    protected internal override bool ContextIndependent
    {
      get
      {
        for (int index = 0; index < this.expressions.Length; ++index)
        {
          if (!this.expressions[index].ContextIndependent)
            return false;
        }
        return true;
      }
    }

    internal TemplateString(string[] strings, Expression[] expression)
    {
      this.strings = strings;
      this.expressions = expression;
    }

    public static CodeNode Parse(ParseInfo state, ref int index) => (CodeNode) TemplateString.Parse(state, ref index, TemplateStringMode.Regular);

    public static Expression Parse(ParseInfo state, ref int index, TemplateStringMode mode)
    {
      if (state.Code[index] != '`')
        ExceptionHelper.ThrowSyntaxError(NiL.JS.Strings.UnexpectedToken, state.Code, index);
      List<string> stringList = new List<string>();
      List<Expression> expressionList = new List<Expression>();
      int num = index + 1;
      do
      {
        int startIndex = num;
        for (bool flag = false; flag || (state.Code[num] != '$' || state.Code[num + 1] != '{') && state.Code[num] != '`'; ++num)
          flag = !flag && state.Code[num] == '\\';
        string code = state.Code.Substring(startIndex, num - startIndex);
        if (mode == TemplateStringMode.Regular)
          code = Tools.Unescape(code, state.Strict);
        stringList.Add(code);
        if (state.Code[num] == '$')
        {
          num += 2;
          expressionList.Add((Expression) ExpressionTree.Parse(state, ref num));
          Tools.SkipSpaces(state.Code, ref num);
          if (state.Code[num] != '}')
            ExceptionHelper.ThrowSyntaxError(NiL.JS.Strings.UnexpectedToken, state.Code, num);
        }
        ++num;
      }
      while (state.Code[num - 1] != '`');
      index = num;
      return (Expression) new TemplateString(stringList.ToArray(), expressionList.ToArray())
      {
        Mode = mode
      };
    }

    public override JSValue Evaluate(Context context)
    {
      StringBuilder stringBuilder = (StringBuilder) null;
      Array array1 = (Array) null;
      JSValue[] jsValueArray = (JSValue[]) null;
      int index = 0;
      if (context != null && context._executionMode >= ExecutionMode.Resume)
      {
        TemplateString.SuspendData suspendData = context.SuspendData[(CodeNode) this] as TemplateString.SuspendData;
        if (this.Mode == TemplateStringMode.Regular)
        {
          stringBuilder = suspendData.result as StringBuilder;
        }
        else
        {
          jsValueArray = suspendData.result as JSValue[];
          array1 = jsValueArray[0]["raw"].Value as Array;
        }
        index = suspendData.Index;
      }
      else if (this.Mode == TemplateStringMode.Regular)
      {
        stringBuilder = new StringBuilder();
      }
      else
      {
        Array array2 = new Array((ICollection) this.strings);
        jsValueArray = new JSValue[1 + this.expressions.Length];
        jsValueArray[0] = (JSValue) array2;
        array1 = (array2["raw"] = (JSValue) new Array(this.strings.Length)) as Array;
      }
      for (; index < this.strings.Length; ++index)
      {
        if (index > 0)
        {
          JSValue jsValue = this.expressions[index - 1].Evaluate(context);
          if (context != null && context._executionMode != ExecutionMode.Regular)
          {
            if (context._executionMode == ExecutionMode.Suspend)
              context.SuspendData[(CodeNode) this] = (object) new TemplateString.SuspendData()
              {
                Index = index,
                result = (object) stringBuilder
              };
            return (JSValue) null;
          }
          if (this.Mode == TemplateStringMode.Regular)
            stringBuilder.Append((object) jsValue);
          else
            jsValueArray[index] = jsValue;
        }
        if (this.Mode == TemplateStringMode.Regular)
          stringBuilder.Append(this.strings[index]);
        else
          array1.Add((JSValue) this.strings[index].Replace("\\", "\\\\"));
      }
      if (this.Mode == TemplateStringMode.Regular)
        return (JSValue) stringBuilder.ToString();
      return new JSValue()
      {
        _oValue = (object) jsValueArray,
        _valueType = JSValueType.SpreadOperatorResult
      };
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
      if (this.expressions.Length == 0)
      {
        if (this.Mode == TemplateStringMode.Regular)
          _this = (CodeNode) new Constant((JSValue) this.strings[0]);
        return false;
      }
      for (int index = 0; index < this.expressions.Length; ++index)
        Parser.Build(ref this.expressions[index], expressionDepth, variables, codeContext, message, stats, opts);
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      for (int index = 0; index < this.expressions.Length; ++index)
        this.expressions[index].Optimize(ref this.expressions[index], owner, message, opts, stats);
    }

    public override void Decompose(ref Expression self, IList<CodeNode> result)
    {
      int num = -1;
      for (int index = 0; index < this.expressions.Length; ++index)
      {
        this.expressions[index].Decompose(ref this.expressions[index], result);
        if (this.expressions[index].NeedDecompose)
          num = index;
      }
      for (int index = 0; index < num; ++index)
      {
        if (!(this.expressions[index] is ExtractStoredValue))
        {
          result.Add((CodeNode) new StoreValue(this.expressions[index], false));
          this.expressions[index] = (Expression) new ExtractStoredValue(this.expressions[index]);
        }
      }
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      for (int index = 0; index < this.expressions.Length; ++index)
        this.expressions[index].RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder().Append('`');
      for (int index1 = 0; index1 < this.strings.Length; ++index1)
      {
        if (index1 > 0)
          sb.Append("${ ").Append((object) this.expressions[index1 - 1]).Append(" }");
        for (int index2 = 0; index2 < this.strings[index1].Length; ++index2)
          JSON.escapeIfNeed(sb, this.strings[index1][index2]);
      }
      return sb.Append('`').ToString();
    }

    private sealed class SuspendData
    {
      public int Index;
      public object result;
    }
  }
}
