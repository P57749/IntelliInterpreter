using System;

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

    public class PrintStatementNode : StatementNode
    {
        public ExpressionNode Expression { get; }

        public PrintStatementNode(ExpressionNode expression)
        {
            Expression = expression;
        }

        public override void Execute()
        {
            var result = Expression.Evaluate();
            Console.WriteLine(result);
        }
    }

    public class VariableDeclarationNode : StatementNode
{
    public string Identifier { get; }
    public ExpressionNode Expression { get; }

    public VariableDeclarationNode(string identifier, ExpressionNode expression)
    {
        Identifier = identifier;
        Expression = expression;
    }

    public override void Execute()
    {
        // Obtener el valor de la expresión
        var value = Expression.Evaluate();

        // Declarar la variable y asignarle el valor
        VariableScope.AddVariable(Identifier, value);
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


    public class IfStatementNode : StatementNode
    {
        public ExpressionNode Condition { get; }
        public StatementNode IfBody { get; }
        public StatementNode ElseBody { get; }

        public IfStatementNode(ExpressionNode condition, StatementNode ifBody, StatementNode elseBody)
        {
            Condition = condition;
            IfBody = ifBody;
            ElseBody = elseBody;
        }

        public override void Execute()
        {
            var conditionValue = Condition.Evaluate();
            if (Convert.ToBoolean(conditionValue))
            {
                IfBody.Execute();
            }
            else
            {
                ElseBody.Execute();
            }
        }
    }



public class FunctionDeclarationNode : StatementNode
    {
        public string Identifier { get; }
        public List<string> Parameters { get; }
        public StatementNode Body { get; }

        public FunctionDeclarationNode(string identifier, List<string> parameters, StatementNode body)
        {
            Identifier = identifier;
            Parameters = parameters;
            Body = body;
        }

        public override void Execute()
{
    var function = new FunctionDefinition(Parameters, Body);
    FunctionScope.AddFunction(Identifier, function);
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

}
