// Decompiled with JetBrains decompiler
// Type: NiL.JS.Extensions.ContextExtensions
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;

namespace NiL.JS.Extensions
{
  public static class ContextExtensions
  {
    public static void Add(this Context context, string key, object value) => context.DefineVariable(key).Assign(value);
  }
}
