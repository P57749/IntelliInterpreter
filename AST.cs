using System;
using System.Collections.Generic;
namespace HulkInterpreter
{
    public abstract class ExpressionNode
    {
        public abstract object Evaluate();
    }

    public abstract class StatementNode
    {
        public abstract void Execute();
    }

    public class VariableReferenceNode : ExpressionNode
{
    public string Identifier { get; }

    public VariableReferenceNode(string identifier)
    {
        Identifier = identifier;
    }

    public override object Evaluate()
    {
        if (VariableScope.ContainsVariable(Identifier))
        {
            return VariableScope.GetVariableValue(Identifier);
        }
        else
        {
            throw new Exception($"Semantic error: Variable '{Identifier}' does not exist.");
        }
    }
}


    public class NumberLiteralNode : ExpressionNode
    {
        public double Value { get; }

        public NumberLiteralNode(double value)
        {
            Value = value;
        }

        public override object Evaluate()
        {
            return Value;
        }
    }

    public class StringLiteralNode : ExpressionNode
    {
        public string Value { get; }

        public StringLiteralNode(string value)
        {
            Value = value;
        }

        public override object Evaluate()
        {
            return Value;
        }
    }

    public class ParenthesizedExpressionNode : ExpressionNode
    {
        public ExpressionNode Expression { get; }

        public ParenthesizedExpressionNode(ExpressionNode expression)
        {
            Expression = expression;
        }

        public override object Evaluate()
        {
            return Expression.Evaluate();
        }
    }

    public class PrintStatementNode : ExpressionNode
{
    public ExpressionNode Expression { get; }

    public PrintStatementNode(ExpressionNode expression)
    {
        Expression = expression;
    }

    public override object Evaluate()
    {
        var result = Expression.Evaluate();
        Console.WriteLine(result);
        return null; //
    }
}

    public class VariableDeclarationNode : ExpressionNode
{
    public string Identifier { get; }
    public ExpressionNode Expression { get; }

    public VariableDeclarationNode(string identifier, ExpressionNode expression)
    {
        Identifier = identifier;
        Expression = expression;
    }

    public override object Evaluate()
    {
        // Obtener el valor de la expresión
        var value = Expression.Evaluate();

        // Declarar la variable y asignarle el valor
        VariableScope.AddVariable(Identifier, value);

        return null; // 
    }
}


    public class AssignmentStatementNode : StatementNode
{
    public string Identifier { get; }
    public ExpressionNode Expression { get; }

    public AssignmentStatementNode(string identifier, ExpressionNode expression)
    {
        Identifier = identifier;
        Expression = expression;
    }

    public override void Execute()
    {
        // Obtener el valor de la expresión
        var value = Expression.Evaluate();

        // Asignar el valor a la variable existente
        if (VariableScope.ContainsVariable(Identifier))
        {
            VariableScope.AssignVariable(Identifier, value);
        }
        else
        {
            throw new Exception($"Semantic error: Variable '{Identifier}' does not exist.");
        }
    }
}


    public class IfStatementNode : ExpressionNode
{
    public ExpressionNode Condition { get; }
    public ExpressionNode IfBody { get; }
    public ExpressionNode ElseBody { get; }

    public IfStatementNode(ExpressionNode condition, ExpressionNode ifBody, ExpressionNode elseBody)
    {
        Condition = condition;
        IfBody = ifBody;
        ElseBody = elseBody;
    }

    public override object Evaluate()
    {
        var conditionValue = Condition.Evaluate();
        if (Convert.ToBoolean(conditionValue))
        {
            return IfBody.Evaluate();
        }
        else
        {
            return ElseBody.Evaluate();
        }
    }
}



public class FunctionDeclarationNode : ExpressionNode
{
    public string Identifier { get; }
    public List<string> Parameters { get; }
    public ExpressionNode Body { get; }

    public FunctionDeclarationNode(string identifier, List<string> parameters, ExpressionNode body)
    {
        Identifier = identifier;
        Parameters = parameters;
        Body = body;
    }

