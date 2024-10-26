using System;
using System.Collections.Generic;
using System.IO;

namespace Lox;

internal class Program
{
    private static bool _hadError = false;

    private static void Main(string[] args)
    {
        if (args.Length == 1)
            RunFile(args[0]);
        else if (args.Length == 0)
            RunPrompt();
        else
        {
            Console.WriteLine("Usage: lox [script]");
        }
    }

    private static void RunFile(string file)
    {
        string text = File.ReadAllText(file);
        Run(text);

        if (_hadError)
            Environment.Exit(65);
    }

    private static void RunPrompt()
    {
        while (true)
        {
            Console.Write(">>> ");
            string? line = Console.ReadLine();
            if (line is null)
            {
                Console.WriteLine();
                break;
            }
            Run(line);
        }
    }

    private static void Run(string source)
    {
        Lexer scanner = new(source);

        List<Token> tokens = scanner.Scan();

        foreach (Token token in tokens)
            Console.WriteLine(token);
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string message)
    {
        Console.WriteLine($"[Line {line}] {where} : {message}");
    }
}
