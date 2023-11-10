// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.ExportStatement
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Expressions;
using System.Collections.Generic;
using System.Text;

namespace NiL.JS.Statements
{
  public sealed class ExportStatement : CodeNode
  {
    private string _reexportSourceModuleName;
    private CodeNode _internalDefinition;
    private readonly List<KeyValuePair<string, Expression>> _map = new List<KeyValuePair<string, Expression>>();

    public string ReExportSourceModuleName => this._reexportSourceModuleName;

    public CodeNode InternalDefinition => this._internalDefinition;

    public IList<KeyValuePair<string, Expression>> ExportMap => (IList<KeyValuePair<string, Expression>>) this._map.AsReadOnly();

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      if (!Parser.Validate(state.Code, "export", ref index))
        return (CodeNode) null;
      Tools.SkipSpaces(state.Code, ref index);
      ExportStatement export = new ExportStatement();
      int num1 = 0;
      if (Parser.Validate(state.Code, "*", ref index))
        num1 = 1;
      else if (Parser.Validate(state.Code, "default", ref index))
      {
        num1 = -1;
        Tools.SkipSpaces(state.Code, ref index);
        using (state.WithCodeContext(CodeContext.InExport))
        {
          CodeNode codeNode1 = VariableDefinition.Parse(state, ref index);
          if (codeNode1 != null)
          {
            export._internalDefinition = codeNode1;
          }
          else
          {
            CodeNode codeNode2 = ClassDefinition.Parse(state, ref index);
            if (codeNode2 == null)
            {
              Expression expression = FunctionDefinition.Parse(state, ref index, FunctionKind.Function);
              codeNode2 = expression != null ? (CodeNode) expression : ExpressionTree.Parse(state, ref index);
            }
            CodeNode codeNode3 = codeNode2;
            export._map.Add(new KeyValuePair<string, Expression>("", (Expression) codeNode3));
          }
        }
      }
      else if (state.Code[index] == '{')
      {
        ExportStatement.parseExportMap(export, state, ref index);
      }
      else
      {
        using (state.WithCodeContext(CodeContext.InExport))
        {
          num1 = -1;
          CodeNode codeNode = VariableDefinition.Parse(state, ref index) ?? ClassDefinition.Parse(state, ref index) ?? (CodeNode) FunctionDefinition.Parse(state, ref index, FunctionKind.Function);
          if (codeNode == null)
            ExceptionHelper.ThrowSyntaxError(Strings.UnexpectedToken, state.Code, index);
          export._internalDefinition = codeNode;
        }
      }
      Tools.SkipSpaces(state.Code, ref index);
      if (Parser.Validate(state.Code, "from", ref index))
      {
        if (num1 == -1)
          ExceptionHelper.ThrowSyntaxError("Reexport is not allowed with this syntax", state.Code, index - 4);
        Tools.SkipSpaces(state.Code, ref index);
        int num2 = index;
        if (!Parser.ValidateString(state.Code, ref index, false))
          ExceptionHelper.ThrowSyntaxError("Expected module name", state.Code, index);
        export._reexportSourceModuleName = Tools.Unescape(state.Code.Substring(num2 + 1, index - num2 - 2), false);
      }
      else if (num1 == 1)
        ExceptionHelper.ThrowSyntaxError("Expected 'from'", state.Code, index);
      return (CodeNode) export;
    }

