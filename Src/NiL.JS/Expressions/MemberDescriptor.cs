// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.MemberDescriptor
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

namespace NiL.JS.Expressions
{
  public sealed class MemberDescriptor
  {
    internal Expression _name;
    internal Expression _value;
    internal bool _static;

    public Expression Name => this._name;

    public Expression Value => this._value;

    public bool Static => this._static;

    public MemberDescriptor(Expression name, Expression value, bool @static)
    {
      this._name = name;
      this._value = value;
      this._static = @static;
    }

    public override string ToString() => this._static ? "static " + this._value?.ToString() : this._value.ToString();
  }
}
