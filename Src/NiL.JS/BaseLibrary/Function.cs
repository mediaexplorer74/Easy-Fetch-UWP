// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.Function
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Functions;
using NiL.JS.Core.Interop;
using NiL.JS.Expressions;
using NiL.JS.Statements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NiL.JS.BaseLibrary
{
  public class Function : JSObject, ICallable
  {
    private static readonly FunctionDefinition creatorDummy = new FunctionDefinition();
    internal static readonly Function Empty = new Function();
    private static readonly Function TTEProxy;
    internal static readonly JSValue propertiesDummySM;
    private Dictionary<Type, Delegate> _delegateCache;
    internal readonly FunctionDefinition _functionDefinition;
    [Hidden]
    internal readonly Context _initialContext;
    [Hidden]
    internal Number _length;
    [Hidden]
    internal JSValue _prototype;

    protected static void ThrowTypeError() => ExceptionHelper.Throw((Error) new TypeError("Properties \"caller\", \"callee\" and \"arguments\" may not be accessed in strict mode."));

    [Hidden]
    public Context Context
    {
      [Hidden] get => this._initialContext;
    }

    [Field]
    [DoNotDelete]
    [DoNotEnumerate]
    public virtual string name
    {
      [Hidden] get => this._functionDefinition._name;
    }

    [Field]
    [ReadOnly]
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public virtual JSValue length
    {
      [Hidden] get
      {
        if (this._length == null)
        {
          Number number = new Number(0);
          number._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable;
          this._length = number;
          this._length._iValue = this._functionDefinition.parameters.Length;
        }
        return (JSValue) this._length;
      }
    }

    [Hidden]
    public virtual bool Strict
    {
      [Hidden] get
      {
        FunctionDefinition functionDefinition = this._functionDefinition;
        return functionDefinition == null || functionDefinition._body._strict;
      }
    }

    [Hidden]
    public virtual CodeBlock Body
    {
      [Hidden] get => this._functionDefinition == null ? (CodeBlock) null : this._functionDefinition._body;
    }

    [Hidden]
    public virtual FunctionKind Kind
    {
      [Hidden] get => this._functionDefinition.kind;
    }

    [Hidden]
    public virtual RequireNewKeywordLevel RequireNewKeywordLevel { [Hidden] get; [Hidden] protected internal set; }

    [Field]
    [DoNotDelete]
    [DoNotEnumerate]
    [NotConfigurable]
    public virtual JSValue prototype
    {
      [Hidden] get
      {
        if (this._prototype == null)
        {
          if ((this._attributes & JSValueAttributesInternal.ProxyPrototype) != JSValueAttributesInternal.None)
          {
            this.prototype = (JSValue) new JSObject();
            this._prototype._attributes = JSValueAttributesInternal.None;
          }
          else
          {
            JSObject jsObject = JSObject.CreateObject(true);
            jsObject._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.NonConfigurable;
            (jsObject._fields["constructor"] = this.CloneImpl(false))._attributes = JSValueAttributesInternal.DoNotEnumerate;
            this._prototype = (JSValue) jsObject;
          }
        }
        return this._prototype;
      }
      [Hidden] set
      {
        if (value == this._prototype)
          return;
        JSValue prototype = this._prototype;
        this._prototype = value != null ? (value._valueType >= JSValueType.Object ? (JSValue) (value._oValue as JSObject) ?? value : value) : (JSValue) JSValue.@null;
        this._prototype = this._prototype.CloneImpl(true);
        if (prototype == null)
          this._prototype._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.NonConfigurable | JSValueAttributesInternal.Field;
        else
          this._prototype._attributes = prototype._attributes;
      }
    }

    [Field]
    [DoNotDelete]
    [DoNotEnumerate]
    public virtual JSValue arguments
    {
      [Hidden] get
      {
        Context runningContextFor = this.Context.GetRunningContextFor(this);
        if (runningContextFor == null)
          return (JSValue) null;
        if (this._functionDefinition._body._strict)
          ExceptionHelper.Throw((Error) new TypeError("Property \"arguments\" may not be accessed in strict mode."));
        if (runningContextFor._arguments == null && this._functionDefinition.recursionDepth > 0)
          this.BuildArgumentsObject();
        return runningContextFor._arguments;
      }
      [Hidden] set
      {
        Context runningContextFor = this.Context.GetRunningContextFor(this);
        if (runningContextFor == null)
          return;
        if (this._functionDefinition._body._strict)
          ExceptionHelper.Throw((Error) new TypeError("Property \"arguments\" may not be accessed in strict mode."));
        runningContextFor._arguments = value;
      }
    }

    [Field]
    [DoNotDelete]
    [DoNotEnumerate]
    public virtual JSValue caller
    {
      [Hidden] get
      {
        Context prevContext;
        Context runningContextFor = this.Context.GetRunningContextFor(this, out prevContext);
        if (runningContextFor == null || prevContext == null)
          return (JSValue) null;
        if (runningContextFor._strict || prevContext._strict && prevContext._owner != null)
          ExceptionHelper.Throw((Error) new TypeError("Property \"caller\" may not be accessed in strict mode."));
        return (JSValue) prevContext._owner;
      }
      [Hidden] set
      {
        Context prevContext;
        Context runningContextFor = this.Context.GetRunningContextFor(this, out prevContext);
        if (runningContextFor == null || prevContext == null || !runningContextFor._strict && (!prevContext._strict || prevContext._owner == null))
          return;
        ExceptionHelper.Throw((Error) new TypeError("Property \"caller\" may not be accessed in strict mode."));
      }
    }

    [DoNotEnumerate]
    public Function()
    {
      this._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.SystemObject;
      this._functionDefinition = Function.creatorDummy;
      this._valueType = JSValueType.Function;
      this._oValue = (object) this;
      this.RequireNewKeywordLevel = RequireNewKeywordLevel.WithoutNewOnly;
    }

    [Hidden]
    public Function(Context context)
      : this()
    {
      this._initialContext = context != null ? context : throw new ArgumentNullException(nameof (context));
      this.RequireNewKeywordLevel = RequireNewKeywordLevel.Both;
    }

    [DoNotEnumerate]
    public Function(Arguments args)
    {
      this._initialContext = (Context.CurrentContext ?? (Context) Context.DefaultGlobalContext).RootContext;
      if (this._initialContext == Context._DefaultGlobalContext || this._initialContext == null)
        throw new InvalidOperationException("Special Functions constructor can be called from javascript code only");
      this._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.SystemObject;
      int index1 = 0;
      int index2 = args.Length - 1;
      string str1 = "";
      for (int index3 = 0; index3 < index2; ++index3)
        str1 = str1 + args[index3]?.ToString() + (index3 + 1 < index2 ? "," : "");
      string str2 = "function (" + str1 + "){" + Environment.NewLine + (index2 == -1 ? (object) (JSValue) "undefined" : (object) args[index2])?.ToString() + Environment.NewLine + "}";
      NiL.JS.Expressions.Expression expression = FunctionDefinition.Parse(new ParseInfo(Parser.RemoveComments(str2, 0), str2, (InternalCompilerMessageCallback) null)
      {
        CodeContext = CodeContext.InExpression | CodeContext.AllowDirectives
      }, ref index1, FunctionKind.Function);
      if (expression != null && str2.Length == index1)
      {
        Parser.Build(ref expression, 0, new Dictionary<string, VariableDescriptor>(), (CodeContext) ((this._initialContext._strict ? 1 : 0) | 64), (InternalCompilerMessageCallback) null, (FunctionInfo) null, Options.None);
        expression.RebuildScope((FunctionInfo) null, (Dictionary<string, VariableDescriptor>) null, 0);
        expression.Optimize(ref expression, (FunctionDefinition) null, (InternalCompilerMessageCallback) null, Options.None, (FunctionInfo) null);
        expression.Decompose(ref expression);
        this._functionDefinition = expression as FunctionDefinition;
      }
      else
        ExceptionHelper.Throw((Error) new SyntaxError("Unknown syntax error"));
      this._valueType = JSValueType.Function;
      this._oValue = (object) this;
    }

    [Hidden]
    internal Function(Context context, FunctionDefinition functionDefinition)
    {
      this._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.SystemObject;
      this._initialContext = context;
      this._functionDefinition = functionDefinition;
      this._valueType = JSValueType.Function;
      this._oValue = (object) this;
    }

    [Hidden]
    public virtual JSValue Construct(Arguments arguments)
    {
      if (this.RequireNewKeywordLevel == RequireNewKeywordLevel.WithoutNewOnly)
        ExceptionHelper.ThrowTypeError(string.Format(Strings.InvalidTryToCreateWithNew, (object) this.name));
      JSValue targetObject = this.ConstructObject();
      targetObject._attributes |= JSValueAttributesInternal.ConstructingObject;
      try
      {
        return this.Construct(targetObject, arguments);
      }
      finally
      {
        targetObject._attributes &= ~JSValueAttributesInternal.ConstructingObject;
      }
    }

    [Hidden]
    public virtual JSValue Construct(JSValue targetObject, Arguments arguments)
    {
      if (this.RequireNewKeywordLevel == RequireNewKeywordLevel.WithoutNewOnly)
        ExceptionHelper.ThrowTypeError(string.Format(Strings.InvalidTryToCreateWithNew, (object) this.name));
      JSValue jsValue = this.Invoke(true, targetObject, arguments);
      return jsValue._valueType < JSValueType.Object || jsValue._oValue == null ? targetObject : jsValue;
    }

    protected internal virtual JSValue ConstructObject()
    {
      JSObject jsObject = new JSObject();
      jsObject._valueType = JSValueType.Object;
      jsObject._oValue = (object) jsObject;
      jsObject.__proto__ = this.prototype._valueType < JSValueType.Object ? this.Context.GlobalContext._globalPrototype : this.prototype._oValue as JSObject;
      return (JSValue) jsObject;
    }

    internal virtual JSValue InternalInvoke(
      JSValue targetObject,
      NiL.JS.Expressions.Expression[] arguments,
      Context initiator,
      bool withSpread,
      bool construct)
    {
      if (this._functionDefinition._body == null)
        return JSValue.NotExists;
      Arguments arguments1 = Tools.CreateArguments(arguments, initiator);
      initiator._objectSource = (JSValue) null;
      if (!construct)
        return this.Call(targetObject, arguments1);
      return targetObject == null || targetObject._valueType < JSValueType.Object ? this.Construct(arguments1) : this.Construct(targetObject, arguments1);
    }

    [Hidden]
    [DebuggerStepThrough]
    public JSValue Call(Arguments args) => this.Call(JSValue.undefined, args);

    [Hidden]
    public JSValue Call(JSValue targetObject, Arguments arguments)
    {
      if (this.RequireNewKeywordLevel == RequireNewKeywordLevel.WithNewOnly)
        ExceptionHelper.ThrowTypeError(string.Format(Strings.InvalidTryToCreateWithoutNew, (object) this.name));
      targetObject = this.correctTargetObject(targetObject, this._functionDefinition._body._strict);
      return this.Invoke(false, targetObject, arguments);
    }

    protected internal virtual JSValue Invoke(
      bool construct,
      JSValue targetObject,
      Arguments arguments)
    {
      JSValue jsValue = (JSValue) null;
      CodeBlock body = this._functionDefinition._body;
      if (body._lines.Length == 0)
      {
        JSValue.notExists._valueType = JSValueType.NotExists;
        return JSValue.notExists;
      }
      Context currentContext = Context.CurrentContext;
      bool flag = this._functionDefinition._functionInfo.ContainsEval || this._functionDefinition._functionInfo.ContainsWith || this._functionDefinition._functionInfo.NeedDecompose || currentContext != null && currentContext._debugging;
      if (this._functionDefinition.recursionDepth > this._functionDefinition.parametersStored)
      {
        if (!flag)
          this.storeParameters();
        this._functionDefinition.parametersStored = this._functionDefinition.recursionDepth;
      }
      if (arguments == null)
        arguments = new Arguments(currentContext);
      while (true)
      {
        Context internalContext = new Context(this._initialContext, flag, this);
        internalContext._callDepth = (currentContext != null ? currentContext._callDepth : 0) + 1;
        internalContext._definedVariables = body._variables;
        internalContext.Activate();
        try
        {
          this.initContext(targetObject, arguments, flag, internalContext);
          this.initParameters(arguments, flag, internalContext);
          ++this._functionDefinition.recursionDepth;
          jsValue = this.evaluateBody(internalContext);
        }
        finally
        {
          --this._functionDefinition.recursionDepth;
          if (this._functionDefinition.parametersStored > this._functionDefinition.recursionDepth)
            this._functionDefinition.parametersStored = this._functionDefinition.recursionDepth;
          this.exit(internalContext);
        }
        if (jsValue == null)
        {
          arguments = internalContext._executionInfo as Arguments;
          targetObject = this.correctTargetObject(internalContext._objectSource, body._strict);
        }
        else
          break;
      }
      return jsValue;
    }

    internal JSValue evaluateBody(Context internalContext)
    {
      this._functionDefinition._body.Evaluate(internalContext);
      if (internalContext._executionMode == ExecutionMode.TailRecursion)
        return (JSValue) null;
      JSValue executionInfo = internalContext._executionInfo;
      if (executionInfo == null || executionInfo._valueType < JSValueType.Undefined)
      {
        JSValue.notExists._valueType = JSValueType.NotExists;
        return JSValue.notExists;
      }
      if (executionInfo._valueType == JSValueType.Undefined)
        return JSValue.undefined;
      return (executionInfo._attributes & JSValueAttributesInternal.SystemObject) == JSValueAttributesInternal.None ? executionInfo.CloneImpl(false) : executionInfo;
    }

    internal void exit(Context internalContext)
    {
      this._functionDefinition?._body?.clearVariablesCache();
      internalContext._executionMode = ExecutionMode.Return;
      internalContext.Deactivate();
    }

    internal void BuildArgumentsObject()
    {
      Context prevContext;
      Context runningContextFor = this.Context.GetRunningContextFor(this, out prevContext);
      if (runningContextFor == null || runningContextFor._arguments != null)
        return;
      Arguments arguments1 = new Arguments();
      arguments1._caller = (JSValue) prevContext?._owner;
      arguments1._callee = (JSValue) this;
      arguments1._iValue = this._functionDefinition.parameters.Length;
      Arguments arguments2 = arguments1;
      for (int index = 0; index < this._functionDefinition.parameters.Length; ++index)
        arguments2[index] = !this._functionDefinition._body._strict ? this._functionDefinition.parameters[index].cacheRes : this._functionDefinition.parameters[index].cacheRes.CloneImpl(false);
      runningContextFor._arguments = (JSValue) arguments2;
    }

    internal void initCachedReference(Context internalContext)
    {
      VariableDescriptor descriptor = this._functionDefinition.reference._descriptor;
      if (descriptor == null || descriptor.name == null || descriptor.cacheContext == internalContext._parent)
        return;
      if (descriptor.cacheContext != null)
      {
        JSValue jsValue = descriptor.cacheContext.DefineVariable(descriptor.name);
        jsValue.Assign(descriptor.cacheRes);
        jsValue._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly;
      }
      descriptor.cacheContext = internalContext._parent;
      descriptor.cacheRes = (JSValue) this;
    }

    internal void initContext(
      JSValue targetObject,
      Arguments arguments,
      bool storeValuesInContext,
      Context internalContext)
    {
      this.initCachedReference(internalContext);
      internalContext._thisBind = targetObject;
      internalContext._strict |= this._functionDefinition._body._strict;
      if (this._functionDefinition.kind == FunctionKind.Arrow)
      {
        internalContext._arguments = internalContext._parent._arguments;
        internalContext._thisBind = internalContext._parent._thisBind;
      }
      else
      {
        internalContext._arguments = (JSValue) arguments;
        if (storeValuesInContext)
        {
          internalContext._variables[nameof (arguments)] = (JSValue) arguments;
          if (!string.IsNullOrEmpty(this.name) && (this.Kind == FunctionKind.Function || this.Kind == FunctionKind.Generator))
            internalContext._variables[this.name] = (JSValue) this;
        }
        if (this._functionDefinition._body._strict)
        {
          arguments._attributes |= JSValueAttributesInternal.ReadOnly;
          arguments._callee = Function.propertiesDummySM;
          arguments._caller = Function.propertiesDummySM;
        }
        else
          arguments._callee = (JSValue) this;
      }
    }

    internal void initParameters(
      Arguments args,
      bool storeVariablesIntoContext,
      Context internalContext)
    {
      storeVariablesIntoContext |= this._functionDefinition._functionInfo.ContainsArguments;
      int num = System.Math.Min(args._iValue, this._functionDefinition.parameters.Length - (this._functionDefinition._functionInfo.ContainsRestParameters ? 1 : 0));
      JSValue[] jsValueArray = (JSValue[]) null;
      Array array = (Array) null;
      if (this._functionDefinition._functionInfo.ContainsRestParameters)
        array = new Array();
      for (int index = 0; index < this._functionDefinition.parameters.Length; ++index)
      {
        JSValue jsValue = args[index];
        ParameterDescriptor parameter = this._functionDefinition.parameters[index];
        if (!jsValue.Defined && parameter.initializer != null)
        {
          if (jsValueArray == null)
            jsValueArray = new JSValue[this._functionDefinition.parameters.Length];
          jsValueArray[index] = parameter.initializer.Evaluate(internalContext);
        }
      }
      for (int index = 0; index < num; ++index)
      {
        JSValue jsValue = args[index];
        ParameterDescriptor parameter = this._functionDefinition.parameters[index];
        if (!jsValue.Defined)
          jsValue = parameter.initializer == null ? JSValue.undefined : jsValueArray?[index] ?? JSValue.undefined;
        if (this._functionDefinition._body._strict)
        {
          if (storeVariablesIntoContext)
          {
            args[index] = jsValue.CloneImpl(false);
            jsValue = jsValue.CloneImpl(false);
          }
          else if (parameter.assignments != null)
          {
            jsValue = jsValue.CloneImpl(false);
            args[index] = jsValue;
          }
        }
        else if (parameter.assignments != null | storeVariablesIntoContext || (jsValue._attributes & JSValueAttributesInternal.Temporary) != JSValueAttributesInternal.None)
        {
          jsValue = jsValue.CloneImpl(false);
          args[index] = jsValue;
          jsValue._attributes |= JSValueAttributesInternal.Argument;
        }
        jsValue._attributes &= ~JSValueAttributesInternal.Cloned;
        if (parameter.captured | storeVariablesIntoContext)
          (internalContext._variables ?? (internalContext._variables = JSObject.getFieldsContainer()))[parameter.Name] = jsValue;
        parameter.cacheContext = internalContext;
        parameter.cacheRes = jsValue;
        if (string.CompareOrdinal(parameter.name, "arguments") == 0)
          internalContext._arguments = jsValue;
      }
      for (int index = num; index < args._iValue; ++index)
      {
        JSValue jsValue = args[index];
        if (storeVariablesIntoContext)
          args[index] = jsValue = jsValue.CloneImpl(false);
        jsValue._attributes |= JSValueAttributesInternal.Argument;
        array?._data.Add(jsValue);
      }
      for (int index = num; index < this._functionDefinition.parameters.Length; ++index)
      {
        ParameterDescriptor parameter = this._functionDefinition.parameters[index];
        if (parameter.initializer != null)
        {
          if (storeVariablesIntoContext || parameter.assignments != null)
          {
            parameter.cacheRes = (jsValueArray?[index] ?? JSValue.undefined).CloneImpl(false);
          }
          else
          {
            parameter.cacheRes = jsValueArray?[index] ?? JSValue.undefined;
            if (!parameter.cacheRes.Defined)
              parameter.cacheRes = JSValue.undefined;
          }
        }
        else if (storeVariablesIntoContext || parameter.assignments != null)
        {
          if (index == num && array != null)
            parameter.cacheRes = array.CloneImpl(false);
          else
            parameter.cacheRes = new JSValue()
            {
              _valueType = JSValueType.Undefined
            };
          parameter.cacheRes._attributes = JSValueAttributesInternal.Argument;
        }
        else if (index == num && array != null)
          parameter.cacheRes = (JSValue) array;
        else
          parameter.cacheRes = JSValue.undefined;
        parameter.cacheContext = internalContext;
        if (parameter.Destructor == null && parameter.captured | storeVariablesIntoContext)
        {
          if (internalContext._variables == null)
            internalContext._variables = JSObject.getFieldsContainer();
          internalContext._variables[parameter.Name] = parameter.cacheRes;
        }
        if (string.CompareOrdinal(parameter.name, "arguments") == 0)
          internalContext._arguments = parameter.cacheRes;
      }
    }

    internal JSValue correctTargetObject(JSValue thisBind, bool strict)
    {
      if (thisBind == null)
      {
        if (strict)
          return JSValue.undefined;
        return this._initialContext == null ? (JSValue) null : this._initialContext.RootContext._thisBind;
      }
      if (this._initialContext != null)
      {
        if (!strict)
        {
          if (thisBind._valueType > JSValueType.Undefined && thisBind._valueType < JSValueType.Object)
            return (JSValue) thisBind.ToObject();
          if (thisBind._valueType <= JSValueType.Undefined || thisBind._oValue == null)
            return this._initialContext.RootContext._thisBind;
        }
        else if (thisBind._valueType <= JSValueType.Undefined)
          return JSValue.undefined;
      }
      return thisBind;
    }

    internal void storeParameters()
    {
      if (this._functionDefinition.parameters.Length == 0)
        return;
      Context cacheContext = this._functionDefinition.parameters[0].cacheContext;
      if (cacheContext._variables == null)
        cacheContext._variables = JSObject.getFieldsContainer();
      for (int index = 0; index < this._functionDefinition.parameters.Length; ++index)
        cacheContext._variables[this._functionDefinition.parameters[index].Name] = this._functionDefinition.parameters[index].cacheRes;
    }

    [Hidden]
    protected internal override JSValue GetProperty(
      JSValue nameObj,
      bool forWrite,
      PropertyScope memberScope)
    {
      if (memberScope < PropertyScope.Super && nameObj._valueType != JSValueType.Symbol)
      {
        string str = nameObj.ToString();
        if (nameObj._valueType != JSValueType.String)
          nameObj = (JSValue) str;
        if (this._functionDefinition._body._strict && (str == "caller" || str == "arguments"))
          return Function.propertiesDummySM;
        if (str == "prototype")
        {
          if (forWrite)
            this._prototype = this.prototype.CloneImpl(true);
          return this.prototype;
        }
      }
      return base.GetProperty(nameObj, forWrite, memberScope);
    }

    [CLSCompliant(false)]
    [DoNotEnumerate]
    [ArgumentsCount(0)]
    public new JSValue toString(Arguments args) => (JSValue) this.ToString();

    [Hidden]
    public override sealed string ToString() => this.ToString(false);

    [Hidden]
    public virtual string ToString(bool headerOnly) => this._functionDefinition.ToString(headerOnly);

    [Hidden]
    public override JSValue valueOf() => base.valueOf();

    [DoNotEnumerate]
    [CLSCompliant(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public JSValue call(Arguments args)
    {
      JSValue targetObject = args[0];
      int index1 = args._iValue - 1;
      if (index1 >= 0)
      {
        for (int index2 = 0; index2 <= index1; ++index2)
          args[index2] = args[index2 + 1];
        args[index1] = (JSValue) null;
        args._iValue = index1;
      }
      else
        args[0] = (JSValue) null;
      return this.Call(targetObject, args);
    }

    [DoNotEnumerate]
    [ArgumentsCount(2)]
    [AllowNullArguments]
    public JSValue apply(Arguments args)
    {
      Arguments arguments = args ?? new Arguments();
      JSValue targetObject1 = arguments[1];
      JSValue targetObject2 = arguments[0];
      if (args != null)
        arguments.Reset();
      if (targetObject1.Defined)
      {
        if (targetObject1._valueType < JSValueType.Object)
          ExceptionHelper.Throw((Error) new TypeError("Argument list has wrong type."));
        JSValue jsValue = targetObject1["length"];
        if (jsValue._valueType == JSValueType.Property)
          jsValue = (jsValue._oValue as NiL.JS.Core.PropertyPair).getter.Call(targetObject1, (Arguments) null);
        arguments._iValue = Tools.JSObjectToInt32(jsValue);
        if (arguments._iValue >= 50000)
          ExceptionHelper.Throw((Error) new RangeError("Too many arguments."));
        int iValue = arguments._iValue;
        while (iValue-- > 0)
          arguments[iValue] = targetObject1[Tools.Int32ToString(iValue)];
      }
      return this.Call(targetObject2, arguments);
    }

    [DoNotEnumerate]
    public virtual Function bind(Arguments args)
    {
      if (args.Length == 0)
        return this;
      JSValue jsValue = args[0];
      int num = this._functionDefinition._body == null || !this._functionDefinition._body._strict ? (Context.CurrentContext._strict ? 1 : 0) : 1;
      return (Function) new BindedFunction(this, args);
    }

    [Hidden]
    public T MakeDelegate<T>() => (T) this.MakeDelegate(typeof (T));

    [Hidden]
    public virtual Delegate MakeDelegate(Type delegateType)
    {
      Delegate delegate1;
      if (this._delegateCache != null && this._delegateCache.TryGetValue(delegateType, out delegate1))
        return delegate1;
      MethodInfo method = delegateType.GetRuntimeMethods().First<MethodInfo>((Func<MethodInfo, bool>) (x => x.Name == "Invoke"));
      Delegate delegate2 = Tools.BuildJsCallTree("<delegate>" + this.name, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) this), (ParameterExpression) null, method, delegateType).Compile();
      if (this._delegateCache == null)
        this._delegateCache = new Dictionary<Type, Delegate>();
      this._delegateCache.Add(delegateType, delegate2);
      return delegate2;
    }

    static Function()
    {
      MethodProxy methodProxy = new MethodProxy(new Context((Context) null, false, Function.Empty), (MethodBase) typeof (Function).GetTypeInfo().GetDeclaredMethod("ThrowTypeError"));
      methodProxy._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.Immutable;
      Function.TTEProxy = (Function) methodProxy;
      Function.propertiesDummySM = new JSValue()
      {
        _valueType = JSValueType.Property,
        _oValue = (object) new NiL.JS.Core.PropertyPair()
        {
          getter = Function.TTEProxy,
          setter = Function.TTEProxy
        },
        _attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.Immutable | JSValueAttributesInternal.NonConfigurable
      };
    }
  }
}
