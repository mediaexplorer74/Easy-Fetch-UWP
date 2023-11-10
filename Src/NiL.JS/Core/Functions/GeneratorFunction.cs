// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Functions.GeneratorFunction
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Interop;
using NiL.JS.Expressions;

namespace NiL.JS.Core.Functions
{
  [Prototype(typeof (Function), true)]
  internal sealed class GeneratorFunction : Function
  {
    public override JSValue prototype
    {
      get => (JSValue) null;
      set
      {
      }
    }

    public GeneratorFunction(Context context, FunctionDefinition generator)
      : base(context, generator)
    {
      this.RequireNewKeywordLevel = RequireNewKeywordLevel.WithoutNewOnly;
    }

    protected internal override JSValue Invoke(
      bool construct,
      JSValue targetObject,
      Arguments arguments)
    {
      if (construct)
        ExceptionHelper.ThrowTypeError("Generators cannot be invoked as a constructor");
      return this.Context.GlobalContext.ProxyValue((object) new GeneratorIterator(this, targetObject, arguments));
    }
  }
}
