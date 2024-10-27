using System;
using System.Collections.Generic;

using static Lox.TokenType;

namespace Lox;

internal class Lexer {
    private readonly List<Token> _tokens = new();
    private readonly string _source;

    private int _start = 0;
    private int _current = 0;
    private int _line = 1;

    public Lexer(string source) {
        _source = source;
    }

    public List<Token> Scan() {
        while (!AtEnd()) {
            // At the beginning of the next lexeme
            _start = _current;
            ScanToken();
        }

        _tokens.Add(new Token(EOF, "", null, _line));
        return _tokens;
    }

    private void ScanToken() {
        char c = Advance();
        switch (c) {
            case '(': AddToken(LEFT_PARENTHESIS); break;
            case ')': AddToken(RIGHT_PARENTHESIS); break;
            case '{': AddToken(LEFT_BRACE); break;
            case '}': AddToken(RIGHT_PARENTHESIS); break;
            case ',': AddToken(COMMA); break;
            case '.': AddToken(DOT); break;
            case '-': AddToken(MINUS); break;
            case '+': AddToken(PLUS); break;
            case ';': AddToken(SEMICOLON); break;
            case '*': AddToken(ASTERISK); break;
            case '!': AddToken(Match('=') ? BANG_EQUAL : BANG); break;
            case '=': AddToken(Match('=') ? EQUAL_EQUAL : EQUAL); break;
            case '<': AddToken(Match('=') ? LESS_EQUAL : LESS); break;
            case '>': AddToken(Match('=') ? GREATER_EQUAL : GREATER); break;
            case '/':
                if (Match('/')) {
                    while (Peek() != '\n' && !AtEnd()) Next();
                }
                else AddToken(SLASH);
                break;

            case ' ':
            case '\r':
            case '\t':
                break;

            case 'o':
                if (Match('r')) AddToken(OR);
                break;

            case '\n': _line++; break;


            default:
                if (Char.IsAsciiDigit(c)) {
                    Number();
                }
                else if (IsAlpha(c)) {
                    Identifier();
                }
                else {
                    Program.Error(_line, "Unexpected character");
                }
                break;
        }
    }

    #region Helpers

    // Lexer movements
    private bool AtEnd() => _current >= _source.Length;
    private char Advance() => _source[_current++];
    private void Next(int step = 1) => _current += step;
    private bool Match(char expected) {
        if (AtEnd() || _source[_current] != expected) return false;
        _current++;
        return true;
    }
    private char? Peek(int lookahead = 0) => _current + lookahead >= _source.Length ? null : _source[_current + lookahead];

    // Token manipulation
    private void AddToken(TokenType type) => AddToken(type, null);
    private void AddToken(TokenType type, object? literal) {
        string lexeme = _source[_start.._current];
        _tokens.Add(new Token(type, lexeme, literal, _line));
    }

    // Helpers
    private bool IsAlpha(char c) => Char.IsAsciiLetter(c) || c == '_';
    private bool IsAlphaNumeric(char c) => IsAlpha(c) || Char.IsAsciiDigit(c);

    // Literals
    private void String() {
        while (Peek() != '"' && !AtEnd()) {
            if (Peek() == '\n') _line++;
            Next();

            if (AtEnd()) {
                Program.Error(_line, "String literal not terminated");
                return;
            }
        }

        Next();

        string value = _source.Substring(_start + 1, _current - 1);
        AddToken(STRING, value);
    }

    private void Number() {
        while (Peek() is not null && Char.IsAsciiDigit((Char)Peek()!)) Next();

        if (Peek() is not null && Peek() == '.' && Char.IsAsciiDigit((Char)Peek(1)!)) Next();

        while (Peek() is not null && Char.IsAsciiDigit((Char)Peek()!)) Next();

        AddToken(NUMBER, Double.Parse(_source.Substring(_start, _current - _start)));
    }

    private void Identifier() {
        while (Peek() is not null && IsAlphaNumeric((Char)Peek()!)) Next();

        string text = _source.Substring(_start, _current - _start);
        bool isInDict = _reservedIdentifiers.TryGetValue(text, out TokenType type);
        if (!isInDict) type = IDENTIFIER;

        AddToken(type);
    }

    #endregion

    private readonly Dictionary<string, TokenType> _reservedIdentifiers = new() {
        { "and", AND },
        { "class", CLASS },
        { "else", ELSE },
        { "false", FALSE },
        { "fun", FUNCTION },
        { "for", FOR },
        { "if", IF },
        { "nil", NIL },
        { "or", OR },
        { "print", PRINT },
        { "return", RETURN },
        { "super", SUPER },
        { "this", THIS },
        { "true", TRUE },
        { "var", VAR },
        { "while", WHILE },
    };

}
