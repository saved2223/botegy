using System.Collections.Generic;

public class ArrayAccessBlock : IExpression
{
    private string name = "arr_1";
    private List<IExpression> indices;

    public string Name
    {
        get => name;
        set => name = value;
    }

    public List<IExpression> Indices
    {
        get => indices;
        set => indices = value;
    }

    public string toString()
    {
        return "";
    }
}