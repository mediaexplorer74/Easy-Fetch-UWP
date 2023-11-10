// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Interop.PrototypeAttribute
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;

namespace NiL.JS.Core.Interop
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false, Inherited = true)]
  public sealed class PrototypeAttribute : Attribute
  {
    public Type PrototypeType { get; private set; }

    public bool Replace { get; private set; }

    public PrototypeAttribute(Type type)
      : this(type, false)
    {
    }

    internal PrototypeAttribute(Type type, bool doNotChainButReplace)
    {
      this.Replace = doNotChainButReplace;
      this.PrototypeType = type;
    }
  }
}
