using System;
using System.Collections.Generic;

namespace Lox;

internal class Environment {
    private readonly Dictionary<string, object> _values = new();

    public void Define(string name, object value) {
        _values[name] = value;
    }

    public object Get(Token identifier) {
        if (_values.TryGetValue(identifier.Lexeme, out object value)) {
            return value;
        }

        throw new LoxRuntimeException("Variable not defined", identifier);
    }

    internal void Assign(Token name, object value) {
        if (_values.ContainsKey(name.Lexeme)) _values[name.Lexeme] = value;
        else throw new LoxRuntimeException("Undefined variable", name);
    }

    public void LookInside() {
        foreach (var item in _values) {
            System.Console.WriteLine(item);
        }
    }
}
