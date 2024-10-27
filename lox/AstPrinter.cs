using System;
using System.Text;
using Lox;
using static Lox.Expression;

internal class AstPrinter : Expression.IVisitor<string> {
    public string GetAst(Expression expression) {
        return expression.Accept(this);
    }

    public void PrintAst(Expression expression) => Console.WriteLine(GetAst(expression));

    private string Parenthesise(string name, params Expression[] expressions) {
        StringBuilder builder = new();

        builder.Append("(").Append(name);
        foreach (Expression expression in expressions) {
            builder.Append(" ");
            builder.Append(expression.Accept(this));
        }
        builder.Append(")");

        return builder.ToString();
    }

    public string VisitBinaryExpression(Binary binary) {
        return Parenthesise(binary.Token.Lexeme, binary.Left, binary.Right);
    }

    public string VisitGroupingExpression(Grouping grouping) {
        return Parenthesise("group", grouping.Expression);
    }

    public string VisitLiteralExpression(Literal literal) {
        return literal.Value is null ? "nil" : literal.Value.ToString()!;
    }

    public string VisitUnaryExpression(Unary unary) {
        return Parenthesise(unary.Token.ToString(), unary.Expression);
    }
}
