// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Interop.JsonSerializersRegistry
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;
using System.Collections.Generic;
using System.Reflection;

namespace NiL.JS.Core.Interop
{
  public class JsonSerializersRegistry
  {
    private readonly List<JsonSerializer> _serializers;

    public JsonSerializersRegistry() => this._serializers = new List<JsonSerializer>();

    public void AddJsonSerializer(JsonSerializer jsonSerializer)
    {
      int index = jsonSerializer != null ? this.getSerializerIndex(jsonSerializer.TargetType, true) : throw new ArgumentNullException(nameof (jsonSerializer));
      if (index < 0)
        this._serializers.Add(jsonSerializer);
      else
        this._serializers.Insert(index, jsonSerializer);
    }

    public JsonSerializer GetJsonSerializer(Type targetType)
    {
      int index = (object) targetType != null ? this.getSerializerIndex(targetType, false) : throw new ArgumentNullException(nameof (targetType));
      return index < 0 ? (JsonSerializer) null : this._serializers[index];
    }

    public JsonSerializer GetSuitableJsonSerializer(object value)
    {
      int index = value != null ? this.getSerializerIndex(value.GetType(), true) : throw new ArgumentNullException(nameof (value));
      if (index < 0)
        return (JsonSerializer) null;
      for (; index < this._serializers.Count; ++index)
      {
        if (this._serializers[index].CanSerialize(value))
          return this._serializers[index];
      }
      return (JsonSerializer) null;
    }

    public bool RemoveJsonSerializer(Type targetType)
    {
      int index = (object) targetType != null ? this.getSerializerIndex(targetType, false) : throw new ArgumentNullException(nameof (targetType));
      if (index < 0)
        return false;
      this._serializers.RemoveAt(index);
      return true;
    }

    private int getSerializerIndex(Type type, bool skipTypeCheck)
    {
      if (this._serializers.Count == 0)
        return -1;
      int num = 0;
      for (Type type1 = type; (object) type1 != null && (object) type1 != (object) typeof (object); type1 = type1.GetTypeInfo().BaseType)
        ++num;
      int index1 = 0;
      int index2 = this._serializers.Count - 1;
      while (index1 < index2 - 1)
      {
        int index3 = index1 + (index2 - index1) / 2;
        if (this._serializers[index3].Weight > num)
          index1 = index3;
        else
          index2 = index3;
      }
      if (skipTypeCheck)
        return num >= this._serializers[index1].Weight ? index1 : index2;
      if (num > this._serializers[index1].Weight || num < this._serializers[index2].Weight)
        return -1;
      for (; index1 < this._serializers.Count && num <= this._serializers[index1].Weight; ++index1)
      {
        if ((object) this._serializers[index1].TargetType == (object) type)
          return index1;
      }
      return -1;
    }
  }
}
