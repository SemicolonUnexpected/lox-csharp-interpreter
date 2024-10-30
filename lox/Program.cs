using System;
using System.Collections.Generic;
using System.IO;

namespace Lox;

internal class Program {
    private static bool _hadError = false;
    private static bool _hadRuntimeError = false;

    private static readonly Interpreter interpreter = new();

    private static void Main(string[] args) {
        if (args.Length == 1)
            RunFile(args[0]);
        else if (args.Length == 0)
            RunPrompt();
        else {
            Console.WriteLine("Usage: lox [script]");
        }
    }

    private static void RunFile(string file) {
        string text = File.ReadAllText(file);
        Run(text);

        if (_hadError) System.Environment.Exit(65);
        if (_hadRuntimeError) System.Environment.Exit(70);
    }

    private static void RunPrompt() {
        while (true) {
            Console.Write(">>> ");
            string? line = Console.ReadLine();
            if (line is null) {
                Console.WriteLine();
                break;
            }
            Run(line);
        }
    }

    private static void Run(string source) {
        _hadError = false;

        Lexer scanner = new(source);

        List<Token> tokens = scanner.Scan();

        // Show me each of the tokens that the lexer has 'lexed'
        // foreach (Token token in tokens) Console.WriteLine(token);

        Parser parser = new(tokens);
        List<Stmt> statements = parser.Parse();

        if (_hadError) return;

        // Print out the AST for debugging purposes
        // AstPrinter printer = new();
        // foreach (Stmt stmt in statements) Console.WriteLine($"Abstract syntax tree : {printer.GetAst(stmt.)}");

        interpreter.Interpret(statements);


    }

    public static void Error(int line, string message) {
        Report(line, "", message);
    }

    public static void Error(Token token, string message) {
        if (token.Type == TokenType.EOF) {
            Report(token.Line, " at end", message);
        }
        else {
            Report(token.Line, $" at '{token.Lexeme}'", message);
        }
    }

    private static void Report(int line, string where, string message) {
        Console.WriteLine($"[Line {line}] {where} : {message}");
        _hadError = true;
    }

    internal static void RuntimeError(LoxRuntimeException exception) {
        Console.WriteLine($"[Line {exception.Token.Line}] at '{exception.Token.Lexeme}' : {exception.Message}");
        _hadError = true;
    }
}
