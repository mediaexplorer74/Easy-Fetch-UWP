// Decompiled with JetBrains decompiler
// Type: NiL.JS.ModuleRequest
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;

namespace NiL.JS
{
  public sealed class ModuleRequest
  {
    public Module Initiator { get; }

    public string CmdArgument { get; }

    public string AbsolutePath { get; }

    public ModuleRequest(Module initiator, string cmdArgument, string absolutePath)
    {
      if (string.IsNullOrWhiteSpace(cmdArgument))
        throw new ArgumentException("message", nameof (cmdArgument));
      if (string.IsNullOrWhiteSpace(absolutePath))
        throw new ArgumentException("message", nameof (absolutePath));
      this.Initiator = initiator ?? throw new ArgumentNullException(nameof (initiator));
      this.CmdArgument = cmdArgument;
      this.AbsolutePath = absolutePath;
    }
  }
}
