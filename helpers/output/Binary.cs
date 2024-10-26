namespace Lox.Ast;

internal class Binary : IExpression {
    private IExpression _left;
    private Token _token;
    private IExpression _right;

    public Binary(IExpression left, Token token, IExpression right) {
        _left = left;
        _token = token;
        _right = right;
    }
}
