// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.Uint16Array
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Interop;
using System;

namespace NiL.JS.BaseLibrary
{
  public sealed class Uint16Array : TypedArray
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

    private ushort getValue(int index) => (ushort) ((uint) this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset] | (uint) this.buffer.data[index * this.BYTES_PER_ELEMENT + this.byteOffset + 1] << 8);

    public override int BYTES_PER_ELEMENT => 2;

    public Uint16Array()
    {
    }

    public Uint16Array(int length)
      : base(length)
    {
    }

    public Uint16Array(ArrayBuffer buffer)
      : base(buffer, 0, buffer.byteLength)
    {
    }

    public Uint16Array(ArrayBuffer buffer, int bytesOffset)
      : base(buffer, bytesOffset, buffer.byteLength - bytesOffset)
    {
    }

    public Uint16Array(ArrayBuffer buffer, int bytesOffset, int length)
      : base(buffer, bytesOffset, length)
    {
    }

    public Uint16Array(JSValue src)
      : base(src)
    {
    }

    [ArgumentsCount(2)]
    public override TypedArray subarray(Arguments args) => (TypedArray) this.subarrayImpl<Uint16Array>(args[0], args[1]);

    [Hidden]
    public override Type ElementType
    {
      [Hidden] get => typeof (ushort);
    }

    protected internal override System.Array ToNativeArray()
    {
      ushort[] nativeArray = new ushort[this.length._iValue];
      for (int index = 0; index < nativeArray.Length; ++index)
        nativeArray[index] = this.getValue(index);
      return (System.Array) nativeArray;
    }
  }
}
