namespace Lox;

internal abstract class Stmt {
    public interface IVisitor<T> {
        T VisitPrintStmt(Print print);
        T VisitExpressionStmt(Expression expression);
    }

    public class Print : Stmt {
        public Expr Expression { get; private set; }

        public Print(Expr expression) {
            Expression = expression;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitPrintStmt(this);
        }
    }

    public class Expression : Stmt {
        public Expr LoxExpression { get; private set; }

        public Expression(Expr LoxExpression) {
            this.LoxExpression = LoxExpression;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitExpressionStmt(this);
        }
    }

    public abstract T Accept<T>(IVisitor<T> visitor);
}
