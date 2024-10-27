namespace Lox;

class Interpreter : Expr.IVisitor<object> {
    public object VisitBinaryExpr(Expr.Binary binary) {
        throw new System.NotImplementedException();
    }

    public object VisitGroupingExpr(Expr.Grouping grouping) {
        throw new System.NotImplementedException();
    }

    public object VisitLiteralExpr(Expr.Literal literal) {
        throw new System.NotImplementedException();
    }

    public object VisitUnaryExpr(Expr.Unary unary) {
        throw new System.NotImplementedException();
    }
}
