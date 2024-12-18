namespace Lox;

internal abstract class Expr {
    public interface IVisitor<T> {
        T VisitBinaryExpr(Binary binary);
        T VisitUnaryExpr(Unary unary);
        T VisitLiteralExpr(Literal literal);
        T VisitLogicalExpr(Logical logical);
        T VisitGroupingExpr(Grouping grouping);
        T VisitVariableExpr(Variable variable);
        T VisitAssignExpr(Assign assign);
    }

    public class Binary : Expr {
        public Expr Left { get; private set; }
        public Token Token { get; private set; }
        public Expr Right { get; private set; }

        public Binary(Expr left, Token token, Expr right) {
            Left = left;
            Token = token;
            Right = right;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitBinaryExpr(this);
        }
    }

    public class Unary : Expr {
        public Expr Expression { get; private set; }
        public Token Token { get; private set; }

        public Unary(Expr expression, Token token) {
            Expression = expression;
            Token = token;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitUnaryExpr(this);
        }
    }

    public class Literal : Expr {
        public object? Value { get; private set; }

        public Literal(object? value) {
            Value = value;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitLiteralExpr(this);
        }
    }

    public class Logical : Expr {
        public Expr Right { get; private set; }
        public Token Op { get; private set; }
        public Expr Left { get; private set; }

        public Logical(Expr right, Token op, Expr left) {
            Right = right;
            Op = op;
            Left = left;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitLogicalExpr(this);
        }
    }

    public class Grouping : Expr {
        public Expr Expression { get; private set; }

        public Grouping(Expr expression) {
            Expression = expression;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitGroupingExpr(this);
        }
    }

    public class Variable : Expr {
        public Token Name { get; private set; }

        public Variable(Token name) {
            Name = name;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitVariableExpr(this);
        }
    }

    public class Assign : Expr {
        public Token Name { get; private set; }
        public Expr Value { get; private set; }

        public Assign(Token name, Expr value) {
            Name = name;
            Value = value;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitAssignExpr(this);
        }
    }

    public abstract T Accept<T>(IVisitor<T> visitor);
}