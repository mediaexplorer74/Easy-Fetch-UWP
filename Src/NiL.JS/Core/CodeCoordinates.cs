// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.CodeCoordinates
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System;

namespace NiL.JS.Core
{
  public sealed class CodeCoordinates
  {
    public int Line { get; private set; }

    public int Column { get; private set; }

    public int Length { get; private set; }

    public CodeCoordinates(int line, int column, int length)
    {
      this.Line = line;
      this.Column = column;
      this.Length = length;
    }

    public override string ToString() => string.Format("({0}:{1}{2})", (object) this.Line, (object) this.Column, this.Length != 0 ? (object) ("*" + this.Length.ToString()) : (object) "");

    public static CodeCoordinates FromTextPosition(string text, int position, int length)
    {
      if (text == null)
        throw new ArgumentNullException(nameof (text));
      if (position < 0)
        throw new ArgumentOutOfRangeException(nameof (position));
      int line = 1;
      int column = 1;
      for (int index = 0; index < position; ++index)
      {
        if (index >= text.Length)
          return (CodeCoordinates) null;
        if (text[index] == '\n')
        {
          column = 0;
          ++line;
          if (text.Length > index + 1 && text[index + 1] == '\r')
            ++index;
        }
        else if (text[index] == '\r')
        {
          column = 0;
          ++line;
          if (text.Length > index + 1 && text[index + 1] == '\n')
            ++index;
        }
        ++column;
      }
      return new CodeCoordinates(line, column, length);
    }
  }
}
