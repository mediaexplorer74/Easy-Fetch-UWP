// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.VariableDescriptor
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Expressions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NiL.JS.Core
{
  public class VariableDescriptor
  {
    internal int definitionScopeLevel;
    internal Context cacheContext;
    internal JSValue cacheRes;
    internal readonly string name;
    internal bool captured;
    internal bool lexicalScope;
    internal Expression initializer;
    internal List<Expression> assignments;
    internal readonly List<VariableReference> references;
    internal CodeNode owner;
    internal PredictedType lastPredictedType;
    internal bool isReadOnly;
    internal bool isDefined;
    internal int scopeBias;

    public bool IsDefined => this.isDefined;

    public CodeNode Owner => this.owner;

    public bool IsReadOnly => this.isReadOnly;

    public Expression Initializer => this.initializer;

    public string Name => this.name;

    public int ReferenceCount => this.references.Count;

    public bool LexicalScope => this.lexicalScope;

    public ReadOnlyCollection<Expression> Assignments => this.assignments != null ? this.assignments.AsReadOnly() : (ReadOnlyCollection<Expression>) null;

    public IEnumerable<VariableReference> References
    {
      get
      {
        for (int i = 0; i < this.references.Count; ++i)
          yield return this.references[i];
      }
    }

    internal JSValue Get(Context context, bool forWrite, int scopeLevel)
    {
      context._objectSource = (JSValue) null;
      if (((this.definitionScopeLevel | scopeLevel) & int.MinValue) != 0)
        return context.GetVariable(this.name, forWrite);
      return context == this.cacheContext && !forWrite ? this.cacheRes : this.deepGet(context, forWrite, scopeLevel);
    }

    private JSValue deepGet(Context context, bool forWrite, int depth)
    {
      JSValue jsValue = (JSValue) null;
      int num = depth - this.definitionScopeLevel;
      while (num > 0)
      {
        --num;
        context = context._parent;
      }
      if (context != this.cacheContext || this.cacheRes == null)
      {
        if (this.lexicalScope)
        {
          if (context._variables == null || !context._variables.TryGetValue(this.name, out jsValue))
            return JSValue.NotExists;
        }
        else
          jsValue = context.GetVariable(this.name, forWrite);
        if (!forWrite && this.IsDefined || (jsValue._attributes & JSValueAttributesInternal.SystemObject) == JSValueAttributesInternal.None)
        {
          this.cacheContext = context;
          this.cacheRes = jsValue;
        }
      }
      else
        jsValue = this.cacheRes;
      if (forWrite && jsValue.NeedClone)
      {
        jsValue = context.GetVariable(this.name, forWrite);
        this.cacheRes = jsValue;
      }
      return jsValue;
    }

    internal VariableDescriptor(string name, int definitionScopeLevel)
    {
      this.isDefined = true;
      this.definitionScopeLevel = definitionScopeLevel;
      this.name = name;
      this.references = new List<VariableReference>();
    }

    internal VariableDescriptor(VariableReference proto, int definitionDepth)
    {
      if (proto._descriptor != null)
        throw new ArgumentException(nameof (proto));
      this.definitionScopeLevel = definitionDepth;
      this.name = proto.Name;
      this.references = new List<VariableReference>()
      {
        proto
      };
      proto._descriptor = this;
    }

    public override string ToString() => this.name;
  }
}
