using System;
using System.Collections.Generic;

namespace HulkInterpreter
{
    public class SemanticAnalyzer
    {
        private readonly ASTNode root;
        private Dictionary<string, bool> variableTable; // Tabla de variables
        private Dictionary<string, InlineFunctionDeclarationNode> functionTable; // Tabla de funciones en línea

        public SemanticAnalyzer(ASTNode root)
        {
            this.root = root;
            variableTable = new Dictionary<string, bool>();
            functionTable = new Dictionary<string, InlineFunctionDeclarationNode>();
        }

        public void Analyze()
        {
            AnalyzeNode(root);
        }

        private void AnalyzeNode(ASTNode node)
        {
            if (node is ExpressionNode expressionNode)
            {
                AnalyzeExpression(expressionNode);
            }
            else if (node is StatementNode statementNode)
            {
                AnalyzeStatement(statementNode);
            }
            else if (node is FunctionDeclarationNode functionDeclarationNode)
            {
                AnalyzeFunctionDeclaration(functionDeclarationNode);
            }
        }

        private void AnalyzeExpression(ExpressionNode expressionNode)
        {
            if (expressionNode is VariableReferenceNode variableReferenceNode)
            {
                AnalyzeVariableReference(variableReferenceNode);
            }
            else if (expressionNode is NumberLiteralNode numberLiteralNode)
            {
                // No se requieren verificaciones semánticas para los literales numéricos
            }
            else if (expressionNode is StringLiteralNode stringLiteralNode)
            {
                // No se requieren verificaciones semánticas para los literales de cadena
            }
            else if (expressionNode is ParenthesizedExpressionNode parenthesizedExpressionNode)
            {
                AnalyzeNode(parenthesizedExpressionNode.Expression);
            }
            else if (expressionNode is BinaryExpressionNode binaryExpressionNode)
            {
                AnalyzeBinaryExpression(binaryExpressionNode);
            }
            // ... Analizar otros tipos de expresiones
        }

        private void AnalyzeStatement(StatementNode statementNode)
        {
            if (statementNode is PrintStatementNode printStatementNode)
            {
                AnalyzeNode(printStatementNode.Expression);
            }
            else if (statementNode is VariableDeclarationNode variableDeclarationNode)
            {
                AnalyzeVariableDeclaration(variableDeclarationNode);
            }
            else if (statementNode is AssignmentStatementNode assignmentStatementNode)
            {
                AnalyzeAssignmentStatement(assignmentStatementNode);
            }
            else if (statementNode is IfStatementNode ifStatementNode)
            {
                AnalyzeIfStatement(ifStatementNode);
            }
            // ... Analizar otros tipos de instrucciones
        }

        private void AnalyzeFunctionDeclaration(FunctionDeclarationNode functionDeclarationNode)
        {
            if (functionDeclarationNode is InlineFunctionDeclarationNode inlineFunctionDeclarationNode)
            {
                AnalyzeInlineFunctionDeclaration(inlineFunctionDeclarationNode);
            }
            // ... Analizar otros tipos de declaraciones de funciones
        }

        private void AnalyzeVariableReference(VariableReferenceNode variableReferenceNode)
        {
            string variableName = variableReferenceNode.VariableName;

            // Verificar si la variable está definida
            if (!IsVariableDefined(variableName))
            {
                throw new Exception($"Semantic error: Variable '{variableName}' is not defined.");
            }
        }

        private void AnalyzeVariableDeclaration(VariableDeclarationNode variableDeclarationNode)
        {
            string variableName = variableDeclarationNode.VariableName;

            // Verificar si la variable ya está definida
            if (IsVariableDefined(variableName))
            {
                throw new Exception($"Semantic error: Variable '{variableName}' is already defined.");
            }

            // Realizar las asignaciones y verificaciones necesarias para mantener un entorno de variables
            variableTable.Add(variableName, false);
        }

        private void AnalyzeAssignmentStatement(AssignmentStatementNode assignmentStatementNode)
        {
            string variableName = assignmentStatementNode.VariableName;

            // Verificar si la variable está definida
            if (!IsVariableDefined(variableName))
            {
                throw new Exception($"Semantic error: Variable '{variableName}' is not defined.");
            }

            // Realizar las verificaciones semánticas necesarias para la asignación
        }

        private void AnalyzeIfStatement(IfStatementNode ifStatementNode)
        {
            // Verificar si la condición es una expresión booleana válida
            AnalyzeNode(ifStatementNode.Condition);

            // Analizar las ramas if y else
            AnalyzeNode(ifStatementNode.IfBody);
            AnalyzeNode(ifStatementNode.ElseBody);
        }

        private void AnalyzeInlineFunctionDeclaration(InlineFunctionDeclarationNode inlineFunctionDeclarationNode)
        {
            string functionName = inlineFunctionDeclarationNode.FunctionName;

            // Verificar si la función ya está definida
            if (IsFunctionDefined(functionName))
            {
                throw new Exception($"Semantic error: Function '{functionName}' is already defined.");
            }

            // Agregar la función a la tabla de funciones
            functionTable.Add(functionName, inlineFunctionDeclarationNode);

            // Realizar las verificaciones semánticas necesarias para la declaración de la función en línea
            AnalyzeNode(inlineFunctionDeclarationNode.Body);
        }

        private void AnalyzeBinaryExpression(BinaryExpressionNode binaryExpressionNode)
        {
            AnalyzeNode(binaryExpressionNode.LeftOperand);
            AnalyzeNode(binaryExpressionNode.RightOperand);

            // Verificar la compatibilidad de tipos y las operaciones válidas para la expresión binaria
            // Por ejemplo, si la operación es una suma, verificar que los operandos sean números o cadenas válidas para concatenar
            // Si la operación es una multiplicación, verificar que los operandos sean números
            // Realizar las comprobaciones semánticas necesarias según las reglas de HULK
        }

        private bool IsVariableDefined(string variableName)
        {
            return variableTable.ContainsKey(variableName);
        }

        private bool IsFunctionDefined(string functionName)
        {
            return functionTable.ContainsKey(functionName);
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

    public override double Evaluate()
    {
        // Aquí puedes implementar la evaluación de la llamada a la función
        // Puedes usar la propiedad FunctionName para determinar qué función se está llamando
        // Y la propiedad Argument para obtener el argumento de la función

        // Por ahora, solo devuelvo 0 como un valor de ejemplo
        return 0;
    }
}

}

