// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.Uint32Array
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System;

namespace NiL.JS.BaseLibrary
{
  public sealed class Uint32Array : TypedArray
  {
    protected override JSValue this[int index]
    {
      get
      {
        TypedArray.Element element = new TypedArray.Element((TypedArray) this, index);
        element._iValue = (int) this.getValue(index);
        if (element._iValue >= 0)
        {
          element._valueType = JSValueType.Integer;
        }
        else
        {
          element._dValue = (double) (uint) element._iValue;
          element._valueType = JSValueType.Double;
        }
        return (JSValue) element;
      }
      set
      {
        if (index < 0 || index > this.length._iValue)
          ExceptionHelper.Throw((Error) new RangeError());
        int int32 = Tools.JSObjectToInt32(value, 0, false);
        this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset] = (byte) int32;
        this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 1] = (byte) (int32 >> 8);
        this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 2] = (byte) (int32 >> 16);
        this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 3] = (byte) (int32 >> 24);
      }
    }

    private uint getValue(int index) => (uint) ((int) this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset] | (int) this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 1] << 8 | (int) this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 2] << 16 | (int) this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 3] << 24);

    public override int BYTES_PER_ELEMENT => 4;

    public Uint32Array()
    {
    }

    public Uint32Array(int length)
      : base(length)
    {
    }

    public Uint32Array(ArrayBuffer buffer)
      : base(buffer, 0, buffer.byteLength)
    {
    }

    public Uint32Array(ArrayBuffer buffer, int bytesOffset)
      : base(buffer, bytesOffset, buffer.byteLength - bytesOffset)
    {
    }

    public Uint32Array(ArrayBuffer buffer, int bytesOffset, int length)
      : base(buffer, bytesOffset, length)
    {
    }

    public Uint32Array(JSValue src)
      : base(src)
    {
    }

    [ArgumentsCount(2)]
    public override TypedArray subarray(Arguments args) => (TypedArray) this.subarrayImpl<Uint32Array>(args[0], args[1]);

    [Hidden]
    public override Type ElementType
    {
      [Hidden] get => typeof (uint);
    }

    protected internal override System.Array ToNativeArray()
    {
      uint[] nativeArray = new uint[this.length._iValue];
      for (int index = 0; index < nativeArray.Length; ++index)
        nativeArray[index] = this.getValue(index);
      return (System.Array) nativeArray;
    }
  }
}
