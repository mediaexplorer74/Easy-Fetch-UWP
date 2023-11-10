// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Rule
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

namespace NiL.JS.Core
{
  internal class Rule
  {
    public ValidateDelegate Validate { get; private set; }

    public ParseDelegate Parse { get; private set; }

    public Rule(string token, ParseDelegate parseDel)
    {
      this.Validate = (ValidateDelegate) ((code, pos) => Parser.Validate(code, token, pos));
      this.Parse = parseDel;
    }

    public Rule(ValidateDelegate valDel, ParseDelegate parseDel)
    {
      this.Validate = valDel;
      this.Parse = parseDel;
    }
  }
}
