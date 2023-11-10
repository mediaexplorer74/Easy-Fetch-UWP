// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.In
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;

namespace NiL.JS.Expressions
{
  public sealed class In : Expression
  {
    protected internal override PredictedType ResultType => PredictedType.Bool;

    internal override bool ResultInTempContainer => false;

    public In(Expression first, Expression second)
      : base(first, second, false)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      if (this._tempContainer == null)
        this._tempContainer = new JSValue()
        {
          _attributes = JSValueAttributesInternal.Temporary
        };
      this._tempContainer.Assign(this._left.Evaluate(context));
      JSValue tempContainer = this._tempContainer;
      this._tempContainer = (JSValue) null;
      JSValue jsValue = this._right.Evaluate(context);
      if (jsValue._valueType < JSValueType.Object)
        ExceptionHelper.Throw((Error) new TypeError("Right-hand value of operator in is not an object."));
      if (tempContainer._valueType == JSValueType.Integer && jsValue._oValue is Array oValue)
      {
        int num = tempContainer._iValue < 0 || (long) tempContainer._iValue >= (long) oValue._data.Length ? 0 : ((oValue._data[tempContainer._iValue] ?? JSValue.notExists).Exists ? 1 : 0);
        this._tempContainer = tempContainer;
        return (JSValue) (num != 0);
      }
      JSValue property = jsValue.GetProperty(tempContainer, false, PropertyScope.Common);
      this._tempContainer = tempContainer;
      return (JSValue) property.Exists;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override string ToString() => "(" + this._left?.ToString() + " in " + this._right?.ToString() + ")";
  }
}
