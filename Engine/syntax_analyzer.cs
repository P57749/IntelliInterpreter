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
            return ParseBinaryExpression();
        }

        private ExpressionNode ParseBinaryExpression(int precedence = 0)
        {
            ExpressionNode left = ParsePrimaryExpression();

            while (IsBinaryOperator(PeekToken()?.Lexeme) && GetPrecedence(PeekToken()?.Lexeme) >= precedence)
            {
                Token operatorToken = GetToken();
                TokenType operatorType = operatorToken.Type;

                if (operatorType == TokenType.Symbol)
                {
                    string operatorLexeme = operatorToken.Lexeme;
                    ExpressionNode right = ParseBinaryExpression(GetPrecedence(operatorLexeme));

                    left = new BinaryExpressionNode(left, operatorLexeme, right);
                }
                else
                {
                    throw new Exception($"Syntax error at llline {operatorToken.Line}: Unexpected token '{operatorToken.Lexeme}'");
                }
            }

            return left;
        }



private ExpressionNode ParsePrimaryExpression()
{
    Token currentToken = GetToken();

    if (currentToken.Type == TokenType.Keyword)
    {
        if (IsKeyword("print", currentToken.Lexeme))
        {
            return ParsePrintStatementt();
        }
        else if (IsKeyword("let", currentToken.Lexeme))
        {
            return ParseVariableDeclarationt();
        }
        else if (IsKeyword("if", currentToken.Lexeme))
        {
            return ParseIfStatementt();
        }
        else if (IsKeyword("function", currentToken.Lexeme))
        {
            return ParseInlineFunctionDeclarationt();
        }
        else if (IsKeyword("sin", currentToken.Lexeme) || IsKeyword("cos", currentToken.Lexeme))
        {
            return ParseFunctionCall(currentToken.Lexeme);
        }
        else
        {
            return ParseVariableReference(currentToken.Lexeme);
        }
    }
    else if (currentToken.Type == TokenType.NumberLiteral)
    {
        return ParseNumberLiteral(currentToken.Lexeme);
    }
    else if (currentToken.Type == TokenType.StringLiteral)
    {
        return ParseStringLiteral(currentToken.Lexeme);
    }
    else if (currentToken.Type == TokenType.Symbol && currentToken.Lexeme == "(")
    {
        ExpressionNode expression = ParseExpression();
        ExpectSymbol(")");
        return expression;
    }
    else
    {
        // Error sintáctico
        throw new Exception($"Syntax error at line {currentToken.Line}: Unexpected token '{currentToken.Lexeme}'");
    }
}







private ExpressionNode ParsePrintStatementt()
{
    ExpectKeyword("print");

    ExpressionNode expression = ParseExpression();

    return new PrintStatementNodeExpression(expression);
}

private ExpressionNode ParseVariableDeclarationt()
{
    ExpectKeyword("let");

    Token identifierToken = GetToken();
    ExpectTokenType(TokenType.Identifier);

    ExpectSymbol("=");

    ExpressionNode expression = ParseExpression();

    ExpectSymbol(";");

    return new VariableDeclarationNodeExpression(identifierToken.Lexeme, expression);
}

private ExpressionNode ParseIfStatementt()
{
    ExpectKeyword("if");

    ExpectSymbol("(");

    ExpressionNode condition = ParseExpression();

    ExpectSymbol(")");

    ExpressionNode ifBody = ParseExpression();

    ExpectKeyword("else");

        //GetToken(); // Consume the "else" keyword
        ExpressionNode elseBody = ParseExpression();
        return new IfStatementNodeExpression(condition, ifBody, elseBody);

}

