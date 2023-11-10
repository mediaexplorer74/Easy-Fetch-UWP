// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.FunctionInfo
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Expressions;
using System.Collections.Generic;

namespace NiL.JS.Core
{
  public sealed class FunctionInfo
  {
    public bool UseGetMember;
    public bool UseCall;
    public bool WithLexicalEnvironment;
    public bool ContainsArguments;
    public bool ContainsRestParameters;
    public bool ContainsEval;
    public bool ContainsWith;
    public bool NeedDecompose;
    public bool ContainsInnerEntities;
    public bool ContainsThis;
    public bool ContainsDebugger;
    public bool ContainsTry;
    public readonly List<Expression> Returns = new List<Expression>();
    public PredictedType ResultType;
  }
}
