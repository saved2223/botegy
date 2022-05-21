using System;
using System.Collections.Generic;
using System.ComponentModel;

public class FunctionBlock : IStatement, IExpression
{
    public enum Functions
    {
        [ArgumentsNum(3)]
        PUT,
        [ArgumentsNum(2)]
        GET_UNITE,
        [ArgumentsNum(2)]
        GET_UNITE_HP,
        [ArgumentsNum(0)]
        GET_OPPONENT_HP,
        [ArgumentsNum(0)]
        GET_SELF_HP,
        [ArgumentsNum(2)]
        IS_EMPTY,
        [ArgumentsNum(0)]
        GET_STEP_NUMBER
    }

    
    [AttributeUsage(AttributeTargets.Field)]
    public class ArgumentsNumAttribute : Attribute
    {
        public int ArgumentsNumValue
        {
            get; 
            private set;
        }

        public ArgumentsNumAttribute(int argumentsNumValue)
        {
            ArgumentsNumValue = argumentsNumValue;
        }
    }

    public FunctionBlock() {}
    
    public FunctionBlock(Functions function)
    {
        this.function = function;
    }

    private Functions function = Functions.GET_SELF_HP;
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