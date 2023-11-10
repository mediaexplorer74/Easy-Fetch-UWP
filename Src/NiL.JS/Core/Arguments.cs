// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Arguments
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Interop;
using System.Collections;
using System.Collections.Generic;

namespace NiL.JS.Core
{
  [Prototype(typeof (JSObject), true)]
  public sealed class Arguments : JSObject, IEnumerable
  {
    private JSValue _a0;
    private JSValue _a1;
    private JSValue _a2;
    private JSValue _a3;
    private JSValue _a4;
    internal JSValue _callee;
    internal JSValue _caller;
    private Arguments._LengthContainer _lengthContainer;
    internal bool _suppressClone;

    public int Length => this._iValue;

    public JSValue this[int index]
    {
      get
      {
        JSValue jsValue;
        switch (index)
        {
          case 0:
            jsValue = this._a0;
            break;
          case 1:
            jsValue = this._a1;
            break;
          case 2:
            jsValue = this._a2;
            break;
          case 3:
            jsValue = this._a3;
            break;
          case 4:
            jsValue = this._a4;
            break;
          default:
            return this[index.ToString()];
        }
        return jsValue ?? JSValue.notExists;
      }
      set
      {
        switch (index)
        {
          case 0:
            this._a0 = value;
            break;
          case 1:
            this._a1 = value;
            break;
          case 2:
            this._a2 = value;
            break;
          case 3:
            this._a3 = value;
            break;
          case 4:
            this._a4 = value;
            break;
          default:
            if (this._fields == null)
              this._fields = JSObject.getFieldsContainer();
            this._fields[index.ToString()] = value;
            break;
        }
      }
    }

    internal Arguments(Context callerContext)
      : this()
    {
      if (callerContext != null)
      {
        this._caller = !callerContext._strict || callerContext._owner == null || !callerContext._owner._functionDefinition._body._strict ? (JSValue) callerContext._owner : Function.propertiesDummySM;
        this._objectPrototype = callerContext.GlobalContext._globalPrototype;
      }
      this._suppressClone = true;
    }

    public Arguments()
    {
      this._valueType = JSValueType.Object;
      this._oValue = (object) this;
      this._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.SystemObject;
    }

    public void Add(JSValue arg) => this[this._iValue++] = arg;

    public void Add(object value) => this[this._iValue++] = JSValue.Marshal(value);

