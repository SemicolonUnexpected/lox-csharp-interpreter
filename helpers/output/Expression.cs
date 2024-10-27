namespace Lox;

internal abstract class Expression {
    public interface IVisitor<T> {
        T VisitBinaryExpression(Binary binary);
        T VisitUnaryExpression(Unary unary);
        T VisitLiteralExpression(Literal literal);
        T VisitGroupingExpression(Grouping grouping);
    }

    public class Binary : Expression {
        public Expression Left { get; private set; }
        public Token Token { get; private set; }
        public Expression Right { get; private set; }

        public Binary(Expression left, Token token, Expression right) {
            Left = left;
            Token = token;
            Right = right;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitBinaryExpression(this);
        }
    }

    public class Unary : Expression {
        public Expression Expression { get; private set; }
        public Token Token { get; private set; }

        public Unary(Expression expression, Token token) {
            Expression = expression;
            Token = token;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitUnaryExpression(this);
        }
    }

    public class Literal : Expression {
        public object Value { get; private set; }

        public Literal(object value) {
            Value = value;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitLiteralExpression(this);
        }
    }

    public class Grouping : Expression {
        public Expression Expression { get; private set; }

        public Grouping(Expression expression) {
            Expression = expression;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitGroupingExpression(this);
        }
    }

    public abstract T Accept<T>(IVisitor<T> visitor);
}