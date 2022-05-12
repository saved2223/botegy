using Model;

public class ValueBlock : IExpression
{
    private string _value = "";
    private ValueType _type = ValueType.INT;

    public ValueBlock(string value, ValueType type)
    {
        _value = value;
        _type = type;
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
    
    
    public ValueType Type
    {
        get => _type;
        set => _type = value;
    }
}