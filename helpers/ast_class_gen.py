classes = [
    ["Binary", ("IExpression", "left"), ("Token", "token"), ("IExpression", "right")],
    ["Grouping", ("IExpression", "expression")],
    ["Literal", ("object", "value")],
    ["Unary", ("Token", "token"), ("IExpression", "right")],
]


def main():
    for i in classes:
        create_class(i[0], i[1:])


def create_class(class_name: str, fields: list):
    # Generate the text for the file
    # Double braces ("{{" and "}}") are used to escape
    # braces in a format string
    text = (
        "namespace Lox.Ast;\n"
        "\n"
        "internal class {0} : IExpression {{\n"
        "{1}\n"
        "\n"
        "    public {0}({2}) {{\n"
        "{3}\n"
        "    }}\n"
        "}}"
    ).format(
        class_name,
        "\n".join(
            [
                f"    private {cs_type} _{field_name};"
                for (cs_type, field_name) in fields
            ]
        ),
        ", ".join([f"{cs_type} {field_name}" for (cs_type, field_name) in fields]),
        "\n".join(
            [f"        _{field_name} = {field_name};" for (_, field_name) in fields]
        ),
    )
    print(text)

    # Write to the file
    file = open(f"output/{class_name}.cs", "w")
    file.write(text)
    file.close()


def generate_visitor_interface():
    # Generate the text for the file
    text = (
        "interface Visitor<T> {\n"
        + "".join([f"T Visit{field_name}Expression({fie}" for (cs_type, field_name) in classes)
        + "}"
    )

    # Write to the file
    file = open("output/IVisitor.cs", "w")
    file.write(text)
    file.close()


main()