    public override object Evaluate()
{
    var function = new FunctionDefinition(Parameters, Body);
    FunctionScope.AddFunction(Identifier, function);
    return Identifier; // Devuelve el nombre de la función declarada
}
}



public class FunctionDefinition
{
    public List<string> Parameters { get; }
    public StatementNode Body { get; }

    public FunctionDefinition(List<string> parameters, StatementNode body)
    {
        Parameters = parameters;
        Body = body;
    }

    public object Invoke(List<ExpressionNode> arguments)
    {
        if (arguments.Count != Parameters.Count)
        {
            throw new Exception($"Semantic error: Function '{Parameters[0]}' receives {Parameters.Count} argument(s), but {arguments.Count} were given.");
        }

        // Establece los valores de los argumentos en el ámbito de variables
        for (int i = 0; i < Parameters.Count; i++)
        {
            string parameter = Parameters[i];
            ExpressionNode argument = arguments[i];
            var value = argument.Evaluate();
            VariableScope.AddVariable(parameter, value);
        }

        // Ejecuta el cuerpo de la función
        Body.Execute();

        // Obtiene el valor de retorno de la función
        var returnValue = VariableScope.GetVariableValue(FunctionScope.ReturnVariable);

        // Limpia las variables del ámbito de variables
        VariableScope.ClearVariables();

        return returnValue;
    }
}

public static class FunctionScope
{
    private static Dictionary<string, FunctionDefinition> functions = new Dictionary<string, FunctionDefinition>();
    public static string ReturnVariable { get; } = "__return__";

    public static void AddFunction(string identifier, FunctionDefinition function)
    {
        functions.Add(identifier, function);
    }

    public static FunctionDefinition GetFunction(string identifier)
    {
        if (functions.ContainsKey(identifier))
        {
            return functions[identifier];
        }
        else
        {
            throw new Exception($"Semantic error: Function '{identifier}' does not exist.");
        }
    }

    public static bool ContainsFunction(string identifier)
    {
        return functions.ContainsKey(identifier);
    }
}



    public static class VariableScope
{
    private static Dictionary<string, object> variables = new Dictionary<string, object>();

    public static void AddVariable(string identifier, object value)
    {
        variables.Add(identifier, value);
    }

    public static void AssignVariable(string identifier, object value)
    {
        variables[identifier] = value;
    }

    public static void ClearVariables()
{
    variables.Clear();
}

    public static object GetVariableValue(string identifier)
    {
        if (variables.ContainsKey(identifier))
        {
            return variables[identifier];
        }
        else
        {
            throw new Exception($"Semantic error: Variable '{identifier}' does not exist.");
        }
    }

    public static bool ContainsVariable(string identifier)
    {
        return variables.ContainsKey(identifier);
    }
}


public class FunctionCallNode : ExpressionNode
{
    public string FunctionName { get; }
    public ExpressionNode Argument { get; }

    public FunctionCallNode(string functionName, ExpressionNode argument)
    {
        FunctionName = functionName;
        Argument = argument;
    }

    public override object Evaluate()
    {
        if (FunctionName == "sin")
        {
            double argValue = Convert.ToDouble(Argument.Evaluate());
            return Math.Sin(argValue);
        }
        else if (FunctionName == "cos")
        {
            double argValue = Convert.ToDouble(Argument.Evaluate());
            return Math.Cos(argValue);
        }
        else if (FunctionName == "factorial")
        {
            int argValue = Convert.ToInt32(Argument.Evaluate());
            return Factorial(argValue);
        }
        else if (FunctionName == "+" || FunctionName == "-" || FunctionName == "*" || FunctionName == "/")
        {
            ExpressionNode left = new NumberLiteralNode(0); // Placeholder value
            ExpressionNode right = Argument;
            return new BinaryExpressionNode(left, FunctionName, right).Evaluate();
        }
        else
        {
            throw new Exception($"Unknown function '{FunctionName}'");
        }
    }

