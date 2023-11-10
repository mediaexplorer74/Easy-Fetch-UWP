// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.TryCatch
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Expressions;
using System;
using System.Collections.Generic;

namespace NiL.JS.Statements
{
  public sealed class TryCatch : CodeNode
  {
    private bool _catch;
    private CodeNode body;
    private CodeNode catchBody;
    private CodeNode finallyBody;
    private VariableDescriptor catchVariableDesc;

    public CodeNode Body => this.body;

    public CodeNode CatchBody => this.catchBody;

    public CodeNode FinalBody => this.finallyBody;

    public string ExceptionVariableName => this.catchVariableDesc.name;

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      int num1 = index;
      if (!Parser.Validate(state.Code, "try", ref num1) || !Parser.IsIdentifierTerminator(state.Code[num1]))
        return (CodeNode) null;
      while (num1 < state.Code.Length && Tools.IsWhiteSpace(state.Code[num1]))
        ++num1;
      if (num1 >= state.Code.Length)
        ExceptionHelper.Throw((Error) new SyntaxError(Strings.UnexpectedEndOfSource));
      if (state.Code[num1] != '{')
        ExceptionHelper.Throw((Error) new SyntaxError("Invalid try statement definition at " + CodeCoordinates.FromTextPosition(state.Code, num1, 0)?.ToString()));
      CodeNode codeNode1 = CodeBlock.Parse(state, ref num1);
      while (Tools.IsWhiteSpace(state.Code[num1]))
        ++num1;
      CodeNode codeNode2 = (CodeNode) null;
      string name = (string) null;
      if (Parser.Validate(state.Code, "catch (", ref num1) || Parser.Validate(state.Code, "catch(", ref num1))
      {
        Tools.SkipSpaces(state.Code, ref num1);
        int num2 = num1;
        if (!Parser.ValidateName(state.Code, ref num1, state.Strict))
          ExceptionHelper.Throw((Error) new SyntaxError("Catch block must contain variable name " + CodeCoordinates.FromTextPosition(state.Code, num1, 0)?.ToString()));
        name = Tools.Unescape(state.Code.Substring(num2, num1 - num2), state.Strict);
        if (state.Strict && (name == "arguments" || name == "eval"))
          ExceptionHelper.Throw((Error) new SyntaxError("Varible name can not be \"arguments\" or \"eval\" in strict mode at " + CodeCoordinates.FromTextPosition(state.Code, num2, num1 - num2)?.ToString()));
        Tools.SkipSpaces(state.Code, ref num1);
        if (!Parser.Validate(state.Code, ")", ref num1))
          ExceptionHelper.Throw((Error) new SyntaxError("Expected \")\" at + " + CodeCoordinates.FromTextPosition(state.Code, num1, 0)?.ToString()));
        while (Tools.IsWhiteSpace(state.Code[num1]))
          ++num1;
        if (state.Code[num1] != '{')
          ExceptionHelper.Throw((Error) new SyntaxError("Invalid catch block statement definition at " + CodeCoordinates.FromTextPosition(state.Code, num1, 0)?.ToString()));
        ++state.LexicalScopeLevel;
        try
        {
          codeNode2 = CodeBlock.Parse(state, ref num1);
        }
        finally
        {
          --state.LexicalScopeLevel;
        }
        while (num1 < state.Code.Length && Tools.IsWhiteSpace(state.Code[num1]))
          ++num1;
      }
      CodeNode codeNode3 = (CodeNode) null;
      if (Parser.Validate(state.Code, "finally", num1) && Parser.IsIdentifierTerminator(state.Code[num1 + 7]))
      {
        num1 += 7;
        while (Tools.IsWhiteSpace(state.Code[num1]))
          ++num1;
        if (state.Code[num1] != '{')
          ExceptionHelper.Throw((Error) new SyntaxError("Invalid finally block statement definition at " + CodeCoordinates.FromTextPosition(state.Code, num1, 0)?.ToString()));
        codeNode3 = CodeBlock.Parse(state, ref num1);
      }
      if (codeNode2 == null && codeNode3 == null)
        ExceptionHelper.ThrowSyntaxError("try block must contain 'catch' or/and 'finally' block", state.Code, index);
      int num3 = index;
      index = num1;
      TryCatch tryCatch = new TryCatch();
      tryCatch.body = codeNode1;
      tryCatch.catchBody = codeNode2;
      tryCatch.finallyBody = codeNode3;
      tryCatch.catchVariableDesc = new VariableDescriptor(name, state.LexicalScopeLevel + 1);
      tryCatch.Position = num3;
      tryCatch.Length = index - num3;
      return (CodeNode) tryCatch;
    }