private ExpressionNode ParseInlineFunctionDeclarationt()
{
    ExpectKeyword("function");

    Token identifierToken = GetToken();
    ExpectTokenType(TokenType.Identifier);

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

    return new FunctionDeclarationNodeExpression(identifierToken.Lexeme, parameters, body);
}








        private VariableReferenceNode ParseVariableReference(string identifier)
        {
            return new VariableReferenceNode(identifier);
        }

        private NumberLiteralNode ParseNumberLiteral(string lexeme)
        {
            if (!double.TryParse(lexeme, out double value))
            {
                throw new Exception($"Syntax error: Invalid number literal '{lexeme}'");
            }

            return new NumberLiteralNode(value);
        }

        private StringLiteralNode ParseStringLiteral(string lexeme)
        {
            return new StringLiteralNode(lexeme);
        }

        private FunctionDeclarationNode ParseInlineFunctionDeclaration()
        {
            ExpectKeyword("function");

            Token identifierToken = GetToken();
            ExpectTokenType(TokenType.Identifier);


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

        private FunctionCallNode ParseFunctionCall(string functionName)
        {
            ExpectSymbol("(");

            ExpressionNode argument = ParseExpression();

            ExpectSymbol(")");

            return new FunctionCallNode(functionName, argument);
        }
private StatementNode ParseStatement()
{
    Token currentToken = GetToken();

    if (currentToken.Type == TokenType.Keyword)
    {
        if (currentToken.Lexeme == "print")
        {
            return ParsePrintStatement();
        }
        else if (currentToken.Lexeme == "let")
        {
            return ParseVariableDeclaration();
        }
        else if (currentToken.Lexeme == "if")
        {
            return ParseIfStatement();
        }
    }
    else if (currentToken.Type == TokenType.Identifier)
    {
        Token nextToken = PeekToken();

        if (nextToken?.Lexeme == "(")
        {
            return (StatementNode)ParseFunctionDeclaration();
        }
        else
        {
            return ParseAssignmentStatement(currentToken.Lexeme);
        }
    }

    throw new Exception($"Syntax error at line {currentToken.Line}: Unexpected token '{currentToken.Lexeme}'");
}

        private FunctionDeclarationNode ParseFunctionDeclaration()
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

        private VariableDeclarationNode ParseVariableDeclaration()
        {
            ExpectKeyword("let");

            Token identifierToken = GetToken();
            ExpectTokenType(TokenType.Identifier);

            ExpectSymbol("=");

            ExpressionNode expression = ParseExpression();

            ExpectSymbol(";");

            return new VariableDeclarationNode(identifierToken.Lexeme, expression);
        }

        private AssignmentStatementNode ParseAssignmentStatement(string identifier)
        {
            ExpectSymbol("=");

            ExpressionNode expression = ParseExpression();

            ExpectSymbol(";");

            return new AssignmentStatementNode(identifier, expression);
        }

        private IfStatementNode ParseIfStatement()
        {
            ExpectSymbol("(");

            ExpressionNode condition = ParseExpression();

            ExpectSymbol(")");

            StatementNode ifBody = ParseStatement();

            ExpectKeyword("else");

            StatementNode elseBody = ParseStatement();

            return new IfStatementNode(condition, ifBody, elseBody);
        }

        private PrintStatementNode ParsePrintStatement()
        {
            //ExpectKeyword("print");

            ExpectSymbol("(");

            ExpressionNode expression = ParseExpression();

            ExpectSymbol(")");

            ExpectSymbol(";");

            return new PrintStatementNode(expression);
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

            if (currentToken.Type != TokenType.Keyword || !IsKeyword(expectedKeyword, currentToken.Lexeme))
            {
                throw new Exception($"Syntax error at line {currentToken.Line}: Expected keywordddd '{expectedKeyword}', but found '{currentToken.Lexeme}'");
            }
        }

        private bool IsKeyword(string expectedKeyword, string actualKeyword)
        {
            return string.Equals(expectedKeyword, actualKeyword, StringComparison.OrdinalIgnoreCase);
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

        private bool IsBinaryOperator(string lexeme)
        {
            return lexeme == "+" || lexeme == "-" || lexeme == "*" || lexeme == "/" || lexeme == "sin" || lexeme == "cos" || lexeme == "!";
        }

        private int GetPrecedence(string lexeme)
        {
            switch (lexeme)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                case "sin":
                case "cos":
                case "!":
                    return 3;
                default:
                    return 0;
            }
        }
    }
}
