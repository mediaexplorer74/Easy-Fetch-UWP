// Decompiled with JetBrains decompiler
// Type: NiL.JS.Statements.ImportStatement
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
  public sealed class ImportStatement : CodeNode
  {
    private readonly List<KeyValuePair<string, Variable>> _map = new List<KeyValuePair<string, Variable>>();
    private string _moduleName;

    public IList<KeyValuePair<string, Variable>> ImportMap => (IList<KeyValuePair<string, Variable>>) this._map.AsReadOnly();

    public string SourceModuleName => this._moduleName;

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      if (!Parser.Validate(state.Code, "import", ref index))
        return (CodeNode) null;
      Tools.SkipSpaces(state.Code, ref index);
      ImportStatement import = new ImportStatement();
      int startIndex = index;
      if (!Parser.ValidateString(state.Code, ref index, true))
      {
        bool flag = false;
        if (Parser.ValidateName(state.Code, ref index))
        {
          string name = state.Code.Substring(startIndex, index - startIndex);
          List<KeyValuePair<string, Variable>> map = import._map;
          string empty = string.Empty;
          Variable variable = new Variable(name, state.LexicalScopeLevel);
          variable.Position = startIndex;
          variable.Length = name.Length;
          KeyValuePair<string, Variable> keyValuePair = new KeyValuePair<string, Variable>(empty, variable);
          map.Add(keyValuePair);
          flag = true;
          Tools.SkipSpaces(state.Code, ref index);
          if (state.Code[index] == ',')
          {
            flag = false;
            ++index;
            Tools.SkipSpaces(state.Code, ref index);
          }
        }
        if (!flag)
        {
          if (import._map.Count == 0 && state.Code[index] == '*')
          {
            ++index;
            Tools.SkipSpaces(state.Code, ref index);
            string alias = ImportStatement.parseAlias(state.Code, ref index);
            if (alias == null)
              ExceptionHelper.ThrowSyntaxError("Expected identifier", state.Code, index);
            Variable variable1 = new Variable(alias, state.LexicalScopeLevel);
            variable1.Position = index - alias.Length - 1;
            variable1.Length = alias.Length;
            Variable variable2 = variable1;
            import._map.Add(new KeyValuePair<string, Variable>("*", variable2));
          }
          else if (state.Code[index] == '{')
            ImportStatement.parseImportMap(import, state.Code, ref index, state);
          else
            ExceptionHelper.ThrowSyntaxError(Strings.UnexpectedToken, state.Code, index);
        }
        for (int index1 = 0; index1 < import._map.Count; ++index1)
          state.Variables.Add(new VariableDescriptor((VariableReference) import._map[index1].Value, state.LexicalScopeLevel)
          {
            lexicalScope = true,
            isReadOnly = true
          });
        Tools.SkipSpaces(state.Code, ref index);
        if (!Parser.Validate(state.Code, "from", ref index))
          ExceptionHelper.ThrowSyntaxError("Expected 'from'", state.Code, index);
        Tools.SkipSpaces(state.Code, ref index);
        startIndex = index;
        if (!Parser.ValidateString(state.Code, ref index, true))
          ExceptionHelper.ThrowSyntaxError("Expected module name", state.Code, index);
      }
      import._moduleName = Tools.Unescape(state.Code.Substring(startIndex + 1, index - startIndex - 2), false);
      return (CodeNode) import;
    }

    private static void parseImportMap(
      ImportStatement import,
      string code,
      ref int index,
      ParseInfo state)
    {
      ++index;
      Tools.SkipSpaces(code, ref index);
      if (code[index] == '}')
        ExceptionHelper.ThrowSyntaxError("Empty import map", code, index);
      while (code[index] != '}')
      {
        int startIndex = index;
        if (!Parser.ValidateName(code, ref index))
          ExceptionHelper.ThrowSyntaxError("Invalid import name", code, index);
        string str = code.Substring(startIndex, index - startIndex);
        Tools.SkipSpaces(code, ref index);
        string name = ImportStatement.parseAlias(code, ref index) ?? str;
        for (int index1 = 0; index1 < import._map.Count; ++index1)
        {
          if (import._map[index1].Key == str)
            ExceptionHelper.ThrowSyntaxError("Duplicate import", code, index);
        }
        List<KeyValuePair<string, Variable>> map = import._map;
        string key = str;
        Variable variable = new Variable(name, state.LexicalScopeLevel);
        variable.Position = startIndex;
        variable.Length = str.Length;
        KeyValuePair<string, Variable> keyValuePair = new KeyValuePair<string, Variable>(key, variable);
        map.Add(keyValuePair);
        if (Parser.Validate(code, ",", ref index))
          Tools.SkipSpaces(code, ref index);
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
        if (!Parser.ValidateName(code, ref index))
          ExceptionHelper.ThrowSyntaxError("Invalid import alias", code, index);
        alias = code.Substring(startIndex, index - startIndex);
        Tools.SkipSpaces(code, ref index);
      }
      return alias;
    }

    public override void Decompose(ref CodeNode self)
    {
    }

    public override JSValue Evaluate(Context context)
    {
      if (context._module == null)
        ExceptionHelper.Throw(new Error("Module undefined"));
      if (string.IsNullOrEmpty(context._module.FilePath))
        ExceptionHelper.Throw(new Error("Module must has name"));
      Module module = context._module.Import(this._moduleName);
      if (module == null)
        return (JSValue) null;
      if (this._map.Count > 0)
      {
        for (int index = 0; index < this._map.Count; ++index)
        {
          JSValue jsValue;
          switch (this._map[index].Key)
          {
            case "":
              jsValue = module.Exports.Default;
              break;
            case "*":
              jsValue = (JSValue) module.Exports.CreateExportList();
              break;
            default:
              jsValue = module.Exports[this._map[index].Key];
              break;
          }
          context._variables[this._map[index].Value._variableName] = jsValue;
        }
      }
      else
      {
        foreach (KeyValuePair<string, JSValue> variable in (IEnumerable<KeyValuePair<string, JSValue>>) module.Context._variables)
          context._variables[variable.Key] = variable.Value;
      }
      return (JSValue) null;
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder("import ");
      int index = 0;
      if (this._map[index].Key == "")
      {
        stringBuilder.Append((object) this._map[index++].Value);
        if (this._map.Count > 1)
          stringBuilder.Append(", ");
      }
      else if (this._map[index].Key == "*")
        stringBuilder.Append("* as ").Append((object) this._map[index++].Value);
      if (index < this._map.Count)
      {
        stringBuilder.Append("{ ");
        while (true)
        {
          KeyValuePair<string, Variable> keyValuePair = this._map[index];
          stringBuilder.Append(keyValuePair.Key);
          if (keyValuePair.Key != keyValuePair.Value._variableName)
            stringBuilder.Append(" as ").Append((object) keyValuePair.Value);
          ++index;
          if (index < this._map.Count)
            stringBuilder.Append(", ");
          else
            break;
        }
        stringBuilder.Append(" }");
      }
      stringBuilder.Append(" from \"").Append(this._moduleName).Append("\"");
      return stringBuilder.ToString();
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
      for (int index = 0; index < this._map.Count; ++index)
      {
        Variable self = this._map[index].Value;
        Parser.Build<Variable>(ref self, 1, variables, codeContext, message, stats, opts);
      }
      return false;
    }
  }
}
