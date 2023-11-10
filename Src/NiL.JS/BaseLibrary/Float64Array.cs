// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.Float64Array
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System;

namespace NiL.JS.BaseLibrary
{
  public sealed class Float64Array : TypedArray
  {
    protected override JSValue this[int index]
    {
      get
      {
        TypedArray.Element element = new TypedArray.Element((TypedArray) this, index);
        element._dValue = this.getValue(index);
        element._valueType = JSValueType.Double;
        return (JSValue) element;
      }
      set
      {
        if (index < 0 || index > this.length._iValue)
          ExceptionHelper.Throw((Error) new RangeError());
        long int64Bits = BitConverter.DoubleToInt64Bits(Tools.JSObjectToDouble(value));
        if (BitConverter.IsLittleEndian)
        {
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset] = (byte) int64Bits;
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 1] = (byte) (int64Bits >> 8);
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 2] = (byte) (int64Bits >> 16);
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 3] = (byte) (int64Bits >> 24);
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 4] = (byte) (int64Bits >> 32);
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 5] = (byte) (int64Bits >> 40);
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 6] = (byte) (int64Bits >> 48);
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 7] = (byte) (int64Bits >> 56);
        }
        else
        {
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset] = (byte) (int64Bits >> 56);
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 1] = (byte) (int64Bits >> 48);
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 2] = (byte) (int64Bits >> 40);
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 3] = (byte) (int64Bits >> 32);
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 4] = (byte) (int64Bits >> 24);
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 5] = (byte) (int64Bits >> 16);
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 6] = (byte) (int64Bits >> 8);
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 7] = (byte) int64Bits;
        }
      }
    }

    private double getValue(int index) => BitConverter.ToDouble(this.buffer.data, index * this.BYTES_PER_ELEMENT + this.byteOffset);

    public override int BYTES_PER_ELEMENT => 8;

    public Float64Array()
    {
    }

    public Float64Array(int length)
      : base(length)
    {
    }

    public Float64Array(ArrayBuffer buffer)
      : base(buffer, 0, buffer.byteLength)
    {
    }

    public Float64Array(ArrayBuffer buffer, int bytesOffset)
      : base(buffer, bytesOffset, buffer.byteLength - bytesOffset)
    {
    }

    public Float64Array(ArrayBuffer buffer, int bytesOffset, int length)
      : base(buffer, bytesOffset, length)
    {
    }

    public Float64Array(JSValue src)
      : base(src)
    {
    }

    [ArgumentsCount(2)]
    public override TypedArray subarray(Arguments args) => (TypedArray) this.subarrayImpl<Float64Array>(args[0], args[1]);

    [Hidden]
    public override Type ElementType
    {
      [Hidden] get => typeof (double);
    }

    protected internal override System.Array ToNativeArray()
    {
      double[] nativeArray = new double[this.length._iValue];
      for (int index = 0; index < nativeArray.Length; ++index)
        nativeArray[index] = this.getValue(index);
      return (System.Array) nativeArray;
    }
  }
}
