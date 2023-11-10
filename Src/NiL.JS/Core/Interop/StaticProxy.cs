// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Interop.StaticProxy
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;

namespace NiL.JS.Core.Interop
{
  [Prototype(typeof (JSObject), true)]
  internal sealed class StaticProxy : Proxy
  {
    internal override JSObject PrototypeInstance => (JSObject) null;

    internal override bool IsInstancePrototype => false;

    [Hidden]
    public StaticProxy(GlobalContext context, Type type, bool indexersSupport)
      : base(context, type, indexersSupport)
    {
    }
  }
}
