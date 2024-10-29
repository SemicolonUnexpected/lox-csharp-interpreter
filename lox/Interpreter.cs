using System;
using System.Collections.Generic;
using static Lox.TokenType;
namespace Lox;

class Interpreter : Expr.IVisitor<object>, Stmt.IVisitor<object> {
    private Environment _env = new();

    public void Interpret(List<Stmt> statements) {
        try {
            foreach (Stmt statement in statements) {
                Execute(statement);
            }
        }
        catch (LoxRuntimeException exception) {
            Program.RuntimeError(exception);
        }
    }

    private void Execute(Stmt statement) {
        statement.Accept(this);
    }

    #region Expressions

    public object VisitBinaryExpr(Expr.Binary binary) {
        object left = Evaluate(binary.Left);
        object right = Evaluate(binary.Right);

        // Equality operators
        switch (binary.Token.Type) {
            case GREATER:
                CheckNumberOperands(left, right, binary.Token);
                return (double)left > (double)right;
            case LESS:
                CheckNumberOperands(left, right, binary.Token);
                return (double)left < (double)right;
            case GREATER_EQUAL:
                CheckNumberOperands(left, right, binary.Token);
                return (double)left >= (double)right;
            case LESS_EQUAL:
                CheckNumberOperands(left, right, binary.Token);
                return (double)left <= (double)right;

            case EQUAL_EQUAL:
                return IsEqual(left, right);
            case BANG_EQUAL:
                return !IsEqual(left, right);
        }

        // Arithemetic operators (plus string concatenation)
        switch (binary.Token.Type) {
            case MINUS:
                CheckNumberOperands(left, right, binary.Token);
                return (double)left - (double)right;
            case ASTERISK:
                CheckNumberOperands(left, right, binary.Token);
                return (double)left * (double)right;
            case SLASH:
                CheckNumberOperands(left, right, binary.Token);
                return (double)left / (double)right;
            case PLUS:
                if (left is string && right is string) return (string)left + (string)right;
                if (left is double && right is double) return (double)left + (double)right;

                throw new LoxRuntimeException("Expected two numbers or two strings", binary.Token);
        }

        return null;
    }

    public object VisitGroupingExpr(Expr.Grouping grouping) {
        return Evaluate(grouping.Expression);
    }

    public object VisitLiteralExpr(Expr.Literal literal) {
        return literal.Value!;
    }

    public object VisitUnaryExpr(Expr.Unary unary) {
        object right = Evaluate(unary.Expression);

        switch (unary.Token.Type) {
            case BANG:
                return !IsTruthy(right);
            case MINUS:
                CheckNumberOperands(right, unary.Token);
                return -(double)right;
        }

        return null;
    }

    public object VisitVariableExpr(Expr.Variable variable) {
        return _env.Get(variable.Name);
    }

    #endregion

    private object Evaluate(Expr expression) {
        return expression.Accept(this);
    }

    private bool IsTruthy(object? literal) {
        System.Console.WriteLine(literal is null);
        if (literal is null) return false;
        if (literal is bool) return (bool)literal;
        if (literal is double && (double)literal == 0) return false;
        return true;
    }

    private bool IsEqual(object? a, object? b) => object.Equals(a, b);

    private void CheckNumberOperands(object a, object b, Token token) {
        if (a is not double || b is not double) throw new LoxRuntimeException("Expected two numbers", token);
    }

    private void CheckNumberOperands(object a, Token token) {
        if (a is not double) throw new LoxRuntimeException("Expected number", token);
    }

    private string Stringify(object value) {
        if (value is null) return "nil";
        else return value.ToString()!;
    }

    #region Stmt

    public object VisitExpressionStmt(Stmt.Expression stmt) {
        Evaluate(stmt.LoxExpression);
        return null;
    }

    public object VisitPrintStmt(Stmt.Print stmt) {
        object value = Evaluate(stmt.LoxExpression);
        Console.WriteLine(Stringify(value));
        return null;
    }

    public object VisitVarStmt(Stmt.Var var) {
        object value = null;
        if (var.Initialiser is not null) value = Evaluate(var.Initialiser);

        _env.Define(var.Name.Lexeme, value);
        return null;
    }

    public object VisitAssignExpr(Expr.Assign assign) {
        object value = Evaluate(assign.Value);
        _env.Assign(assign.Name, value);
        return value;
    }

    public object VisitBlockStmt(Stmt.Block block) {
        ExecuteBlock(block.Statements, new Environment(_env));
        return null;
    }

    private void ExecuteBlock(List<Stmt> statements, Environment environment) {
        Environment previous = _env;
        try {
            _env = environment;
            foreach (Stmt statement in statements) Execute(statement);
        }
        finally {
            _env = previous;
        }
    }


    #endregion
}
