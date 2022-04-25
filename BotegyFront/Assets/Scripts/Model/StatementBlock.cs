public class StatementBlock : IStatement
{
    private VariableBlock _variableBlock = new VariableBlock();
    private IExpression _expressionBlock = null;

    public StatementBlock(VariableBlock variableBlock, IExpression expressionBlock)
    {
        _variableBlock = variableBlock;
        _expressionBlock = expressionBlock;
    }
    
    public StatementBlock (){}

    public string toString()
    {
        return (_variableBlock != null ? _variableBlock.toString() : "") 
            + " = " + (_expressionBlock != null ? _expressionBlock.toString() : "");
    }

    public VariableBlock VariableBlock
    {
        get => _variableBlock;
        set => _variableBlock = value;
    }

    public IExpression ExpressionBlock
    {
        get => _expressionBlock;
        set => _expressionBlock = value;
    }
    
    public string insertTabs(string str)
    {
        return str;
    }
}