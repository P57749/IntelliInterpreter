public class SemanticAnalyzer
{
    private readonly ASTNode root;

    public SemanticAnalyzer(ASTNode root)
    {
        this.root = root;
    }
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
    // Guardar la información de la variable en una tabla de símbolos o un entorno de variables
    // Puedes utilizar un diccionario, una lista o cualquier otra estructura de datos adecuada para almacenar las variables y sus valores
}

private void AnalyzeAssignmentStatement(AssignmentStatementNode assignmentStatementNode)
{
    string variableName = assignmentStatementNode.VariableName;

    // Verificar si la variable está definida
    if (!IsVariableDefined(variableName))
    {
        throw new Exception($"Semantic error: Variable '{variableName}' is not defined.");
    }
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
    // Implementa la lógica para analizar una declaración de función en línea en una declaración de función.
    // Verifica si la función está correctamente definida y si es válida según las reglas de HULK.
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
    // Implementa la lógica para verificar si una variable está definida en el entorno de variables
    // Puedes utilizar una tabla de símbolos, un entorno de variables o cualquier otra estructura de datos donde almacenes las variables definidas

    // Retorna true si la variable está definida, de lo contrario retorna false
}

