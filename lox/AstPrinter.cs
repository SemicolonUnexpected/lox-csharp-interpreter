using System;
using System.Text;
using static Lox.Expr;

namespace Lox;

internal class AstPrinter : Expr.IVisitor<string> {
    public string GetAst(Expr expression) {
        return expression.Accept(this);
    }

    public void PrintAst(Expr expression) => Console.WriteLine(GetAst(expression));

    private string Parenthesise(string name, params Expr[] expressions) {
        StringBuilder builder = new();

        builder.Append("(").Append(name);
        foreach (Expr expression in expressions) {
            builder.Append(" ");
            builder.Append(expression.Accept(this));
        }
        builder.Append(")");

        return builder.ToString();
    }

    public string VisitBinaryExpr(Binary binary) {
        return Parenthesise(binary.Token.Lexeme, binary.Left, binary.Right);
    }

    public string VisitGroupingExpr(Grouping grouping) {
        return Parenthesise("group", grouping.Expression);
    }

    public string VisitLiteralExpr(Literal literal) {
        return literal.Value is null ? "nil" : literal.Value.ToString()!;
    }

    public string VisitUnaryExpr(Unary unary) {
        return Parenthesise(unary.Token.Lexeme, unary.Expression);
    }

    public string VisitVariableExpr(Variable variable) {
        return Parenthesise(variable.Name.Lexeme);
    }
}
