// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.ArrayBuffer
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NiL.JS.BaseLibrary
{
  public sealed class ArrayBuffer : CustomType
  {
    internal byte[] data;

    [Hidden]
    public byte this[int index]
    {
      [Hidden] get => this.data[index];
      [Hidden] set => this.data[index] = value;
    }

    [DoNotEnumerate]
    public ArrayBuffer()
      : this(0)
    {
    }

    [DoNotEnumerate]
    public ArrayBuffer(int length)
      : this(new byte[length])
    {
    }

    [Hidden]
    public ArrayBuffer(byte[] data)
    {
      this.data = data != null ? data : throw new ArgumentNullException();
      this._attributes |= JSValueAttributesInternal.SystemObject;
    }

    public int byteLength
    {
      [Hidden] get => this.data.Length;
    }

    [Hidden]
    public ArrayBuffer slice(int begin, int end)
    {
      if (end < begin || begin >= this.data.Length || end >= this.data.Length)
        ExceptionHelper.Throw((Error) new RangeError("Invalid begin or end index"));
      ArrayBuffer arrayBuffer = new ArrayBuffer(end - begin + 1);
      int index1 = 0;
      int index2 = begin;
      while (index2 <= end)
      {
        arrayBuffer.data[index1] = this.data[index2];
        ++index2;
        ++index1;
      }
      return arrayBuffer;
    }

    [Hidden]
    public ArrayBuffer slice(int begin) => this.slice(begin, this.data.Length - 1);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public ArrayBuffer slice(Arguments args)
    {
      if (args == null)
        throw new ArgumentNullException(nameof (args));
      switch (Tools.JSObjectToInt32(args.GetProperty("length")))
      {
        case 0:
          return this;
        case 1:
          return this.slice(Tools.JSObjectToInt32(args[0]), this.data.Length - 1);
        default:
          return this.slice(Tools.JSObjectToInt32(args[0]), Tools.JSObjectToInt32(args[1]));
      }
    }

    [Hidden]
    protected internal override JSValue GetProperty(
      JSValue key,
      bool forWrite,
      PropertyScope memberScope)
    {
      if (memberScope < PropertyScope.Super && key._valueType != JSValueType.Symbol)
      {
        double d = Tools.JSObjectToDouble(key);
        uint index;
        if (!double.IsInfinity(d) && !double.IsNaN(d) && (double) (index = (uint) d) == d)
          return this.getElement((int) index);
      }
      return base.GetProperty(key, forWrite, memberScope);
    }

    private JSValue getElement(int index)
    {
      if (index < 0)
        ExceptionHelper.Throw((Error) new RangeError("Invalid array index"));
      return index >= this.data.Length ? JSValue.undefined : (JSValue) new ArrayBuffer.Element(index, this);
    }

    protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(
      bool hideNonEnumerable,
      EnumerationMode enumerationMode)
    {
      IEnumerator<KeyValuePair<string, JSValue>> be = base.GetEnumerator(hideNonEnumerable, enumerationMode);
      while (be.MoveNext())
        yield return be.Current;
      for (int i = 0; i < this.data.Length; ++i)
        yield return new KeyValuePair<string, JSValue>(Tools.Int32ToString(i), enumerationMode > EnumerationMode.KeysOnly ? this.getElement(i) : (JSValue) null);
    }

    [Hidden]
    public byte[] GetData() => this.data;

    private sealed class Element : JSValue
    {
      private int index;
      private byte[] data;

      public Element(int index, ArrayBuffer parent)
      {
        this._valueType = JSValueType.Integer;
        this.index = index;
        this._iValue = (int) parent.data[index];
        this.data = parent.data;
        this._attributes |= JSValueAttributesInternal.Reassign;
      }

      public override void Assign(JSValue value) => this.data[this.index] = (byte) Tools.JSObjectToInt32(value);
    }
  }
}
