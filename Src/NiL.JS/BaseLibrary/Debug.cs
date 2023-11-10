// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.Debug
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;

namespace NiL.JS.BaseLibrary
{
  public static class Debug
  {
    public static void writeln(Arguments args)
    {
      int num = 0;
      while (num < args._iValue)
        ++num;
    }

    public static void write(Arguments args)
    {
      int num = 0;
      while (num < args._iValue)
        ++num;
    }

    public static void assert(Arguments args)
    {
    }

    public static JSValue asserta(Function f, JSValue sample)
    {
      if (sample == null || !sample.Exists)
        sample = (JSValue) Boolean.True;
      if (!JSObject.@is(f.Call((Arguments) null), sample))
      {
        string str = f.ToString();
        str.Substring(str.IndexOf("=>") + 2).Trim();
      }
      return JSValue.undefined;
    }
  }
}
