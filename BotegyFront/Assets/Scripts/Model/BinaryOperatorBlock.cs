public class BinaryOperatorBlock : IExpression
{
    private BinaryOperator _operator = BinaryOperator.ADD;
    private IExpression _expr1 = null;
    private IExpression _expr2 = null;

    public BinaryOperatorBlock(BinaryOperator @operator, IExpression expr1, IExpression expr2)
    {
        _operator = @operator;
        _expr1 = expr1;
        _expr2 = expr2;
    }
    
    public BinaryOperatorBlock(){}
    public string toString()
    {
        return (_expr1 == null ? "" : _expr1.toString()) + " " + _operator.GetOperatorText() + " " + (_expr2 == null ? "" : _expr2.toString());
    }

    public BinaryOperator Operator
    {
        get => _operator;
        set => _operator = value;
    }

    public IExpression Expr1
    {
        get => _expr1;
        set => _expr1 = value;
    }

    public IExpression Expr2
    {
        get => _expr2;
        set => _expr2 = value;
    }
}