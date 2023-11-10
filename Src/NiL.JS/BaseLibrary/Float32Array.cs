// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.Float32Array
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System;

namespace NiL.JS.BaseLibrary
{
  public sealed class Float32Array : TypedArray
  {
    protected override JSValue this[int index]
    {
      get
      {
        TypedArray.Element element = new TypedArray.Element((TypedArray) this, index);
        element._dValue = (double) BitConverter.ToSingle(this.buffer.data, index * this.BYTES_PER_ELEMENT + this.byteOffset);
        element._valueType = JSValueType.Double;
        return (JSValue) element;
      }
      set
      {
        if (index < 0 || index > this.length._iValue)
          ExceptionHelper.Throw((Error) new RangeError());
        byte[] bytes = BitConverter.GetBytes((float) Tools.JSObjectToDouble(value));
        if (BitConverter.IsLittleEndian)
        {
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset] = bytes[3];
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 1] = bytes[2];
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 2] = bytes[1];
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 3] = bytes[0];
        }
        else
        {
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset] = bytes[0];
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 1] = bytes[1];
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 2] = bytes[2];
          this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 3] = bytes[3];
        }
      }
    }

    public override int BYTES_PER_ELEMENT => 4;

    public Float32Array()
    {
    }

    public Float32Array(int length)
      : base(length)
    {
    }

    public Float32Array(ArrayBuffer buffer)
      : base(buffer, 0, buffer.byteLength)
    {
    }

    public Float32Array(ArrayBuffer buffer, int bytesOffset)
      : base(buffer, bytesOffset, buffer.byteLength - bytesOffset)
    {
    }

    public Float32Array(ArrayBuffer buffer, int bytesOffset, int length)
      : base(buffer, bytesOffset, length)
    {
    }

    public Float32Array(JSValue src)
      : base(src)
    {
    }

    [ArgumentsCount(2)]
    public override TypedArray subarray(Arguments args) => (TypedArray) this.subarrayImpl<Float32Array>(args[0], args[1]);

    [Hidden]
    public override Type ElementType
    {
      [Hidden] get => typeof (float);
    }

    protected internal override System.Array ToNativeArray() => throw new NotImplementedException();
  }
}
