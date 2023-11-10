// Decompiled with JetBrains decompiler
// Type: NiL.JS.Extensions.IterationProtocolExtensions
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NiL.JS.Extensions
{
  public static class IterationProtocolExtensions
  {
    public static IEnumerable<JSValue> AsEnumerable(this IIterable iterableObject)
    {
      IIterator iterator = iterableObject.iterator();
      if (iterator != null)
      {
        for (IIteratorResult iteratorResult = iterator.next(); !iteratorResult.done; iteratorResult = iterator.next())
          yield return iteratorResult.value;
      }
    }

    public static IIterable AsIterable(this JSValue source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return source.Value is IIterable iterable ? iterable : (IIterable) new IterableAdapter(source);
    }

    public static bool IsIterable(this JSValue source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return source.Value is IIterable || source.GetProperty((JSValue) Symbol.iterator, false, PropertyScope.Common)._valueType == JSValueType.Function;
    }

    public static IIterable AsIterable(this IEnumerable enumerable) => (IIterable) new EnumerableToIterableWrapper(enumerable);

    public static IIterator AsIterator(this IEnumerator enumerator) => (IIterator) new EnumeratorToIteratorWrapper(enumerator);
  }
}