    private static void parseExportMap(ExportStatement export, ParseInfo state, ref int index)
    {
      ++index;
      Tools.SkipSpaces(state.Code, ref index);
      if (state.Code[index] == '}')
        ExceptionHelper.ThrowSyntaxError("Empty export map", state.Code, index);
      while (state.Code[index] != '}')
      {
        int startIndex = index;
        if (!Parser.ValidateName(state.Code, ref index, false, true, false))
          ExceptionHelper.ThrowSyntaxError("Invalid export name", state.Code, index);
        string name = state.Code.Substring(startIndex, index - startIndex);
        Tools.SkipSpaces(state.Code, ref index);
        string str = ExportStatement.parseAlias(state.Code, ref index) ?? name;
        for (int index1 = 0; index1 < export._map.Count; ++index1)
        {
          if (export._map[index1].Key == name)
            ExceptionHelper.ThrowSyntaxError("Duplicate import", state.Code, index);
        }
        List<KeyValuePair<string, Expression>> map = export._map;
        string key = str;
        Variable variable = new Variable(name, state.LexicalScopeLevel, false);
        variable.Position = startIndex;
        variable.Length = name.Length;
        KeyValuePair<string, Expression> keyValuePair = new KeyValuePair<string, Expression>(key, (Expression) variable);
        map.Add(keyValuePair);
        if (Parser.Validate(state.Code, ",", ref index))
          Tools.SkipSpaces(state.Code, ref index);
      }
      ++index;
    }

    private static string parseAlias(string code, ref int index)
    {
      string alias = (string) null;
      if (Parser.Validate(code, "as", ref index))
      {
        Tools.SkipSpaces(code, ref index);
        int startIndex = index;
        if (!Parser.ValidateName(code, ref index, false, true, false))
          ExceptionHelper.ThrowSyntaxError("Invalid export alias", code, index);
        alias = code.Substring(startIndex, index - startIndex);
        if (alias == "default")
          alias = "";
        Tools.SkipSpaces(code, ref index);
      }
      return alias;
    }

    public override void Decompose(ref CodeNode self)
    {
      this._internalDefinition?.Decompose(ref this._internalDefinition);
      for (int index1 = 0; index1 < this._map.Count; ++index1)
      {
        KeyValuePair<string, Expression> keyValuePair1 = this._map[index1];
        Expression self1 = keyValuePair1.Value;
        self1.Decompose(ref self1);
        List<KeyValuePair<string, Expression>> map = this._map;
        int index2 = index1;
        keyValuePair1 = this._map[index1];
        KeyValuePair<string, Expression> keyValuePair2 = new KeyValuePair<string, Expression>(keyValuePair1.Key, self1);
        map[index2] = keyValuePair2;
      }
    }

    public override JSValue Evaluate(Context context)
    {
      if (context._module == null)
        ExceptionHelper.Throw(new Error("Module undefined"));
      if (this._reexportSourceModuleName != null)
      {
        if (string.IsNullOrEmpty(context._module.FilePath))
          ExceptionHelper.Throw(new Error("Module must has name"));
        Module module = context._module.Import(this._reexportSourceModuleName);
        if (this._map.Count == 0)
        {
          foreach (KeyValuePair<string, JSValue> export in module.Exports)
            context._module.Exports[export.Key] = export.Value;
        }
        else
        {
          for (int index = 0; index < this._map.Count; ++index)
          {
            ExportTable exports1 = context._module.Exports;
            KeyValuePair<string, Expression> keyValuePair = this._map[index];
            string key1 = keyValuePair.Key;
            ExportTable exports2 = module.Exports;
            keyValuePair = this._map[index];
            string key2 = keyValuePair.Value.ToString();
            JSValue jsValue = exports2[key2];
            exports1[key1] = jsValue;
          }
        }
      }
      else if (this._internalDefinition != null)
      {
        JSValue jsValue = this._internalDefinition.Evaluate(context);
        if (this._internalDefinition is VariableDefinition internalDefinition1)
        {
          for (int index = 0; index < internalDefinition1._variables.Length; ++index)
            context._module.Exports[internalDefinition1._variables[index].name] = internalDefinition1._variables[index].references[0].Evaluate(context);
        }
        else
        {
          EntityDefinition internalDefinition = this._internalDefinition as EntityDefinition;
          context._module.Exports[internalDefinition.Name] = jsValue;
        }
      }
      else
      {
        for (int index = 0; index < this._map.Count; ++index)
        {
          ExportTable exports = context._module.Exports;
          KeyValuePair<string, Expression> keyValuePair = this._map[index];
          string key = keyValuePair.Key;
          keyValuePair = this._map[index];
          JSValue jsValue = keyValuePair.Value.Evaluate(context);
          exports[key] = jsValue;
        }
      }
      return (JSValue) null;
    }

