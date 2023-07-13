using System;
using System.Collections.Generic;
using System.Linq;

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

                    // Realiza el análisis sintáctico
                    Parser parser = new Parser(tokens);
                    ExpressionNode expression = parser.ParseExpression();

                    // Ejecuta el árbol de sintaxis abstracta (AST)
                    var result = expression.Evaluate();

                    // Imprime el resultado de la ejecución
                    Console.WriteLine(result);
                }
                catch (LexerException lexEx)
                {
                    Console.WriteLine($"LEXICAL ERROR: {lexEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
    
}