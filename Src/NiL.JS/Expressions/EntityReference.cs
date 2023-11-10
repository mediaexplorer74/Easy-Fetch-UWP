// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.EntityReference
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using System;

namespace NiL.JS.Expressions
{
  internal sealed class EntityReference : VariableReference
  {
    public EntityDefinition Entity => (EntityDefinition) this.Descriptor.initializer;

    public override string Name => this.Entity._name;

    public override JSValue Evaluate(Context context) => throw new InvalidOperationException();

    public EntityReference(EntityDefinition entityDefinition)
    {
      this.ScopeLevel = 1;
      this._descriptor = new VariableDescriptor(entityDefinition._name, 1)
      {
        lexicalScope = !entityDefinition.Hoist,
        initializer = (Expression) entityDefinition
      };
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit((VariableReference) this);

    public override string ToString() => this.Descriptor.ToString();
  }
}
