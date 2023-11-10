// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.DebuggerCallbackEventArgs
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;

namespace NiL.JS.Core
{
  public sealed class DebuggerCallbackEventArgs : EventArgs
  {
    public CodeNode Statement { get; internal set; }
  }
}
