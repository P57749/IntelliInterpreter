using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
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

                    // Realiza el análisis léxico
                    Lexer lexer = new Lexer(input);
                    List<Token> tokens = lexer.Tokenize();

                    // Realiza el análisis sintáctico
                    Parser parser = new Parser(tokens);
                    ExpressionNode expression = parser.ParseExpression();

                    // Realiza el análisis semántico
                    // SemanticAnalyzer semanticAnalyzer = new SemanticAnalyzer(expression);
                    // semanticAnalyzer.Analyze();

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
                    Console.WriteLine($"SYNTAX ERROR: {ex.Message}");
                }
                // catch (SemanticException semEx)
                // {
                //     Console.WriteLine($"SEMANTIC ERROR: {semEx.Message}");
                // }
            }
        }
    }
}
