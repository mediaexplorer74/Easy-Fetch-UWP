// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Interop.Proxy
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NiL.JS.Core.Interop
{
  internal abstract class Proxy : JSObject
  {
    internal Type _hostedType;
    internal StringMap<IList<MemberInfo>> _members;
    internal GlobalContext _context;
    private PropertyPair _indexerProperty;
    private bool _indexersSupported;
    private ConstructorInfo _instanceCtor;
    private JSObject _prototypeInstance;

    internal virtual JSObject PrototypeInstance
    {
      get
      {
        if (this._prototypeInstance == null && this.IsInstancePrototype && !this._hostedType.GetTypeInfo().IsAbstract && (object) this._instanceCtor != null)
        {
          if ((object) this._hostedType == (object) typeof (JSObject))
          {
            this._prototypeInstance = JSObject.CreateObject();
            this._prototypeInstance._objectPrototype = JSValue.@null;
            this._prototypeInstance._fields = this._fields;
            this._prototypeInstance._attributes |= JSValueAttributesInternal.ProxyPrototype;
          }
          else if (TypeExtensions.IsAssignableFrom(typeof (JSObject), this._hostedType))
          {
            this._prototypeInstance = this._instanceCtor.Invoke((object[]) null) as JSObject;
            this._prototypeInstance._objectPrototype = this.__proto__;
            this._prototypeInstance._attributes |= JSValueAttributesInternal.ProxyPrototype;
            this._prototypeInstance._fields = this._fields;
            this._valueType = (JSValueType) System.Math.Max(131, (int) this._prototypeInstance._valueType);
          }
          else
          {
            ObjectWrapper objectWrapper = new ObjectWrapper(this._instanceCtor.Invoke((object[]) null), (JSObject) this);
            objectWrapper._attributes = this._attributes | JSValueAttributesInternal.ProxyPrototype;
            objectWrapper._fields = this._fields;
            objectWrapper._objectPrototype = this._context.GlobalContext._globalPrototype;
            this._prototypeInstance = (JSObject) objectWrapper;
          }
        }
        return this._prototypeInstance;
      }
    }

    internal abstract bool IsInstancePrototype { get; }

    internal Proxy(GlobalContext context, Type type, bool indexersSupport)
    {
      this._indexersSupported = indexersSupport;
      this._valueType = JSValueType.Object;
      this._oValue = (object) this;
      this._attributes |= JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.SystemObject;
      this._fields = JSObject.getFieldsContainer();
      this._context = context;
      this._hostedType = type;
      this._instanceCtor = this._hostedType.GetTypeInfo().DeclaredConstructors.Where<ConstructorInfo>((Func<ConstructorInfo, bool>) (x => x.IsPublic)).FirstOrDefault<ConstructorInfo>((Func<ConstructorInfo, bool>) (x => x.GetParameters().Length == 0 && !x.IsStatic));
    }

    private void fillMembers()
    {
      lock (this)
      {
        if (this._members != null)
          return;
        StringMap<IList<MemberInfo>> stringMap1 = new StringMap<IList<MemberInfo>>();
        string str1 = (string) null;
        IList<MemberInfo> memberInfoList = (IList<MemberInfo>) null;
        MemberInfo[] array1 = this._hostedType.GetTypeInfo().DeclaredMembers.Union<MemberInfo>((IEnumerable<MemberInfo>) this._hostedType.GetRuntimeMethods()).Union<MemberInfo>((IEnumerable<MemberInfo>) this._hostedType.GetRuntimeProperties()).Union<MemberInfo>((IEnumerable<MemberInfo>) this._hostedType.GetRuntimeFields()).Union<MemberInfo>((IEnumerable<MemberInfo>) this._hostedType.GetRuntimeEvents()).ToArray<MemberInfo>();
        for (int index1 = 0; index1 < array1.Length; ++index1)
        {
          MemberInfo element = array1[index1];
          if (!CustomAttributeExtensions.IsDefined(element, typeof (HiddenAttribute), false))
          {
            bool flag = CustomAttributeExtensions.IsDefined(element, typeof (InstanceMemberAttribute), false);
            if (!(!this.IsInstancePrototype & flag))
            {
              PropertyInfo propertyInfo1 = element as PropertyInfo;
              if ((object) propertyInfo1 != null)
              {
                MethodInfo methodInfo1 = PropertyInfoExtensions.GetSetMethod(propertyInfo1, true);
                if ((object) methodInfo1 == null)
                  methodInfo1 = PropertyInfoExtensions.GetGetMethod(propertyInfo1, true);
                if (methodInfo1.IsStatic == (this.IsInstancePrototype == flag) && ((object) PropertyInfoExtensions.GetSetMethod(propertyInfo1, true) != null && PropertyInfoExtensions.GetSetMethod(propertyInfo1, true).IsPublic || (object) PropertyInfoExtensions.GetGetMethod(propertyInfo1, true) != null && PropertyInfoExtensions.GetGetMethod(propertyInfo1, true).IsPublic))
                {
                  try
                  {
                    Type baseType;
                    for (PropertyInfo propertyInfo2 = propertyInfo1; (object) propertyInfo2 != null; propertyInfo2 = (object) baseType != null ? baseType.GetRuntimeProperty(propertyInfo1.Name) : (PropertyInfo) null)
                    {
                      if ((object) propertyInfo2.DeclaringType != (object) typeof (object))
                      {
                        MethodInfo methodInfo2 = PropertyInfoExtensions.GetGetMethod(propertyInfo1);
                        if ((object) methodInfo2 == null)
                          methodInfo2 = PropertyInfoExtensions.GetSetMethod(propertyInfo1);
                        if ((methodInfo2.Attributes & MethodAttributes.VtableLayoutMask) == MethodAttributes.PrivateScope)
                        {
                          propertyInfo1 = propertyInfo2;
                          baseType = propertyInfo1.DeclaringType.GetTypeInfo().BaseType;
                        }
                        else
                          break;
                      }
                      else
                        break;
                    }
                  }
                  catch (AmbiguousMatchException ex)
                  {
                    continue;
                  }
                  element = (MemberInfo) propertyInfo1;
                }
                else
                  continue;
              }
              if ((object) (element as EventInfo) == null || EventInfoExtensions.GetAddMethod(element as EventInfo, true).IsPublic && EventInfoExtensions.GetAddMethod(element as EventInfo, true).IsStatic == !this.IsInstancePrototype)
              {
                FieldInfo fieldInfo = element as FieldInfo;
                if (((object) fieldInfo == null || fieldInfo.IsPublic && fieldInfo.IsStatic == !this.IsInstancePrototype) && (!(array1[index1] is TypeInfo) || (array1[index1] as TypeInfo).IsPublic))
                {
                  MethodBase methodBase1 = element as MethodBase;
                  if ((object) methodBase1 != null)
                  {
                    if (methodBase1.IsStatic == (this.IsInstancePrototype == flag) && (methodBase1.IsPublic || (object) methodBase1.DeclaringType == (object) typeof (Enum) && methodBase1.Name == "GetValue") && ((object) methodBase1.DeclaringType != (object) typeof (object) || !(element.Name == "GetType")) && (object) (methodBase1 as ConstructorInfo) == null)
                    {
                      if ((object) methodBase1.DeclaringType == (object) typeof (Enum) && methodBase1.Name == "GetValue")
                      {
                        stringMap1.Add("valueOf", (IList<MemberInfo>) new MemberInfo[1]
                        {
                          element
                        });
                        continue;
                      }
                      Type[] array2 = ((IEnumerable<ParameterInfo>) methodBase1.GetParameters()).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (x => x.ParameterType)).ToArray<Type>();
                      if (!((IEnumerable<Type>) array2).Any<Type>((Func<Type, bool>) (x => x.IsByRef)))
                      {
                        Type baseType;
                        if (methodBase1.IsVirtual && (methodBase1.Attributes & MethodAttributes.VtableLayoutMask) == MethodAttributes.PrivateScope)
                        {
                          for (MethodBase methodBase2 = methodBase1; (object) methodBase2 != null && (object) methodBase2.DeclaringType != (object) typeof (object) && (methodBase1.Attributes & MethodAttributes.VtableLayoutMask) == MethodAttributes.PrivateScope; methodBase2 = (object) baseType != null ? (MethodBase) TypeExtensions.GetMethod(baseType, methodBase1.Name, array2) : (MethodBase) null)
                          {
                            methodBase1 = methodBase2;
                            baseType = methodBase1.DeclaringType.GetTypeInfo().BaseType;
                          }
                        }
                        element = (MemberInfo) methodBase1;
                      }
                      else
                        continue;
                    }
                    else
                      continue;
                  }
                  string name = element.Name;
                  string key1;
                  if (CustomAttributeExtensions.IsDefined(element, typeof (JavaScriptNameAttribute), false))
                  {
                    key1 = (CustomAttributeExtensions.GetCustomAttributes(element, typeof (JavaScriptNameAttribute), false).ToArray<Attribute>()[0] as JavaScriptNameAttribute).Name;
                  }
                  else
                  {
                    key1 = name[0] == '.' ? name : (name.Contains(".") ? name.Substring(name.LastIndexOf('.') + 1) : name);
                    if (array1[index1] is TypeInfo && key1.Contains("`"))
                      key1 = key1.Substring(0, key1.IndexOf('`'));
                  }
                  int num1;
                  if (str1 != key1)
                  {
                    if (memberInfoList != null && memberInfoList.Count > 1)
                    {
                      Type declaringType = memberInfoList[0].DeclaringType;
                      for (int index2 = 1; index2 < memberInfoList.Count; ++index2)
                      {
                        if ((object) declaringType != (object) memberInfoList[index2].DeclaringType && TypeExtensions.IsAssignableFrom(declaringType, memberInfoList[index2].DeclaringType))
                          declaringType = memberInfoList[index2].DeclaringType;
                      }
                      int num2 = 0;
                      for (int index3 = 1; index3 < memberInfoList.Count; ++index3)
                      {
                        if (!TypeExtensions.IsAssignableFrom(declaringType, memberInfoList[index3].DeclaringType))
                        {
                          memberInfoList.RemoveAt(index3--);
                          StringMap<IList<MemberInfo>> stringMap2 = stringMap1;
                          string str2 = str1;
                          num1 = ++num2 + index3;
                          string str3 = num1.ToString();
                          string key2 = str2 + "$" + str3;
                          stringMap2.Remove(key2);
                        }
                      }
                      if (memberInfoList.Count == 1)
                        stringMap1.Remove(str1 + "$0");
                    }
                    if (!stringMap1.TryGetValue(key1, out memberInfoList))
                      stringMap1[key1] = memberInfoList = (IList<MemberInfo>) new List<MemberInfo>();
                    str1 = key1;
                  }
                  if (key1.StartsWith("@@"))
                  {
                    if (this._symbols == null)
                      this._symbols = (IDictionary<Symbol, JSValue>) new Dictionary<Symbol, JSValue>();
                    this._symbols.Add(Symbol.@for(key1.Substring(2)), this.proxyMember(false, (IList<MemberInfo>) new MemberInfo[1]
                    {
                      element
                    }));
                  }
                  else
                  {
                    if (memberInfoList.Count == 1)
                      stringMap1.Add(key1 + "$0", (IList<MemberInfo>) new MemberInfo[1]
                      {
                        memberInfoList[0]
                      });
                    memberInfoList.Add(element);
                    if (memberInfoList.Count != 1)
                    {
                      StringMap<IList<MemberInfo>> stringMap3 = stringMap1;
                      string str4 = key1;
                      num1 = memberInfoList.Count - 1;
                      string str5 = num1.ToString();
                      string key3 = str4 + "$" + str5;
                      MemberInfo[] memberInfoArray = new MemberInfo[1]
                      {
                        element
                      };
                      stringMap3.Add(key3, (IList<MemberInfo>) memberInfoArray);
                    }
                  }
                }
              }
            }
          }
        }
        this._members = stringMap1;
        if (this.IsInstancePrototype)
        {
          if (TypeExtensions.IsAssignableFrom(typeof (IIterable), this._hostedType))
          {
            IList<MemberInfo> m = (IList<MemberInfo>) null;
            if (this._members.TryGetValue("iterator", out m))
            {
              if (this._symbols == null)
                this._symbols = (IDictionary<Symbol, JSValue>) new Dictionary<Symbol, JSValue>();
              this._symbols.Add(Symbol.iterator, this.proxyMember(false, m));
              this._members.Remove("iterator");
            }
          }
          ToStringTagAttribute customAttribute = this._hostedType.GetTypeInfo().GetCustomAttribute<ToStringTagAttribute>();
          if (customAttribute != null)
          {
            if (this._symbols == null)
              this._symbols = (IDictionary<Symbol, JSValue>) new Dictionary<Symbol, JSValue>();
            this._symbols.Add(Symbol.toStringTag, (JSValue) customAttribute.Tag);
          }
        }
        if (!this._indexersSupported)
          return;
        IList<MemberInfo> m1 = (IList<MemberInfo>) null;
        IList<MemberInfo> m2 = (IList<MemberInfo>) null;
        this._members.TryGetValue("get_Item", out m1);
        this._members.TryGetValue("set_Item", out m2);
        if (m1 != null || m2 != null)
        {
          this._indexerProperty = new PropertyPair();
          if (m1 != null)
          {
            this._indexerProperty.getter = (Function) this.proxyMember(false, m1);
            this._fields["get_Item"] = (JSValue) this._indexerProperty.getter;
          }
          if (m2 == null)
            return;
          this._indexerProperty.setter = (Function) this.proxyMember(false, m2);
          this._fields["set_Item"] = (JSValue) this._indexerProperty.setter;
        }
        else
          this._indexersSupported = false;
      }
    }

    protected internal override JSValue GetProperty(
      JSValue key,
      bool forWrite,
      PropertyScope memberScope)
    {
      if (this._members == null)
        this.fillMembers();
      if (memberScope == PropertyScope.Super || key._valueType == JSValueType.Symbol)
        return base.GetProperty(key, forWrite, memberScope);
      forWrite &= (this._attributes & JSValueAttributesInternal.Immutable) == JSValueAttributesInternal.None;
      string key1 = key.ToString();
      JSValue property1 = (JSValue) null;
      if (this._fields.TryGetValue(key1, out property1))
      {
        if (!property1.Exists && !forWrite)
        {
          JSValue property2 = base.GetProperty(key, false, memberScope);
          if (property2.Exists)
          {
            property1.Assign(property2);
            property1._valueType = property2._valueType;
          }
        }
        if (forWrite && property1.NeedClone)
          this._fields[key1] = property1 = property1.CloneImpl(false);
        return property1;
      }
      IList<MemberInfo> m = (IList<MemberInfo>) null;
      this._members.TryGetValue(key1, out m);
      if (m == null || m.Count == 0)
      {
        JSValue prototypeInstance = (JSValue) this.PrototypeInstance;
        JSValue property3 = prototypeInstance == null ? base.GetProperty(key, forWrite && !this._indexersSupported, memberScope) : prototypeInstance.GetProperty(key, forWrite && !this._indexersSupported, memberScope);
        if (!this._indexersSupported)
          return property3;
        if (property3.Exists)
        {
          if (forWrite && (property3._attributes & JSValueAttributesInternal.None) == JSValueAttributesInternal.SystemObject)
            property3 = prototypeInstance == null ? base.GetProperty(key, true, memberScope) : prototypeInstance.GetProperty(key, true, memberScope);
          return property3;
        }
        if (forWrite)
          return new JSValue()
          {
            _valueType = JSValueType.Property,
            _oValue = (object) new PropertyPair((Function) null, this._indexerProperty.setter.bind(new Arguments()
            {
              (JSValue) null,
              key
            }))
          };
        return new JSValue()
        {
          _valueType = JSValueType.Property,
          _oValue = (object) new PropertyPair(this._indexerProperty.getter.bind(new Arguments()
          {
            (JSValue) null,
            key
          }), (Function) null)
        };
      }
      JSValue property4 = this.proxyMember(forWrite, m);
      this._fields[key1] = property4;
      return property4;
    }

    internal JSValue proxyMember(bool forWrite, IList<MemberInfo> m)
    {
      JSValue jsValue = (JSValue) null;
      if (m.Count > 1)
      {
        for (int index = 0; index < m.Count; ++index)
        {
          if ((object) (m[index] as MethodBase) == null)
            ExceptionHelper.Throw(this._context.ProxyValue((object) new TypeError("Incompatible fields types.")));
        }
        MethodProxy[] methods = new MethodProxy[m.Count];
        for (int index = 0; index < m.Count; ++index)
          methods[index] = new MethodProxy((Context) this._context, m[index] as MethodBase);
        jsValue = (JSValue) new MethodGroup(methods);
      }
      else
      {
        switch (m[0].GetMemberType())
        {
          case NiL.JS.Backward.MemberTypes.Event:
            EventInfo eventInfo = (EventInfo) m[0];
            jsValue = new JSValue()
            {
              _valueType = JSValueType.Property,
              _oValue = (object) new PropertyPair((Function) null, (Function) new MethodProxy((Context) this._context, (MethodBase) eventInfo.AddMethod))
            };
            break;
          case NiL.JS.Backward.MemberTypes.Field:
            FieldInfo field = m[0] as FieldInfo;
            if ((field.Attributes & (FieldAttributes.InitOnly | FieldAttributes.Literal)) != FieldAttributes.PrivateScope && (field.Attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope)
            {
              jsValue = this._context.ProxyValue(field.GetValue((object) null));
              jsValue._attributes |= JSValueAttributesInternal.ReadOnly;
              break;
            }
            jsValue = new JSValue()
            {
              _valueType = JSValueType.Property,
              _oValue = (object) new PropertyPair((Function) new ExternalFunction((ExternalFunctionDelegate) ((thisBind, a) => this._context.ProxyValue(field.GetValue(field.IsStatic ? (object) null : thisBind.Value)))), !CustomAttributeExtensions.IsDefined(m[0], typeof (ReadOnlyAttribute), false) ? (Function) new ExternalFunction((ExternalFunctionDelegate) ((thisBind, a) =>
              {
                field.SetValue(field.IsStatic ? (object) null : thisBind.Value, a[0].Value);
                return (JSValue) null;
              })) : (Function) null)
            };
            jsValue._attributes = JSValueAttributesInternal.Immutable | JSValueAttributesInternal.Field;
            if ((jsValue._oValue as PropertyPair).setter == null)
            {
              jsValue._attributes |= JSValueAttributesInternal.ReadOnly;
              break;
            }
            break;
          case NiL.JS.Backward.MemberTypes.Method:
            jsValue = (JSValue) new MethodProxy((Context) this._context, (MethodBase) m[0]);
            jsValue._attributes &= ~(JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable);
            break;
          case NiL.JS.Backward.MemberTypes.Property:
            PropertyInfo element = (PropertyInfo) m[0];
            jsValue = new JSValue()
            {
              _valueType = JSValueType.Property,
              _oValue = (object) new PropertyPair(!element.CanRead || (object) element.GetMethod == null ? (Function) null : (Function) new MethodProxy((Context) this._context, (MethodBase) element.GetMethod), !element.CanWrite || (object) element.SetMethod == null || CustomAttributeExtensions.IsDefined(element, typeof (ReadOnlyAttribute), false) ? (Function) null : (Function) new MethodProxy((Context) this._context, (MethodBase) element.SetMethod))
            };
            jsValue._attributes = JSValueAttributesInternal.Immutable;
            if ((jsValue._oValue as PropertyPair).setter == null)
              jsValue._attributes |= JSValueAttributesInternal.ReadOnly;
            if (CustomAttributeExtensions.IsDefined(element, typeof (FieldAttribute), false))
            {
              jsValue._attributes |= JSValueAttributesInternal.Field;
              break;
            }
            break;
          case NiL.JS.Backward.MemberTypes.TypeInfo:
            jsValue = JSValue.GetConstructor((m[0] as TypeInfo).AsType());
            break;
        }
      }
      if (CustomAttributeExtensions.IsDefined(m[0], typeof (DoNotEnumerateAttribute), false))
        jsValue._attributes |= JSValueAttributesInternal.DoNotEnumerate;
      if (forWrite && jsValue.NeedClone)
        jsValue = jsValue.CloneImpl(false);
      int count = m.Count;
      while (count-- > 0)
      {
        if (!CustomAttributeExtensions.IsDefined(m[count], typeof (DoNotEnumerateAttribute), false))
          jsValue._attributes &= ~JSValueAttributesInternal.DoNotEnumerate;
        if (CustomAttributeExtensions.IsDefined(m[count], typeof (ReadOnlyAttribute), false))
          jsValue._attributes |= JSValueAttributesInternal.ReadOnly;
        if (CustomAttributeExtensions.IsDefined(m[count], typeof (NotConfigurable), false))
          jsValue._attributes |= JSValueAttributesInternal.NonConfigurable;
        if (CustomAttributeExtensions.IsDefined(m[count], typeof (DoNotDeleteAttribute), false))
          jsValue._attributes |= JSValueAttributesInternal.DoNotDelete;
      }
      return jsValue;
    }

    protected internal override bool DeleteProperty(JSValue name)
    {
      if (this._members == null)
        this.fillMembers();
      string str = (string) null;
      JSValue jsValue = (JSValue) null;
      if (this._fields != null && this._fields.TryGetValue(str = name.ToString(), out jsValue) && (!jsValue.Exists || (jsValue._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None))
      {
        if ((jsValue._attributes & JSValueAttributesInternal.SystemObject) == JSValueAttributesInternal.None)
          jsValue._valueType = JSValueType.NotExistsInObject;
        return this._fields.Remove(str) | this._members.Remove(str);
      }
      IList<MemberInfo> memberInfoList = (IList<MemberInfo>) null;
      if (this._members.TryGetValue(str.ToString(), out memberInfoList))
      {
        int count = memberInfoList.Count;
        while (count-- > 0)
        {
          if (CustomAttributeExtensions.IsDefined(memberInfoList[count], typeof (DoNotDeleteAttribute), false))
            return false;
        }
      }
      return this._members.Remove(str) || this.PrototypeInstance == null || this._prototypeInstance.DeleteProperty(str);
    }

    public override JSValue propertyIsEnumerable(Arguments args)
    {
      string key = args != null ? args[0].ToString() : throw new ArgumentNullException(nameof (args));
      JSValue jsValue;
      if (this._fields != null && this._fields.TryGetValue(key, out jsValue))
        return (JSValue) (jsValue.Exists && (jsValue._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None);
      IList<MemberInfo> memberInfoList = (IList<MemberInfo>) null;
      if (!this._members.TryGetValue(key, out memberInfoList))
        return (JSValue) false;
      int count = memberInfoList.Count;
      while (count-- > 0)
      {
        if (!CustomAttributeExtensions.IsDefined(memberInfoList[count], typeof (DoNotEnumerateAttribute), false))
          return (JSValue) true;
      }
      return (JSValue) false;
    }

    protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(
      bool hideNonEnumerable,
      EnumerationMode enumerationMode)
    {
      Proxy proxy = this;
      if (proxy._members == null)
        proxy.fillMembers();
      if (proxy.PrototypeInstance != null)
      {
        IEnumerator<KeyValuePair<string, JSValue>> @enum = proxy.PrototypeInstance.GetEnumerator(hideNonEnumerable, enumerationMode);
        while (@enum.MoveNext())
          yield return @enum.Current;
        @enum = (IEnumerator<KeyValuePair<string, JSValue>>) null;
      }
      else
      {
        foreach (KeyValuePair<string, JSValue> field in (IEnumerable<KeyValuePair<string, JSValue>>) proxy._fields)
        {
          if (!hideNonEnumerable || (field.Value._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None)
            yield return field;
        }
      }
      using (IEnumerator<KeyValuePair<string, IList<MemberInfo>>> enumerator = proxy._members.GetEnumerator())
      {
label_21:
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, IList<MemberInfo>> current = enumerator.Current;
          if (!proxy._fields.ContainsKey(current.Key))
          {
            int count = current.Value.Count;
            while (count-- > 0)
            {
              if (!CustomAttributeExtensions.IsDefined(current.Value[count], typeof (HiddenAttribute), false) && (!hideNonEnumerable || !CustomAttributeExtensions.IsDefined(current.Value[count], typeof (DoNotEnumerateAttribute), false)))
              {
                switch (enumerationMode)
                {
                  case EnumerationMode.KeysOnly:
                    yield return new KeyValuePair<string, JSValue>(current.Key, (JSValue) null);
                    goto label_21;
                  case EnumerationMode.RequireValues:
                  case EnumerationMode.RequireValuesForWrite:
                    yield return new KeyValuePair<string, JSValue>(current.Key, proxy._fields[current.Key] = proxy.proxyMember(enumerationMode == EnumerationMode.RequireValuesForWrite, current.Value));
                    goto label_21;
                  default:
                    goto label_21;
                }
              }
            }
          }
        }
      }
    }
  }
}
