namespace Lox;

internal abstract class Stmt {
    public interface IVisitor<T> {
        T VisitPrintStmt(Print print);
        T VisitExpressionStmt(Expression expression);
        T VisitVarStmt(Var var);
    }

    public class Print : Stmt {
        public Expr LoxExpression { get; private set; }

        public Print(Expr loxExpression) {
            LoxExpression = loxExpression;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitPrintStmt(this);
        }
    }

    public class Expression : Stmt {
        public Expr LoxExpression { get; private set; }

        public Expression(Expr loxExpression) {
            LoxExpression = loxExpression;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitExpressionStmt(this);
        }
    }

    public class Var : Stmt {
        public Token Name { get; private set; }
        public Expr Initialiser { get; private set; }

        public Var(Token name, Expr initialiser) {
            Name = name;
            Initialiser = initialiser;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitVarStmt(this);
        }
    }

    public abstract T Accept<T>(IVisitor<T> visitor);
}