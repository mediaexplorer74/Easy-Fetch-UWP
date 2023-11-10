// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Interop.JsonSerializer
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NiL.JS.Core.Interop
{
  public class JsonSerializer
  {
    private PropertyInfo[] _properties;
    private FieldInfo[] _fields;

    public Type TargetType { get; }

    public int Weight { get; }

    public JsonSerializer(Type targetType)
    {
      this.TargetType = targetType ?? throw new ArgumentNullException(nameof (targetType));
      this._properties = targetType.GetRuntimeProperties().ToArray<PropertyInfo>();
      this._fields = targetType.GetRuntimeFields().ToArray<FieldInfo>();
      int num = 0;
      for (Type type = targetType; (object) type != null && (object) type != (object) typeof (object); type = type.GetTypeInfo().BaseType)
        ++num;
      this.Weight = num;
    }

    public virtual bool CanSerialize(object value) => value != null ? this.TargetType.GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()) : throw new ArgumentNullException(nameof (value));

    public virtual string Serialize(
      string key,
      object value,
      Function replacer,
      HashSet<string> keys,
      string space,
      HashSet<JSValue> processed)
    {
      if (!this.CanSerialize(value))
        throw new ArgumentException("Cannot serialize value with type \"" + value.GetType().FullName + "\" as \"" + this.TargetType.FullName + "\"");
      StringBuilder result = new StringBuilder("{");
      bool flag = true;
      for (int index = 0; index < this._properties.Length; ++index)
      {
        if (!flag)
          result.Append(",");
        flag = false;
        if (space != null)
          result.Append(Environment.NewLine).Append(space);
        object obj = this._properties[index].GetValue(value, (object[]) null);
        result.Append("\"").Append(this._properties[index].Name).Append("\"").Append(":");
        this.WriteValue(result, this._properties[index].Name, obj, replacer, keys, space, processed);
      }
      for (int index = 0; index < this._fields.Length; ++index)
      {
        if (!flag)
          result.Append(",");
        flag = false;
        if (space != null)
          result.Append(Environment.NewLine).Append(space);
        object obj = this._fields[index].GetValue(value);
        result.Append("\"").Append(this._fields[index].Name).Append("\"");
        this.WriteValue(result, this._fields[index].Name, obj, replacer, keys, space, processed);
      }
      if (space != null)
        result.Append(Environment.NewLine).Append(space);
      result.Append("}");
      return result.ToString();
    }

    public object Deserialize(JSValue deserializedJson, object resultContainer = null)
    {
      if (deserializedJson == null)
        throw new ArgumentNullException(nameof (deserializedJson));
      if (deserializedJson._valueType < JSValueType.Object)
        return deserializedJson.Value;
      object obj1 = resultContainer ?? this.TargetType.GetTypeInfo().DeclaredConstructors.Where<ConstructorInfo>((Func<ConstructorInfo, bool>) (x => x.IsPublic)).First<ConstructorInfo>((Func<ConstructorInfo, bool>) (x => x.GetParameters().Length == 0)).Invoke(new object[0]);
      foreach (KeyValuePair<string, JSValue> field1 in (IEnumerable<KeyValuePair<string, JSValue>>) (deserializedJson._oValue as JSObject)._fields)
      {
        PropertyInfo property = this.getProperty(field1.Key);
        if ((object) property != null)
        {
          JsonSerializer serializer = this.GetSerializer((object) field1.Value, Context.CurrentGlobalContext);
          object obj2 = serializer == null ? Convert.ChangeType(field1.Value.Value, property.PropertyType) : serializer.Deserialize(field1.Value);
          property.SetValue(obj1, obj2, (object[]) null);
        }
        else
        {
          FieldInfo field2 = this.getField(field1.Key);
          if ((object) field2 != null)
          {
            JsonSerializer serializer = this.GetSerializer((object) field1.Value, Context.CurrentGlobalContext);
            object obj3 = serializer == null ? Convert.ChangeType(field1.Value.Value, field2.FieldType) : serializer.Deserialize(field1.Value);
            field2.SetValue(obj1, Convert.ChangeType((object) field1.Value, field2.FieldType));
          }
        }
      }
      return obj1;
    }

    protected internal virtual JsonSerializer GetSerializer(object value, GlobalContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      return context.GlobalContext?.JsonSerializersRegistry.GetSuitableJsonSerializer(value);
    }

    protected virtual void WriteValue(
      StringBuilder result,
      string key,
      object value,
      Function replacer,
      HashSet<string> keys,
      string space,
      HashSet<JSValue> processed)
    {
      if (value == null)
        result.Append("null");
      else if (value is JSValue jsValue)
      {
        result.Append(JSON.stringify(jsValue, replacer, keys, space));
      }
      else
      {
        switch (value.GetType().GetTypeCode())
        {
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
          case TypeCode.UInt32:
          case TypeCode.Int64:
          case TypeCode.UInt64:
          case TypeCode.Single:
          case TypeCode.Double:
          case TypeCode.Decimal:
            result.Append(value);
            break;
          default:
            if (!(value is string))
            {
              JsonSerializer serializer = this.GetSerializer(value, Context.CurrentGlobalContext);
              if (serializer != null)
              {
                result.Append(serializer.Serialize(key, value, replacer, keys, space, processed));
                break;
              }
            }
            result.Append('"').Append(value).Append('"');
            break;
        }
      }
    }

    private PropertyInfo getProperty(string name)
    {
      for (int index = 0; index < this._properties.Length; ++index)
      {
        if (this._properties[index].Name == name)
          return this._properties[index];
      }
      return (PropertyInfo) null;
    }

    private FieldInfo getField(string name)
    {
      for (int index = 0; index < this._fields.Length; ++index)
      {
        if (this._fields[index].Name == name)
          return this._fields[index];
      }
      return (FieldInfo) null;
    }
  }
}
