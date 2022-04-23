using System.Collections.Generic;
using System.ComponentModel;

public class FunctionBlock : IStatement, IExpression
{
    public enum Functions
    {
        GETINFO = 2
    }

    public FunctionBlock() {}
    
    public FunctionBlock(Functions function)
    {
        this.function = function;
    }

    private Functions function = Functions.GETINFO;
    private List<IExpression> arguments = new List<IExpression>();

    public Functions Function
    {
        get => function;
        set => function = value;
    }

    public List<IExpression> Arguments
    {
        get => arguments;
        set => arguments = value;
    }

    public void AddArgument(IExpression arg)
    {
        arguments.Add(arg);
    }

    public string toString()
    {
        string str = "";
        foreach (var arg in arguments)
        {
            str += arg == null ? "" : arg.toString() + ", ";
        }

        return function.ToString().ToLower() + "(" + (str.Length > 0 ? str.Substring(0, str.Length - 2): "" )+ ")";
    }

    public string insertTabs(string str)
    {
        return str;
    }
}