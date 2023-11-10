// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Interop.NativeReadOnlyListCtors
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace NiL.JS.Core.Interop
{
  internal static class NativeReadOnlyListCtors
  {
    public static readonly string ReadOnlyInterfaceName = typeof (IReadOnlyList<>).FullName;
    private static readonly Type ReadOnlyListType = typeof (IReadOnlyList<>);
    private static readonly Dictionary<Type, Func<object, JSValue>> _ctors = new Dictionary<Type, Func<object, JSValue>>();

    public static JSValue Create(object roList)
    {
      lock (NativeReadOnlyListCtors._ctors)
      {
        Type type1 = roList.GetType();
        Func<object, JSValue> func1;
        if (!NativeReadOnlyListCtors._ctors.TryGetValue(type1, out func1))
        {
          Type genericArgument = TypeExtensions.GetGenericArguments(NiL.JS.Backward.Backward.GetInterface(type1, NativeReadOnlyListCtors.ReadOnlyInterfaceName))[0];
          Type type2 = typeof (NativeReadOnlyList<>).MakeGenericType(typeof (int));
          ParameterExpression parameterExpression = Expression.Parameter(typeof (object));
          Dictionary<Type, Func<object, JSValue>> ctors = NativeReadOnlyListCtors._ctors;
          Type key = type1;
          NewExpression body = Expression.New(TypeExtensions.GetConstructors(type2)[0], (Expression) Expression.Convert((Expression) parameterExpression, type1));
          ParameterExpression[] parameterExpressionArray = new ParameterExpression[1]
          {
            parameterExpression
          };
          Func<object, JSValue> func2;
          func1 = func2 = Expression.Lambda<Func<object, JSValue>>((Expression) body, parameterExpressionArray).Compile();
          ctors[key] = func2;
        }
        return func1(roList);
      }
    }

    internal static bool IsReadOnlyList(object value)
    {
      Type[] interfaces = TypeExtensions.GetInterfaces(value.GetType());
      for (int index = 0; index < interfaces.Length; ++index)
      {
        if (interfaces[index].IsConstructedGenericType && (object) interfaces[index].GetGenericTypeDefinition() == (object) NativeReadOnlyListCtors.ReadOnlyListType)
          return true;
      }
      return false;
    }
  }
}
