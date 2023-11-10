// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Functions.MethodProxy
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NiL.JS.Core.Functions
{
  [Prototype(typeof (Function), true)]
  internal sealed class MethodProxy : Function
  {
    private static readonly Dictionary<MethodBase, MethodProxy.WrapperDelegate> WrapperCache = new Dictionary<MethodBase, MethodProxy.WrapperDelegate>();
    private static readonly MethodInfo ArgumentsGetItemMethod = TypeExtensions.GetMethod(typeof (Arguments), "get_Item", new Type[1]
    {
      typeof (int)
    });
    private readonly MethodProxy.WrapperDelegate _fastWrapper;
    private readonly bool _forceInstance;
    private readonly bool _strictConversion;
    private readonly MethodProxy.RestPrmsConverter _restPrmsArrayCreator;
    private readonly ConvertValueAttribute[] _paramsConverters;
    private readonly string _name;
    internal readonly ParameterInfo[] _parameters;
    internal readonly MethodBase _method;
    internal readonly object _hardTarget;
    internal readonly ConvertValueAttribute _returnConverter;

    public ParameterInfo[] Parameters => this._parameters;

    public override string name => this._name;

    public override JSValue prototype
    {
      get => (JSValue) null;
      set
      {
      }
    }

    public MethodProxy(Context context, MethodBase methodBase)
      : this(context, methodBase, (object) null)
    {
    }

    public MethodProxy(Context context, MethodBase methodBase, object hardTarget)
      : base(context)
    {
      this._method = methodBase;
      this._hardTarget = hardTarget;
      this._parameters = methodBase.GetParameters();
      this._strictConversion = CustomAttributeExtensions.IsDefined(methodBase, typeof (StrictConversionAttribute), true);
      this._name = methodBase.Name;
      if (CustomAttributeExtensions.IsDefined(methodBase, typeof (JavaScriptNameAttribute), false))
      {
        this._name = (CustomAttributeExtensions.GetCustomAttributes(methodBase, typeof (JavaScriptNameAttribute), false).First<Attribute>() as JavaScriptNameAttribute).Name;
        if (this._name.StartsWith("@@"))
          this._name = this._name.Substring(2);
      }
      if (this._length == null)
      {
        Number number = new Number(0);
        number._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.SystemObject;
        this._length = number;
      }
      if (CustomAttributeExtensions.IsDefined(methodBase, typeof (ArgumentsCountAttribute), false))
        this._length._iValue = (CustomAttributeExtensions.GetCustomAttributes(methodBase, typeof (ArgumentsCountAttribute), false).First<Attribute>() as ArgumentsCountAttribute).Count;
      else
        this._length._iValue = this._parameters.Length;
      for (int index = 0; index < this._parameters.Length; ++index)
      {
        if (CustomAttributeExtensions.IsDefined(this._parameters[index], typeof (ConvertValueAttribute), false))
        {
          Attribute attribute = CustomAttributeExtensions.GetCustomAttributes(this._parameters[index], typeof (ConvertValueAttribute), false).First<Attribute>();
          if (this._paramsConverters == null)
            this._paramsConverters = new ConvertValueAttribute[this._parameters.Length];
          this._paramsConverters[index] = attribute as ConvertValueAttribute;
        }
      }
      MethodInfo methodInfo = methodBase as MethodInfo;
      if ((object) methodInfo != null)
      {
        this._returnConverter = methodInfo.ReturnParameter.GetCustomAttribute(typeof (ConvertValueAttribute), false) as ConvertValueAttribute;
        this._forceInstance = CustomAttributeExtensions.IsDefined(methodBase, typeof (InstanceMemberAttribute), false);
        if (this._forceInstance)
        {
          if (!methodInfo.IsStatic || this._parameters.Length == 0 || (object) this._parameters[0].ParameterType != (object) typeof (JSValue))
            throw new ArgumentException("Force-instance method \"" + methodBase?.ToString() + "\" has invalid signature");
          this._parameters = ((IEnumerable<ParameterInfo>) this._parameters).Skip<ParameterInfo>(1).ToArray<ParameterInfo>();
          if (this._paramsConverters != null)
            this._paramsConverters = ((IEnumerable<ConvertValueAttribute>) this._paramsConverters).Skip<ConvertValueAttribute>(1).ToArray<ConvertValueAttribute>();
        }
        if (this._parameters.Length != 0 && ((IEnumerable<ParameterInfo>) this._parameters).Last<ParameterInfo>().GetCustomAttribute(typeof (ParamArrayAttribute), false) != null)
          this._restPrmsArrayCreator = this.makeRestPrmsArrayCreator();
        if (!MethodProxy.WrapperCache.TryGetValue(methodBase, out this._fastWrapper))
          MethodProxy.WrapperCache[methodBase] = this._fastWrapper = this.makeFastWrapper(methodInfo);
        this.RequireNewKeywordLevel = RequireNewKeywordLevel.WithoutNewOnly;
      }
      else
      {
        if ((object) (methodBase as ConstructorInfo) == null)
          throw new NotImplementedException();
        if (!MethodProxy.WrapperCache.TryGetValue(methodBase, out this._fastWrapper))
          MethodProxy.WrapperCache[methodBase] = this._fastWrapper = this.makeFastWrapper(methodBase as ConstructorInfo);
        if (this._parameters.Length == 0 || ((IEnumerable<ParameterInfo>) this._parameters).Last<ParameterInfo>().GetCustomAttribute(typeof (ParamArrayAttribute), false) == null)
          return;
        this._restPrmsArrayCreator = this.makeRestPrmsArrayCreator();
      }
    }

    private MethodProxy(
      Context context,
      object hardTarget,
      MethodBase method,
      ParameterInfo[] parameters,
      MethodProxy.WrapperDelegate fastWrapper,
      bool forceInstance)
      : base(context)
    {
      this._hardTarget = hardTarget;
      this._method = method;
      this._parameters = parameters;
      this._fastWrapper = fastWrapper;
      this._forceInstance = forceInstance;
      this.RequireNewKeywordLevel = RequireNewKeywordLevel.WithoutNewOnly;
    }

    private MethodProxy.RestPrmsConverter makeRestPrmsArrayCreator()
    {
      MethodInfo methodInfo1 = new Func<int, JSValue, object>(this.convertArgument).GetMethodInfo();
      MethodInfo methodInfo2 = new Func<NiL.JS.Expressions.Expression[], Context, int, object>(this.processArgument).GetMethodInfo();
      ParameterExpression parameterExpression4 = System.Linq.Expressions.Expression.Parameter(typeof (Context), "context");
      ParameterExpression array = System.Linq.Expressions.Expression.Parameter(typeof (NiL.JS.Expressions.Expression[]), "arguments");
      ParameterExpression parameterExpression5 = System.Linq.Expressions.Expression.Parameter(typeof (Arguments), "argumentsObject");
      Type elementType = ((IEnumerable<ParameterInfo>) this._parameters).Last<ParameterInfo>().ParameterType.GetElementType();
      LabelTarget target = System.Linq.Expressions.Expression.Label("return");
      ParameterExpression parameterExpression6 = System.Linq.Expressions.Expression.Variable(typeof (int), "argumentIndex");
      ParameterExpression parameterExpression7 = System.Linq.Expressions.Expression.Variable(((IEnumerable<ParameterInfo>) this._parameters).Last<ParameterInfo>().ParameterType, "resultArray");
      ParameterExpression left1 = System.Linq.Expressions.Expression.Variable(typeof (int), "resultArrayIndex");
      ParameterExpression left2 = System.Linq.Expressions.Expression.Variable(typeof (object), "temp");
      ConstructorInfo constructor = TypeExtensions.GetConstructor(parameterExpression7.Type, new Type[1]
      {
        typeof (int)
      });
      MethodCallExpression methodCallExpression = System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) this), methodInfo1, (System.Linq.Expressions.Expression) parameterExpression6, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) parameterExpression5, MethodProxy.ArgumentsGetItemMethod, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.PostIncrementAssign((System.Linq.Expressions.Expression) parameterExpression6)));
      BinaryExpression test1 = System.Linq.Expressions.Expression.GreaterThanOrEqual((System.Linq.Expressions.Expression) parameterExpression6, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.PropertyOrField((System.Linq.Expressions.Expression) parameterExpression5, "Length"));
      BinaryExpression ifFalse = System.Linq.Expressions.Expression.Assign((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.ArrayAccess((System.Linq.Expressions.Expression) parameterExpression7, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.PostIncrementAssign((System.Linq.Expressions.Expression) left1)), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) methodCallExpression, elementType));
      BinaryExpression test2 = System.Linq.Expressions.Expression.GreaterThanOrEqual((System.Linq.Expressions.Expression) parameterExpression6, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.ArrayLength((System.Linq.Expressions.Expression) array));
      MethodCallExpression right = System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) this), methodInfo2, (System.Linq.Expressions.Expression) array, (System.Linq.Expressions.Expression) parameterExpression4, (System.Linq.Expressions.Expression) parameterExpression6);
      BinaryExpression binaryExpression = System.Linq.Expressions.Expression.Assign((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.ArrayAccess((System.Linq.Expressions.Expression) parameterExpression7, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.PostIncrementAssign((System.Linq.Expressions.Expression) left1)), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) right, elementType));
      System.Linq.Expressions.Expression[] expressionArray = new System.Linq.Expressions.Expression[5]
      {
        (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Assign((System.Linq.Expressions.Expression) parameterExpression6, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) (this._parameters.Length - 1))),
        (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Assign((System.Linq.Expressions.Expression) left1, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) 0)),
        (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.IfThenElse((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.NotEqual((System.Linq.Expressions.Expression) parameterExpression5, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) null)), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Block((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.IfThen((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.PropertyOrField((System.Linq.Expressions.Expression) parameterExpression5, "Length"), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) this._parameters.Length)), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Block((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Assign((System.Linq.Expressions.Expression) left2, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) this), methodInfo1, (System.Linq.Expressions.Expression) parameterExpression6, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) parameterExpression5, MethodProxy.ArgumentsGetItemMethod, (System.Linq.Expressions.Expression) parameterExpression6))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.IfThen((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.NotEqual((System.Linq.Expressions.Expression) left2, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) null)), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Return(target, (System.Linq.Expressions.Expression) left2)))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Assign((System.Linq.Expressions.Expression) parameterExpression7, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.New(constructor, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Subtract((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.PropertyOrField((System.Linq.Expressions.Expression) parameterExpression5, "Length"), (System.Linq.Expressions.Expression) parameterExpression6))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Loop((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.IfThenElse((System.Linq.Expressions.Expression) test1, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Return(target, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Assign((System.Linq.Expressions.Expression) left2, (System.Linq.Expressions.Expression) parameterExpression7)), (System.Linq.Expressions.Expression) ifFalse))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Block((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.IfThen((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.ArrayLength((System.Linq.Expressions.Expression) array), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) this._parameters.Length)), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Block((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Assign((System.Linq.Expressions.Expression) left2, (System.Linq.Expressions.Expression) right), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.IfThen((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.TypeIs((System.Linq.Expressions.Expression) left2, ((IEnumerable<ParameterInfo>) this._parameters).Last<ParameterInfo>().ParameterType), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Return(target, (System.Linq.Expressions.Expression) left2)), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Assign((System.Linq.Expressions.Expression) left2, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.NewArrayInit(elementType, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) left2, elementType))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Return(target, (System.Linq.Expressions.Expression) left2))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Assign((System.Linq.Expressions.Expression) parameterExpression7, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.New(constructor, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Subtract((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.ArrayLength((System.Linq.Expressions.Expression) array), (System.Linq.Expressions.Expression) parameterExpression6))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Loop((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.IfThenElse((System.Linq.Expressions.Expression) test2, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Return(target, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Assign((System.Linq.Expressions.Expression) left2, (System.Linq.Expressions.Expression) parameterExpression7)), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Block((System.Linq.Expressions.Expression) binaryExpression, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.PostIncrementAssign((System.Linq.Expressions.Expression) parameterExpression6)))))),
        (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Label(target),
        (System.Linq.Expressions.Expression) left2
      };
      return ((Expression<MethodProxy.RestPrmsConverter>) ((parameterExpression1, parameterExpression2, parameterExpression3) => System.Linq.Expressions.Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[4]
      {
        parameterExpression6,
        parameterExpression7,
        left1,
        left2
      }, expressionArray))).Compile();
    }

    private MethodProxy.WrapperDelegate makeFastWrapper(MethodInfo methodInfo)
    {
      ParameterExpression parameterExpression1 = System.Linq.Expressions.Expression.Parameter(typeof (object), "target");
      ParameterExpression parameterExpression2 = System.Linq.Expressions.Expression.Parameter(typeof (Context), "context");
      ParameterExpression parameterExpression3 = System.Linq.Expressions.Expression.Parameter(typeof (NiL.JS.Expressions.Expression[]), "arguments");
      ParameterExpression parameterExpression4 = System.Linq.Expressions.Expression.Parameter(typeof (Arguments), "argumentsObject");
      ConditionalExpression conditionalExpression = System.Linq.Expressions.Expression.Condition((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.NotEqual((System.Linq.Expressions.Expression) parameterExpression4, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) null)), (System.Linq.Expressions.Expression) parameterExpression4, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Assign((System.Linq.Expressions.Expression) parameterExpression4, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(new Func<NiL.JS.Expressions.Expression[], Context, Arguments>(Tools.CreateArguments).GetMethodInfo(), (System.Linq.Expressions.Expression) parameterExpression3, (System.Linq.Expressions.Expression) parameterExpression2)));
      System.Linq.Expressions.Expression expression;
      if (this._parameters.Length == 0)
        expression = !this._forceInstance ? (!methodInfo.IsStatic ? (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) parameterExpression1, methodInfo.DeclaringType), methodInfo) : (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(methodInfo)) : (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(methodInfo, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) parameterExpression1, typeof (JSValue)));
      else if ((this._parameters.Length == 1 || this._parameters.Length == 2 && this._forceInstance) && (object) ((IEnumerable<ParameterInfo>) this._parameters).Last<ParameterInfo>().ParameterType == (object) typeof (Arguments))
      {
        if (this._forceInstance)
          expression = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(methodInfo, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) parameterExpression1, typeof (JSValue)), (System.Linq.Expressions.Expression) conditionalExpression);
        else if (methodInfo.IsStatic)
          expression = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(methodInfo, (System.Linq.Expressions.Expression) conditionalExpression);
        else
          expression = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) parameterExpression1, methodInfo.DeclaringType), methodInfo, (System.Linq.Expressions.Expression) conditionalExpression);
      }
      else
      {
        MethodInfo methodInfo1 = new Func<NiL.JS.Expressions.Expression[], Context, int, object>(this.processArgument).GetMethodInfo();
        MethodInfo methodInfo2 = new Func<NiL.JS.Expressions.Expression[], Context, int, object>(this.processArgumentsTail).GetMethodInfo();
        MethodInfo methodInfo3 = new Func<int, JSValue, object>(this.convertArgument).GetMethodInfo();
        System.Linq.Expressions.Expression[] expressionArray = new System.Linq.Expressions.Expression[this._parameters.Length + (this._forceInstance ? 1 : 0)];
        if (this._restPrmsArrayCreator != null)
          expressionArray[expressionArray.Length - 1] = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) this), new Func<Context, NiL.JS.Expressions.Expression[], Arguments, object>(this.callRestPrmsConverter).GetMethodInfo(), (System.Linq.Expressions.Expression) parameterExpression2, (System.Linq.Expressions.Expression) parameterExpression3, (System.Linq.Expressions.Expression) parameterExpression4), this._parameters[this._parameters.Length - 1].ParameterType);
        int index1 = 0;
        if (this._forceInstance)
          expressionArray[index1++] = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) parameterExpression1, typeof (JSValue));
        int index2 = 0;
        for (; index1 < expressionArray.Length; ++index1)
        {
          if (index1 != expressionArray.Length - 1 || this._restPrmsArrayCreator == null)
            expressionArray[index1] = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) this), index1 + 1 < expressionArray.Length ? methodInfo1 : methodInfo2, (System.Linq.Expressions.Expression) parameterExpression3, (System.Linq.Expressions.Expression) parameterExpression2, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) index2)), this._parameters[index2].ParameterType);
          ++index2;
        }
        System.Linq.Expressions.Expression ifTrue = !methodInfo.IsStatic ? (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) parameterExpression1, methodInfo.DeclaringType), methodInfo, expressionArray) : (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(methodInfo, expressionArray);
        int index3 = 0;
        if (this._forceInstance)
          ++index3;
        int index4 = 0;
        for (; index3 < expressionArray.Length; ++index3)
        {
          if (index3 != expressionArray.Length - 1 || this._restPrmsArrayCreator == null)
            expressionArray[index3] = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) this), methodInfo3, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) index4), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) parameterExpression4, MethodProxy.ArgumentsGetItemMethod, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) index4))), this._parameters[index4].ParameterType);
          ++index4;
        }
        System.Linq.Expressions.Expression ifFalse = !methodInfo.IsStatic ? (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) parameterExpression1, methodInfo.DeclaringType), methodInfo, expressionArray) : (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(methodInfo, expressionArray);
        expression = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Condition((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal((System.Linq.Expressions.Expression) parameterExpression4, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) null)), ifTrue, ifFalse);
      }
      if ((object) methodInfo.ReturnType == (object) typeof (void))
        expression = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Block(expression, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) null));
      try
      {
        return System.Linq.Expressions.Expression.Lambda<MethodProxy.WrapperDelegate>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert(expression, typeof (object)), methodInfo.Name, (IEnumerable<ParameterExpression>) new ParameterExpression[4]
        {
          parameterExpression1,
          parameterExpression2,
          parameterExpression3,
          parameterExpression4
        }).Compile();
      }
      catch
      {
        throw;
      }
    }

    private MethodProxy.WrapperDelegate makeFastWrapper(ConstructorInfo constructorInfo)
    {
      ParameterExpression parameterExpression1 = System.Linq.Expressions.Expression.Parameter(typeof (object), "target");
      ParameterExpression parameterExpression2 = System.Linq.Expressions.Expression.Parameter(typeof (Context), "context");
      ParameterExpression parameterExpression3 = System.Linq.Expressions.Expression.Parameter(typeof (NiL.JS.Expressions.Expression[]), "arguments");
      ParameterExpression parameterExpression4 = System.Linq.Expressions.Expression.Parameter(typeof (Arguments), "argumentsObject");
      ConditionalExpression instance = System.Linq.Expressions.Expression.Condition((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.NotEqual((System.Linq.Expressions.Expression) parameterExpression4, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) null)), (System.Linq.Expressions.Expression) parameterExpression4, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Assign((System.Linq.Expressions.Expression) parameterExpression4, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(new Func<NiL.JS.Expressions.Expression[], Context, Arguments>(Tools.CreateArguments).GetMethodInfo(), (System.Linq.Expressions.Expression) parameterExpression3, (System.Linq.Expressions.Expression) parameterExpression2)));
      System.Linq.Expressions.Expression expression;
      if (this._parameters.Length == 0)
        expression = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.New(constructorInfo);
      else if (this._parameters.Length == 1 && (object) this._parameters[0].ParameterType == (object) typeof (Arguments))
      {
        expression = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.New(constructorInfo, (System.Linq.Expressions.Expression) instance);
      }
      else
      {
        Func<NiL.JS.Expressions.Expression[], Context, int, object> del1 = new Func<NiL.JS.Expressions.Expression[], Context, int, object>(this.processArgument);
        Func<NiL.JS.Expressions.Expression[], Context, int, object> del2 = new Func<NiL.JS.Expressions.Expression[], Context, int, object>(this.processArgumentsTail);
        Func<int, JSValue, object> del3 = new Func<int, JSValue, object>(this.convertArgument);
        System.Linq.Expressions.Expression[] expressionArray = new System.Linq.Expressions.Expression[this._parameters.Length];
        for (int index = 0; index < expressionArray.Length; ++index)
          expressionArray[index] = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) this), index + 1 < expressionArray.Length ? del1.GetMethodInfo() : del2.GetMethodInfo(), (System.Linq.Expressions.Expression) parameterExpression3, (System.Linq.Expressions.Expression) parameterExpression2, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) index)), this._parameters[index].ParameterType);
        System.Linq.Expressions.Expression ifTrue = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.New(constructorInfo, expressionArray);
        for (int index = 0; index < expressionArray.Length; ++index)
          expressionArray[index] = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) this), del3.GetMethodInfo(), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) index), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) instance, MethodProxy.ArgumentsGetItemMethod, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) index))), this._parameters[index].ParameterType);
        System.Linq.Expressions.Expression ifFalse = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.New(constructorInfo, expressionArray);
        expression = (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Condition((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal((System.Linq.Expressions.Expression) parameterExpression4, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) null)), ifTrue, ifFalse);
      }
      try
      {
        return System.Linq.Expressions.Expression.Lambda<MethodProxy.WrapperDelegate>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert(expression, typeof (object)), constructorInfo.DeclaringType.Name, (IEnumerable<ParameterExpression>) new ParameterExpression[4]
        {
          parameterExpression1,
          parameterExpression2,
          parameterExpression3,
          parameterExpression4
        }).Compile();
      }
      catch
      {
        throw;
      }
    }

    private object callRestPrmsConverter(
      Context initiator,
      NiL.JS.Expressions.Expression[] arguments,
      Arguments argumentsObject)
    {
      return this._restPrmsArrayCreator(initiator, arguments, argumentsObject);
    }

    internal override JSValue InternalInvoke(
      JSValue targetValue,
      NiL.JS.Expressions.Expression[] argumentsSource,
      Context initiator,
      bool withSpread,
      bool withNew)
    {
      if (withNew)
      {
        if (this.RequireNewKeywordLevel == RequireNewKeywordLevel.WithoutNewOnly)
          ExceptionHelper.ThrowTypeError(string.Format(Strings.InvalidTryToCreateWithNew, (object) this.name));
      }
      else if (this.RequireNewKeywordLevel == RequireNewKeywordLevel.WithNewOnly)
        ExceptionHelper.ThrowTypeError(string.Format(Strings.InvalidTryToCreateWithoutNew, (object) this.name));
      return this.Context.GlobalContext.ProxyValue(this.invokeMethod(targetValue, argumentsSource, (Arguments) null, initiator));
    }

    private object invokeMethod(
      JSValue targetValue,
      NiL.JS.Expressions.Expression[] argumentsSource,
      Arguments argumentsObject,
      Context initiator)
    {
      object targetObject = this.GetTargetObject(targetValue, this._hardTarget);
      object source;
      try
      {
        if (this._parameters.Length == 0 && argumentsSource != null)
        {
          for (int index = 0; index < argumentsSource.Length; ++index)
            argumentsSource[index].Evaluate(initiator);
        }
        source = this._fastWrapper(targetObject, initiator, argumentsSource, argumentsObject);
        if (this._returnConverter != null)
          source = this._returnConverter.From(source);
      }
      catch (Exception ex)
      {
        Exception innerException = ex;
        while (innerException.InnerException != null)
          innerException = innerException.InnerException;
        if (innerException is JSException)
          throw innerException;
        ExceptionHelper.Throw((Error) new TypeError(innerException.Message), innerException);
        throw;
      }
      return source;
    }

    private object processArgumentsTail(NiL.JS.Expressions.Expression[] arguments, Context context, int index)
    {
      object obj = this.processArgument(arguments, context, index);
      while (++index < arguments.Length)
        arguments[index].Evaluate(context);
      return obj;
    }

    internal object GetTargetObject(JSValue targetValue, object hardTarget)
    {
      object targetObject = hardTarget;
      if (targetObject == null)
      {
        if (this._forceInstance)
        {
          if (targetValue != null && targetValue._valueType >= JSValueType.Object)
          {
            targetObject = targetValue.Value;
            if (targetObject is Proxy proxy)
              targetObject = (object) proxy.PrototypeInstance ?? targetValue.Value;
            if (!(targetObject is JSValue))
              targetObject = (object) targetValue;
          }
          else
            targetObject = (object) (targetValue ?? JSValue.undefined);
        }
        else if (!this._method.IsStatic && !this._method.IsConstructor)
        {
          targetObject = MethodProxy.convertTargetObject(targetValue ?? JSValue.undefined, this._method.DeclaringType);
          if (targetObject == null)
          {
            if (this._method.Name == "get_length" && TypeExtensions.IsAssignableFrom(typeof (Function), this._method.DeclaringType))
              return (object) Function.Empty;
            ExceptionHelper.Throw((Error) new TypeError("Can not call function \"" + this.name + "\" for object of another type."));
          }
        }
      }
      return targetObject;
    }

    private static object convertTargetObject(JSValue target, Type targetType)
    {
      if (target == null)
        return (object) null;
      if (!(target._oValue is JSValue jsValue))
        jsValue = target;
      target = jsValue;
      return Tools.convertJStoObj(target, targetType, false);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private object processArgument(NiL.JS.Expressions.Expression[] arguments, Context initiator, int index)
    {
      JSValue jsValue = arguments.Length > index ? Tools.EvalExpressionSafe(initiator, arguments[index]) : JSValue.notExists;
      return this.convertArgument(index, jsValue);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private object convertArgument(int index, JSValue value)
    {
      ConvertArgsOptions options = ConvertArgsOptions.ThrowOnError;
      if (this._strictConversion)
        options |= ConvertArgsOptions.StrictConversion;
      return this.convertArgument(index, value, options);
    }

    private object convertArgument(int index, JSValue value, ConvertArgsOptions options)
    {
      if (this._paramsConverters != null && this._paramsConverters[index] != null)
        return this._paramsConverters[index].To(value);
      bool flag = (options & ConvertArgsOptions.StrictConversion) == ConvertArgsOptions.StrictConversion;
      int num = this._restPrmsArrayCreator == null || index < this._parameters.Length - 1 ? 0 : (index >= this._parameters.Length || value.ValueType != JSValueType.Object ? 1 : (!(value.Value is NiL.JS.BaseLibrary.Array) ? 1 : 0));
      ParameterInfo parameterInfo = num != 0 ? this._parameters[this._parameters.Length - 1] : this._parameters[index];
      Type type = num != 0 ? parameterInfo.ParameterType.GetElementType() : parameterInfo.ParameterType;
      object obj = (object) null;
      if (value._valueType >= JSValueType.Object && value._oValue == null && type.GetTypeInfo().IsClass)
        return (object) null;
      if (value._valueType > JSValueType.Undefined)
      {
        obj = Tools.convertJStoObj(value, type, !flag);
        if (flag && obj == null)
        {
          if (options.HasFlag((Enum) ConvertArgsOptions.ThrowOnError))
            ExceptionHelper.ThrowTypeError("Unable to convert " + value?.ToString() + " to type " + type?.ToString());
          if (!options.HasFlag((Enum) ConvertArgsOptions.DummyValues))
            return (object) null;
        }
      }
      else if (TypeExtensions.IsAssignableFrom(type, value.GetType()))
        return (object) value;
      if (obj == null && this._restPrmsArrayCreator == null && (options.HasFlag((Enum) ConvertArgsOptions.DummyValues) || parameterInfo.Attributes.HasFlag((Enum) ParameterAttributes.HasDefault)))
      {
        obj = parameterInfo.DefaultValue;
        if (obj != null && obj.GetType().FullName == "System.DBNull")
        {
          if (flag && options.HasFlag((Enum) ConvertArgsOptions.ThrowOnError))
            ExceptionHelper.ThrowTypeError("Unable to convert " + value?.ToString() + " to type " + type?.ToString());
          obj = !type.GetTypeInfo().IsValueType ? (object) null : Activator.CreateInstance(type);
        }
      }
      return obj;
    }

    internal object[] ConvertArguments(Arguments arguments, ConvertArgsOptions options)
    {
      if (this._parameters.Length == 0)
        return (object[]) null;
      if (this._forceInstance)
        ExceptionHelper.Throw((Exception) new InvalidOperationException());
      object[] objArray = (object[]) null;
      int length = this._parameters.Length;
      int index = length;
      while (index-- > 0)
      {
        JSValue jsValue = arguments?[index] ?? JSValue.undefined;
        object obj = this.convertArgument(index, jsValue, options);
        if (obj == null && !jsValue.IsNull)
          return (object[]) null;
        if (objArray == null)
          objArray = new object[length];
        objArray[index] = obj;
      }
      return objArray;
    }

    protected internal override JSValue Invoke(
      bool construct,
      JSValue targetObject,
      Arguments arguments)
    {
      if (arguments == null)
        arguments = new Arguments();
      object obj = this.invokeMethod(targetObject, (NiL.JS.Expressions.Expression[]) null, arguments, this.Context);
      if (obj == null)
        return JSValue.undefined;
      return obj is JSValue jsValue ? jsValue : this.Context.GlobalContext.ProxyValue(obj);
    }

    public override sealed Function bind(Arguments args) => this._hardTarget != null || args.Length == 0 ? (Function) this : (Function) new MethodProxy(this.Context, MethodProxy.convertTargetObject(args[0], this._method.DeclaringType) ?? (args[0].Value is JSObject jsObject ? (object) jsObject : (object) args[0]), this._method, this._parameters, this._fastWrapper, this._forceInstance);

    public override Delegate MakeDelegate(Type delegateType)
    {
      try
      {
        return (this._method as MethodInfo).CreateDelegate(delegateType, this._hardTarget);
      }
      catch
      {
      }
      return base.MakeDelegate(delegateType);
    }

    public override string ToString(bool headerOnly)
    {
      string str = "function " + this.name + "()";
      if (!headerOnly)
        str += " { [native code] }";
      return str;
    }

    private delegate object WrapperDelegate(
      object target,
      Context initiator,
      NiL.JS.Expressions.Expression[] arguments,
      Arguments argumentsObject);

    private delegate object RestPrmsConverter(
      Context initiator,
      NiL.JS.Expressions.Expression[] arguments,
      Arguments argumentsObject);
  }
}
