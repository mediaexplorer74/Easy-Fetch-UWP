// Decompiled with JetBrains decompiler
// Type: NiL.JS.Backward.Backward
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace NiL.JS.Backward
{
  internal static class Backward
  {
    private static readonly Type[] _Types = new Type[19]
    {
      null,
      typeof (object),
      Type.GetType("System.DBNull"),
      typeof (bool),
      typeof (char),
      typeof (sbyte),
      typeof (byte),
      typeof (short),
      typeof (ushort),
      typeof (int),
      typeof (uint),
      typeof (long),
      typeof (ulong),
      typeof (float),
      typeof (double),
      typeof (Decimal),
      typeof (DateTime),
      null,
      typeof (string)
    };

    internal static ConstructorInfo[] GetConstructors<T>(this Type self) => self.GetTypeInfo().DeclaredConstructors.ToArray<ConstructorInfo>();

    internal static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> self) => new ReadOnlyCollection<T>(self);

    internal static ReadOnlyCollection<T> AsReadOnly<T>(this List<T> self) => new ReadOnlyCollection<T>((IList<T>) self);

    internal static bool IsSubclassOf(this Type self, Type sourceType) => (object) self != (object) sourceType && self.GetTypeInfo().IsAssignableFrom(sourceType.GetTypeInfo());

    internal static object[] GetCustomAttributes(this Type self, Type attributeType, bool inherit) => (object[]) CustomAttributeExtensions.GetCustomAttributes(self.GetTypeInfo(), attributeType, inherit).ToArray<Attribute>();

    internal static bool IsDefined(this Type self, Type attributeType, bool inherit) => CustomAttributeExtensions.IsDefined(self.GetTypeInfo(), attributeType, inherit);

    internal static MemberTypes GetMemberType(this MemberInfo self)
    {
      if ((object) (self as ConstructorInfo) != null)
        return MemberTypes.Constructor;
      if ((object) (self as EventInfo) != null)
        return MemberTypes.Event;
      if ((object) (self as FieldInfo) != null)
        return MemberTypes.Field;
      if ((object) (self as MethodInfo) != null)
        return MemberTypes.Method;
      if (self is TypeInfo)
        return MemberTypes.TypeInfo;
      return (object) (self as PropertyInfo) != null ? MemberTypes.Property : MemberTypes.Custom;
    }

    internal static TypeCode GetTypeCode(this Type type)
    {
      if ((object) type == null)
        return TypeCode.Empty;
      if (type.GetTypeInfo().IsClass)
      {
        if ((object) type == (object) NiL.JS.Backward.Backward._Types[2])
          return TypeCode.DBNull;
        return (object) type == (object) typeof (string) ? TypeCode.String : TypeCode.Object;
      }
      for (int typeCode = 3; typeCode < NiL.JS.Backward.Backward._Types.Length; ++typeCode)
      {
        if ((object) NiL.JS.Backward.Backward._Types[typeCode] == (object) type)
          return (TypeCode) typeCode;
      }
      return TypeCode.Object;
    }

    internal static Type GetInterface(this Type type, string name)
    {
      foreach (Type implementedInterface in type.GetTypeInfo().ImplementedInterfaces)
      {
        if (implementedInterface.FullName.Contains(name))
          return implementedInterface;
      }
      return (Type) null;
    }
  }
}
