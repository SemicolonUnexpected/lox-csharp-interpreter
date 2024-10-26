namespace Lox.Ast;

internal class Literal : IExpression {
    private object _value;

    public Literal(object value) {
        _value = value;
    }
}
