public class BracesBlock : IExpression
{
    private IExpression _expressionBlock = null;

    public BracesBlock(IExpression expressionBlock)
    {
        _expressionBlock = expressionBlock;
    }
    
    public BracesBlock(){}
    
    public string toString()
    {
        return "(" + ( _expressionBlock == null ? "" : _expressionBlock.toString() )+ ")";
    }

    public IExpression ExpressionBlock
    {
        get => _expressionBlock;
        set => _expressionBlock = value;
    }
}