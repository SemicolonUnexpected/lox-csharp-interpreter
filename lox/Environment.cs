using System.Collections.Generic;

namespace Lox;

internal class Environment {
    public Environment? Enclosing { get; init; }

    public Environment() => Enclosing = null;
    public Environment(Environment enclosing) => Enclosing = enclosing;

    private readonly Dictionary<string, object> _values = new();

    public void Define(string name, object value) {
        _values[name] = value;
    }

    public object Get(Token identifier) {
        if (_values.TryGetValue(identifier.Lexeme, out object value)) {
            return value;
        }

        if (Enclosing is not null) return Enclosing.Get(identifier);

        throw new LoxRuntimeException("Variable not defined", identifier);
    }

    internal void Assign(Token name, object value) {
        if (_values.ContainsKey(name.Lexeme)) {
            _values[name.Lexeme] = value;
            return;
        }
        if (Enclosing is not null) {
            Enclosing.Assign(name, value);
            return;
        }
        throw new LoxRuntimeException("Undefined variable", name);
    }
}
