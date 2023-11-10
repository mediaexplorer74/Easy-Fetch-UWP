// Decompiled with JetBrains decompiler
// Type: NiL.JS.Backward.EmpryArrayHelper
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

namespace NiL.JS.Backward
{
  internal static class EmpryArrayHelper
  {
    public static T[] Empty<T>() => EmpryArrayHelper.EmptyArrayContainer<T>.EmptyArray;

    private static class EmptyArrayContainer<T>
    {
      public static readonly T[] EmptyArray = new T[0];
    }
  }
}
