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
        switch(c) {
            case '(' : AddToken(LEFT_PARENTHESIS); break;
            case ')' : AddToken(RIGHT_PARENTHESIS); break;
            case '{' : AddToken(LEFT_BRACE); break;
            case '}' : AddToken(RIGHT_PARENTHESIS); break;
            case ',' : AddToken(COMMA); break;
            case '.' : AddToken(DOT); break;
            case '-' : AddToken(MINUS); break;
            case '+' : AddToken(PLUS); break;
            case ';' : AddToken(SEMICOLON); break;
            case '*' : AddToken(ASTERISK); break;
            case '!' : AddToken(Match('=') ? BANG_EQUAL : BANG); break;
            case '=' : AddToken(Match('=') ? EQUAL_EQUAL : EQUAL); break;
            case '<' : AddToken(Match('=') ? LESS_EQUAL : LESS); break;
            case '>' : AddToken(Match('=') ? GREATER_EQUAL : GREATER); break;
            case '/':
                if (Match('/')) {
                    while (Peek() != '\n' && !AtEnd()) Next();
                }
                else AddToken(SLASH);
                break;

            case ' ' :
            case '\r' :
            case '\t' :
                break;

            case '\n' : _line++; break;
            default:
                Program.Error(_line, "Unexpected character");
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
    private char? Peek() => AtEnd() ? null : _source[_current]; 


    // Token manipulation
    private void AddToken(TokenType type) => AddToken(type, null);
    private void AddToken(TokenType type, object? literal) {
        string lexeme = _source[_start.._current];
        _tokens.Add(new Token(type, lexeme, literal, _line));
    }

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

    #endregion
}
