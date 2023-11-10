// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Functions.ConstructorProxy
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NiL.JS.Core.Functions
{
  [Prototype(typeof (Function), true)]
  internal class ConstructorProxy : Function
  {
    private const int passesCount = 3;
    private static readonly object[] _emptyObjectArray = new object[0];
    internal readonly StaticProxy _staticProxy;
    private MethodProxy[] constructors;

    public override string name => this._staticProxy._hostedType.Name;

    public override JSValue prototype
    {
      get => this._prototype;
      set => this._prototype = value;
    }

    public ConstructorProxy(Context context, StaticProxy staticProxy, JSObject prototype)
      : base(context)
    {
      if (staticProxy == null)
        throw new ArgumentNullException(nameof (staticProxy));
      if (prototype == null)
        throw new ArgumentNullException(nameof (prototype));
      this._fields = staticProxy._fields;
      this._staticProxy = staticProxy;
      this._prototype = (JSValue) prototype;
      if (this._staticProxy._hostedType.GetTypeInfo().ContainsGenericParameters)
        ExceptionHelper.Throw((Error) new TypeError(this._staticProxy._hostedType.Name + " can't be created because it's generic type."));
      bool flag1 = CustomAttributeExtensions.IsDefined(staticProxy._hostedType.GetTypeInfo(), typeof (RequireNewKeywordAttribute), true);
      bool flag2 = CustomAttributeExtensions.IsDefined(staticProxy._hostedType.GetTypeInfo(), typeof (DisallowNewKeywordAttribute), true);
      if (flag1 & flag2)
        ExceptionHelper.Throw((Exception) new InvalidOperationException("Unacceptably use of " + typeof (RequireNewKeywordAttribute).Name + " and " + typeof (DisallowNewKeywordAttribute).Name + " for same type."));
      if (flag1)
        this.RequireNewKeywordLevel = RequireNewKeywordLevel.WithNewOnly;
      if (flag2)
        this.RequireNewKeywordLevel = RequireNewKeywordLevel.WithoutNewOnly;
      if (this._length == null)
      {
        Number number = new Number(0);
        number._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly;
        this._length = number;
      }
      ConstructorInfo[] array = staticProxy._hostedType.GetTypeInfo().DeclaredConstructors.Where<ConstructorInfo>((Func<ConstructorInfo, bool>) (x => x.IsPublic)).ToArray<ConstructorInfo>();
      List<MethodProxy> methodProxyList = new List<MethodProxy>(array.Length + (staticProxy._hostedType.GetTypeInfo().IsValueType ? 1 : 0));
      for (int index = 0; index < array.Length; ++index)
      {
        if (!array[index].IsStatic && (!CustomAttributeExtensions.IsDefined(array[index], typeof (HiddenAttribute), false) || CustomAttributeExtensions.IsDefined(array[index], typeof (ForceUseAttribute), true)))
        {
          methodProxyList.Add(new MethodProxy(context, (MethodBase) array[index]));
          this.length._iValue = System.Math.Max(methodProxyList[methodProxyList.Count - 1]._length._iValue, this._length._iValue);
        }
      }
      methodProxyList.Sort((Comparison<MethodProxy>) ((x, y) =>
      {
        if (x.Parameters.Length == 1 && (object) x.Parameters[0].ParameterType == (object) typeof (Arguments))
          return 1;
        return y.Parameters.Length != 1 || (object) y.Parameters[0].ParameterType != (object) typeof (Arguments) ? x.Parameters.Length - y.Parameters.Length : -1;
      }));
      this.constructors = methodProxyList.ToArray();
    }

    protected internal override JSValue GetProperty(
      JSValue key,
      bool forWrite,
      PropertyScope memberScope)
    {
      if (memberScope < PropertyScope.Super && key._valueType != JSValueType.Symbol)
      {
        string str = key.ToString();
        if (str == "prototype")
          return this.prototype;
        if (key._valueType != JSValueType.String)
          key = (JSValue) str;
        if (forWrite || str != "toString" && str != "constructor")
        {
          JSValue property1 = this._staticProxy.GetProperty(key, forWrite && memberScope == PropertyScope.Own, memberScope);
          if (property1.Exists || memberScope == PropertyScope.Own & forWrite)
          {
            if (forWrite && property1.NeedClone)
              property1 = this._staticProxy.GetProperty(key, true, memberScope);
            return property1;
          }
          JSValue property2 = this.__proto__.GetProperty(key, forWrite, memberScope);
          return memberScope == PropertyScope.Own && (property2._valueType != JSValueType.Property || (property2._attributes & JSValueAttributesInternal.Field) == JSValueAttributesInternal.None) ? JSValue.notExists : property2;
        }
      }
      return base.GetProperty(key, forWrite, memberScope);
    }

    protected internal override bool DeleteProperty(JSValue name) => this._staticProxy.DeleteProperty(name) && this.__proto__.DeleteProperty(name);

    protected internal override JSValue Invoke(
      bool construct,
      JSValue targetObject,
      Arguments arguments)
    {
      ObjectWrapper objectWrapper1 = targetObject as ObjectWrapper;
      if (!construct && (object) this._staticProxy._hostedType == (object) typeof (Date))
        return (JSValue) new Date().ToString();
      try
      {
        object instance;
        if ((object) this._staticProxy._hostedType == (object) typeof (NiL.JS.BaseLibrary.Array))
        {
          if (arguments == null)
          {
            instance = (object) new NiL.JS.BaseLibrary.Array();
          }
          else
          {
            switch (arguments._iValue)
            {
              case 0:
                instance = (object) new NiL.JS.BaseLibrary.Array();
                break;
              case 1:
                JSValue jsValue1 = arguments[0];
                switch (jsValue1._valueType)
                {
                  case JSValueType.Integer:
                    instance = (object) new NiL.JS.BaseLibrary.Array(jsValue1._iValue);
                    break;
                  case JSValueType.Double:
                    instance = (object) new NiL.JS.BaseLibrary.Array(jsValue1._dValue);
                    break;
                  default:
                    instance = (object) new NiL.JS.BaseLibrary.Array(arguments);
                    break;
                }
                break;
              default:
                instance = (object) new NiL.JS.BaseLibrary.Array(arguments);
                break;
            }
          }
        }
        else if ((arguments == null || arguments._iValue == 0) && this._staticProxy._hostedType.GetTypeInfo().IsValueType)
        {
          instance = Activator.CreateInstance(this._staticProxy._hostedType);
        }
        else
        {
          object[] args = (object[]) null;
          MethodProxy constructor = this.findConstructor(arguments, ref args);
          if (constructor == null)
            ExceptionHelper.ThrowTypeError(this._staticProxy._hostedType.Name + " can't be created.");
          if (args == null)
            args = new object[1]{ (object) arguments };
          object targetObject1 = constructor.GetTargetObject(targetObject, (object) null);
          instance = targetObject1 == null ? (constructor._method as ConstructorInfo).Invoke(args) : constructor._method.Invoke(targetObject1, args);
        }
        JSValue jsValue2 = instance as JSValue;
        if (construct)
        {
          if (jsValue2 != null)
          {
            if (jsValue2._valueType < JSValueType.Object)
            {
              objectWrapper1.instance = instance;
              if (objectWrapper1._objectPrototype == null)
                objectWrapper1._objectPrototype = jsValue2.__proto__;
              jsValue2 = (JSValue) objectWrapper1;
            }
            else if (jsValue2._oValue is JSValue)
              jsValue2._oValue = (object) jsValue2;
          }
          else
          {
            objectWrapper1.instance = instance;
            objectWrapper1._attributes |= CustomAttributeExtensions.IsDefined(this._staticProxy._hostedType.GetTypeInfo(), typeof (ImmutableAttribute), false) ? JSValueAttributesInternal.Immutable : JSValueAttributesInternal.None;
            if ((object) instance.GetType() == (object) typeof (Date))
              objectWrapper1._valueType = JSValueType.Date;
            else if (jsValue2 != null)
              objectWrapper1._valueType = (JSValueType) System.Math.Max((int) objectWrapper1._valueType, (int) jsValue2._valueType);
            jsValue2 = (JSValue) objectWrapper1;
          }
        }
        else
        {
          if ((object) this._staticProxy._hostedType == (object) typeof (JSValue) && jsValue2._oValue is JSValue && (jsValue2._oValue as JSValue)._valueType >= JSValueType.Object)
            return jsValue2._oValue as JSValue;
          JSValue jsValue3 = jsValue2;
          if (jsValue3 == null)
          {
            ObjectWrapper objectWrapper2 = new ObjectWrapper(instance);
            objectWrapper2._attributes = (JSValueAttributesInternal) (131072 | (CustomAttributeExtensions.IsDefined(this._staticProxy._hostedType.GetTypeInfo(), typeof (ImmutableAttribute), false) ? 8 : 0));
            jsValue3 = (JSValue) objectWrapper2;
          }
          jsValue2 = jsValue3;
        }
        return jsValue2;
      }
      catch (TargetInvocationException ex)
      {
        throw ex.GetBaseException();
      }
    }

    protected internal override JSValue ConstructObject()
    {
      ObjectWrapper objectWrapper = new ObjectWrapper((object) null);
      objectWrapper._objectPrototype = this.Context.GlobalContext.GetPrototype(this._staticProxy._hostedType);
      return (JSValue) objectWrapper;
    }

    private MethodProxy findConstructor(Arguments arguments, ref object[] args)
    {
      args = (object[]) null;
      int iValue = arguments == null ? 0 : arguments._iValue;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        for (int index2 = 0; index2 < this.constructors.Length; ++index2)
        {
          if (this.constructors[index2]._parameters.Length == 1 && (object) this.constructors[index2]._parameters[0].ParameterType == (object) typeof (Arguments))
            return this.constructors[index2];
          if (index1 == 1 || this.constructors[index2]._parameters.Length == iValue)
          {
            if (iValue == 0)
            {
              args = ConstructorProxy._emptyObjectArray;
            }
            else
            {
              args = this.constructors[index2].ConvertArguments(arguments, (ConvertArgsOptions) ((index1 >= 1 ? 0 : 2) | (index1 >= 2 ? 4 : 0)));
              if (args != null)
              {
                int index3 = args.Length;
                while (index3-- > 0)
                {
                  if ((args[index3] != null ? (!TypeExtensions.IsAssignableFrom(this.constructors[index2]._parameters[index3].ParameterType, args[index3].GetType()) ? 1 : 0) : (this.constructors[index2]._parameters[index3].ParameterType.GetTypeInfo().IsValueType ? 1 : 0)) != 0)
                  {
                    index3 = 0;
                    args = (object[]) null;
                  }
                }
                if (args == null)
                  continue;
              }
              else
                continue;
            }
            return this.constructors[index2];
          }
        }
      }
      return (MethodProxy) null;
    }

    protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(
      bool hideNonEnumerable,
      EnumerationMode enumerationMode)
    {
      ConstructorProxy constructorProxy = this;
      IEnumerator<KeyValuePair<string, JSValue>> e = constructorProxy.__proto__.GetEnumerator(hideNonEnumerable, enumerationMode);
      while (e.MoveNext())
        yield return e.Current;
      e = constructorProxy._staticProxy.GetEnumerator(hideNonEnumerable, enumerationMode);
      while (e.MoveNext())
        yield return e.Current;
    }

    public override string ToString(bool headerOnly)
    {
      string str = "function " + this.name + "()";
      if (!headerOnly)
        str += " { [native code] }";
      return str;
    }
  }
}