    private int Factorial(int n)
    {
        if (n == 0)
            return 1;
        else
            return n * Factorial(n - 1);
    }
}







public class BinaryExpressionNode : ExpressionNode
{
    public ExpressionNode LeftOperand { get; }
    public string Operator { get; }
    public ExpressionNode RightOperand { get; }

    public BinaryExpressionNode(ExpressionNode leftOperand, string @operator, ExpressionNode rightOperand)
    {
        LeftOperand = leftOperand;
        Operator = @operator;
        RightOperand = rightOperand;
    }


    public override object Evaluate()
    {
        var leftValue = LeftOperand.Evaluate();
        var rightValue = RightOperand.Evaluate();

        switch (Operator)
        {
            case "+":
                return Add(leftValue, rightValue);
            case "-":
                return Subtract(leftValue, rightValue);
            case "*":
                return Multiply(leftValue, rightValue);
            case "/":
                return Divide(leftValue, rightValue);
            case "sin":
                return Sin(leftValue);
            case "cos":
                return Cos(leftValue);
            case "!":
                return Factorial(leftValue);
            default:
                throw new InvalidOperationException($"Unsupported symbolic operator: {Operator}");
        }
    }

//     private object PerformSymbolicOperation(object leftValue, object rightValue)
// {
//     // Realizar las operaciones según el operador
//     switch (Operator)
//     {
//         case TokenType.Symbol when Operator.Lexeme == "+":
//             return Add(leftValue, rightValue);
//         case TokenType.Symbol when Operator.Lexeme == "-":
//             return Subtract(leftValue, rightValue);
//         case TokenType.Symbol when Operator.Lexeme == "*":
//             return Multiply(leftValue, rightValue);
//         case TokenType.Symbol when Operator.Lexeme == "/":
//             return Divide(leftValue, rightValue);
//         case TokenType.Symbol when Operator.Lexeme == "sin":
//             return Sin(leftValue);
//         case TokenType.Symbol when Operator.Lexeme == "cos":
//             return Cos(leftValue);
//         case TokenType.Symbol when Operator.Lexeme == "!":
//             return Factorial(leftValue);
//         default:
//             throw new InvalidOperationException($"Unsupported symbolic operator: {Operator}");
//     }
// }


private object Add(object leftValue, object rightValue)
    {
        if (leftValue is double left && rightValue is double right)
            return left + right;

        if (leftValue is string leftStr && rightValue is string rightStr)
            return leftStr + rightStr;

        throw new InvalidOperationException($"Invalid operands for addition: {leftValue}, {rightValue}");
    }

    private object Subtract(object leftValue, object rightValue)
    {
        if (leftValue is double left && rightValue is double right)
            return left - right;

        throw new InvalidOperationException($"Invalid operands for subtraction: {leftValue}, {rightValue}");
    }

    private object Multiply(object leftValue, object rightValue)
    {
        if (leftValue is double left && rightValue is double right)
            return left * right;

        throw new InvalidOperationException($"Invalid operands for multiplication: {leftValue}, {rightValue}");
    }

    private object Divide(object leftValue, object rightValue)
    {
        if (leftValue is double left && rightValue is double right)
        {
            if (right == 0)
                throw new InvalidOperationException("Division by zero is not allowed");

            return left / right;
        }

        throw new InvalidOperationException($"Invalid operands for division: {leftValue}, {rightValue}");
    }

    private object Sin(object operandValue)
    {
        if (operandValue is double operand)
            return Math.Sin(operand);

        throw new InvalidOperationException($"Invalid operand for sine operation: {operandValue}");
    }

    private object Cos(object operandValue)
    {
        if (operandValue is double operand)
            return Math.Cos(operand);

        throw new InvalidOperationException($"Invalid operand for cosine operation: {operandValue}");
    }

    private object Factorial(object operandValue)
    {
        if (operandValue is double operand && operand >= 0)
        {
            double result = 1;
            for (double i = 2; i <= operand; i++)
            {
                result *= i;
            }
            return result;
        }

        throw new InvalidOperationException($"Invalid operand for factorial operation: {operandValue}");
    }
}



}
