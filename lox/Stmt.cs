using System.Collections.Generic;

namespace Lox;

internal abstract class Stmt {
    public interface IVisitor<T> {
        T VisitPrintStmt(Print print);
        T VisitIfStmt(If if);
        T VisitExpressionStmt(Expression expression);
        T VisitBlockStmt(Block block);
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

    public class If : Stmt {
        public Expr Condition { get; private set; }
        public Stmt ThenBranch { get; private set; }
        public Stmt ElseBranch { get; private set; }

        public If(Expr condition, Stmt thenBranch, Stmt elseBranch) {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitIfStmt(this);
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

    public class Block : Stmt {
        public List<Stmt> Statements { get; private set; }

        public Block(List<Stmt> statements) {
            Statements = statements;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitBlockStmt(this);
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
