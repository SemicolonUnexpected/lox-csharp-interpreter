namespace Lox.Ast;

internal class Unary : IExpression {
    private Token _token;
    private IExpression _right;

    public Unary(Token token, IExpression right) {
        _token = token;
        _right = right;
    }
}