    public override JSValue Evaluate(Context context)
    {
      Exception exception = (Exception) null;
      if (context._executionMode >= ExecutionMode.Resume)
      {
        if (context.SuspendData[(CodeNode) this] is Action<Context> action)
        {
          action(context);
          return (JSValue) null;
        }
      }
      else if (context._debugging && !(this.body is CodeBlock))
        context.raiseDebugger(this.body);
      try
      {
        this.body.Evaluate(context);
        if (context._executionMode == ExecutionMode.Suspend)
          context.SuspendData[(CodeNode) this] = (object) null;
      }
      catch (Exception ex)
      {
        if (this._catch)
        {
          if (this.catchBody != null)
            this.catchHandler(context, ex);
        }
        else if (this.finallyBody == null)
          throw;
        else
          exception = ex;
      }
      finally
      {
        if (context._executionMode != ExecutionMode.Suspend && this.finallyBody != null)
        {
          this.finallyHandler(context, exception);
          exception = (Exception) null;
        }
      }
      if (context._executionMode != ExecutionMode.Suspend && exception != null)
        throw exception;
      return (JSValue) null;
    }

    private void finallyHandler(Context context, Exception exception)
    {
      if (context._debugging)
        context.raiseDebugger(this.finallyBody);
      ExecutionMode abort = context._executionMode;
      JSValue ainfo = context._executionInfo;
      if (abort == ExecutionMode.Return && ainfo != null)
        ainfo = !ainfo.Defined ? JSValue.Undefined : ainfo.CloneImpl(false);
      context._executionMode = ExecutionMode.Regular;
      context._executionInfo = JSValue.undefined;
      Action<Context> finallyAction = (Action<Context>) null;
      finallyAction = (Action<Context>) (c =>
      {
        c._lastResult = this.finallyBody.Evaluate(c) ?? context._lastResult;
        if (c._executionMode == ExecutionMode.Regular)
        {
          c._executionMode = abort;
          c._executionInfo = ainfo;
          if (exception != null)
            jsException2 = !(exception is JSException jsException2) ? new JSException((JSValue) null, exception) : throw jsException2;
        }
        else
        {
          if (c._executionMode != ExecutionMode.Suspend)
            return;
          c.SuspendData[(CodeNode) this] = (object) finallyAction;
        }
      });
      finallyAction(context);
    }

