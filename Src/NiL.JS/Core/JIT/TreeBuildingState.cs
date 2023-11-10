// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.JIT.TreeBuildingState
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System.Collections.Generic;
using System.Linq.Expressions;

namespace NiL.JS.Core.JIT
{
  internal sealed class TreeBuildingState
  {
    public readonly Stack<LabelTarget> BreakLabels;
    public readonly Stack<LabelTarget> ContinueLabels;
    public readonly Dictionary<string, LabelTarget> NamedBreakLabels;
    public readonly Dictionary<string, LabelTarget> NamedContinueLabels;

    public TreeBuildingState()
    {
      this.BreakLabels = new Stack<LabelTarget>();
      this.ContinueLabels = new Stack<LabelTarget>();
      this.NamedBreakLabels = new Dictionary<string, LabelTarget>();
      this.NamedContinueLabels = new Dictionary<string, LabelTarget>();
    }
  }
}
