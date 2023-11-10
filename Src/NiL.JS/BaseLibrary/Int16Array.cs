// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.Int16Array
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System;

namespace NiL.JS.BaseLibrary
{
  public sealed class Int16Array : TypedArray
  {
    protected override JSValue this[int index]
    {
      get
      {
        TypedArray.Element element = new TypedArray.Element((TypedArray) this, index);
        element._iValue = (int) this.getValue(index);
        element._valueType = JSValueType.Integer;
        return (JSValue) element;
      }
      set
      {
        if (index < 0 || index > this.length._iValue)
          ExceptionHelper.Throw((Error) new RangeError());
        ushort int32 = (ushort) Tools.JSObjectToInt32(value, 0, false);
        this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset] = (byte) int32;
        this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 1] = (byte) ((uint) int32 >> 8);
      }
    }

    private short getValue(int index) => (short) ((int) this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset] | (int) this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 1] << 8);

    public override int BYTES_PER_ELEMENT => 2;

    public Int16Array()
    {
    }

    public Int16Array(int length)
      : base(length)
    {
    }

    public Int16Array(ArrayBuffer buffer)
      : base(buffer, 0, buffer.byteLength)
    {
    }

    public Int16Array(ArrayBuffer buffer, int bytesOffset)
      : base(buffer, bytesOffset, buffer.byteLength - bytesOffset)
    {
    }

    public Int16Array(ArrayBuffer buffer, int bytesOffset, int length)
      : base(buffer, bytesOffset, length)
    {
    }

    public Int16Array(JSValue src)
      : base(src)
    {
    }

    [ArgumentsCount(2)]
    public override TypedArray subarray(Arguments args) => (TypedArray) this.subarrayImpl<Int16Array>(args[0], args[1]);

    [Hidden]
    public override Type ElementType
    {
      [Hidden] get => typeof (short);
    }

    protected internal override System.Array ToNativeArray()
    {
      short[] nativeArray = new short[this.length._iValue];
      for (int index = 0; index < nativeArray.Length; ++index)
        nativeArray[index] = this.getValue(index);
      return (System.Array) nativeArray;
    }
  }
}
