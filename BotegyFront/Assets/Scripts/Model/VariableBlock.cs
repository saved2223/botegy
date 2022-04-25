public class VariableBlock : IExpression
{
    private string _name = "new_var";

    public VariableBlock(string name)
    {
        _name = name;
    }
    
    public VariableBlock() {}

    public string toString()
    {
        return _name;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }
}