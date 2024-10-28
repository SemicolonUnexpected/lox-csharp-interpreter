using System;

namespace Lox;

internal class LoxRuntimeException : Exception {
    public Token Token { get; init; }

    public LoxRuntimeException(string message, Token token) : base(message) {
        Token = token;
    }
}
