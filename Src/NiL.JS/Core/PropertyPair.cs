// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.PropertyPair
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;

namespace NiL.JS.Core
{
  public sealed class PropertyPair
  {
    internal Function getter;
    internal Function setter;

    public Function Getter => this.getter;

    public Function Setter => this.setter;

    internal PropertyPair()
    {
    }

    public PropertyPair(Function getter, Function setter)
    {
      this.getter = getter;
      this.setter = setter;
    }

    public override string ToString()
    {
      string str = "[";
      if (this.getter != null)
        str += "Getter";
      if (this.setter != null)
        str += str.Length != 1 ? "/Setter" : "Setter";
      return str.Length == 1 ? "[Invalid Property]" : str + "]";
    }
  }
}
