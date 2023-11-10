// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.ObjectDefinition
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Extensions;
using NiL.JS.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NiL.JS.Expressions
{
  public sealed class ObjectDefinition : Expression
  {
    private string[] _fieldNames;
    private Expression[] _values;
    private KeyValuePair<Expression, Expression>[] _computedProperties;

    public string[] FieldNames => this._fieldNames;

    public Expression[] Values => this._values;

    public KeyValuePair<Expression, Expression>[] ComputedProperties => this._computedProperties;

    protected internal override bool ContextIndependent => false;

    protected internal override PredictedType ResultType => PredictedType.Object;

    internal override bool ResultInTempContainer => false;

    protected internal override bool NeedDecompose => ((IEnumerable<Expression>) this._values).Any<Expression>((Func<Expression, bool>) (x => x.NeedDecompose));

    private ObjectDefinition(
      Dictionary<string, Expression> fields,
      KeyValuePair<Expression, Expression>[] computedProperties)
    {
      this._computedProperties = computedProperties;
      this._fieldNames = new string[fields.Count];
      this._values = new Expression[fields.Count];
      int index = 0;
      foreach (KeyValuePair<string, Expression> field in fields)
      {
        this._fieldNames[index] = field.Key;
        this._values[index++] = field.Value;
      }
    }

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      if (state.Code[index] != '{')
        throw new ArgumentException("Invalid JSON definition");
      Dictionary<string, Expression> fields = new Dictionary<string, Expression>();
      List<KeyValuePair<Expression, Expression>> keyValuePairList = new List<KeyValuePair<Expression, Expression>>();
      int num1 = index;
      while (state.Code[num1] != '}')
      {
        ++num1;
        Tools.SkipSpaces(state.Code, ref num1);
        int num2 = num1;
        if (state.Code[num1] != '}')
        {
          bool flag1 = Parser.Validate(state.Code, "get", ref num1) || Parser.Validate(state.Code, "set", ref num1);
          Tools.SkipSpaces(state.Code, ref num1);
          if (flag1 && state.Code[num1] == '(')
          {
            flag1 = false;
            num1 = num2;
          }
          bool flag2 = state.Code[num1] == '*';
          Tools.SkipSpaces(state.Code, ref num1);
          bool flag3 = false;
          if (!flag2)
          {
            flag3 = Parser.Validate(state.Code, "async", ref num1);
            Tools.SkipSpaces(state.Code, ref num1);
          }
          if (Parser.Validate(state.Code, "[", ref num1))
          {
            Expression key = ExpressionTree.Parse(state, ref num1, false, false, false, true, false);
            while (Tools.IsWhiteSpace(state.Code[num1]))
              ++num1;
            if (state.Code[num1] != ']')
              ExceptionHelper.ThrowSyntaxError("Expected ']'", state.Code, num1);
            do
            {
              ++num1;
            }
            while (Tools.IsWhiteSpace(state.Code[num1]));
            Tools.SkipSpaces(state.Code, ref num1);
            CodeNode codeNode;
            if (state.Code[num1] == '(')
            {
              codeNode = (CodeNode) FunctionDefinition.Parse(state, ref num1, flag2 ? FunctionKind.AnonymousGenerator : (flag3 ? FunctionKind.AsyncAnonymousFunction : FunctionKind.AnonymousFunction));
            }
            else
            {
              if (!Parser.Validate(state.Code, ":", ref num1))
                ExceptionHelper.ThrowSyntaxError(Strings.UnexpectedToken, state.Code, num1);
              codeNode = ExpressionTree.Parse(state, ref num1);
            }
            switch (state.Code[num2])
            {
              case 'g':
                keyValuePairList.Add(new KeyValuePair<Expression, Expression>(key, (Expression) new PropertyPair((Expression) codeNode, (Expression) null)));
                break;
              case 's':
                keyValuePairList.Add(new KeyValuePair<Expression, Expression>(key, (Expression) new PropertyPair((Expression) null, (Expression) codeNode)));
                break;
              default:
                keyValuePairList.Add(new KeyValuePair<Expression, Expression>(key, (Expression) codeNode));
                break;
            }
          }
          else if (flag1 && state.Code[num1] != ':')
          {
            num1 = num2;
            FunctionKind kind = state.Code[num1] == 's' ? FunctionKind.Setter : FunctionKind.Getter;
            FunctionDefinition functionDefinition = FunctionDefinition.Parse(state, ref num1, kind) as FunctionDefinition;
            string name = functionDefinition._name;
            if (!fields.ContainsKey(name))
            {
              PropertyPair propertyPair = new PropertyPair(kind == FunctionKind.Getter ? (Expression) functionDefinition : (Expression) null, kind == FunctionKind.Setter ? (Expression) functionDefinition : (Expression) null);
              fields.Add(name, (Expression) propertyPair);
            }
            else
            {
              if (!(fields[name] is PropertyPair propertyPair))
                ExceptionHelper.ThrowSyntaxError("Try to define " + kind.ToString().ToLowerInvariant() + " for defined field", state.Code, num2);
              if (kind == FunctionKind.Getter)
              {
                if (propertyPair.Getter == null)
                {
                  propertyPair.Getter = (Expression) functionDefinition;
                  goto label_68;
                }
              }
              else if (propertyPair.Setter == null)
              {
                propertyPair.Setter = (Expression) functionDefinition;
                goto label_68;
              }
              ExceptionHelper.ThrowSyntaxError("Try to redefine " + kind.ToString().ToLowerInvariant() + " of " + functionDefinition.Name, state.Code, num2);
            }
          }
          else
          {
            if (flag2)
            {
              do
              {
                ++num1;
              }
              while (Tools.IsWhiteSpace(state.Code[num1]));
            }
            num1 = num2;
            string str = "";
            if (Parser.ValidateName(state.Code, ref num1, false, true, state.Strict))
            {
              str = Tools.Unescape(state.Code.Substring(num2, num1 - num2), state.Strict);
            }
            else
            {
              if (!Parser.ValidateValue(state.Code, ref num1))
                return (CodeNode) null;
              if (state.Code[num2] == '-')
                ExceptionHelper.Throw((Error) new SyntaxError("Invalid char \"-\" at " + CodeCoordinates.FromTextPosition(state.Code, num2, 1)?.ToString()));
              int index1 = num2;
              double d;
              if (Tools.ParseNumber(state.Code, ref index1, out d))
                str = Tools.DoubleToString(d);
              else if (state.Code[num2] == '\'' || state.Code[num2] == '"')
              {
                str = Tools.Unescape(state.Code.Substring(num2 + 1, num1 - num2 - 2), state.Strict);
              }
              else
              {
                if (fields.Count == 0)
                  return (CodeNode) null;
                ExceptionHelper.Throw((Error) new SyntaxError("Invalid field name at " + CodeCoordinates.FromTextPosition(state.Code, num2, num1 - num2)?.ToString()));
              }
            }
            Tools.SkipSpaces(state.Code, ref num1);
            Expression expression1;
            if (state.Code[num1] == '(')
            {
              num1 = num2;
              expression1 = FunctionDefinition.Parse(state, ref num1, flag2 ? FunctionKind.MethodGenerator : (flag3 ? FunctionKind.AsyncMethod : FunctionKind.Method));
            }
            else
            {
              if (flag2 || flag3 && state.Code[num1] != ':')
                ExceptionHelper.ThrowSyntaxError("Unexpected token", state.Code, num1);
              if (state.Code[num1] != ':' && state.Code[num1] != ',' && state.Code[num1] != '}')
                ExceptionHelper.ThrowSyntaxError("Expected ',', ';' or '}'", state.Code, num1);
              Expression expression2;
              if (fields.TryGetValue(str, out expression2))
              {
                if ((state.Strict ? (!(expression2 is Constant constant) ? 1 : (constant.value != JSValue.undefined ? 1 : 0)) : (expression2 is PropertyPair ? 1 : 0)) != 0)
                  ExceptionHelper.ThrowSyntaxError("Try to redefine field \"" + str + "\"", state.Code, num2, num1 - num2);
                if (state.Message != null)
                  state.Message(MessageLevel.Warning, num1, 0, "Duplicate key \"" + str + "\"");
              }
              if (state.Code[num1] == ',' || state.Code[num1] == '}')
              {
                if (!Parser.ValidateName(str, 0))
                  ExceptionHelper.ThrowSyntaxError("Invalid variable name", state.Code, num1);
                Variable variable = new Variable(str, state.LexicalScopeLevel);
                variable.Position = num2;
                variable.Length = str.Length;
                expression1 = (Expression) variable;
              }
              else
              {
                ++num1;
                Tools.SkipSpaces(state.Code, ref num1);
                expression1 = ExpressionTree.Parse(state, ref num1, false, false, false, true, false);
              }
            }
            fields[str] = expression1;
          }
label_68:
          while (Tools.IsWhiteSpace(state.Code[num1]))
            ++num1;
          if (state.Code[num1] != ',' && state.Code[num1] != '}')
            return (CodeNode) null;
        }
        else
          break;
      }
      ++num1;
      int num3 = index;
      index = num1;
      ObjectDefinition objectDefinition = new ObjectDefinition(fields, keyValuePairList.ToArray());
      objectDefinition.Position = num3;
      objectDefinition.Length = index - num3;
      return (CodeNode) objectDefinition;
    }

    public override JSValue Evaluate(Context context)
    {
      JSObject jsObject = JSObject.CreateObject();
      if (this._fieldNames.Length == 0 && this._computedProperties.Length == 0)
        return (JSValue) jsObject;
      jsObject._fields = JSObject.getFieldsContainer();
      for (int index = 0; index < this._fieldNames.Length; ++index)
      {
        JSValue jsValue = this._values[index].Evaluate(context).CloneImpl(false);
        jsValue._attributes = JSValueAttributesInternal.None;
        if (this._fieldNames[index] == "__proto__")
          jsObject.__proto__ = jsValue._oValue as JSObject;
        else
          jsObject._fields[this._fieldNames[index]] = jsValue;
      }
      for (int index = 0; index < this._computedProperties.Length; ++index)
      {
        JSValue self1 = this._computedProperties[index].Key.Evaluate(context).CloneImpl(false);
        JSValue self2 = this._computedProperties[index].Value.Evaluate(context).CloneImpl(false);
        Symbol key1 = (Symbol) null;
        string key2 = (string) null;
        JSValue self3;
        if (self1.Is<Symbol>())
        {
          key1 = self1.As<Symbol>();
          if (jsObject._symbols == null)
            jsObject._symbols = (IDictionary<Symbol, JSValue>) new Dictionary<Symbol, JSValue>();
          if (!jsObject._symbols.TryGetValue(key1, out self3))
            jsObject._symbols[key1] = self3 = self2;
        }
        else
        {
          key2 = self1.As<string>();
          if (!jsObject._fields.TryGetValue(key2, out self3))
            jsObject._fields[key2] = self3 = self2;
        }
        if (self3 != self2)
        {
          if (self3.Is(JSValueType.Property) && self2.Is(JSValueType.Property))
          {
            NiL.JS.Core.PropertyPair propertyPair1 = self3.As<NiL.JS.Core.PropertyPair>();
            NiL.JS.Core.PropertyPair propertyPair2 = self2.As<NiL.JS.Core.PropertyPair>();
            propertyPair1.getter = propertyPair2.getter ?? propertyPair1.getter;
            propertyPair1.setter = propertyPair2.setter ?? propertyPair1.setter;
          }
          else if (self1.Is<Symbol>())
            jsObject._symbols[key1] = self2;
          else
            jsObject._fields[key2] = self2;
        }
      }
      return (JSValue) jsObject;
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
      this._codeContext = codeContext;
      for (int index = 0; index < this._values.Length; ++index)
        Parser.Build(ref this._values[index], 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);
      for (int index = 0; index < this._computedProperties.Length; ++index)
      {
        Expression key = this._computedProperties[index].Key;
        Parser.Build(ref key, 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);
        Expression s = this._computedProperties[index].Value;
        Parser.Build(ref s, 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);
        this._computedProperties[index] = new KeyValuePair<Expression, Expression>(key, s);
      }
      return false;
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      int length = this.Values.Length;
      while (length-- > 0)
      {
        CodeNode _this1 = (CodeNode) this.Values[length];
        _this1.Optimize(ref _this1, owner, message, opts, stats);
        this.Values[length] = _this1 as Expression;
      }
      for (int index = 0; index < this._computedProperties.Length; ++index)
      {
        Expression key = this._computedProperties[index].Key;
        key.Optimize(ref key, owner, message, opts, stats);
        Expression self = this._computedProperties[index].Value;
        self.Optimize(ref self, owner, message, opts, stats);
        this._computedProperties[index] = new KeyValuePair<Expression, Expression>(key, self);
      }
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      base.RebuildScope(functionInfo, transferedVariables, scopeBias);
      for (int index = 0; index < this._values.Length; ++index)
        this._values[index].RebuildScope(functionInfo, transferedVariables, scopeBias);
      for (int index = 0; index < this._computedProperties.Length; ++index)
      {
        this._computedProperties[index].Key.RebuildScope(functionInfo, transferedVariables, scopeBias);
        this._computedProperties[index].Value.RebuildScope(functionInfo, transferedVariables, scopeBias);
      }
    }

    protected internal override CodeNode[] GetChildrenImpl() => (CodeNode[]) this._values;

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    public override void Decompose(ref Expression self, IList<CodeNode> result)
    {
      int num1 = -1;
      int num2 = -1;
      for (int index = 0; index < this._values.Length; ++index)
      {
        this._values[index].Decompose(ref this._values[index], result);
        if (this._values[index].NeedDecompose)
          num1 = index;
      }
      for (int index = 0; index < this._computedProperties.Length; ++index)
      {
        Expression key = this._computedProperties[index].Key;
        key.Decompose(ref key, result);
        Expression self1 = this._computedProperties[index].Value;
        self1.Decompose(ref self1, result);
        if (key != this._computedProperties[index].Key || self1 != this._computedProperties[index].Value)
          this._computedProperties[index] = new KeyValuePair<Expression, Expression>(key, self1);
        if (this._computedProperties[index].Value.NeedDecompose || this._computedProperties[index].Key.NeedDecompose)
          num2 = index;
      }
      if (num2 >= 0)
        num1 = this._values.Length;
      for (int index = 0; index < num1; ++index)
      {
        if (!(this._values[index] is ExtractStoredValue))
        {
          result.Add((CodeNode) new StoreValue(this._values[index], false));
          this._values[index] = (Expression) new ExtractStoredValue(this._values[index]);
        }
      }
      for (int index = 0; index < num1; ++index)
      {
        Expression expression1 = (Expression) null;
        Expression expression2 = (Expression) null;
        if (!(this._computedProperties[index].Key is ExtractStoredValue))
        {
          result.Add((CodeNode) new StoreValue(this._computedProperties[index].Key, false));
          expression1 = (Expression) new ExtractStoredValue(this._computedProperties[index].Key);
        }
        if (!(this._computedProperties[index].Value is ExtractStoredValue))
        {
          result.Add((CodeNode) new StoreValue(this._computedProperties[index].Value, false));
          expression2 = (Expression) new ExtractStoredValue(this._computedProperties[index].Value);
        }
        if (expression1 != null || expression2 != null)
          this._computedProperties[index] = new KeyValuePair<Expression, Expression>(expression1 ?? this._computedProperties[index].Key, expression2 ?? this._computedProperties[index].Value);
      }
    }

    public override string ToString()
    {
      string str = "{ ";
      for (int index = 0; index < this._fieldNames.Length; ++index)
      {
        str = str + "\"" + this._fieldNames[index] + "\" : " + this._values[index]?.ToString();
        if (index + 1 < this._fieldNames.Length)
          str += ", ";
      }
      for (int index = 0; index < this._computedProperties.Length; ++index)
      {
        str = str + "[" + this._computedProperties[index].Key?.ToString() + "] : " + this._computedProperties[index].Value?.ToString();
        if (index + 1 < this._fieldNames.Length)
          str += ", ";
      }
      return str + " }";
    }
  }
}
