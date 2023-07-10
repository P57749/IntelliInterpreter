public enum TokenType
{
    Identifier,     // Identificador (nombres de variables y funciones)
    StringLiteral,  // Cadena de texto
    NumberLiteral,  // Número
    BooleanLiteral, // Valor booleano (true o false)
    Symbol,         // Símbolo (operadores, paréntesis, comas, etc.)
    Keyword         // Palabra clave (print, function, let, in, if, else)
}

public class Token
{
    public TokenType Type { get; }
    public string Lexeme { get; }
    public int Line { get; }

    public Token(TokenType type, string lexeme, int line)
    {
        Type = type;
        Lexeme = lexeme;
        Line = line;
    }
}

public class Lexer
{
    private readonly string input;
    private int position;
    private int line;

    public Lexer(string input)
    {
        this.input = input;
        position = 0;
        line = 1;
    }

    public List<Token> Tokenize()
    {
        var tokens = new List<Token>();

        while (position < input.Length)
        {
            char currentChar = input[position];

            if (IsWhitespace(currentChar))
            {
                ConsumeWhitespace();
            }
            else if (IsLetter(currentChar))
            {
                tokens.Add(ScanIdentifier());
            }
            else if (IsDigit(currentChar))
            {
                tokens.Add(ScanNumber());
            }
            else if (currentChar == '"')
            {
                tokens.Add(ScanStringLiteral());
            }
            else
            {
                tokens.Add(ScanSymbol());
            }
        }

        return tokens;
    }

    private Token ScanIdentifier()
    {
        int start = position;
        while (position < input.Length && IsAlphaNumeric(input[position]))
        {
            position++;
        }

        string lexeme = input[start..position];
        TokenType type = IsKeyword(lexeme) ? TokenType.Keyword : TokenType.Identifier;
        return new Token(type, lexeme, line);
    }

    private Token ScanNumber()
    {
        int start = position;
        while (position < input.Length && IsDigit(input[position]))
        {
            position++;
        }

        if (position < input.Length && input[position] == '.')
        {
            position++;
            while (position < input.Length && IsDigit(input[position]))
            {
                position++;
            }
        }

        string lexeme = input[start..position];
        return new Token(TokenType.NumberLiteral, lexeme, line);
    }

    private Token ScanStringLiteral()
    {
        int start = position;
        position++; // Ignore the opening quote

        while (position < input.Length && input[position] != '"')
        {
            if (input[position] == '\n')
            {
                line++;
            }

            position++;
        }

        if (position >= input.Length)
        {
            // Unterminated string literal
            throw new Exception($"Unterminated string literal at line {line}");
        }

        position++; // Ignore the closing quote

        string lexeme = input[start..position];
        return new Token(TokenType.StringLiteral, lexeme, line);
    }

    private Token ScanSymbol()
    {
        int start = position;
        position++;

        string lexeme = input[start..position];
        return new Token(TokenType.Symbol, lexeme, line);
    }

    private bool IsWhitespace(char c)
    {
        return c == ' ' || c == '\t' || c == '\r' || c == '\n';
    }

    private void ConsumeWhitespace()
    {
        while (position < input.Length && IsWhitespace(input[position]))
        {
            if (input[position] == '\n')
            {
                line++;
            }

            position++;
        }
    }

    private bool IsLetter(char c)
    {
        return char.IsLetter(c) || c == '_';
    }

    private bool IsAlphaNumeric(char c)
    {
        return char.IsLetterOrDigit(c) || c == '_';
    }

    private bool IsDigit(char c)
    {
        return char.IsDigit(c);
    }

    private bool IsKeyword(string lexeme)
    {
        // Lista de palabras clave de HULK
        string[] keywords = { "print", "function", "let", "in", "if", "else" };
        return keywords.Contains(lexeme);
    }
}
