// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.CustomCodeFragment
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;

namespace NiL.JS.Core
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public sealed class CustomCodeFragment : Attribute
  {
    public CodeFragmentType Type { get; private set; }

    public string[] ReservedWords { get; private set; }

    public CustomCodeFragment()
      : this(CodeFragmentType.Statement)
    {
    }

    public CustomCodeFragment(CodeFragmentType codeFragmentType)
      : this(codeFragmentType, (string[]) null)
    {
    }

    public CustomCodeFragment(CodeFragmentType codeFragmentType, params string[] reservedWords)
    {
      this.Type = codeFragmentType;
      this.ReservedWords = reservedWords ?? new string[0];
    }
  }
}
