// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Parser
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Expressions;
using NiL.JS.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NiL.JS.Core
{
  public static class Parser
  {
    private static HashSet<string> customReservedWords;
    private static List<Rule>[] rules = new List<Rule>[3]
    {
      new List<Rule>()
      {
        new Rule("[", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("{", new ParseDelegate(CodeBlock.Parse)),
        new Rule("var ", new ParseDelegate(VariableDefinition.Parse)),
        new Rule("let ", new ParseDelegate(VariableDefinition.Parse)),
        new Rule("const ", new ParseDelegate(VariableDefinition.Parse)),
        new Rule("if", new ParseDelegate(IfElse.Parse)),
        new Rule("for", new ParseDelegate(ForOf.Parse)),
        new Rule("for", new ParseDelegate(ForIn.Parse)),
        new Rule("for", new ParseDelegate(For.Parse)),
        new Rule("while", new ParseDelegate(While.Parse)),
        new Rule("return", new ParseDelegate(Return.Parse)),
        new Rule("await", new ParseDelegate(AwaitExpression.Parse)),
        new Rule("function", new ParseDelegate(FunctionDefinition.ParseFunction)),
        new Rule("async function", FunctionDefinition.ParseFunction(FunctionKind.AsyncFunction)),
        new Rule("class", new ParseDelegate(ClassDefinition.Parse)),
        new Rule("switch", new ParseDelegate(Switch.Parse)),
        new Rule("with", new ParseDelegate(With.Parse)),
        new Rule("do", new ParseDelegate(DoWhile.Parse)),
        new Rule(new ValidateDelegate(Parser.ValidateArrow), FunctionDefinition.ParseFunction(FunctionKind.Arrow)),
        new Rule(new ValidateDelegate(Parser.ValidateAsyncArrow), FunctionDefinition.ParseFunction(FunctionKind.AsyncArrow)),
        new Rule("(", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("+", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("-", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("!", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("~", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("`", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("true", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("false", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("null", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("this", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("super", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("typeof", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("try", new ParseDelegate(TryCatch.Parse)),
        new Rule("new", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("delete", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("void", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("yield", new ParseDelegate(Yield.Parse)),
        new Rule("break", new ParseDelegate(Break.Parse)),
        new Rule("continue", new ParseDelegate(Continue.Parse)),
        new Rule("throw", new ParseDelegate(Throw.Parse)),
        new Rule("import (", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("import", new ParseDelegate(ImportStatement.Parse)),
        new Rule("export", new ParseDelegate(ExportStatement.Parse)),
        new Rule(new ValidateDelegate(Parser.ValidateName), new ParseDelegate(LabeledStatement.Parse)),
        new Rule(new ValidateDelegate(Parser.ValidateName), new ParseDelegate(ExpressionTree.Parse)),
        new Rule(new ValidateDelegate(Parser.ValidateValue), new ParseDelegate(ExpressionTree.Parse)),
        new Rule("debugger", new ParseDelegate(Debugger.Parse))
      },
      new List<Rule>()
      {
        new Rule("[", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("{", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("await", new ParseDelegate(AwaitExpression.Parse)),
        new Rule("function", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("class", new ParseDelegate(ExpressionTree.Parse)),
        new Rule(new ValidateDelegate(Parser.ValidateArrow), FunctionDefinition.ParseFunction(FunctionKind.Arrow)),
        new Rule(new ValidateDelegate(Parser.ValidateAsyncArrow), FunctionDefinition.ParseFunction(FunctionKind.AsyncArrow)),
        new Rule("(", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("+", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("-", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("!", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("~", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("`", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("true", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("false", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("null", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("this", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("typeof", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("new", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("delete", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("void", new ParseDelegate(ExpressionTree.Parse)),
        new Rule("yield", new ParseDelegate(Yield.Parse)),
        new Rule("import (", new ParseDelegate(Import.Parse)),
        new Rule(new ValidateDelegate(Parser.ValidateName), new ParseDelegate(ExpressionTree.Parse)),
        new Rule(new ValidateDelegate(Parser.ValidateValue), new ParseDelegate(ExpressionTree.Parse))
      },
      new List<Rule>()
      {
        new Rule("`", new ParseDelegate(TemplateString.Parse)),
        new Rule("[", new ParseDelegate(ArrayDefinition.Parse)),
        new Rule("{", new ParseDelegate(ObjectDefinition.Parse)),
        new Rule("await", new ParseDelegate(AwaitExpression.Parse)),
        new Rule("function", new ParseDelegate(FunctionDefinition.ParseFunction)),
        new Rule("class", new ParseDelegate(ClassDefinition.Parse)),
        new Rule("new", new ParseDelegate(New.Parse)),
        new Rule("yield", new ParseDelegate(Yield.Parse)),
        new Rule("import (", new ParseDelegate(Import.Parse)),
        new Rule(new ValidateDelegate(Parser.ValidateArrow), FunctionDefinition.ParseFunction(FunctionKind.Arrow)),
        new Rule(new ValidateDelegate(Parser.ValidateAsyncArrow), FunctionDefinition.ParseFunction(FunctionKind.AsyncArrow)),
        new Rule(new ValidateDelegate(Parser.ValidateRegex), new ParseDelegate(RegExpExpression.Parse))
      }
    };

    private static bool ValidateAsyncArrow(string code, int index)
    {
      if (!Parser.Validate(code, "async", ref index))
        return false;
      Tools.SkipSpaces(code, ref index);
      return Parser.ValidateArrow(code, index);
    }

    private static bool ValidateArrow(string code, int index)
    {
      bool flag = code[index] == '(';
      if (flag)
        ++index;
      else if (!Parser.ValidateName(code, ref index))
        return false;
      if (code.Length == index)
        return false;
      while (Tools.IsWhiteSpace(code[index]))
      {
        ++index;
        if (code.Length == index)
          return false;
      }
      if (flag)
      {
        if (code[index] != ')')
        {
          --index;
          do
          {
            do
            {
              ++index;
              if (code.Length == index)
                return false;
            }
            while (Tools.IsWhiteSpace(code[index]));
            Parser.Validate(code, "...", ref index);
            if (!Parser.ValidateName(code, ref index))
              return false;
            while (Tools.IsWhiteSpace(code[index]))
            {
              ++index;
              if (code.Length == index)
                return false;
            }
            if (code[index] == '=')
            {
              ++index;
              Tools.SkipSpaces(code, ref index);
              for (int index1 = 0; index1 >= 0 && code[index] != ',' && code[index] != ')'; ++index)
              {
                if (code[index] == '(')
                  ++index1;
                if (code[index] == ')')
                  --index1;
              }
            }
          }
          while (code[index] == ',');
          if (code[index] != ')')
            return false;
        }
        do
        {
          ++index;
          if (code.Length == index)
            return false;
        }
        while (Tools.IsWhiteSpace(code[index]));
      }
      return Parser.Validate(code, "=>", index);
    }

    [CLSCompliant(false)]
    public static bool Validate(string code, string patern, int index) => Parser.Validate(code, patern, ref index);

    public static bool Validate(string code, string pattern, ref int index)
    {
      if (string.IsNullOrEmpty(pattern))
        return true;
      if (string.IsNullOrEmpty(code))
        return false;
      int index1 = 0;
      int index2 = index;
      bool flag = false;
      while (index1 < pattern.Length)
      {
        if (index2 >= code.Length)
          return false;
        if (Tools.IsWhiteSpace(pattern[index1]))
        {
          while (code.Length > index2 && Tools.IsWhiteSpace(code[index2]))
          {
            ++index2;
            flag = true;
          }
          if (flag)
          {
            ++index1;
            if (index1 != pattern.Length)
            {
              if (code.Length <= index2)
                return false;
              flag = false;
            }
            else
              break;
          }
          else if (index1 < pattern.Length - 1 && Parser.IsIdentifierTerminator(pattern[index1 + 1]))
            ++index1;
        }
        if ((int) code[index2] != (int) pattern[index1])
          return false;
        ++index1;
        ++index2;
      }
      int num = Parser.IsIdentifierTerminator(pattern[pattern.Length - 1]) || index2 >= code.Length ? 1 : (Parser.IsIdentifierTerminator(code[index2]) ? 1 : 0);
      if (num == 0)
        return num != 0;
      index = index2;
      return num != 0;
    }

    public static bool ValidateName(string code) => Parser.ValidateName(code, 0);

    public static bool ValidateName(string code, int index) => Parser.ValidateName(code, ref index, true, true, false);

    [CLSCompliant(false)]
    public static bool ValidateName(string code, ref int index) => Parser.ValidateName(code, ref index, true, true, false);

    public static bool ValidateName(string code, ref int index, bool strict) => Parser.ValidateName(code, ref index, true, true, strict);

    [CLSCompliant(false)]
    public static bool ValidateName(string code, int index, bool strict) => Parser.ValidateName(code, ref index, true, true, strict);

    [CLSCompliant(false)]
    public static bool ValidateName(
      string name,
      int index,
      bool reserveControl,
      bool allowEscape,
      bool strict)
    {
      return Parser.ValidateName(name, ref index, reserveControl, allowEscape, strict);
    }

    public static bool ValidateName(
      string code,
      ref int index,
      bool checkReservedWords,
      bool allowEscape,
      bool strict)
    {
      int index1 = index;
      if ((!allowEscape || code[index1] != '\\') && code[index1] != '$' && code[index1] != '_' && !char.IsLetter(code[index1]))
        return false;
      int index2 = index1 + 1;
      while (index2 < code.Length && (allowEscape && code[index2] == '\\' || code[index2] == '$' || code[index2] == '_' || char.IsLetterOrDigit(code[index2])))
        ++index2;
      if (index == index2)
        return false;
      if (allowEscape | checkReservedWords)
      {
        string code1 = code.Substring(index, index2 - index);
        if (allowEscape)
        {
          int index3 = 0;
          string code2 = Tools.Unescape(code1, strict, false, false);
          if (code2 != code1)
          {
            int num = !Parser.ValidateName(code2, ref index3, checkReservedWords, false, strict) ? 0 : (index3 == code2.Length ? 1 : 0);
            if (num == 0)
              return num != 0;
            index = index2;
            return num != 0;
          }
          if (code2.IndexOf('\\') != -1)
            return false;
        }
        if (checkReservedWords)
        {
          switch (code1)
          {
            case "break":
            case "case":
            case "catch":
            case "class":
            case "const":
            case "continue":
            case "debugger":
            case "default":
            case "delete":
            case "do":
            case "else":
            case "enum":
            case "export":
            case "extends":
            case "false":
            case "finally":
            case "for":
            case "function":
            case "if":
            case "import":
            case "in":
            case "instanceof":
            case "new":
            case "null":
            case "return":
            case "super":
            case "switch":
            case "this":
            case "throw":
            case "true":
            case "try":
            case "typeof":
            case "var":
            case "void":
            case "while":
            case "with":
            case "yield":
              return false;
            case "implements":
            case "interface":
            case "let":
            case "package":
            case "private":
            case "protected":
            case "public":
            case "static":
              if (strict)
                return false;
              break;
            default:
              if (Parser.customReservedWords != null && Parser.customReservedWords.Contains(code1))
                return false;
              break;
          }
        }
      }
      index = index2;
      return true;
    }

    public static bool ValidateNumber(string code, ref int index)
    {
      double num = 0.0;
      return Tools.ParseNumber(code, ref index, out num, 0, ParseNumberOptions.AllowFloat | ParseNumberOptions.AllowAutoRadix);
    }

    public static bool ValidateRegex(string code, int index) => Parser.ValidateRegex(code, ref index, false);

    public static bool ValidateRegex(string code, ref int index, bool throwError)
    {
      int index1 = index;
      if (code[index1] != '/')
        return false;
      bool flag1 = false;
      int index2;
      for (index2 = index1 + 1; index2 < code.Length && (flag1 || code[index2] != '/'); ++index2)
      {
        if (Tools.IsLineTerminator(code[index2]))
          return false;
        if (code[index2] == '\\')
        {
          ++index2;
          if (Tools.IsLineTerminator(code[index2]))
            return false;
        }
        else
        {
          if (code[index2] == '[')
            flag1 = true;
          if (code[index2] == ']')
            flag1 = false;
        }
      }
      if (index2 == code.Length)
        return false;
      bool flag2 = true;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      bool flag6 = false;
      bool flag7 = false;
      while (flag2)
      {
        ++index2;
        if (index2 < code.Length)
        {
          char c = code[index2];
          switch (c)
          {
            case 'g':
              if (flag3)
              {
                if (throwError)
                  throw new ArgumentException("Invalid flag in RegExp definition");
                return false;
              }
              flag3 = true;
              continue;
            case 'i':
              if (flag4)
              {
                if (throwError)
                  throw new ArgumentException("Invalid flag in RegExp definition");
                return false;
              }
              flag4 = true;
              continue;
            case 'm':
              if (flag5)
              {
                if (throwError)
                  throw new ArgumentException("Invalid flag in RegExp definition");
                return false;
              }
              flag5 = true;
              continue;
            case 'u':
              if (flag6)
              {
                if (throwError)
                  throw new ArgumentException("Invalid flag in RegExp definition");
                return false;
              }
              flag6 = true;
              continue;
            case 'y':
              if (flag7)
              {
                if (throwError)
                  throw new ArgumentException("Invalid flag in RegExp definition");
                return false;
              }
              flag7 = true;
              continue;
            default:
              if (Parser.IsIdentifierTerminator(c))
              {
                flag2 = false;
                continue;
              }
              if (throwError)
                throw new ArgumentException("Invalid flag in regexp definition");
              return false;
          }
        }
        else
          break;
      }
      index = index2;
      return true;
    }

    public static bool ValidateString(string code, ref int index, bool @throw)
    {
      int index1 = index;
      if (index1 + 1 >= code.Length || code[index1] != '\'' && code[index1] != '"' && code[index1] != '`')
        return false;
      char ch = code[index1];
      int index2 = index1 + 1;
      while ((int) code[index2] != (int) ch)
      {
        if (code[index2] == '\\')
        {
          ++index2;
          if (code[index2] == '\r' && code[index2 + 1] == '\n')
            ++index2;
          else if (code[index2] == '\n' && code[index2 + 1] == '\r')
            ++index2;
        }
        else if (Tools.IsLineTerminator(code[index2]) || index2 + 1 >= code.Length)
        {
          if (!@throw)
            return false;
          ExceptionHelper.Throw((Error) new SyntaxError("Unterminated string constant"));
        }
        ++index2;
        if (index2 >= code.Length)
          return false;
      }
      int num;
      index = num = index2 + 1;
      return true;
    }

    [CLSCompliant(false)]
    public static bool ValidateValue(string code, int index) => Parser.ValidateValue(code, ref index);

    public static bool ValidateValue(string code, ref int index)
    {
      int num = index;
      if (code[num] == '/')
        return Parser.ValidateRegex(code, ref index, true);
      if (code[num] == '\'' || code[num] == '"')
        return Parser.ValidateString(code, ref index, false);
      if (code.Length - num >= 4 && (code[num] == 'n' || code[num] == 't' || code[num] == 'f') && code.Length - num >= 4)
      {
        if (code.IndexOf("null", num, 4) != -1 || code.IndexOf("true", num, 4) != -1)
        {
          index += 4;
          return true;
        }
        if (code.Length - num >= 5 && code.IndexOf("false", num, 5) != -1)
        {
          index += 5;
          return true;
        }
      }
      return Parser.ValidateNumber(code, ref index);
    }

    public static int SkipComment(string code, int index, bool skipSpaces)
    {
      while (code.Length > index)
      {
        bool flag = false;
        if (code[index] == '/' && index + 1 < code.Length)
        {
          switch (code[index + 1])
          {
            case '*':
              index += 2;
              while (index + 1 < code.Length && (code[index] != '*' || code[index + 1] != '/'))
                ++index;
              if (index + 1 >= code.Length)
                ExceptionHelper.ThrowSyntaxError(Strings.UnexpectedEndOfSource);
              index += 2;
              flag = true;
              break;
            case '/':
              index += 2;
              while (index < code.Length && !Tools.IsLineTerminator(code[index]))
                ++index;
              flag = true;
              break;
          }
        }
        if (Parser.Validate(code, "<!--", index))
        {
          while (index < code.Length && !Tools.IsLineTerminator(code[index]))
            ++index;
          flag = true;
        }
        if (!flag)
        {
          if (skipSpaces)
          {
            while (index < code.Length && Tools.IsWhiteSpace(code[index]))
              ++index;
          }
          return index;
        }
      }
      return index;
    }

    internal static string RemoveComments(string code, int startPosition)
    {
      StringBuilder stringBuilder = (StringBuilder) null;
      int index1 = startPosition;
      while (index1 < code.Length)
      {
        while (index1 < code.Length && Tools.IsWhiteSpace(code[index1]))
        {
          if (stringBuilder != null)
            stringBuilder.Append(code[index1++]);
          else
            ++index1;
        }
        int index2 = index1;
        index1 = Parser.SkipComment(code, index1, false);
        if (index2 != index1 && stringBuilder == null)
        {
          stringBuilder = new StringBuilder(code.Length);
          for (int index3 = 0; index3 < index2; ++index3)
            stringBuilder.Append(code[index3]);
        }
        for (; index2 < index1; ++index2)
        {
          if (Tools.IsWhiteSpace(code[index2]))
            stringBuilder.Append(code[index2]);
          else
            stringBuilder.Append(' ');
        }
        if (index1 < code.Length)
        {
          if (Parser.ValidateRegex(code, ref index1, false))
          {
            if (stringBuilder != null)
            {
              for (; index2 <= index1; ++index2)
                stringBuilder.Append(code[index2]);
              break;
            }
            break;
          }
          if (Parser.ValidateString(code, ref index1, false))
          {
            if (stringBuilder != null)
            {
              for (; index2 < index1; ++index2)
                stringBuilder.Append(code[index2]);
            }
          }
          else if (stringBuilder != null)
            stringBuilder.Append(code[index1++]);
          else
            ++index1;
        }
      }
      return ((object) stringBuilder ?? (object) code).ToString();
    }

    public static bool IsOperator(char c) => c == '+' || c == '-' || c == '*' || c == '/' || c == '%' || c == '^' || c == '&' || c == '!' || c == '<' || c == '>' || c == '=' || c == '?' || c == ':' || c == ',' || c == '.' || c == '|';

    public static bool IsIdentifierTerminator(char c) => c == ' ' || Tools.IsLineTerminator(c) || Parser.IsOperator(c) || Tools.IsWhiteSpace(c) || c == '{' || c == '\v' || c == '}' || c == '(' || c == ')' || c == ';' || c == '[' || c == ']' || c == '\'' || c == '"' || c == '~' || c == '`';

    internal static CodeNode Parse(ParseInfo state, ref int index, CodeFragmentType ruleSet) => Parser.Parse(state, ref index, ruleSet, true);

    internal static CodeNode Parse(
      ParseInfo state,
      ref int index,
      CodeFragmentType ruleSet,
      bool throwError)
    {
      while (index < state.Code.Length && Tools.IsWhiteSpace(state.Code[index]))
        ++index;
      if (index >= state.Code.Length || state.Code[index] == '}')
        return (CodeNode) null;
      int index1 = index;
      if (state.Code[index] == ',' || state.Code[index] == ';')
        return (CodeNode) null;
      if (index >= state.Code.Length)
        return (CodeNode) null;
      for (int index2 = 0; index2 < Parser.rules[(int) ruleSet].Count; ++index2)
      {
        if (Parser.rules[(int) ruleSet][index2].Validate(state.Code, index))
        {
          CodeNode codeNode = Parser.rules[(int) ruleSet][index2].Parse(state, ref index);
          if (codeNode != null)
            return codeNode;
        }
      }
      if (throwError)
        ExceptionHelper.ThrowUnknownToken(state.Code, index1);
      return (CodeNode) null;
    }

    internal static void Build<T>(
      ref T self,
      int expressionDepth,
      Dictionary<string, VariableDescriptor> variables,
      CodeContext codeContext,
      InternalCompilerMessageCallback message,
      FunctionInfo stats,
      Options opts)
      where T : CodeNode
    {
      CodeNode _this = (CodeNode) self;
      while (_this != null && _this.Build(ref _this, expressionDepth, variables, codeContext, message, stats, opts))
        self = (T) _this;
      self = (T) _this;
    }

    internal static void Build(
      ref Expression s,
      int expressionDepth,
      Dictionary<string, VariableDescriptor> variables,
      CodeContext codeContext,
      InternalCompilerMessageCallback message,
      FunctionInfo stats,
      Options opts)
    {
      CodeNode self = (CodeNode) s;
      Parser.Build<CodeNode>(ref self, expressionDepth, variables, codeContext, message, stats, opts);
      if (self == null)
      {
        s = (Expression) null;
      }
      else
      {
        ref Expression local = ref s;
        if (!(self is Expression expression))
          expression = (Expression) new ExpressionWrapper(self);
        local = expression;
      }
    }

    public static void DefineCustomCodeFragment(Type type)
    {
      if ((object) type == null)
        ExceptionHelper.ThrowArgumentNull(nameof (type));
      Attribute[] array = CustomAttributeExtensions.GetCustomAttributes(type.GetTypeInfo(), typeof (CustomCodeFragment), false).ToArray<Attribute>();
      CustomCodeFragment customCodeFragment = array.Length != 0 ? array[0] as CustomCodeFragment : throw new ArgumentException("type must be marked with attribute \"" + typeof (CustomCodeFragment).Name + "\"");
      if (customCodeFragment.Type == CodeFragmentType.Statement)
      {
        if (!TypeExtensions.IsAssignableFrom(typeof (CodeNode), type))
          throw new ArgumentException("type must be sub-class of " + typeof (CodeNode).Name);
      }
      else
      {
        if (customCodeFragment.Type != CodeFragmentType.Expression)
          throw new ArgumentException();
        if (!TypeExtensions.IsAssignableFrom(typeof (Expression), type))
          throw new ArgumentException("type must be sub-class of " + typeof (Expression).Name);
      }
      MethodInfo runtimeMethod1 = type.GetRuntimeMethod("Validate", new Type[2]
      {
        typeof (string),
        typeof (int)
      });
      if ((object) runtimeMethod1 == null || (object) runtimeMethod1.ReturnType != (object) typeof (bool))
        throw new ArgumentException("type must contain static method \"Validate\" which get String and Int32 and returns Boolean");
      MethodInfo runtimeMethod2 = type.GetRuntimeMethod("Parse", new Type[2]
      {
        typeof (ParseInfo),
        typeof (int).MakeByRefType()
      });
      if ((object) runtimeMethod2 == null || (object) runtimeMethod2.ReturnType != (object) typeof (CodeNode))
        throw new ArgumentException("type must contain static method \"Parse\" which get " + typeof (ParseInfo).Name + " and Int32 by reference and returns " + typeof (CodeNode).Name);
      ValidateDelegate valDel = runtimeMethod1.CreateDelegate(typeof (ValidateDelegate)) as ValidateDelegate;
      ParseDelegate parseDel = runtimeMethod2.CreateDelegate(typeof (ParseDelegate)) as ParseDelegate;
      Parser.rules[0].Insert(Parser.rules[0].Count - 4, new Rule(valDel, parseDel));
      if (customCodeFragment.Type == CodeFragmentType.Expression)
      {
        Parser.rules[1].Insert(Parser.rules[1].Count - 2, new Rule(valDel, parseDel));
        Parser.rules[2].Add(new Rule(valDel, parseDel));
      }
      if (customCodeFragment.ReservedWords.Length == 0)
        return;
      if (Parser.customReservedWords == null)
        Parser.customReservedWords = new HashSet<string>();
      for (int index = 0; index < customCodeFragment.ReservedWords.Length; ++index)
      {
        if (Parser.ValidateName(customCodeFragment.ReservedWords[0], 0))
          Parser.customReservedWords.Add(customCodeFragment.ReservedWords[0]);
      }
    }
  }
}
