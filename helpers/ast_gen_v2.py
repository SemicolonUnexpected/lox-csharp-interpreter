classes = [
    ["Binary", [["Expression", "left"], ["Token", "token"], ["Expression", "right"]]],
    ["Unary", [["Expression", "expression"], ["Token", "token"]]],
    ["Literal", [["object", "value"]]],
    ["Grouping", [["Expression", "expression"]]],
]


def main():
    text = ["namespace Lox;", "", "internal abstract class Expression {"]
    text.append(get_visitor())
    for name, fields in classes:
        text.append(get_class(name, fields))
    text += ["    public abstract T Accept<T>(IVisitor<T> visitor);", "}"]

    # print("\n".join(text))
    file = open("output/Expression.cs", "w")
    file.write("\n".join(text))
    file.close()


def get_visitor():
    text = [
        "    public interface IVisitor<T> {\n",
        "".join(
            [
                f"        T Visit{cs_class[0]}Expression({cs_class[0]} {cs_class[0].lower()});\n"
                for cs_class in classes
            ]
        ),
        "    }\n",
    ]

    return "".join(text)


def get_class(name, fields):
    text = [
        f"    public class {name} : Expression {{\n",
        "".join(
            [
                f"        public {cs_type} {field.title()} {{ get; private set; }}\n"
                for (cs_type, field) in fields
            ]
        ),
        f"\n        public {name}(",
        ", ".join([f"{cs_type} {field}" for (cs_type, field) in fields]),
        ") {\n",
        "".join([f"            {field.title()} = {field};\n" for (_, field) in fields]),
        "        }\n\n",
        "        public override T Accept<T>(IVisitor<T> visitor) {\n",
        f"            return visitor.Visit{name}Expression(this);\n",
        "        }\n",
        "    }\n",
    ]

    return "".join(text)


main()