    private void catchHandler(Context context, Exception e)
    {
      if (context._debugging)
        context.raiseDebugger(this.catchBody);
      if (this.catchBody is Empty)
        return;
      JSValue e1 = e is JSException ? (e as JSException).Error.CloneImpl(false) : context.GlobalContext.ProxyValue((object) e);
      e1._attributes |= JSValueAttributesInternal.DoNotDelete;
      CatchContext catchContext = new CatchContext(e1, context, this.catchVariableDesc.name);
      Action<Context> catchAction = (Action<Context>) null;
      catchAction = (Action<Context>) (c =>
      {
        try
        {
          catchContext._executionMode = c._executionMode;
          catchContext._executionInfo = c._executionInfo;
          catchContext.Activate();
          catchContext._lastResult = this.catchBody.Evaluate((Context) catchContext) ?? catchContext._lastResult;
        }
        finally
        {
          c._lastResult = catchContext._lastResult ?? c._lastResult;
          catchContext.Deactivate();
        }
        c._executionMode = catchContext._executionMode;
        c._executionInfo = catchContext._executionInfo;
        if (c._executionMode != ExecutionMode.Suspend)
          return;
        if (this.finallyBody != null)
          c.SuspendData[(CodeNode) this] = (object) (Action<Context>) (c2 =>
          {
            try
            {
              catchAction(c2);
            }
            finally
            {
              if (c2._executionMode != ExecutionMode.Suspend)
                this.finallyHandler(c2, e);
            }
          });
        else
          c.SuspendData[(CodeNode) this] = (object) catchAction;
      });
      catchAction(context);
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
      if (stats != null)
        stats.ContainsTry = true;
      Parser.Build<CodeNode>(ref this.body, expressionDepth, variables, codeContext | CodeContext.Conditional, message, stats, opts);
      int position = this.Position;
      if (this.catchBody != null)
      {
        this._catch = true;
        this.catchVariableDesc.owner = (CodeNode) this;
        VariableDescriptor variableDescriptor = (VariableDescriptor) null;
        variables.TryGetValue(this.catchVariableDesc.name, out variableDescriptor);
        variables[this.catchVariableDesc.name] = this.catchVariableDesc;
        position = this.catchBody.Position;
        Parser.Build<CodeNode>(ref this.catchBody, expressionDepth, variables, codeContext | CodeContext.Conditional, message, stats, opts);
        if (variableDescriptor != null)
          variables[this.catchVariableDesc.name] = variableDescriptor;
        else
          variables.Remove(this.catchVariableDesc.name);
      }
      int num = 0;
      if (this.finallyBody != null)
      {
        num = this.finallyBody.Position;
        Parser.Build<CodeNode>(ref this.finallyBody, expressionDepth, variables, codeContext, message, stats, opts);
      }
      if (this.body == null || this.body is Empty)
      {
        if (message != null)
          message(MessageLevel.Warning, this.Position, this.Length, "Empty (or reduced to empty) try" + (this.catchBody != null ? "..catch" : "") + (this.finallyBody != null ? "..finally" : "") + " block. Maybe, something missing.");
        _this = this.finallyBody;
      }
      if (this._catch && (this.catchBody == null || this.catchBody is Empty) && message != null)
        message(MessageLevel.Warning, position, (this.catchBody ?? (CodeNode) this).Length, "Empty (or reduced to empty) catch block. Do not ignore exceptions.");
      if (num != 0 && (this.finallyBody == null || this.finallyBody is Empty) && message != null)
        message(MessageLevel.Warning, position, (this.catchBody ?? (CodeNode) this).Length, "Empty (or reduced to empty) finally block.");
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      this.body.Optimize(ref this.body, owner, message, opts, stats);
      if (this.catchBody != null)
        this.catchBody.Optimize(ref this.catchBody, owner, message, opts, stats);
      if (this.finallyBody == null)
        return;
      this.finallyBody.Optimize(ref this.finallyBody, owner, message, opts, stats);
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
      List<CodeNode> codeNodeList = new List<CodeNode>();
      codeNodeList.Add(this.body);
      codeNodeList.Add(this.catchBody);
      codeNodeList.Add(this.finallyBody);
      codeNodeList.RemoveAll((Predicate<CodeNode>) (x => x == null));
      return codeNodeList.ToArray();
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override void Decompose(ref CodeNode self)
    {
      this.body.Decompose(ref this.body);
      if (this.catchBody != null)
        this.catchBody.Decompose(ref this.catchBody);
      if (this.finallyBody == null)
        return;
      this.finallyBody.Decompose(ref this.finallyBody);
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      this.body.RebuildScope(functionInfo, transferedVariables, scopeBias);
      if (this.catchBody != null)
      {
        VariableDescriptor variableDescriptor = (VariableDescriptor) null;
        if (transferedVariables != null)
        {
          transferedVariables.TryGetValue(this.catchVariableDesc.name, out variableDescriptor);
          transferedVariables[this.catchVariableDesc.name] = this.catchVariableDesc;
        }
        this.catchBody.RebuildScope(functionInfo, transferedVariables, scopeBias);
        if (transferedVariables != null)
        {
          if (variableDescriptor != null)
            transferedVariables[variableDescriptor.name] = variableDescriptor;
          else
            transferedVariables.Remove(this.catchVariableDesc.name);
        }
      }
      this.finallyBody?.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    public override string ToString()
    {
      object obj1 = (object) (this.body as CodeBlock);
      if (obj1 == null)
        obj1 = (object) ("{" + Environment.NewLine + " " + this.body?.ToString() + Environment.NewLine + "}");
      string str1 = obj1.ToString();
      string str2;
      if (this.finallyBody != null)
      {
        object obj2 = (object) (this.finallyBody as CodeBlock);
        if (obj2 == null)
          obj2 = (object) ("{" + Environment.NewLine + " " + this.finallyBody?.ToString() + Environment.NewLine + "}");
        str2 = obj2.ToString();
      }
      else
        str2 = "";
      string str3 = str2;
      string str4;
      if (this.catchBody != null)
      {
        object obj3 = (object) (this.catchBody as CodeBlock);
        if (obj3 == null)
          obj3 = (object) ("{" + Environment.NewLine + " " + this.catchBody?.ToString() + Environment.NewLine + "}");
        str4 = obj3.ToString();
      }
      else
        str4 = "";
      string str5 = str4;
      string str6 = str1;
      string str7;
      if (this.catchBody == null)
        str7 = "";
      else
        str7 = Environment.NewLine + "catch (" + this.catchVariableDesc?.ToString() + ")" + Environment.NewLine + str5;
      string str8 = this.finallyBody != null ? Environment.NewLine + "finally" + str3 : "";
      return "try" + str6 + str7 + str8;
    }
  }
}
