// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Context
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Functions;
using NiL.JS.Expressions;
using NiL.JS.Statements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace NiL.JS.Core
{
  [DebuggerTypeProxy(typeof (ContextDebuggerProxy))]
  public class Context : IEnumerable<string>, IEnumerable
  {
    [ThreadStatic]
    internal static List<Context> currentContextStack;
    internal static readonly GlobalContext _DefaultGlobalContext;
    internal ExecutionMode _executionMode;
    internal JSValue _objectSource;
    internal JSValue _executionInfo;
    internal JSValue _lastResult;
    internal JSValue _arguments;
    internal JSValue _thisBind;
    internal Function _owner;
    internal Context _parent;
    internal IDictionary<string, JSValue> _variables;
    internal bool _strict;
    internal VariableDescriptor[] _definedVariables;
    internal NiL.JS.Module _module;
    private Dictionary<CodeNode, object> _suspendData;
    internal int _callDepth;
    internal bool _debugging;

    internal static List<Context> GetCurrectContextStack()
    {
      if (Context.currentContextStack == null)
        Context.currentContextStack = new List<Context>()
        {
          (Context) Context.DefaultGlobalContext
        };
      return Context.currentContextStack;
    }

    public static Context CurrentContext
    {
      get
      {
        List<Context> currectContextStack = Context.GetCurrectContextStack();
        return currectContextStack == null || currectContextStack.Count == 0 ? (Context) null : currectContextStack[currectContextStack.Count - 1];
      }
    }

    public static GlobalContext DefaultGlobalContext => Context._DefaultGlobalContext;

    public Context RootContext
    {
      get
      {
        Context rootContext = this;
        while (rootContext._parent != null && rootContext._parent._parent != null)
          rootContext = rootContext._parent;
        return rootContext;
      }
    }

    public GlobalContext GlobalContext
    {
      get
      {
        Context context = this;
        if (context._parent != null)
        {
          do
          {
            context = context._parent;
          }
          while (context._parent != null);
        }
        return context is GlobalContext globalContext ? globalContext : throw new Exception("Incorrect state");
      }
    }

    public static GlobalContext CurrentGlobalContext => (Context.CurrentContext ?? (Context) Context._DefaultGlobalContext).GlobalContext;

    public JSValue ThisBind
    {
      get
      {
        if (this._parent == null)
          ExceptionHelper.Throw((Exception) new InvalidOperationException("Unable to get this-binding for Global Context"));
        Context context = this;
        if (this._thisBind == null)
        {
          if (this._strict)
            return JSValue.undefined;
          for (; context._thisBind == null; context = context._parent)
          {
            if (context._parent._parent == null)
            {
              this._thisBind = (JSValue) new GlobalObject(context);
              context._thisBind = this._thisBind;
              break;
            }
          }
          this._thisBind = context._thisBind;
        }
        return this._thisBind;
      }
    }

    public bool Debugging
    {
      get => this._debugging;
      set => this._debugging = value;
    }

    public event NiL.JS.Core.DebuggerCallback DebuggerCallback;

    public bool Running => Context.GetCurrectContextStack().Contains(this);

    public ExecutionMode AbortReason => this._executionMode;

    public JSValue AbortInfo => this._executionInfo;

    public Dictionary<CodeNode, object> SuspendData
    {
      get => this._suspendData ?? (this._suspendData = new Dictionary<CodeNode, object>());
      internal set => this._suspendData = value;
    }

    static Context()
    {
      GlobalContext globalContext = new GlobalContext();
      globalContext._strict = true;
      Context._DefaultGlobalContext = globalContext;
    }

    public Context()
      : this((Context) Context.CurrentGlobalContext, true, Function.Empty)
    {
    }

    public Context(Context prototype)
      : this(prototype, true, Function.Empty)
    {
    }

    public Context(Context prototype, bool strict)
      : this(prototype, true, Function.Empty)
    {
      this._strict = strict;
    }

    public Context(bool strict)
      : this((Context) Context.CurrentGlobalContext, strict)
    {
    }

    internal Context(Context prototype, bool createFields, Function owner)
    {
      this._callDepth = prototype == null ? 0 : prototype._callDepth;
      this._owner = owner;
      if (prototype != null)
      {
        if (owner == prototype._owner)
          this._arguments = prototype._arguments;
        this._definedVariables = this._owner?.Body?._variables;
        this._parent = prototype;
        this._thisBind = prototype._thisBind;
        this._debugging = prototype._debugging;
        this._module = prototype._module;
      }
      if (createFields)
        this._variables = JSObject.getFieldsContainer();
      this._executionInfo = JSValue.notExists;
    }

    public static void ResetGlobalContext() => Context._DefaultGlobalContext.ResetContext();

    internal bool Activate()
    {
      if (Context.currentContextStack == null)
        Context.currentContextStack = new List<Context>();
      if (Context.currentContextStack.Count > 0 && Context.currentContextStack[Context.currentContextStack.Count - 1] == this)
        return false;
      Context.currentContextStack.Add(this);
      return true;
    }

    internal Context Deactivate()
    {
      if (Context.currentContextStack[Context.currentContextStack.Count - 1] != this)
        throw new InvalidOperationException("Context is not running");
      Context.currentContextStack.RemoveAt(Context.currentContextStack.Count - 1);
      return Context.CurrentContext;
    }

    internal Context GetRunningContextFor(Function function)
    {
      Context prevContext = (Context) null;
      return this.GetRunningContextFor(function, out prevContext);
    }

    internal Context GetRunningContextFor(Function function, out Context prevContext)
    {
      prevContext = (Context) null;
      if (function == null)
        return (Context) null;
      List<Context> currectContextStack = Context.GetCurrectContextStack();
      int count = currectContextStack.Count;
      while (count-- > 0)
      {
        if (currectContextStack[count]._owner == function)
        {
          if (count > 0)
            prevContext = currectContextStack[count - 1];
          return currectContextStack[count];
        }
      }
      return (Context) null;
    }

    internal virtual void ReplaceVariableInstance(string name, JSValue instance)
    {
      if (this._variables != null && this._variables.ContainsKey(name))
        this._variables[name] = instance;
      else
        this._parent?.ReplaceVariableInstance(name, instance);
    }

    public virtual JSValue DefineVariable(string name, bool deletable = false)
    {
      JSValue jsValue;
      if (this._variables == null || !this._variables.TryGetValue(name, out jsValue))
      {
        if (this._variables == null)
          this._variables = JSObject.getFieldsContainer();
        jsValue = new JSValue()
        {
          _valueType = JSValueType.Undefined
        };
        this._variables[name] = jsValue;
        if (!deletable)
          jsValue._attributes = JSValueAttributesInternal.DoNotDelete;
      }
      else if (jsValue.NeedClone)
      {
        jsValue = jsValue.CloneImpl(false);
        this._variables[name] = jsValue;
      }
      else
        jsValue._valueType |= JSValueType.Undefined;
      return jsValue;
    }

    public void DefineGetSetVariable(string name, Func<object> getter, Action<object> setter)
    {
      JSValue jsValue = this.GetVariable(name).ValueType < JSValueType.Undefined ? this.DefineVariable(name) : throw new ArgumentException();
      if (jsValue.ValueType < JSValueType.Undefined)
        throw new InvalidOperationException();
      jsValue._valueType = JSValueType.Property;
      Function getter1 = (Function) null;
      if (getter != null)
        getter1 = (Function) new MethodProxy(this, (MethodBase) getter.GetMethodInfo(), getter.Target);
      Function setter1 = (Function) null;
      if (setter != null)
        setter1 = (Function) new MethodProxy(this, (MethodBase) setter.GetMethodInfo(), setter.Target);
      jsValue._oValue = (object) new PropertyPair(getter1, setter1);
    }

    public JSValue GetVariable(string name) => this.GetVariable(name, false);

    protected internal virtual JSValue GetVariable(string name, bool forWrite)
    {
      JSValue variable = (JSValue) null;
      bool flag = this._variables == null || !this._variables.TryGetValue(name, out variable) && this._parent != null;
      if (flag)
        variable = this._parent.GetVariable(name, forWrite);
      if (variable == null)
      {
        if (this._parent == null)
          return (JSValue) null;
        if (forWrite)
        {
          variable = new JSValue()
          {
            _valueType = JSValueType.NotExists
          };
          this._variables[name] = variable;
        }
        else
        {
          variable = this.GlobalContext._globalPrototype.GetProperty(name, false, PropertyScope.Common);
          if (variable._valueType == JSValueType.NotExistsInObject)
            variable._valueType = JSValueType.NotExists;
        }
      }
      else if (flag)
        this._objectSource = this._parent._objectSource;
      else if (forWrite && variable.NeedClone)
      {
        variable = variable.CloneImpl(false);
        this._variables[name] = variable;
      }
      return variable;
    }

    internal void raiseDebugger(CodeNode nextStatement)
    {
      for (Context context = this; context != null; context = context._parent)
      {
        if (context.DebuggerCallback != null)
        {
          context.DebuggerCallback(this, new DebuggerCallbackEventArgs()
          {
            Statement = nextStatement
          });
          return;
        }
      }
      if (!System.Diagnostics.Debugger.IsAttached)
        return;
      System.Diagnostics.Debugger.Break();
    }

    public void DefineConstructor(Type moduleType)
    {
      if (this._variables == null)
        this._variables = JSObject.getFieldsContainer();
      string name = !moduleType.GetTypeInfo().IsGenericType ? moduleType.Name : moduleType.Name.Substring(0, moduleType.Name.LastIndexOf('`'));
      this.DefineConstructor(moduleType, name);
    }

    public void DefineConstructor(Type type, string name)
    {
      JSObject constructor = this.GlobalContext.GetConstructor(type);
      this._variables.Add(name, (JSValue) constructor);
      constructor._attributes |= JSValueAttributesInternal.DoNotEnumerate;
    }

    public virtual bool DeleteVariable(string variableName) => this._variables.Remove(variableName);

    internal void SetAbortState(ExecutionMode abortReason, JSValue abortInfo)
    {
      this._executionMode = abortReason;
      this._executionInfo = abortInfo;
    }

    public JSValue Eval(string code, bool suppressScopeCreation = false) => this.Eval(code, this.ThisBind, suppressScopeCreation);

    public JSValue Eval(string code, JSValue thisBind, bool suppressScopeCreation = false)
    {
      if (this._parent == null)
        throw new InvalidOperationException("Cannot execute script in global context");
      if (string.IsNullOrEmpty(code))
        return JSValue.undefined;
      Context context1 = this;
      List<Context> currectContextStack = Context.GetCurrectContextStack();
      while (currectContextStack != null && currectContextStack.Count > 1 && currectContextStack[currectContextStack.Count - 2] == context1._parent && currectContextStack[currectContextStack.Count - 2]._owner == context1._owner)
        context1 = context1._parent;
      int index1 = 0;
      string codeWithoutComments = Parser.RemoveComments(code, 0);
      ParseInfo state = new ParseInfo(codeWithoutComments, code, (InternalCompilerMessageCallback) null);
      state.CodeContext |= (CodeContext) ((this._strict ? 1 : 0) | 32);
      CodeBlock codeBlock1 = CodeBlock.Parse(state, ref index1) as CodeBlock;
      if (index1 < codeWithoutComments.Length)
        throw new ArgumentException("Invalid char");
      Dictionary<string, VariableDescriptor> variables = new Dictionary<string, VariableDescriptor>();
      FunctionInfo functionInfo = new FunctionInfo();
      CodeNode codeNode = (CodeNode) codeBlock1;
      Parser.Build<CodeNode>(ref codeNode, 0, variables, (CodeContext) ((this._strict ? 1 : 0) | 32), (InternalCompilerMessageCallback) null, functionInfo, Options.None);
      Dictionary<string, VariableDescriptor> transferedVariables = functionInfo.WithLexicalEnvironment ? (Dictionary<string, VariableDescriptor>) null : new Dictionary<string, VariableDescriptor>();
      codeBlock1.RebuildScope(functionInfo, transferedVariables, codeBlock1._variables.Length == 0 || !functionInfo.WithLexicalEnvironment ? 1 : 0);
      if (transferedVariables != null)
      {
        VariableDescriptor[] array = new VariableDescriptor[transferedVariables.Values.Count];
        transferedVariables.Values.CopyTo(array, 0);
        codeBlock1._variables = array;
        codeBlock1._suppressScopeIsolation = SuppressScopeIsolationMode.DoNotSuppress;
      }
      codeBlock1.Optimize(ref codeNode, (FunctionDefinition) null, (InternalCompilerMessageCallback) null, Options.SuppressUselessExpressionsElimination | Options.SuppressConstantPropogation, functionInfo);
      if (!(codeNode is CodeBlock codeBlock2))
        codeBlock2 = codeBlock1;
      CodeBlock codeBlock3 = codeBlock2;
      if (functionInfo.NeedDecompose)
        codeBlock3.Decompose(ref codeNode);
      codeBlock3._suppressScopeIsolation = SuppressScopeIsolationMode.Suppress;
      bool debugging = this._debugging;
      this._debugging = false;
      bool flag1 = this.Activate();
      try
      {
        Context context2;
        if (!suppressScopeCreation && (functionInfo.WithLexicalEnvironment || codeBlock3._strict || this._strict))
          context2 = new Context(this, false, this._owner)
          {
            _strict = this._strict || codeBlock3._strict
          };
        else
          context2 = this;
        Context context3 = context2;
        if (suppressScopeCreation || !this._strict && !codeBlock3._strict)
        {
          for (int index2 = 0; index2 < codeBlock3._variables.Length; ++index2)
          {
            if (!codeBlock3._variables[index2].lexicalScope)
            {
              Context context4 = context1;
              JSValue jsValue;
              while (context4._parent._parent != null && (context4._variables == null || !context4._variables.TryGetValue(codeBlock3._variables[index2].name, out jsValue)))
                context4 = context4._parent;
              if (context4._definedVariables != null)
              {
                for (int index3 = 0; index3 < context4._definedVariables.Length; ++index3)
                {
                  if (context4._definedVariables[index3].name == codeBlock3._variables[index2].name)
                    context4._definedVariables[index3].definitionScopeLevel = -1;
                }
              }
              jsValue = context1.DefineVariable(codeBlock3._variables[index2].name, !suppressScopeCreation);
              if (codeBlock3._variables[index2].initializer != null)
                jsValue.Assign(codeBlock3._variables[index2].initializer.Evaluate(context3));
              codeBlock3._variables[index2].lexicalScope = true;
              codeBlock3._variables[index2].definitionScopeLevel = -1;
            }
          }
        }
        if (codeBlock3._lines.Length == 0)
          return JSValue.undefined;
        JSValue thisBind1 = this.ThisBind;
        bool flag2 = context3.Activate();
        context3._thisBind = thisBind;
        try
        {
          return codeBlock3.Evaluate(context3) ?? context3._lastResult ?? JSValue.notExists;
        }
        catch (JSException ex)
        {
          if ((ex.Code == null || ex.CodeCoordinates == null) && ex.ExceptionMaker != null)
          {
            ex.Code = code;
            ex.CodeCoordinates = CodeCoordinates.FromTextPosition(code, ex.ExceptionMaker.Position, ex.ExceptionMaker.Length);
          }
          throw;
        }
        finally
        {
          context3._thisBind = thisBind1;
          if (flag2)
            context3.Deactivate();
        }
      }
      finally
      {
        if (flag1)
          this.Deactivate();
        this._debugging = debugging;
      }
    }

    public IEnumerator<string> GetEnumerator()
    {
      IDictionary<string, JSValue> variables = this._variables;
      return ((IEnumerable<string>) ((variables != null ? (object) variables.Keys : (object) null) ?? (object) System.Array.Empty<string>())).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public override string ToString() => "Context of " + this._owner.name;
  }
}
