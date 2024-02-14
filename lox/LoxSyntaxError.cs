using System;

namespace Lox;

public class LoxSyntaxException : Exception {
    public int Line { get; init; }

    public LoxSyntaxException(string message, int line) : base(message) {
        Line = line;
    }
}
