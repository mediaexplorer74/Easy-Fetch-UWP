// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Interop.JavaScriptNameAttribute
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;

namespace NiL.JS.Core.Interop
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event)]
  public sealed class JavaScriptNameAttribute : Attribute
  {
    public string Name { get; private set; }

    public JavaScriptNameAttribute(string name) => this.Name = name;
  }
}
