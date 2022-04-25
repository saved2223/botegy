public class ValueBlock : IExpression
{
    private string _value = "";

    public ValueBlock(string value)
    {
        _value = value;
    }

    public ValueBlock()
    { }

    public string toString()
    {
        return _value;
    }

    public string Value
    {
        get => _value;
        set => _value = value;
    }
}