// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Visitor`1
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Expressions;
using NiL.JS.Statements;

namespace NiL.JS.Core
{
  public abstract class Visitor<T>
  {
    protected internal abstract T Visit(CodeNode node);

    protected internal virtual T Visit(Addition node) => this.Visit((Expression) node);

    protected internal virtual T Visit(BitwiseConjunction node) => this.Visit((Expression) node);

    protected internal virtual T Visit(ArrayDefinition node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Assignment node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Call node) => this.Visit((Expression) node);

    protected internal virtual T Visit(ClassDefinition node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Constant node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Decrement node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Delete node) => this.Visit((Expression) node);

    protected internal virtual T Visit(DeleteProperty node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Division node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Equal node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Expression node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(FunctionDefinition node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Property node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Variable node) => this.Visit((VariableReference) node);

    protected internal virtual T Visit(VariableReference node) => this.Visit((Expression) node);

    protected internal virtual T Visit(In node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Import node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Increment node) => this.Visit((Expression) node);

    protected internal virtual T Visit(InstanceOf node) => this.Visit((Expression) node);

    protected internal virtual T Visit(ObjectDefinition node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Less node) => this.Visit((Expression) node);

    protected internal virtual T Visit(LessOrEqual node) => this.Visit((Expression) node);

    protected internal virtual T Visit(LogicalConjunction node) => this.Visit((Expression) node);

    protected internal virtual T Visit(LogicalNegation node) => this.Visit((Expression) node);

    protected internal virtual T Visit(LogicalDisjunction node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Modulo node) => this.Visit((Expression) node);

    protected internal virtual T Visit(More node) => this.Visit((Expression) node);

    protected internal virtual T Visit(MoreOrEqual node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Multiplication node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Negation node) => this.Visit((Expression) node);

    protected internal virtual T Visit(New node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Comma node) => this.Visit((Expression) node);

    protected internal virtual T Visit(BitwiseNegation node) => this.Visit((Expression) node);

    protected internal virtual T Visit(NotEqual node) => this.Visit((Expression) node);

    protected internal virtual T Visit(NumberAddition node) => this.Visit((Expression) node);

    protected internal virtual T Visit(NumberLess node) => this.Visit((Expression) node);

    protected internal virtual T Visit(NumberLessOrEqual node) => this.Visit((Expression) node);

    protected internal virtual T Visit(NumberMore node) => this.Visit((Expression) node);

    protected internal virtual T Visit(NumberMoreOrEqual node) => this.Visit((Expression) node);

    protected internal virtual T Visit(BitwiseDisjunction node) => this.Visit((Expression) node);

    protected internal virtual T Visit(RegExpExpression node) => this.Visit((Expression) node);

    protected internal virtual T Visit(SetProperty node) => this.Visit((Expression) node);

    protected internal virtual T Visit(SignedShiftLeft node) => this.Visit((Expression) node);

    protected internal virtual T Visit(SignedShiftRight node) => this.Visit((Expression) node);

    protected internal virtual T Visit(StrictEqual node) => this.Visit((Expression) node);

    protected internal virtual T Visit(StrictNotEqual node) => this.Visit((Expression) node);

    protected internal virtual T Visit(StringConcatenation node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Substract node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Conditional node) => this.Visit((Expression) node);

    protected internal virtual T Visit(ConvertToBoolean node) => this.Visit((Expression) node);

    protected internal virtual T Visit(ConvertToInteger node) => this.Visit((Expression) node);

    protected internal virtual T Visit(ConvertToNumber node) => this.Visit((Expression) node);

    protected internal virtual T Visit(ConvertToString node) => this.Visit((Expression) node);

    protected internal virtual T Visit(ConvertToUnsignedInteger node) => this.Visit((Expression) node);

    protected internal virtual T Visit(TypeOf node) => this.Visit((Expression) node);

    protected internal virtual T Visit(UnsignedShiftRight node) => this.Visit((Expression) node);

    protected internal virtual T Visit(BitwiseExclusiveDisjunction node) => this.Visit((Expression) node);

    protected internal virtual T Visit(Break node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(CodeBlock node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(Continue node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(Debugger node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(DoWhile node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(Empty node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(ForIn node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(ForOf node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(For node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(IfElse node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(InfinityLoop node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(LabeledStatement node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(Return node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(Switch node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(Throw node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(TryCatch node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(VariableDefinition node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(While node) => this.Visit((CodeNode) node);

    protected internal virtual T Visit(With node) => this.Visit((CodeNode) node);
  }
}
