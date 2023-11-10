// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.ExpressionTree
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using System;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class ExpressionTree : Expression
  {
    private OperationType _operationKind;

    internal override bool ResultInTempContainer => false;

    internal OperationType Type => this._operationKind;

    private Expression getFastImpl()
    {
      switch (this._operationKind)
      {
        case OperationType.None:
          return this._right == null ? this._left : (Expression) new Comma(this._left, this._right);
        case OperationType.Assignment:
          return (Expression) new Assignment(this._left, this._right);
        case OperationType.Conditional:
          while (this._right is ExpressionTree && (this._right as ExpressionTree)._operationKind == OperationType.None && (this._right as ExpressionTree)._right == null)
            this._right = (this._right as ExpressionTree)._left;
          return (Expression) new Conditional(this._left, (Expression[]) this._right.Evaluate((Context) null)._oValue);
        case OperationType.LogicalOr:
          return (Expression) new LogicalDisjunction(this._left, this._right);
        case OperationType.LogicalAnd:
          return (Expression) new LogicalConjunction(this._left, this._right);
        case OperationType.NullishCoalescing:
          return (Expression) new NullishCoalescing(this._left, this._right);
        case OperationType.Or:
          return (Expression) new BitwiseDisjunction(this._left, this._right);
        case OperationType.Xor:
          return (Expression) new BitwiseExclusiveDisjunction(this._left, this._right);
        case OperationType.And:
          return (Expression) new BitwiseConjunction(this._left, this._right);
        case OperationType.Equal:
          return (Expression) new Equal(this._left, this._right);
        case OperationType.NotEqual:
          return (Expression) new NotEqual(this._left, this._right);
        case OperationType.StrictEqual:
          return (Expression) new StrictEqual(this._left, this._right);
        case OperationType.StrictNotEqual:
          return (Expression) new StrictNotEqual(this._left, this._right);
        case OperationType.InstanceOf:
          return (Expression) new InstanceOf(this._left, this._right);
        case OperationType.In:
          return (Expression) new In(this._left, this._right);
        case OperationType.More:
          return (Expression) new More(this._left, this._right);
        case OperationType.Less:
          return (Expression) new Less(this._left, this._right);
        case OperationType.MoreOrEqual:
          return (Expression) new MoreOrEqual(this._left, this._right);
        case OperationType.LessOrEqual:
          return (Expression) new LessOrEqual(this._left, this._right);
        case OperationType.SignedShiftLeft:
          return (Expression) new SignedShiftLeft(this._left, this._right);
        case OperationType.SignedShiftRight:
          return (Expression) new SignedShiftRight(this._left, this._right);
        case OperationType.UnsignedShiftRight:
          return (Expression) new UnsignedShiftRight(this._left, this._right);
        case OperationType.Addition:
          return (Expression) new Addition(this._left, this._right);
        case OperationType.Substract:
          return (Expression) new Substract(this._left, this._right);
        case OperationType.Multiply:
          return (Expression) new Multiplication(this._left, this._right);
        case OperationType.Modulo:
          return (Expression) new Modulo(this._left, this._right);
        case OperationType.Division:
          return (Expression) new Division(this._left, this._right);
        case OperationType.Power:
          return (Expression) new Power(this._left, this._right);
        case OperationType.LogicalNot:
          return (Expression) new LogicalNegation(this._left);
        case OperationType.Not:
          return (Expression) new BitwiseNegation(this._left);
        case OperationType.TypeOf:
          return (Expression) new TypeOf(this._left);
        case OperationType.Delete:
          return (Expression) new Delete(this._left);
        case OperationType.Incriment:
          return (Expression) new Increment(this._left ?? this._right, this._left == null ? IncrimentType.Postincriment : IncrimentType.Preincriment);
        case OperationType.Decriment:
          return (Expression) new Decrement(this._left ?? this._right, this._left == null ? DecrimentType.Postdecriment : DecrimentType.Postdecriment);
        case OperationType.Call:
          throw new InvalidOperationException("Call instance mast be created immediatly.");
        case OperationType.New:
          throw new InvalidOperationException("New instance mast be created immediatly.");
        default:
          throw new ArgumentException("invalid operation type");
      }
    }

    private static Expression deicstra(ExpressionTree statement)
    {
      if (statement == null)
        return (Expression) null;
      if (!(statement._right is ExpressionTree right))
        return (Expression) statement;
      Stack<Expression> expressionStack1 = new Stack<Expression>();
      Stack<Expression> expressionStack2 = new Stack<Expression>();
      expressionStack2.Push((Expression) statement);
      expressionStack1.Push(statement._left);
      for (; right != null; right = right._right as ExpressionTree)
      {
        expressionStack1.Push(right._left);
        while (expressionStack2.Count > 0)
        {
          int operationKind = (int) (expressionStack2.Peek() as ExpressionTree)._operationKind;
          if ((OperationType) (operationKind & 4080) > (right._operationKind & OperationType.Call) || (OperationType) (operationKind & 4080) == (right._operationKind & OperationType.Call) && (right._operationKind & OperationType.Call) > OperationType.Conditional)
          {
            ExpressionTree expressionTree = expressionStack2.Pop() as ExpressionTree;
            expressionTree._right = expressionStack1.Pop();
            expressionTree._left = expressionStack1.Pop();
            expressionTree.Position = (expressionTree._left ?? (Expression) expressionTree).Position;
            expressionTree.Length = (expressionTree._right ?? expressionTree._left ?? (Expression) expressionTree).Length + (expressionTree._right ?? expressionTree._left ?? (Expression) expressionTree).Position - expressionTree.Position;
            expressionStack1.Push((Expression) expressionTree);
          }
          else
            break;
        }
        expressionStack2.Push((Expression) right);
        if (!(right._right is ExpressionTree))
          expressionStack1.Push(right._right);
      }
      while (expressionStack1.Count > 1)
      {
        Expression expression = expressionStack2.Pop();
        expression._right = expressionStack1.Pop();
        expression._left = expressionStack1.Pop();
        expression.Position = (expression._left ?? expression).Position;
        expression.Length = (expression._right ?? expression._left ?? expression).Length + (expression._right ?? expression._left ?? expression).Position - expression.Position;
        expressionStack1.Push(expression);
      }
      return expressionStack1.Peek();
    }

    public static CodeNode Parse(ParseInfo state, ref int index) => (CodeNode) ExpressionTree.Parse(state, ref index, false, true, false, true, false);

    public static Expression Parse(ParseInfo state, ref int index, bool forUnaryOperator) => ExpressionTree.Parse(state, ref index, forUnaryOperator, true, false, true, false);

    internal static Expression Parse(
      ParseInfo state,
      ref int index,
      bool forUnary = false,
      bool processComma = true,
      bool forNew = false,
      bool root = true,
      bool forForLoop = false)
    {
      int i = index;
      Expression operand = ExpressionTree.parseOperand(state, ref i, forNew, forForLoop);
      if (operand == null)
        return (Expression) null;
      Expression statement = ExpressionTree.parseContinuation(state, operand, index, ref i, ref root, forUnary, processComma, forNew, forForLoop);
      if (root)
        statement = ExpressionTree.deicstra(statement as ExpressionTree) ?? statement;
      if (!forForLoop & processComma && !forUnary && i < state.Code.Length && state.Code[i] == ';')
        ++i;
      index = i;
      return statement;
    }

    private static Expression parseContinuation(
      ParseInfo state,
      Expression first,
      int startIndex,
      ref int i,
      ref bool root,
      bool forUnary,
      bool processComma,
      bool forNew,
      bool forForLoop)
    {
      Expression statement = (Expression) null;
      OperationType operationType = OperationType.None;
      bool flag1 = !forUnary;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4;
      do
      {
        flag4 = false;
        while (i < state.Code.Length && Tools.IsWhiteSpace(state.Code[i]) && !Tools.IsLineTerminator(state.Code[i]))
          ++i;
        if (state.Code.Length > i)
        {
          int num1 = i;
          while (i < state.Code.Length && Tools.IsWhiteSpace(state.Code[i]))
            ++i;
          if (state.Code.Length <= i)
          {
            i = num1;
            break;
          }
          switch (state.Code[i])
          {
            case '\n':
            case '\v':
            case '\r':
            case ')':
            case ':':
            case ';':
            case ']':
            case '}':
              flag3 = false;
              break;
            case '!':
              if (forUnary)
              {
                flag3 = false;
                flag4 = false;
                i = num1;
                break;
              }
              if (state.Code[i + 1] != '=')
                throw new ArgumentException("Invalid operator '!'");
              ++i;
              if (state.Code[i + 1] == '=')
              {
                ++i;
                flag3 = true;
                operationType = OperationType.StrictNotEqual;
                break;
              }
              flag3 = true;
              operationType = OperationType.NotEqual;
              break;
            case '%':
              if (forUnary)
              {
                flag3 = false;
                flag4 = false;
                break;
              }
              flag3 = true;
              operationType = OperationType.Modulo;
              if (state.Code[i + 1] == '=')
              {
                flag2 = true;
                ++i;
                break;
              }
              break;
            case '&':
              if (forUnary)
              {
                flag3 = false;
                flag4 = false;
                i = num1;
                break;
              }
              if (state.Code[i + 1] == '&')
              {
                ++i;
                flag3 = true;
                flag2 = false;
                operationType = OperationType.LogicalAnd;
                break;
              }
              flag3 = true;
              flag2 = false;
              operationType = OperationType.And;
              if (state.Code[i + 1] == '=')
              {
                flag2 = true;
                ++i;
                break;
              }
              break;
            case '(':
              List<Expression> expressionList = new List<Expression>();
              ++i;
              int position1 = i;
              bool flag5 = false;
              while (true)
              {
                do
                {
                  Tools.SkipSpaces(state.Code, ref i);
                  Tools.CheckEndOfInput(state.Code, ref i);
                  if (state.Code[i] != ')')
                  {
                    bool flag6 = expressionList.Count == 0;
                    while (state.Code[i] == ',')
                    {
                      if (flag6)
                        ExceptionHelper.ThrowSyntaxError("Missing argument of function call", state.Code, i);
                      do
                      {
                        ++i;
                      }
                      while (Tools.IsWhiteSpace(state.Code[i]));
                      flag6 = true;
                    }
                    if (!flag6)
                      ExceptionHelper.ThrowSyntaxError("Expected ','", state.Code, i);
                    if (i + 1 == state.Code.Length)
                      ExceptionHelper.ThrowSyntaxError(Strings.UnexpectedEndOfSource, state.Code, i);
                    int num2 = Parser.Validate(state.Code, "...", ref i) ? 1 : 0;
                    expressionList.Add(ExpressionTree.Parse(state, ref i, false, false, false, true, false));
                    if (num2 != 0)
                    {
                      expressionList[expressionList.Count - 1] = (Expression) new Spread(expressionList[expressionList.Count - 1]);
                      flag5 = true;
                    }
                  }
                  else
                    goto label_135;
                }
                while (expressionList[expressionList.Count - 1] != null);
                ExceptionHelper.ThrowSyntaxError("Expected \")\"", state.Code, position1);
              }
label_135:
              Call call1 = new Call(first, expressionList.ToArray());
              call1.Position = first.Position;
              call1.Length = i - first.Position + 1;
              call1.withSpread = flag5;
              first = (Expression) call1;
              ++i;
              flag4 = !forNew;
              flag1 = false;
              flag3 = false;
              break;
            case '*':
              if (forUnary)
              {
                flag3 = false;
                flag4 = false;
                i = num1;
                break;
              }
              flag3 = true;
              if (state.Code[i + 1] == '*')
              {
                operationType = OperationType.Power;
                ++i;
              }
              else
                operationType = OperationType.Multiply;
              if (state.Code[i + 1] == '=')
              {
                flag2 = true;
                ++i;
                break;
              }
              break;
            case '+':
              if (state.Code[i + 1] == '+')
              {
                if (num1 == i)
                {
                  if (state.Strict && first is Variable && ((first as Variable).Name == "arguments" || (first as Variable).Name == "eval"))
                    ExceptionHelper.ThrowSyntaxError("Cannot incriment \"" + (first as Variable).Name + "\" in strict mode.", state.Code, i);
                  Increment increment = new Increment(first, IncrimentType.Postincriment);
                  increment.Position = first.Position;
                  increment.Length = i + 2 - first.Position;
                  first = (Expression) increment;
                  flag4 = true;
                  i += 2;
                  break;
                }
                goto default;
              }
              else
              {
                if (forUnary)
                {
                  flag3 = false;
                  flag4 = false;
                  i = num1;
                  break;
                }
                flag3 = true;
                operationType = OperationType.Addition;
                if (state.Code[i + 1] == '=')
                {
                  flag2 = true;
                  ++i;
                  break;
                }
                break;
              }
            case ',':
              if (forUnary || !processComma)
              {
                flag3 = false;
                flag4 = false;
                i = num1;
                break;
              }
              operationType = OperationType.None;
              flag3 = true;
              flag4 = false;
              break;
            case '-':
              if (state.Code[i + 1] == '-')
              {
                if (num1 == i)
                {
                  if (state.Strict && first is Variable && ((first as Variable).Name == "arguments" || (first as Variable).Name == "eval"))
                    ExceptionHelper.Throw((Error) new SyntaxError("Can not decriment \"" + (first as Variable).Name + "\" in strict mode."));
                  Decrement decrement = new Decrement(first, DecrimentType.Postdecriment);
                  decrement.Position = first.Position;
                  decrement.Length = i + 2 - first.Position;
                  first = (Expression) decrement;
                  flag4 = true;
                  i += 2;
                  break;
                }
                goto default;
              }
              else
              {
                if (forUnary)
                {
                  flag3 = false;
                  flag4 = false;
                  i = num1;
                  break;
                }
                flag3 = true;
                operationType = OperationType.Substract;
                if (state.Code[i + 1] == '=')
                {
                  flag2 = true;
                  ++i;
                  break;
                }
                break;
              }
            case '.':
              ++i;
              Tools.SkipSpaces(state.Code, ref i);
              int startIndex1 = i;
              if (!Parser.ValidateName(state.Code, ref i, false, true, state.Strict))
                ExceptionHelper.Throw((Error) new SyntaxError(string.Format(Strings.InvalidPropertyName, (object) CodeCoordinates.FromTextPosition(state.Code, i, 0))));
              string key = state.Code.Substring(startIndex1, i - startIndex1);
              JSValue jsValue = (JSValue) null;
              if (!state.StringConstants.TryGetValue(key, out jsValue))
                state.StringConstants[key] = jsValue = (JSValue) key;
              else
                key = jsValue._oValue.ToString();
              Expression source = first;
              Constant fieldName1 = new Constant((JSValue) key);
              fieldName1.Position = startIndex1;
              fieldName1.Length = i - startIndex1;
              Property property1 = new Property(source, (Expression) fieldName1);
              property1.Position = first.Position;
              property1.Length = i - first.Position;
              first = (Expression) property1;
              flag4 = true;
              flag1 = true;
              break;
            case '/':
              if (forUnary)
              {
                flag3 = false;
                flag4 = false;
                i = num1;
                break;
              }
              state.Code = Parser.RemoveComments(state.SourceCode, i + 1);
              flag3 = true;
              operationType = OperationType.Division;
              if (state.Code[i + 1] == '=')
              {
                flag2 = true;
                ++i;
                break;
              }
              break;
            case '<':
              if (forUnary)
              {
                flag3 = false;
                flag4 = false;
                i = num1;
                break;
              }
              flag3 = true;
              if (state.Code[i + 1] == '<')
              {
                ++i;
                operationType = OperationType.SignedShiftLeft;
              }
              else
              {
                operationType = OperationType.Less;
                if (state.Code[i + 1] == '=')
                {
                  operationType = OperationType.LessOrEqual;
                  ++i;
                }
              }
              if (state.Code[i + 1] == '=')
              {
                flag2 = true;
                ++i;
                break;
              }
              break;
            case '=':
              if (forUnary)
              {
                flag3 = false;
                flag4 = false;
                i = num1;
                break;
              }
              if (state.Code[i + 1] == '=')
              {
                ++i;
                if (state.Code[i + 1] == '=')
                {
                  ++i;
                  operationType = OperationType.StrictEqual;
                }
                else
                  operationType = OperationType.Equal;
              }
              else
                operationType = OperationType.Assignment;
              flag3 = true;
              break;
            case '>':
              if (forUnary)
              {
                flag3 = false;
                flag4 = false;
                i = num1;
                break;
              }
              flag3 = true;
              if (state.Code[i + 1] == '>')
              {
                ++i;
                if (state.Code[i + 1] == '>')
                {
                  operationType = OperationType.UnsignedShiftRight;
                  ++i;
                }
                else
                  operationType = OperationType.SignedShiftRight;
              }
              else
              {
                operationType = OperationType.More;
                if (state.Code[i + 1] == '=')
                {
                  operationType = OperationType.MoreOrEqual;
                  ++i;
                }
              }
              if (state.Code[i + 1] == '=')
              {
                flag2 = true;
                ++i;
                break;
              }
              break;
            case '?':
              if (forUnary)
              {
                flag3 = false;
                flag4 = false;
                i = num1;
                break;
              }
              if (state.Code[i + 1] == '?')
              {
                ++i;
                flag3 = true;
                operationType = OperationType.NullishCoalescing;
                break;
              }
              flag3 = true;
              flag4 = false;
              operationType = OperationType.Conditional;
              break;
            case '[':
              do
              {
                ++i;
              }
              while (Tools.IsWhiteSpace(state.Code[i]));
              int position2 = i;
              Expression fieldName2 = ExpressionTree.Parse(state, ref i, false, true, false, true, false);
              if (fieldName2 == null)
                ExceptionHelper.Throw((Error) new SyntaxError("Unexpected token at " + CodeCoordinates.FromTextPosition(state.Code, position2, 0)?.ToString()));
              while (Tools.IsWhiteSpace(state.Code[i]))
                ++i;
              if (state.Code[i] != ']')
                ExceptionHelper.Throw((Error) new SyntaxError("Expected \"]\" at " + CodeCoordinates.FromTextPosition(state.Code, position2, 0)?.ToString()));
              Property property2 = new Property(first, fieldName2);
              property2.Position = first.Position;
              property2.Length = i + 1 - first.Position;
              first = (Expression) property2;
              ++i;
              flag4 = true;
              flag1 = true;
              if (state.Message != null)
              {
                int index = 0;
                if (fieldName2 is Constant constant && constant.value._valueType == JSValueType.String && Parser.ValidateName(constant.value._oValue.ToString(), ref index, false) && index == constant.value._oValue.ToString().Length)
                {
                  state.Message(MessageLevel.Recomendation, fieldName2.Position, fieldName2.Length, "[\"" + constant.value._oValue?.ToString() + "\"] is better written in dot notation.");
                  break;
                }
                break;
              }
              break;
            case '^':
              if (forUnary)
              {
                flag3 = false;
                flag4 = false;
                i = num1;
                break;
              }
              flag3 = true;
              operationType = OperationType.Xor;
              if (state.Code[i + 1] == '=')
              {
                flag2 = true;
                ++i;
                break;
              }
              break;
            case '`':
              Expression expression = TemplateString.Parse(state, ref i, TemplateStringMode.Tag);
              Call call2 = new Call(first, new Expression[1]
              {
                expression
              });
              call2.Position = first.Position;
              call2.Length = i - first.Position + 1;
              call2.withSpread = true;
              first = (Expression) call2;
              flag4 = !forNew;
              flag1 = false;
              flag3 = false;
              break;
            case 'i':
              if (forUnary)
              {
                flag3 = false;
                flag4 = false;
                i = num1;
                break;
              }
              if (Parser.Validate(state.Code, "instanceof", ref i))
              {
                operationType = OperationType.InstanceOf;
                flag3 = true;
                --i;
                break;
              }
              if (Parser.Validate(state.Code, "in", ref i))
              {
                if (forForLoop)
                {
                  i = num1;
                  goto case '\n';
                }
                else
                {
                  operationType = OperationType.In;
                  flag3 = true;
                  --i;
                  break;
                }
              }
              else
                goto default;
            case '|':
              if (forUnary)
              {
                flag3 = false;
                flag4 = false;
                i = num1;
                break;
              }
              if (state.Code[i + 1] == '|')
              {
                ++i;
                flag3 = true;
                flag2 = false;
                operationType = OperationType.LogicalOr;
                break;
              }
              flag3 = true;
              flag2 = false;
              operationType = OperationType.Or;
              if (state.Code[i + 1] == '=')
              {
                flag2 = true;
                ++i;
                break;
              }
              break;
            default:
              if (!Tools.IsLineTerminator(state.Code[i]))
              {
                if (i != num1)
                {
                  i = num1;
                  goto case '\n';
                }
                else if (state.Code[i] == 'o' && state.Code[i + 1] == 'f')
                {
                  i = num1;
                  goto case '\n';
                }
                else
                {
                  ExceptionHelper.ThrowSyntaxError("Unexpected token '" + state.Code[i].ToString() + "'", state.Code, i);
                  break;
                }
              }
              else
                goto case '\n';
          }
        }
        else
          break;
      }
      while (flag4);
      if (state.Strict && first is Variable && ((first as Variable).Name == "arguments" || (first as Variable).Name == "eval") && (flag2 || operationType == OperationType.Assignment))
        ExceptionHelper.ThrowSyntaxError("Assignment to eval or arguments is not allowed in strict mode", state.Code, i);
      if (operationType == OperationType.Assignment | flag2)
      {
        bool flag7 = !flag1 || !ExpressionTree.canBeAssignee(first);
        if (operationType == OperationType.Assignment && (first is ObjectDefinition || first is ArrayDefinition))
        {
          try
          {
            first = (Expression) new ObjectDesctructor(first);
            flag7 = false;
          }
          catch
          {
          }
        }
        if (flag7)
          ExceptionHelper.ThrowReferenceError(Strings.InvalidLefthandSideInAssignment, state.Code, first.Position, first.Length);
      }
      if (flag3 && !forUnary)
      {
        do
        {
          ++i;
        }
        while (state.Code.Length > i && Tools.IsWhiteSpace(state.Code[i]));
        if (state.Code.Length > i)
        {
          if (operationType == OperationType.Conditional)
          {
            bool root1 = false;
            statement = ExpressionTree.parseContinuation(state, ExpressionTree.parseTernaryBranches(state, forForLoop, ref i), startIndex, ref i, ref root1, forUnary, processComma, false, forForLoop);
          }
          else
            statement = ExpressionTree.Parse(state, ref i, processComma: processComma, root: false, forForLoop: forForLoop);
        }
        else
          ExceptionHelper.ThrowSyntaxError("Expected second operand", state.Code, i);
      }
      Expression continuation;
      if (flag2)
      {
        root = false;
        Expression expression1 = ExpressionTree.deicstra(statement as ExpressionTree);
        AssignmentOperatorCache assignmentOperatorCache = new AssignmentOperatorCache(first);
        if (expression1 is ExpressionTree && (expression1 as ExpressionTree)._operationKind == OperationType.None)
        {
          Expression expression2 = expression1;
          AssignmentOperatorCache left = assignmentOperatorCache;
          ExpressionTree right = new ExpressionTree();
          right._left = (Expression) assignmentOperatorCache;
          right._right = expression1._left;
          right._operationKind = operationType;
          right.Position = startIndex;
          right.Length = i - startIndex;
          Assignment assignment = new Assignment((Expression) left, (Expression) right);
          assignment.Position = startIndex;
          assignment.Length = i - startIndex;
          expression2._left = (Expression) assignment;
          continuation = expression1;
        }
        else
        {
          AssignmentOperatorCache left = assignmentOperatorCache;
          ExpressionTree right = new ExpressionTree();
          right._left = (Expression) assignmentOperatorCache;
          right._right = expression1;
          right._operationKind = operationType;
          right.Position = startIndex;
          right.Length = i - startIndex;
          Assignment assignment = new Assignment((Expression) left, (Expression) right);
          assignment.Position = startIndex;
          assignment.Length = i - startIndex;
          continuation = (Expression) assignment;
        }
      }
      else if (!root || operationType != OperationType.None || statement != null)
      {
        if (forUnary && operationType == OperationType.None && first is ExpressionTree)
        {
          continuation = first;
        }
        else
        {
          if (operationType == OperationType.NullishCoalescing && statement is ExpressionTree expressionTree1 && (expressionTree1._operationKind == OperationType.LogicalAnd || expressionTree1._operationKind == OperationType.LogicalOr))
            ExceptionHelper.ThrowSyntaxError(Strings.LogicalNullishCoalescing, state.Code, startIndex, i - startIndex);
          if ((operationType == OperationType.LogicalAnd || operationType == OperationType.LogicalOr) && statement is ExpressionTree expressionTree2 && expressionTree2._operationKind == OperationType.NullishCoalescing)
            ExceptionHelper.ThrowSyntaxError(Strings.LogicalNullishCoalescing, state.Code, startIndex, i - startIndex);
          ExpressionTree expressionTree3 = new ExpressionTree();
          expressionTree3._left = first;
          expressionTree3._right = statement;
          expressionTree3._operationKind = operationType;
          expressionTree3.Position = startIndex;
          expressionTree3.Length = i - startIndex;
          continuation = (Expression) expressionTree3;
        }
      }
      else
        continuation = first;
      return continuation;
    }

    internal static bool canBeAssignee(Expression first)
    {
      switch (first)
      {
        case Variable _:
        case Property _:
          return true;
        case Constant _:
          return first.Evaluate((Context) null).ValueType <= JSValueType.Undefined;
        default:
          return false;
      }
    }

    private static Expression parseOperand(
      ParseInfo state,
      ref int i,
      bool forNew,
      bool forForLoop)
    {
      int index = i;
      Expression first1;
      if (Parser.Validate(state.Code, "this", ref i) || Parser.Validate(state.Code, "super", ref i) || Parser.Validate(state.Code, "new.target", ref i))
      {
        string str = Tools.Unescape(state.Code.Substring(index, i - index), state.Strict);
        switch (str)
        {
          case "super":
            while (Tools.IsWhiteSpace(state.Code[i]))
              ++i;
            if ((state.CodeContext & CodeContext.InClassDefinition) == CodeContext.None || state.Code[i] != '.' && state.Code[i] != '[' && (state.Code[i] != '(' || (state.CodeContext & CodeContext.InClassConstructor) == CodeContext.None))
              ExceptionHelper.ThrowSyntaxError("super keyword unexpected in this coontext", state.Code, i);
            first1 = (Expression) new Super();
            break;
          case "new.target":
            first1 = (Expression) new NewTarget();
            break;
          case "this":
            first1 = (Expression) new This();
            break;
          default:
            JSValue jsValue;
            if (state.StringConstants.TryGetValue(str, out jsValue))
              str = jsValue._oValue.ToString();
            else
              state.StringConstants[str] = (JSValue) str;
            first1 = (Expression) new Variable(str, state.LexicalScopeLevel);
            break;
        }
      }
      else
      {
        CodeContext codeContext = state.CodeContext;
        state.CodeContext |= CodeContext.InExpression;
        try
        {
          first1 = (Expression) Parser.Parse(state, ref i, CodeFragmentType.ExpressionContinuation, false);
        }
        finally
        {
          state.CodeContext = codeContext;
        }
      }
      if (first1 == null)
      {
        if (Parser.ValidateName(state.Code, ref i, state.Strict))
        {
          string str = Tools.Unescape(state.Code.Substring(index, i - index), state.Strict);
          if (str == "undefined")
          {
            first1 = (Expression) new Constant(JSValue.undefined);
          }
          else
          {
            JSValue jsValue1 = (JSValue) null;
            if (!state.StringConstants.TryGetValue(str, out jsValue1))
            {
              JSValue jsValue2;
              state.StringConstants[str] = jsValue2 = (JSValue) str;
            }
            else
              str = jsValue1._oValue.ToString();
            first1 = (Expression) new Variable(str, state.LexicalScopeLevel);
          }
        }
        else if (Parser.ValidateValue(state.Code, ref i))
        {
          string str = state.Code.Substring(index, i - index);
          if (str[0] == '\'' || str[0] == '"')
          {
            string key = Tools.Unescape(str.Substring(1, str.Length - 2), state.Strict);
            JSValue jsValue = (JSValue) null;
            if (!state.StringConstants.TryGetValue(key, out jsValue))
              state.StringConstants[key] = jsValue = (JSValue) key;
            Constant constant = new Constant(jsValue);
            constant.Position = index;
            constant.Length = i - index;
            first1 = (Expression) constant;
          }
          else
          {
            bool result = false;
            if (str == "null")
            {
              Constant constant = new Constant((JSValue) JSValue.@null);
              constant.Position = index;
              constant.Length = i - index;
              first1 = (Expression) constant;
            }
            else if (bool.TryParse(str, out result))
            {
              Constant constant = new Constant(result ? (JSValue) NiL.JS.BaseLibrary.Boolean.True : (JSValue) NiL.JS.BaseLibrary.Boolean.False);
              constant.Position = index;
              constant.Length = i - index;
              first1 = (Expression) constant;
            }
            else
            {
              double num = 0.0;
              if (Tools.ParseNumber(state.Code, ref index, out num, 0, (ParseNumberOptions) (14 | (state.Strict ? 1 : 0))))
              {
                int key;
                if ((double) (key = (int) num) == num && !Tools.IsNegativeZero(num))
                {
                  if (state.IntConstants.ContainsKey(key))
                  {
                    Constant constant = new Constant(state.IntConstants[key]);
                    constant.Position = index;
                    constant.Length = i - index;
                    first1 = (Expression) constant;
                  }
                  else
                  {
                    Constant constant = new Constant(state.IntConstants[key] = (JSValue) key);
                    constant.Position = index;
                    constant.Length = i - index;
                    first1 = (Expression) constant;
                  }
                }
                else if (state.DoubleConstants.ContainsKey(num))
                {
                  Constant constant = new Constant(state.DoubleConstants[num]);
                  constant.Position = index;
                  constant.Length = i - index;
                  first1 = (Expression) constant;
                }
                else
                {
                  Constant constant = new Constant(state.DoubleConstants[num] = (JSValue) num);
                  constant.Position = index;
                  constant.Length = i - index;
                  first1 = (Expression) constant;
                }
              }
              else
              {
                if (Parser.ValidateRegex(state.Code, ref index, true))
                  throw new InvalidOperationException("This case was moved");
                throw new ArgumentException("Unable to process value (" + str + ")");
              }
            }
          }
        }
        else if (state.Code[i] == '!' || state.Code[i] == '~' || state.Code[i] == '+' || state.Code[i] == '-' || Parser.Validate(state.Code, "delete", i) || Parser.Validate(state.Code, "typeof", i) || Parser.Validate(state.Code, "void", i))
        {
          switch (state.Code[i])
          {
            case '!':
              do
              {
                ++i;
              }
              while (Tools.IsWhiteSpace(state.Code[i]));
              first1 = (Expression) new LogicalNegation(ExpressionTree.Parse(state, ref i, true, forForLoop: forForLoop));
              if (first1 == null)
              {
                ExceptionHelper.Throw((Error) new SyntaxError("Invalid prefix operation. " + CodeCoordinates.FromTextPosition(state.Code, i, 0)?.ToString()));
                break;
              }
              break;
            case '+':
              ++i;
              if (state.Code[i] == '+')
              {
                do
                {
                  ++i;
                }
                while (i < state.Code.Length && Tools.IsWhiteSpace(state.Code[i]));
                if (i >= state.Code.Length)
                  ExceptionHelper.ThrowSyntaxError(Strings.UnexpectedEndOfSource);
                Expression op = ExpressionTree.Parse(state, ref i, true, forForLoop: forForLoop);
                if (((Expression) (op as Property) ?? (Expression) (op as Variable)) == null)
                  ExceptionHelper.ThrowSyntaxError("Invalid prefix operation. ", state.Code, i);
                if (state.Strict && op is Variable && ((op as Variable).Name == "arguments" || (op as Variable).Name == "eval"))
                  ExceptionHelper.ThrowSyntaxError("Can not incriment \"" + (op as Variable).Name + "\" in strict mode.", state.Code, i);
                first1 = (Expression) new Increment(op, IncrimentType.Preincriment);
                break;
              }
              while (Tools.IsWhiteSpace(state.Code[i]))
                ++i;
              first1 = (Expression) new ConvertToNumber(ExpressionTree.Parse(state, ref i, true, forForLoop: forForLoop));
              break;
            case '-':
              ++i;
              if (state.Code[i] == '-')
              {
                do
                {
                  ++i;
                }
                while (i < state.Code.Length && Tools.IsWhiteSpace(state.Code[i]));
                if (i >= state.Code.Length)
                  ExceptionHelper.ThrowSyntaxError(Strings.UnexpectedEndOfSource);
                Expression op = ExpressionTree.Parse(state, ref i, true, forForLoop: forForLoop);
                if (((Expression) (op as Property) ?? (Expression) (op as Variable)) == null)
                  ExceptionHelper.ThrowSyntaxError("Invalid prefix operation.", state.Code, i);
                if (state.Strict && op is Variable && ((op as Variable).Name == "arguments" || (op as Variable).Name == "eval"))
                  ExceptionHelper.Throw((Error) new SyntaxError("Can not decriment \"" + (op as Variable).Name + "\" in strict mode."));
                first1 = (Expression) new Decrement(op, DecrimentType.Predecriment);
                break;
              }
              while (Tools.IsWhiteSpace(state.Code[i]))
                ++i;
              first1 = (Expression) new Negation(ExpressionTree.Parse(state, ref i, true, forForLoop: forForLoop));
              break;
            case 'd':
              i += 5;
              do
              {
                ++i;
              }
              while (Tools.IsWhiteSpace(state.Code[i]));
              Expression first2 = ExpressionTree.Parse(state, ref i, true, false, forForLoop: forForLoop);
              if (first2 == null)
                ExceptionHelper.Throw((Error) new SyntaxError("Invalid prefix operation. " + CodeCoordinates.FromTextPosition(state.Code, i, 0)?.ToString()));
              first1 = (Expression) new Delete(first2);
              break;
            case 't':
              i += 5;
              do
              {
                ++i;
              }
              while (Tools.IsWhiteSpace(state.Code[i]));
              Expression first3 = ExpressionTree.Parse(state, ref i, true, false, forForLoop: forForLoop);
              if (first3 == null)
                ExceptionHelper.Throw((Error) new SyntaxError("Invalid prefix operation. " + CodeCoordinates.FromTextPosition(state.Code, i, 0)?.ToString()));
              first1 = (Expression) new TypeOf(first3);
              break;
            case 'v':
              i += 3;
              do
              {
                ++i;
              }
              while (Tools.IsWhiteSpace(state.Code[i]));
              first1 = (Expression) new Comma(ExpressionTree.Parse(state, ref i, true, false, forForLoop: forForLoop), (Expression) new Constant(JSValue.undefined));
              if (first1 == null)
              {
                ExceptionHelper.Throw((Error) new SyntaxError("Invalid prefix operation. " + CodeCoordinates.FromTextPosition(state.Code, i, 0)?.ToString()));
                break;
              }
              break;
            case '~':
              do
              {
                ++i;
              }
              while (Tools.IsWhiteSpace(state.Code[i]));
              Expression first4 = ExpressionTree.Parse(state, ref i, true, forForLoop: forForLoop);
              if (first4 == null)
                ExceptionHelper.Throw((Error) new SyntaxError("Invalid prefix operation. " + CodeCoordinates.FromTextPosition(state.Code, i, 0)?.ToString()));
              first1 = (Expression) new BitwiseNegation(first4);
              break;
          }
        }
        else if (state.Code[i] == '(')
        {
          while (state.Code[i] != ')')
          {
            do
            {
              ++i;
            }
            while (Tools.IsWhiteSpace(state.Code[i]));
            Expression second = ExpressionTree.Parse(state, ref i, false, false, false, true, false);
            first1 = first1 != null ? (Expression) new Comma(first1, second) : second;
            while (Tools.IsWhiteSpace(state.Code[i]))
              ++i;
            if (state.Code[i] != ')' && state.Code[i] != ',')
              ExceptionHelper.ThrowSyntaxError("Expected \")\"");
          }
          ++i;
          if ((state.CodeContext & (CodeContext.InEval | CodeContext.InExpression)) != CodeContext.None && first1 is FunctionDefinition || forNew && first1 is Call)
            first1 = (Expression) new Comma(first1, (Expression) null);
        }
        else if (forForLoop)
          return (Expression) null;
      }
      if (first1 == null || first1 is Empty)
        ExceptionHelper.ThrowSyntaxError(Strings.UnexpectedToken, state.Code, i);
      if (first1.Position == 0)
      {
        first1.Position = index;
        first1.Length = i - index;
      }
      return first1;
    }

    private static Expression parseTernaryBranches(ParseInfo state, bool forEnumeration, ref int i)
    {
      Constant ternaryBranches = (Constant) null;
      CodeContext codeContext = state.CodeContext;
      state.CodeContext |= CodeContext.InExpression;
      try
      {
        int num = i;
        Expression[] expressionArray = new Expression[2]
        {
          ExpressionTree.Parse(state, ref i, false, true, false, true, false),
          null
        };
        if (state.Code[i] != ':')
          ExceptionHelper.ThrowSyntaxError(Strings.UnexpectedToken, state.Code, i);
        do
        {
          ++i;
        }
        while (Tools.IsWhiteSpace(state.Code[i]));
        Constant constant = new Constant(new JSValue()
        {
          _valueType = JSValueType.Object,
          _oValue = (object) expressionArray
        });
        constant.Position = num;
        ternaryBranches = constant;
        expressionArray[1] = ExpressionTree.Parse(state, ref i, processComma: false, forForLoop: forEnumeration);
        ternaryBranches.Length = i - num;
      }
      finally
      {
        state.CodeContext = codeContext;
      }
      return (Expression) ternaryBranches;
    }

    public override JSValue Evaluate(Context context) => throw new InvalidOperationException();

    protected internal override CodeNode[] GetChildrenImpl() => throw new InvalidOperationException();

    public override T Visit<T>(Visitor<T> visitor) => visitor.Visit((Expression) this);

    public override bool Build(
      ref CodeNode _this,
      int expressionDepth,
      Dictionary<string, VariableDescriptor> variables,
      CodeContext codeContext,
      InternalCompilerMessageCallback message,
      FunctionInfo stats,
      Options opts)
    {
      _this = (CodeNode) this.getFastImpl();
      _this.Position = this.Position;
      _this.Length = this.Length;
      return true;
    }

    public override string ToString() => this._operationKind.ToString() + "(" + this._left?.ToString() + (this._right != null ? ", " + this._right?.ToString() : "") + ")";
  }
}
