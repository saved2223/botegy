using System;
using System.ComponentModel;

public enum BinaryOperator
{
    [Description("+")]
    ADD,
    [Description("-")]
    SUB,
    [Description("*")]
    MUL,
    [Description("/")]
    DIV,
    [Description("%")]
    MOD,
    [Description("==")]
    EQ,
    [Description("!=")]
    NEQ,
    [Description(">")]
    GR,
    [Description("<")]
    LS,
    [Description(">=")]
    GE,
    [Description("<=")]
    LE,
    [Description("and")]
    AND,
    [Description("or")]
    OR,
    [Description("in")]
    IN
}

public static class BinaryOperatorExtensions
{
    public static string GetOperatorText(this BinaryOperator @operator)
    {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])@operator
            .GetType()
            .GetField(@operator.ToString())
            .GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }
    
    public static BinaryOperator GetValueFromDescription(string description)
    {
        foreach (var field in typeof(BinaryOperator).GetFields())
        {
            DescriptionAttribute attribute
                = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))as DescriptionAttribute;
            if(attribute == null)
                continue;
            if(attribute.Description == description)
            {
                return (BinaryOperator) field.GetValue(null);
            }
        }
        return 0;
    }
}