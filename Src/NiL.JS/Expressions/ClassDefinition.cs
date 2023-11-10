// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.ClassDefinition
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiL.JS.Expressions
{
  public sealed class ClassDefinition : EntityDefinition
  {
    private MemberDescriptor[] _members;
    private Expression _baseClass;
    private FunctionDefinition _constructor;
    private MemberDescriptor[] _computedProperties;

    protected internal override PredictedType ResultType => PredictedType.Function;

    internal override bool ResultInTempContainer => false;

    protected internal override bool ContextIndependent => false;

    protected internal override bool NeedDecompose
    {
      get
      {
        if (this._constructor.NeedDecompose)
          return true;
        for (int index = 0; index < this._members.Length; ++index)
        {
          if (this._members[index]._value.NeedDecompose)
            return true;
        }
        return false;
      }
    }

    public IEnumerable<MemberDescriptor> Members => (IEnumerable<MemberDescriptor>) this._members;

    public Expression BaseClass => this._baseClass;

    public FunctionDefinition Constructor => this._constructor;

    public IEnumerable<MemberDescriptor> ComputedProperties => (IEnumerable<MemberDescriptor>) this._computedProperties;

    public override bool Hoist => false;

    private ClassDefinition(
      string name,
      Expression baseType,
      MemberDescriptor[] fields,
      FunctionDefinition ctor,
      MemberDescriptor[] computedProperties)
      : base(name)
    {
      this._baseClass = baseType;
      this._constructor = ctor;
      this._members = fields;
      this._computedProperties = computedProperties;
    }

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
      int num1 = index;
      if (!Parser.Validate(state.Code, "class", ref num1))
        return (CodeNode) null;
      Tools.SkipSpaces(state.Code, ref num1);
      string name1 = (string) null;
      Expression baseType = (Expression) null;
      if (!Parser.Validate(state.Code, "extends ", num1))
      {
        int startIndex = num1;
        if (Parser.ValidateName(state.Code, ref num1, true))
          name1 = state.Code.Substring(startIndex, num1 - startIndex);
        while (Tools.IsWhiteSpace(state.Code[num1]))
          ++num1;
      }
      if (Parser.Validate(state.Code, "extends ", ref num1))
      {
        int startIndex = num1;
        if (!Parser.ValidateName(state.Code, ref num1, true) && !Parser.Validate(state.Code, "null", ref num1))
          ExceptionHelper.ThrowSyntaxError("Invalid base class name", state.Code, num1);
        string name2 = state.Code.Substring(startIndex, num1 - startIndex);
        baseType = !(name2 == "null") ? (Expression) new Variable(name2, 1) : (Expression) new Constant((JSValue) JSValue.@null);
        baseType.Position = startIndex;
        baseType.Length = num1 - startIndex;
        while (Tools.IsWhiteSpace(state.Code[num1]))
          ++num1;
      }
      if (state.Code[num1] != '{')
        ExceptionHelper.ThrowSyntaxError(Strings.UnexpectedToken, state.Code, num1);
      FunctionDefinition ctor = (FunctionDefinition) null;
      Dictionary<string, MemberDescriptor> dictionary = new Dictionary<string, MemberDescriptor>();
      List<MemberDescriptor> memberDescriptorList = new List<MemberDescriptor>();
      while (state.Code[num1] != '}')
      {
        using (state.WithCodeContext(CodeContext.Strict | CodeContext.InExpression))
        {
          do
          {
            ++num1;
          }
          while (Tools.IsWhiteSpace(state.Code[num1]) || state.Code[num1] == ';');
          int num2 = num1;
          if (state.Code[num1] != '}')
          {
            bool @static = Parser.Validate(state.Code, "static", ref num1);
            if (@static)
            {
              Tools.SkipSpaces(state.Code, ref num1);
              num2 = num1;
            }
            bool flag1 = Parser.Validate(state.Code, "async", ref num1);
            if (flag1)
            {
              Tools.SkipSpaces(state.Code, ref num1);
              num2 = num1;
            }
            bool flag2 = Parser.Validate(state.Code, "get", ref num1) || Parser.Validate(state.Code, "set", ref num1);
            if (flag2)
            {
              Tools.SkipSpaces(state.Code, ref num1);
              if (state.Code[num1] == '(')
              {
                num1 = num2;
                flag2 = false;
              }
            }
            bool flag3 = state.Code[num1] == '*';
            if (flag3)
            {
              do
              {
                ++num1;
              }
              while (Tools.IsWhiteSpace(state.Code[num1]));
            }
            if (Parser.Validate(state.Code, "[", ref num1))
            {
              Expression name3 = ExpressionTree.Parse(state, ref num1, false, false, false, true, false);
              while (Tools.IsWhiteSpace(state.Code[num1]))
                ++num1;
              if (state.Code[num1] != ']')
                ExceptionHelper.ThrowSyntaxError("Expected ']'", state.Code, num1);
              do
              {
                ++num1;
              }
              while (Tools.IsWhiteSpace(state.Code[num1]));
              CodeNode codeNode = state.Code[num1] != '(' ? ExpressionTree.Parse(state, ref num1) : (CodeNode) FunctionDefinition.Parse(state, ref num1, flag3 ? FunctionKind.AnonymousGenerator : FunctionKind.AnonymousFunction);
              switch (state.Code[num2])
              {
                case 'g':
                  memberDescriptorList.Add(new MemberDescriptor(name3, (Expression) new PropertyPair((Expression) codeNode, (Expression) null), @static));
                  continue;
                case 's':
                  memberDescriptorList.Add(new MemberDescriptor(name3, (Expression) new PropertyPair((Expression) null, (Expression) codeNode), @static));
                  continue;
                default:
                  memberDescriptorList.Add(new MemberDescriptor(name3, (Expression) codeNode, @static));
                  continue;
              }
            }
            else if (flag2)
            {
              num1 = num2;
              FunctionKind kind = state.Code[num1] == 's' ? FunctionKind.Setter : FunctionKind.Getter;
              FunctionDefinition functionDefinition = FunctionDefinition.Parse(state, ref num1, kind) as FunctionDefinition;
              string key = (@static ? "static " : "") + functionDefinition._name;
              if (!dictionary.ContainsKey(key))
              {
                PropertyPair propertyPair = new PropertyPair(kind == FunctionKind.Getter ? (Expression) functionDefinition : (Expression) null, kind == FunctionKind.Setter ? (Expression) functionDefinition : (Expression) null);
                dictionary.Add(key, new MemberDescriptor((Expression) new Constant((JSValue) functionDefinition._name), (Expression) propertyPair, @static));
              }
              else
              {
                if (!(dictionary[key].Value is PropertyPair propertyPair))
                  ExceptionHelper.Throw((Error) new SyntaxError("Try to define " + kind.ToString().ToLowerInvariant() + " for defined field at " + CodeCoordinates.FromTextPosition(state.Code, num2, 0)?.ToString()));
                if (kind == FunctionKind.Getter)
                {
                  if (propertyPair.Getter == null)
                  {
                    propertyPair.Getter = (Expression) functionDefinition;
                    continue;
                  }
                }
                else if (propertyPair.Setter == null)
                {
                  propertyPair.Setter = (Expression) functionDefinition;
                  continue;
                }
                ExceptionHelper.ThrowSyntaxError("Try to redefine " + kind.ToString().ToLowerInvariant() + " of " + functionDefinition.Name, state.Code, num2);
              }
            }
            else
            {
              num1 = num2;
              string key = (string) null;
              if (state.Code[num1] == '*')
              {
                do
                {
                  ++num1;
                }
                while (Tools.IsWhiteSpace(state.Code[num1]));
              }
              if (Parser.ValidateName(state.Code, ref num1, false, true, state.Strict))
                key = Tools.Unescape(state.Code.Substring(num2, num1 - num2), state.Strict);
              else if (Parser.ValidateValue(state.Code, ref num1))
              {
                double d = 0.0;
                int index1 = num2;
                if (Tools.ParseNumber(state.Code, ref index1, out d))
                  key = Tools.DoubleToString(d);
                else if (state.Code[num2] == '\'' || state.Code[num2] == '"')
                  key = Tools.Unescape(state.Code.Substring(num2 + 1, num1 - num2 - 2), state.Strict);
              }
              if (key == null)
                ExceptionHelper.Throw((Error) new SyntaxError("Invalid member name at " + CodeCoordinates.FromTextPosition(state.Code, num2, num1 - num2)?.ToString()));
              if (key == "constructor")
              {
                if (@static)
                  ExceptionHelper.ThrowSyntaxError(Strings.ConstructorCannotBeStatic, state.Code, num2);
                if (ctor != null)
                  ExceptionHelper.ThrowSyntaxError("Trying to redefinition constructor", state.Code, num2);
                state.CodeContext |= CodeContext.InClassConstructor;
              }
              else if (@static)
              {
                key = "static " + key;
                state.CodeContext |= CodeContext.InStaticMember;
              }
              if (dictionary.ContainsKey(key))
                ExceptionHelper.Throw((Error) new SyntaxError("Trying to redefinition member \"" + key + "\" at " + CodeCoordinates.FromTextPosition(state.Code, num2, num1 - num2)?.ToString()));
              state.CodeContext |= CodeContext.InClassDefinition;
              state.CodeContext &= ~CodeContext.InGenerator;
              if (flag1)
                state.CodeContext |= CodeContext.InAsync;
              num1 = num2;
              if (!(FunctionDefinition.Parse(state, ref num1, flag1 ? FunctionKind.AsyncMethod : FunctionKind.Method) is FunctionDefinition functionDefinition))
                ExceptionHelper.ThrowSyntaxError("Unable to parse method", state.Code, num1);
              if (key == "constructor")
                ctor = functionDefinition;
              else
                dictionary[key] = new MemberDescriptor((Expression) new Constant((JSValue) functionDefinition._name), (Expression) functionDefinition, @static);
            }
          }
          else
            break;
        }
      }
      if (ctor == null)
      {
        int index2 = 0;
        string code;
        switch (baseType)
        {
          case null:
          case Constant _:
            code = "constructor(...args) { }";
            break;
          default:
            code = "constructor(...args) { super(...args); }";
            break;
        }
        ParseInfo state1 = state.AlternateCode(code);
        using (state1.WithCodeContext(CodeContext.InClassDefinition | CodeContext.InClassConstructor))
          ctor = (FunctionDefinition) FunctionDefinition.Parse(state1, ref index2, FunctionKind.Method);
      }
      ClassDefinition classDefinition = new ClassDefinition(name1, baseType, new List<MemberDescriptor>((IEnumerable<MemberDescriptor>) dictionary.Values).ToArray(), ctor, memberDescriptorList.ToArray());
      if ((state.CodeContext & CodeContext.InExpression) == CodeContext.None && ((state.CodeContext & CodeContext.InExport) == CodeContext.None || !string.IsNullOrEmpty(name1)))
      {
        if (string.IsNullOrEmpty(name1))
          ExceptionHelper.ThrowSyntaxError("Class must have a name", state.Code, index);
        if (state.Strict && state.FunctionScopeLevel != state.LexicalScopeLevel)
          ExceptionHelper.ThrowSyntaxError("In strict mode code, class can only be declared at top level or immediately within other function.", state.Code, index);
        state.Variables.Add(classDefinition.reference._descriptor);
      }
      index = num1 + 1;
      return (CodeNode) classDefinition;
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
      if (this.Built)
        return false;
      this.Built = true;
      this._codeContext = codeContext;
      if ((codeContext & CodeContext.InExpression) == CodeContext.None)
        stats.WithLexicalEnvironment = true;
      VariableDescriptor variableDescriptor = (VariableDescriptor) null;
      if (!string.IsNullOrEmpty(this._name))
      {
        variables.TryGetValue(this._name, out variableDescriptor);
        variables[this._name] = this.reference._descriptor;
      }
      Parser.Build<FunctionDefinition>(ref this._constructor, expressionDepth, variables, codeContext | CodeContext.InClassDefinition | CodeContext.InClassConstructor, message, stats, opts);
      Parser.Build(ref this._baseClass, expressionDepth, variables, codeContext, message, stats, opts);
      for (int index = 0; index < this._members.Length; ++index)
        Parser.Build(ref this._members[index]._value, expressionDepth, variables, codeContext | CodeContext.InClassDefinition | (this._members[index]._static ? CodeContext.InStaticMember : CodeContext.None), message, stats, opts);
      for (int index = 0; index < this._computedProperties.Length; ++index)
      {
        Parser.Build(ref this._computedProperties[index]._name, 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);
        Parser.Build(ref this._computedProperties[index]._value, 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);
      }
      if (variableDescriptor != null)
        variables[variableDescriptor.name] = variableDescriptor;
      else if (!string.IsNullOrEmpty(this._name))
        variables.Remove(this._name);
      return false;
    }

    public override JSValue Evaluate(Context context)
    {
      JSValue jsValue1 = (JSValue) null;
      if ((this._codeContext & CodeContext.InExpression) == CodeContext.None && !string.IsNullOrEmpty(this._name))
        jsValue1 = context.DefineVariable(this._name);
      ClassDefinition.ClassConstructor classConstructor = new ClassDefinition.ClassConstructor(context, this._constructor, this);
      classConstructor.RequireNewKeywordLevel = RequireNewKeywordLevel.WithNewOnly;
      if (this._baseClass != null)
      {
        if (!(this._baseClass.Evaluate(context)._oValue is JSObject oValue))
          classConstructor.prototype.__proto__ = (JSObject) null;
        else
          classConstructor.prototype.__proto__ = Tools.InvokeGetter(oValue.GetProperty("prototype"), (JSValue) oValue)._oValue as JSObject;
        classConstructor.__proto__ = oValue;
      }
      for (int index = 0; index < this._members.Length; ++index)
      {
        MemberDescriptor member = this._members[index];
        JSValue jsValue2 = member.Value.Evaluate(context);
        (!member.Static ? classConstructor.prototype : (JSValue) classConstructor).SetProperty(member.Name.Evaluate((Context) null), jsValue2, true);
      }
      for (int index = 0; index < this._computedProperties.Length; ++index)
      {
        MemberDescriptor computedProperty = this._computedProperties[index];
        JSObject jsObject = !computedProperty.Static ? classConstructor.prototype._oValue as JSObject : (JSObject) classConstructor;
        JSValue self1 = computedProperty._name.Evaluate(context).CloneImpl(false);
        JSValue self2 = computedProperty._value.Evaluate(context).CloneImpl(false);
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
      jsValue1?.Assign((JSValue) classConstructor);
      return (JSValue) classConstructor;
    }

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit(this);

    protected internal override CodeNode[] GetChildrenImpl()
    {
      List<CodeNode> codeNodeList = new List<CodeNode>();
      for (int index = 0; index < this._members.Length; ++index)
        codeNodeList.Add((CodeNode) this._members[index]._value);
      for (int index = 0; index < this._computedProperties.Length; ++index)
      {
        codeNodeList.Add((CodeNode) this._computedProperties[index].Name);
        codeNodeList.Add((CodeNode) this._computedProperties[index].Value);
      }
      if (this._baseClass != null)
        codeNodeList.Add((CodeNode) this._baseClass);
      return codeNodeList.ToArray();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("class ").Append(this._name);
      if (this._baseClass != null)
        stringBuilder.Append(" extends ").Append((object) this._baseClass);
      stringBuilder.Append(" {").Append(Environment.NewLine);
      string str1 = this._constructor.ToString().Replace(Environment.NewLine, Environment.NewLine + "  ");
      stringBuilder.Append("constructor");
      stringBuilder.Append(str1.Substring("constructor".Length));
      for (int index = 0; index < this._members.Length; ++index)
      {
        string str2 = this._members[index].ToString().Replace(Environment.NewLine, Environment.NewLine + "  ");
        stringBuilder.Append(str2);
      }
      stringBuilder.Append(Environment.NewLine).Append("}");
      return stringBuilder.ToString();
    }

    public override void Decompose(ref Expression self, IList<CodeNode> result)
    {
      for (int index = 0; index < this._members.Length; ++index)
        this._members[index]._value.Decompose(ref this._members[index]._value, result);
      for (int index = 0; index < this._computedProperties.Length; ++index)
      {
        this._computedProperties[index]._name.Decompose(ref this._computedProperties[index]._name, result);
        this._computedProperties[index]._value.Decompose(ref this._computedProperties[index]._value, result);
      }
    }

    public override void Optimize(
      ref CodeNode _this,
      FunctionDefinition owner,
      InternalCompilerMessageCallback message,
      Options opts,
      FunctionInfo stats)
    {
      this._baseClass?.Optimize(ref this._baseClass, owner, message, opts, stats);
      int length = this._members.Length;
      while (length-- > 0)
        this._members[length]._value.Optimize(ref this._members[length]._value, owner, message, opts, stats);
      for (int index = 0; index < this._computedProperties.Length; ++index)
      {
        this._computedProperties[index]._name.Optimize(ref this._computedProperties[index]._name, owner, message, opts, stats);
        this._computedProperties[index]._value.Optimize(ref this._computedProperties[index]._value, owner, message, opts, stats);
      }
    }

    public override void RebuildScope(
      FunctionInfo functionInfo,
      Dictionary<string, VariableDescriptor> transferedVariables,
      int scopeBias)
    {
      base.RebuildScope(functionInfo, (Dictionary<string, VariableDescriptor>) null, scopeBias);
      this._baseClass?.RebuildScope(functionInfo, (Dictionary<string, VariableDescriptor>) null, scopeBias);
      this._constructor.RebuildScope(functionInfo, (Dictionary<string, VariableDescriptor>) null, scopeBias);
      for (int index = 0; index < this._computedProperties.Length; ++index)
      {
        this._computedProperties[index].Name.RebuildScope(functionInfo, (Dictionary<string, VariableDescriptor>) null, scopeBias);
        this._computedProperties[index].Value.RebuildScope(functionInfo, (Dictionary<string, VariableDescriptor>) null, scopeBias);
      }
      for (int index = 0; index < this._members.Length; ++index)
      {
        this._members[index].Name.RebuildScope(functionInfo, (Dictionary<string, VariableDescriptor>) null, scopeBias);
        this._members[index].Value.RebuildScope(functionInfo, (Dictionary<string, VariableDescriptor>) null, scopeBias);
      }
    }

    private sealed class ClassConstructor : Function
    {
      private readonly ClassDefinition classDefinition;

      public override string name => this.classDefinition.Name;

      public ClassConstructor(
        Context context,
        FunctionDefinition creator,
        ClassDefinition classDefinition)
        : base(context, creator)
      {
        this.classDefinition = classDefinition;
      }

      protected internal override JSValue ConstructObject()
      {
        JSObject jsObject = JSObject.CreateObject();
        jsObject.__proto__ = this.prototype._oValue as JSObject;
        return (JSValue) jsObject;
      }

      public override string ToString(bool headerOnly) => this.classDefinition.ToString();
    }
  }
}