    protected internal override JSValue GetProperty(
      JSValue key,
      bool forWrite,
      PropertyScope memberScope)
    {
      if (forWrite && !this._suppressClone)
        this.cloneValues();
      if (memberScope < PropertyScope.Super && key._valueType != JSValueType.Symbol)
      {
        forWrite &= (this._attributes & JSValueAttributesInternal.Immutable) == JSValueAttributesInternal.None;
        if (key._valueType == JSValueType.Integer)
        {
          switch (key._iValue)
          {
            case 0:
              JSValue a0_1 = this._a0;
              if (a0_1 != null)
                return a0_1;
              if (!forWrite)
                return JSValue.notExists;
              JSValue jsValue1 = new JSValue();
              jsValue1._valueType = JSValueType.NotExistsInObject;
              JSValue property1 = jsValue1;
              this._a0 = jsValue1;
              return property1;
            case 1:
              JSValue a1_1 = this._a1;
              if (a1_1 != null)
                return a1_1;
              if (!forWrite)
                return JSValue.notExists;
              JSValue jsValue2 = new JSValue();
              jsValue2._valueType = JSValueType.NotExistsInObject;
              JSValue property2 = jsValue2;
              this._a1 = jsValue2;
              return property2;
            case 2:
              JSValue a2_1 = this._a2;
              if (a2_1 != null)
                return a2_1;
              if (!forWrite)
                return JSValue.notExists;
              JSValue jsValue3 = new JSValue();
              jsValue3._valueType = JSValueType.NotExistsInObject;
              JSValue property3 = jsValue3;
              this._a2 = jsValue3;
              return property3;
            case 3:
              JSValue a3_1 = this._a3;
              if (a3_1 != null)
                return a3_1;
              if (!forWrite)
                return JSValue.notExists;
              JSValue jsValue4 = new JSValue();
              jsValue4._valueType = JSValueType.NotExistsInObject;
              JSValue property4 = jsValue4;
              this._a3 = jsValue4;
              return property4;
            case 4:
              JSValue a4_1 = this._a4;
              if (a4_1 != null)
                return a4_1;
              if (!forWrite)
                return JSValue.notExists;
              JSValue jsValue5 = new JSValue();
              jsValue5._valueType = JSValueType.NotExistsInObject;
              JSValue property5 = jsValue5;
              this._a4 = jsValue5;
              return property5;
          }
        }
        else
        {
          switch (key.ToString())
          {
            case "0":
              JSValue a0_2 = this._a0;
              if (a0_2 != null)
                return a0_2;
              if (!forWrite)
                return JSValue.notExists;
              JSValue jsValue6 = new JSValue();
              jsValue6._valueType = JSValueType.NotExistsInObject;
              JSValue property6 = jsValue6;
              this._a0 = jsValue6;
              return property6;
            case "1":
              JSValue a1_2 = this._a1;
              if (a1_2 != null)
                return a1_2;
              if (!forWrite)
                return JSValue.notExists;
              JSValue jsValue7 = new JSValue();
              jsValue7._valueType = JSValueType.NotExistsInObject;
              JSValue property7 = jsValue7;
              this._a1 = jsValue7;
              return property7;
            case "2":
              JSValue a2_2 = this._a2;
              if (a2_2 != null)
                return a2_2;
              if (!forWrite)
                return JSValue.notExists;
              JSValue jsValue8 = new JSValue();
              jsValue8._valueType = JSValueType.NotExistsInObject;
              JSValue property8 = jsValue8;
              this._a2 = jsValue8;
              return property8;
            case "3":
              JSValue a3_2 = this._a3;
              if (a3_2 != null)
                return a3_2;
              if (!forWrite)
                return JSValue.notExists;
              JSValue jsValue9 = new JSValue();
              jsValue9._valueType = JSValueType.NotExistsInObject;
              JSValue property9 = jsValue9;
              this._a3 = jsValue9;
              return property9;
            case "4":
              JSValue a4_2 = this._a4;
              if (a4_2 != null)
                return a4_2;
              if (!forWrite)
                return JSValue.notExists;
              JSValue jsValue10 = new JSValue();
              jsValue10._valueType = JSValueType.NotExistsInObject;
              JSValue property10 = jsValue10;
              this._a4 = jsValue10;
              return property10;
            case "callee":
              if (this._callee == null)
                this._callee = JSValue.NotExistsInObject;
              if (forWrite && (this._callee._attributes & JSValueAttributesInternal.SystemObject) != JSValueAttributesInternal.None)
              {
                this._callee = this._callee.CloneImpl(false);
                this._callee._attributes = JSValueAttributesInternal.DoNotEnumerate;
              }
              return this._callee;
            case "caller":
              if (this._caller == null)
                this._caller = JSValue.NotExistsInObject;
              if (forWrite && (this._caller._attributes & JSValueAttributesInternal.SystemObject) != JSValueAttributesInternal.None)
              {
                this._caller = this._caller.CloneImpl(false);
                this._callee._attributes = JSValueAttributesInternal.DoNotEnumerate;
              }
              return this._caller;
            case "length":
              if (this._lengthContainer == null)
              {
                Arguments._LengthContainer lengthContainer = new Arguments._LengthContainer(this);
                lengthContainer._valueType = JSValueType.Integer;
                lengthContainer._iValue = this._iValue;
                lengthContainer._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.Reassign;
                this._lengthContainer = lengthContainer;
              }
              return (JSValue) this._lengthContainer;
          }
        }
      }
      return base.GetProperty(key, forWrite, memberScope);
    }

    protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(
      bool hideNonEnum,
      EnumerationMode enumeratorMode)
    {
      this.cloneValues();
      if (this._a0 != null && this._a0.Exists && (!hideNonEnum || (this._a0._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None))
        yield return new KeyValuePair<string, JSValue>("0", this._a0);
      if (this._a1 != null && this._a1.Exists && (!hideNonEnum || (this._a1._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None))
        yield return new KeyValuePair<string, JSValue>("1", this._a1);
      if (this._a2 != null && this._a2.Exists && (!hideNonEnum || (this._a2._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None))
        yield return new KeyValuePair<string, JSValue>("2", this._a2);
      if (this._a3 != null && this._a3.Exists && (!hideNonEnum || (this._a3._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None))
        yield return new KeyValuePair<string, JSValue>("3", this._a3);
      if (this._a4 != null && this._a4.Exists && (!hideNonEnum || (this._a4._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None))
        yield return new KeyValuePair<string, JSValue>("4", this._a4);
      if (this._callee != null && this._callee.Exists && (!hideNonEnum || (this._callee._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None))
        yield return new KeyValuePair<string, JSValue>("callee", this._callee);
      if (this._caller != null && this._caller.Exists && (!hideNonEnum || (this._caller._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None))
        yield return new KeyValuePair<string, JSValue>("caller", this._caller);
      if (this._lengthContainer != null && this._lengthContainer.Exists && (!hideNonEnum || (this._lengthContainer._attributes & JSValueAttributesInternal.DoNotEnumerate) == JSValueAttributesInternal.None))
        yield return new KeyValuePair<string, JSValue>("length", (JSValue) this._lengthContainer);
      IEnumerator<KeyValuePair<string, JSValue>> be = base.GetEnumerator(hideNonEnum, enumeratorMode);
      while (be.MoveNext())
        yield return be.Current;
    }

    private void cloneValues()
    {
      if (this._suppressClone)
        return;
      this._suppressClone = true;
      JSValueAttributesInternal resetMask = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable | JSValueAttributesInternal.SystemObject | JSValueAttributesInternal.ProxyPrototype | JSValueAttributesInternal.Temporary | JSValueAttributesInternal.Reassign;
      for (int index = 0; index < this._iValue; ++index)
      {
        if (this[index].Exists)
          this[index] = this[index].CloneImpl(false, resetMask);
      }
    }

    protected internal override bool DeleteProperty(JSValue name)
    {
      if (name._valueType == JSValueType.Integer)
      {
        switch (name._iValue)
        {
          case 0:
            if (this._a0 == null)
              return true;
            return (this._a0._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None && (this._a0 = (JSValue) null) == null;
          case 1:
            if (this._a1 == null)
              return true;
            return (this._a1._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None && (this._a1 = (JSValue) null) == null;
          case 2:
            if (this._a2 == null)
              return true;
            return (this._a2._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None && (this._a2 = (JSValue) null) == null;
          case 3:
            if (this._a3 == null)
              return true;
            return (this._a3._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None && (this._a3 = (JSValue) null) == null;
          case 4:
            if (this._a4 == null)
              return true;
            return (this._a4._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None && (this._a4 = (JSValue) null) == null;
        }
      }
      switch (name.ToString())
      {
        case "0":
          if (this._a0 == null)
            return true;
          return (this._a0._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None && (this._a0 = (JSValue) null) == null;
        case "1":
          if (this._a1 == null)
            return true;
          return (this._a1._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None && (this._a1 = (JSValue) null) == null;
        case "2":
          if (this._a2 == null)
            return true;
          return (this._a2._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None && (this._a2 = (JSValue) null) == null;
        case "3":
          if (this._a3 == null)
            return true;
          return (this._a3._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None && (this._a3 = (JSValue) null) == null;
        case "4":
          if (this._a4 == null)
            return true;
          return (this._a4._attributes & JSValueAttributesInternal.DoNotDelete) == JSValueAttributesInternal.None && (this._a4 = (JSValue) null) == null;
        default:
          return base.DeleteProperty(name);
      }
    }

    internal void Reset()
    {
      this._fields = (IDictionary<string, JSValue>) null;
      this._iValue = 0;
      this._a0 = (JSValue) null;
      this._a1 = (JSValue) null;
      this._a2 = (JSValue) null;
      this._a3 = (JSValue) null;
      this._a4 = (JSValue) null;
      this._callee = (JSValue) null;
      this._caller = (JSValue) null;
      this._objectPrototype = (JSObject) null;
      this._lengthContainer = (Arguments._LengthContainer) null;
      this._valueType = JSValueType.Object;
      this._oValue = (object) this;
      this._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.SystemObject;
    }

    private sealed class _LengthContainer : JSValue
    {
      private readonly Arguments _owner;

      public _LengthContainer(Arguments owner) => this._owner = owner;

      public override void Assign(JSValue value)
      {
        base.Assign(value);
        this._owner._iValue = Tools.JSObjectToInt32(value);
      }
    }
  }
}
