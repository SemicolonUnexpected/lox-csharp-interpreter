using System;
using System.Collections.Generic;
using static Lox.TokenType;

namespace Lox;

internal class Parser {
    private class ParseError : Exception { };

    private readonly List<Token> _tokens;
    private int _current = 0;

    public Parser(List<Token> tokens) {
        _tokens = tokens;
    }

    public List<Stmt> Parse() {
        List<Stmt> statements = new();
        while (!IsAtEnd()) {
            statements.Add(Statement());
        }
        return statements;
    }

    private Expr Expression() => Equality();
    private Expr Equality() {
        Expr expression = Comparison();

        while (Match(BANG_EQUAL, EQUAL_EQUAL)) {
            Token op = Previous();
            Expr right = Comparison();
            expression = new Expr.Binary(expression, op, right);
        }

        return expression;
    }
    private Expr Comparison() {
        Expr expression = Term();

        while (Match(GREATER, GREATER_EQUAL, LESS, LESS_EQUAL)) {
            Token op = Previous();
            Expr right = Term();
            expression = new Expr.Binary(expression, op, right);
        }

        return expression;
    }
    private Expr Term() {
        Expr expression = Factor();

        while (Match(PLUS, MINUS)) {
            Token op = Previous();
            Expr right = Factor();
            expression = new Expr.Binary(expression, op, right);
        }

        return expression;
    }
    private Expr Factor() {
        Expr expression = Unary();

        while (Match(SLASH, ASTERISK)) {
            Token op = Previous();
            Expr right = Unary();
            expression = new Expr.Binary(expression, op, right);
        }

        return expression;
    }
    private Expr Unary() {
        if (Match(BANG, MINUS)) {
            Token op = Previous();
            Expr right = Unary();

            return new Expr.Unary(right, op);
        }

        return Primary();
    }
    private Expr Primary() {
        if (Match(FALSE)) return new Expr.Literal(false);
        if (Match(TRUE)) return new Expr.Literal(true);
        if (Match(NIL)) return new Expr.Literal(null);

        if (Match(NUMBER, STRING)) return new Expr.Literal(Previous().Literal!);

        if (Match(LEFT_PARENTHESIS)) {
            Expr expression = Expression();
            Consume(RIGHT_PARENTHESIS, "Expected ')' after expression.");
            return new Expr.Grouping(expression);
        }

        throw Error(Peek(), "Expected expression");
    }

    private void Sychronise() {
        Advance();

        while (!IsAtEnd()) {
            if (Previous().Type == SEMICOLON) return;

            switch (Peek().Type) {
                case CLASS:
                case FUNCTION:
                case VAR:
                case FOR:
                case IF:
                case WHILE:
                case PRINT:
                case RETURN:
                    return;
            }

            Advance();
        }
    }

    #region Parser Motions

    private bool Match(params TokenType[] types) {
        foreach (TokenType type in types) {
            if (Check(type)) {
                Advance();
                return true;
            }
        }

        return false;
    }
    private bool Check(TokenType type) => IsAtEnd() ? false : Peek().Type == type;
    private Token Advance() => IsAtEnd() ? Previous() : _tokens[++_current];
    private Token Previous() => _tokens[_current - 1];
    private Token Peek() => _tokens[_current];
    private bool IsAtEnd() => Peek().Type == EOF;
    private Token Consume(TokenType type, string message) {
        if (Check(type)) return Advance();

        throw Error(Peek(), message);
    }

    #endregion

    #region Statemnts

    private Stmt Statement() {
        if (Match(PRINT)) return PrintStatement();

        return ExpressionStatement();
    }

    private Stmt PrintStatement() {
        Expr value = Expression();

        Consume(SEMICOLON, "Expected ';' after value.");
        return new Stmt.Print(value);
    }

    private Stmt ExpressionStatement() {
        Expr expression = Expression();

        Consume(SEMICOLON, "Expected ';' after expression.");
        return new Stmt.Expression(expression);
    }

    #endregion

    private ParseError Error(Token token, string message) {
        Program.Error(token.Line, message);
        return new ParseError();
    }
}
