using System;
using System.Collections.Generic;
using System.Linq;

namespace HulkInterpreter
{
    public class Parser
    {
        private readonly List<Token> tokens;
        private int currentTokenIndex;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            currentTokenIndex = 0;
        }

        public ExpressionNode ParseExpression()
        {
            Token currentToken = GetToken();

            if (currentToken.Type == TokenType.Identifier)
            {
                return ParseVariableReference();
            }
            else if (currentToken.Type == TokenType.NumberLiteral)
            {
                return ParseNumberLiteral();
            }
            else if (currentToken.Type == TokenType.StringLiteral)
            {
                return ParseStringLiteral();
            }
            else if (currentToken.Type == TokenType.Symbol && currentToken.Lexeme == "(")
            {
                return ParseParenthesizedExpression();
            }
            else
            {
                // Error sintáctico
                throw new Exception($"Syntax error at line {currentToken.Line}: Unexpected token '{currentToken.Lexeme}'");
            }
        }

        public FunctionDeclarationNode ParseInlineFunctionDeclaration()
        {
            Token currentToken = GetToken();
            ExpectTokenType(TokenType.Keyword);
            ExpectSymbol("(");

            List<string> parameters = new List<string>();

            while (PeekToken() != null && PeekToken().Lexeme != ")")
            {
                Token parameterToken = GetToken();
                ExpectTokenType(TokenType.Identifier);
                parameters.Add(parameterToken.Lexeme);

                if (PeekToken()?.Lexeme == ",")
                {
                    GetToken(); // Consume the comma
                }
            }

            ExpectSymbol(")");
            ExpectSymbol("=>");

            StatementNode body = ParseStatement();

            return new FunctionDeclarationNode("", parameters, body);
        }


        public StatementNode ParseStatement()
        {
            Token currentToken = GetToken();

            if (currentToken.Type == TokenType.Keyword && currentToken.Lexeme == "print")
            {
                return ParsePrintStatement();
            }
            else if (currentToken.Type == TokenType.Keyword && currentToken.Lexeme == "let")
            {
                return ParseVariableDeclaration();
            }
            else if (currentToken.Type == TokenType.Identifier)
            {
                return ParseAssignmentStatement();
            }
            else if (currentToken.Type == TokenType.Keyword && currentToken.Lexeme == "if")
            {
                return ParseIfStatement();
            }
            else
            {
                throw new Exception($"Syntax error at line {currentToken.Line}: Unexpected token '{currentToken.Lexeme}'");
            }
        }

        public FunctionDeclarationNode ParseFunctionDeclaration()
        {
            Token currentToken = GetToken();

            if (currentToken.Type == TokenType.Keyword && currentToken.Lexeme == "function")
            {
                return ParseInlineFunctionDeclaration();
            }
            else
            {
                throw new Exception($"Syntax error at line {currentToken.Line}: Unexpected token '{currentToken.Lexeme}'");
            }
        }

        public VariableReferenceNode ParseVariableReference()
        {
            Token currentToken = GetToken();
            ExpectTokenType(TokenType.Identifier);

            return new VariableReferenceNode(currentToken.Lexeme);
        }

        public NumberLiteralNode ParseNumberLiteral()
        {
            Token currentToken = GetToken();
            ExpectTokenType(TokenType.NumberLiteral);

            if (!double.TryParse(currentToken.Lexeme, out double value))
            {
                throw new Exception($"Syntax error at line {currentToken.Line}: Invalid number literal '{currentToken.Lexeme}'");
            }

            return new NumberLiteralNode(value);
        }

        public StringLiteralNode ParseStringLiteral()
        {
            Token currentToken = GetToken();
            ExpectTokenType(TokenType.StringLiteral);

            return new StringLiteralNode(currentToken.Lexeme);
        }

        public ParenthesizedExpressionNode ParseParenthesizedExpression()
        {
            ExpectSymbol("(");

            ExpressionNode expression = ParseExpression();

            ExpectSymbol(")");

            return new ParenthesizedExpressionNode(expression);
        }

        public PrintStatementNode ParsePrintStatement()
        {
            ExpectKeyword("print");

            ExpressionNode expression = ParseExpression();

            ExpectSymbol(";");

            return new PrintStatementNode(expression);
        }

        public VariableDeclarationNode ParseVariableDeclaration()
        {
            ExpectKeyword("let");

            Token identifierToken = GetToken();
            ExpectTokenType(TokenType.Identifier);

            ExpectSymbol("=");

            ExpressionNode expression = ParseExpression();

            ExpectSymbol(";");

            return new VariableDeclarationNode(identifierToken.Lexeme, expression);
        }

        public AssignmentStatementNode ParseAssignmentStatement()
        {
            Token identifierToken = GetToken();
            ExpectTokenType(TokenType.Identifier);

            ExpectSymbol("=");

            ExpressionNode expression = ParseExpression();

            ExpectSymbol(";");

            return new AssignmentStatementNode(identifierToken.Lexeme, expression);
        }

        public IfStatementNode ParseIfStatement()
        {
            ExpectKeyword("if");

            ExpectSymbol("(");

            ExpressionNode condition = ParseExpression();

            ExpectSymbol(")");

            StatementNode ifBody = ParseStatement();

            ExpectKeyword("else");

            StatementNode elseBody = ParseStatement();

            return new IfStatementNode(condition, ifBody, elseBody);
        }

        private void ExpectTokenType(TokenType expectedType)
        {
            Token currentToken = GetToken();

            if (currentToken.Type != expectedType)
            {
                throw new Exception($"Syntax error at line {currentToken.Line}: Expected token type '{expectedType}', but found '{currentToken.Type}'");
            }
        }

        private void ExpectSymbol(string expectedSymbol)
        {
            Token currentToken = GetToken();

            if (currentToken.Type != TokenType.Symbol || currentToken.Lexeme != expectedSymbol)
            {
                throw new Exception($"Syntax error at line {currentToken.Line}: Expected symbol '{expectedSymbol}', but found '{currentToken.Lexeme}'");
            }
        }

        private void ExpectKeyword(string expectedKeyword)
        {
            Token currentToken = GetToken();

            if (currentToken.Type != TokenType.Keyword || currentToken.Lexeme != expectedKeyword)
            {
                throw new Exception($"Syntax error at line {currentToken.Line}: Expected keyword '{expectedKeyword}', but found '{currentToken.Lexeme}'");
            }
        }

        private Token GetToken()
        {
            if (currentTokenIndex < tokens.Count)
            {
                return tokens[currentTokenIndex++];
            }
            else
            {
                // Error sintáctico: se esperaba más tokens
                throw new Exception("Syntax error: Unexpected end of input");
            }
        }

        private Token PeekToken()
        {
            if (currentTokenIndex < tokens.Count)
            {
                return tokens[currentTokenIndex];
            }
            else
            {
                return null;
            }
        }
    }

}
