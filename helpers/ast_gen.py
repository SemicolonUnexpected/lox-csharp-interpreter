ast_classes = [
    ["Binary", [["Expr", "left"], ["Token", "token"], ["Expr", "right"]]],
    ["Unary", [["Expr", "expression"], ["Token", "token"]]],
    ["Literal", [["object?", "value"]]],
    ["Grouping", [["Expr", "expression"]]],
    ["Variable", [["Token", "name"]]],
]

stmt_classes = [
    ["Print", [["Expr", "expression"]]],
    ["Expression", [["Expr", "loxExpression"]]],
    ["Var", [["Token", "name"], ["Expr", "initialiser"]]],
]


def main():
    # Define the Expr.cs class
    text = gen_visitor_pattern("Expr", ast_classes)
    file = open("output/Expr.cs", "w")
    file.write(text)
    file.close()

    # Define the Stmt.cs class
    text = gen_visitor_pattern("Stmt", stmt_classes)
    file = open("output/Stmt.cs", "w")
    file.write(text)
    file.close()


def gen_visitor_pattern(class_name, contents):
    text = ["namespace Lox;", "", f"internal abstract class {class_name} {{"]
    text.append(get_visitor(class_name, contents))
    for name, fields in contents:
        text.append(get_class(class_name, name, fields))
    text += ["    public abstract T Accept<T>(IVisitor<T> visitor);", "}"]

    return "\n".join(text)


def get_visitor(class_name, classes):
    text = [
        "    public interface IVisitor<T> {\n",
        "".join(
            [
                f"        T Visit{cs_class[0]}{class_name}({cs_class[0]} {cs_class[0][0].lower() + cs_class[0][1:]});\n"
                for cs_class in classes
            ]
        ),
        "    }\n",
    ]

    return "".join(text)


def get_class(class_name, name, fields):
    text = [
        f"    public class {name} : {class_name} {{\n",
        "".join(
            [
                f"        public {cs_type} {field[0].upper() + field[1:]} {{ get; private set; }}\n"
                for (cs_type, field) in fields
            ]
        ),
        f"\n        public {name}(",
        ", ".join([f"{cs_type} {field}" for (cs_type, field) in fields]),
        ") {\n",
        "".join(
            [
                f"            {field[0].upper() + field[1:]} = {field};\n"
                for (_, field) in fields
            ]
        ),
        "        }\n\n",
        "        public override T Accept<T>(IVisitor<T> visitor) {\n",
        f"            return visitor.Visit{name}{class_name}(this);\n",
        "        }\n",
        "    }\n",
    ]

    return "".join(text)


main()
