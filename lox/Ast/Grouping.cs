namespace Lox.Ast;

internal class Grouping : IExpression {
    private IExpression _expression;

    public Grouping(IExpression expression) {
        _expression = expression;
    }
}