    public override bool Build(
      ref CodeNode _this,
      int expressionDepth,
      Dictionary<string, VariableDescriptor> variables,
      CodeContext codeContext,
      InternalCompilerMessageCallback message,
      FunctionInfo stats,
      Options opts)
    {
      if (this._reexportSourceModuleName != null)
        return false;
      codeContext &= ~CodeContext.InExpression;
      codeContext |= CodeContext.InExport;
      if (this._internalDefinition != null)
      {
        Parser.Build<CodeNode>(ref this._internalDefinition, expressionDepth, variables, codeContext, message, stats, opts | Options.SuppressUselessStatementsElimination);
      }
      else
      {
        for (int index1 = 0; index1 < this._map.Count; ++index1)
        {
          KeyValuePair<string, Expression> keyValuePair1 = this._map[index1];
          Expression s = keyValuePair1.Value;
          Parser.Build(ref s, expressionDepth + 1, variables, codeContext, message, stats, opts);
          Expression expression1 = s;
          keyValuePair1 = this._map[index1];
          Expression expression2 = keyValuePair1.Value;
          if (expression1 != expression2)
          {
            List<KeyValuePair<string, Expression>> map = this._map;
            int index2 = index1;
            keyValuePair1 = this._map[index1];
            KeyValuePair<string, Expression> keyValuePair2 = new KeyValuePair<string, Expression>(keyValuePair1.Key, s);
            map[index2] = keyValuePair2;
          }
        }
      }
      return false;
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      if (this._reexportSourceModuleName != null)
        return;
      if (this._internalDefinition != null)
      {
        this._internalDefinition.RebuildScope(functionInfo, transferedVariables, scopeBias);
      }
      else
      {
        for (int index = 0; index < this._map.Count; ++index)
          this._map[index].Value.RebuildScope(functionInfo, transferedVariables, scopeBias);
      }
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      if (this._reexportSourceModuleName != null)
        return;
      if (this._internalDefinition != null)
      {
        CodeNode internalDefinition = this._internalDefinition;
        this._internalDefinition.Optimize(ref internalDefinition, owner, message, opts, stats);
        if (internalDefinition == this._internalDefinition)
          return;
        this._internalDefinition = (CodeNode) (internalDefinition as VariableDefinition);
      }
      else
      {
        for (int index = 0; index < this._map.Count; ++index)
        {
          Expression self = this._map[index].Value;
          this._map[index].Value.Optimize(ref self, owner, message, opts, stats);
          if (self != this._map[index].Value)
            this._map[index] = new KeyValuePair<string, Expression>(this._map[index].Key, self);
        }
      }
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder("export ");
      if (this._map.Count == 1 && this._map[0].Key == "")
      {
        stringBuilder.Append(" default ");
        stringBuilder.Append((object) this._map[0].Value);
      }
      int index = 0;
      if (index < this._map.Count)
      {
        stringBuilder.Append("{ ");
        while (true)
        {
          KeyValuePair<string, Expression> keyValuePair = this._map[index];
          stringBuilder.Append(keyValuePair.Key);
          if (keyValuePair.Key != keyValuePair.Value.ToString())
            stringBuilder.Append(" as ").Append(string.IsNullOrEmpty(keyValuePair.Value.ToString()) ? "default" : keyValuePair.Value.ToString());
          ++index;
          if (index < this._map.Count)
            stringBuilder.Append(", ");
          else
            break;
        }
        stringBuilder.Append(" }");
      }
      else if (this._reexportSourceModuleName != null)
        stringBuilder.Append(" * ");
      else
        stringBuilder.Append((object) this._internalDefinition);
      if (this._reexportSourceModuleName != null)
        stringBuilder.Append(" from \"").Append(this._reexportSourceModuleName).Append("\"");
      return stringBuilder.ToString();
    }
  }
}
