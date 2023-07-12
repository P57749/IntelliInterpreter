using System;
using System.Collections.Generic;

namespace HulkInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hulk Interpreter");
            Console.WriteLine("----------------");

            bool exit = false;
            while (!exit)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase) || input.Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    exit = true;
                    continue;
                }

                try
                {
                    Lexer lexer = new Lexer(input);

                    // Realiza el análisis léxico
                    List<Token> tokens = lexer.Tokenize();

                    // Imprime los tokens
                    foreach (var token in tokens)
                    {
                        Console.WriteLine($"{token.Type}: {token.Lexeme} (Line {token.Line})");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
