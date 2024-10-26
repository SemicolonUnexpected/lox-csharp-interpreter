classes = [
    ["Binary", [["Expression", "left"], ["Token", "token"], ["Expression", "right"]]],
    ["Unary", [["Expression", "expression"], ["Token", "token"]]],
]


def main():
    text = ["using Lox;", "", "internal class Expression {"]
    text.append(get_visitor())
    for name, fields in classes:
        text.append(get_class(name, fields))
    text += ["    public abstract T Accept<T>(IVisitor<T> visitor);", "}"]

    print("\n".join(text))


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
            [f"        private {cs_type} _{field};\n" for (cs_type, field) in fields]
        ),
        f"\n        public {name}(",
        ", ".join([f"{cs_type} {field}" for (cs_type, field) in fields]),
        ") {\n",
        "".join([f"            _{field} = {field};\n" for (_, field) in fields]),
        "        }\n    }\n",
    ]

    return "".join(text)


main()
