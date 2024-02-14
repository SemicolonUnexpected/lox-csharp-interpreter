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
            default:
                throw new LoxSyntaxException("Unexpected character", _line); 
        }
    }
    
    #region Helpers

    // Lexer movements
    private bool AtEnd() => _current >= _source.Length;
    private char Advance() => _source[_current++];

    // Token manipulation
    private void AddToken(TokenType type) => AddToken(type, null);
    private void AddToken(TokenType type, object? literal) {
        string lexeme = _source[_start.._current];
        _tokens.Add(new Token(type, lexeme, literal, _line));
    }

    #endregion
}
