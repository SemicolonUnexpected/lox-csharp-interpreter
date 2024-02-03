internal class Program {
    private static void Main(string[] args) {
        if (args.Length == 1) RunFile(args[0]);
        else if (args.Length == 0) RunPrompt();
        else {
            Console.WriteLine("Usage: lox [script]");
        }

        static void RunFile(string file) {
            string text = File.ReadAllText(file);
            Run(text);
        }

        static void RunPrompt() {
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

        static void Run(string source) {
            Scanner scanner = new(source);
    }
}
