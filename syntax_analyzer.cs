public class Parser
{
    private readonly List<Token> tokens;
    private int currentTokenIndex;

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
        currentTokenIndex = 0;
    }
}




public ExpressionNode ParseExpression()
{
    // Implementa la lógica para analizar una expresión de acuerdo con la gramática de HULK.
    // Utiliza métodos auxiliares para analizar los diferentes componentes de una expresión.
    // Utiliza los tokens y el seguimiento de la posición actual para determinar la estructura de la expresión y construir el árbol de sintaxis abstracta (AST).
    // Actualiza el índice de token actual a medida que analizas los tokens.

    // Ejemplo:
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
        // Expresión entre paréntesis
        return ParseParenthesizedExpression();
    }
    else
    {
        // Error sintáctico
        throw new Exception($"Syntax error at line {currentToken.Line}: Unexpected token '{currentToken.Lexeme}'");
    }
}

public StatementNode ParseStatement()
{
    // Implementa la lógica para analizar una instrucción de acuerdo con la gramática de HULK.
    // Utiliza métodos auxiliares para analizar los diferentes componentes de una instrucción.
    // Utiliza los tokens y el seguimiento de la posición actual para determinar la estructura de la instrucción y construir el AST.
    // Actualiza el índice de token actual a medida que analizas los tokens.

    // Ejemplo:
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
        // Error sintáctico
        throw new Exception($"Syntax error at line {currentToken.Line}: Unexpected token '{currentToken.Lexeme}'");
    }
}

public FunctionDeclarationNode ParseFunctionDeclaration()
{
    // Implementa la lógica para analizar una declaración de función de acuerdo con la gramática de HULK.
    // Utiliza métodos auxiliares para analizar los diferentes componentes de una declaración de función.
    // Utiliza los tokens y el seguimiento de la posición actual para determinar la estructura de la declaración y construir el AST.
    // Actualiza el índice de token actual a medida que analizas los tokens.

    // Ejemplo:
    Token currentToken = GetToken();

    if (currentToken.Type == TokenType.Keyword && currentToken.Lexeme == "function")
    {
        return ParseInlineFunctionDeclaration();
    }
    else
    {
        // Error sintáctico
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
        // Error sintáctico
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

// Métodos auxiliares para verificar el tipo de token esperado

private void ExpectTokenType(TokenType expectedType)
{
    Token currentToken = GetToken();

    if (currentToken.Type != expectedType)
    {
        // Error sintáctico
        throw new Exception($"Syntax error at line {currentToken.Line}: Expected token type '{expectedType}', but found '{currentToken.Type}'");
    }
}

private void ExpectSymbol(string expectedSymbol)
{
    Token currentToken = GetToken();

    if (currentToken.Type != TokenType.Symbol || currentToken.Lexeme != expectedSymbol)
    {
        // Error sintáctico
        throw new Exception($"Syntax error at line {currentToken.Line}: Expected symbol '{expectedSymbolContinuación} but found '{currentToken.Lexeme}'");

    }
}

private void ExpectKeyword(string expectedKeyword)
{
    Token currentToken = GetToken();


    if (currentToken.Type != TokenType.Keyword || currentToken.Lexeme != expectedKeyword)
    {
    // Error sintáctico
    throw new Exception($"Syntax error at line {currentToken.Line}: Expected keyword '{expectedKeyword}', but found '{currentToken.Lexeme}'");
    }

}


// Otros métodos auxiliares

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