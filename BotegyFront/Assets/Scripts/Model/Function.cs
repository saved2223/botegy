using System;

namespace Model
{
    public enum Functions
    {
        [ArgumentsNum(3)] PUT,
        [ArgumentsNum(2)] GET_UNITE,
        [ArgumentsNum(2)] GET_UNITE_HP,
        [ArgumentsNum(0)] GET_OPPONENT_HP,
        [ArgumentsNum(0)] GET_SELF_HP,
        [ArgumentsNum(2)] IS_EMPTY,
        [ArgumentsNum(0)] GET_STEP_NUMBER
    }

    public static class FunctionsExtensions
    {
        public static int GetArgumentsNum(this Functions @functions)
        {
            ArgumentsNumAttribute[] attributes = (ArgumentsNumAttribute[]) @functions
                .GetType()
                .GetField(@functions.ToString())
                .GetCustomAttributes(typeof(ArgumentsNumAttribute), false);
            return attributes.Length > 0 ? attributes[0].ArgumentsNumValue : -1;
        }
    }


    [AttributeUsage(AttributeTargets.Field)]
    public class ArgumentsNumAttribute : Attribute
    {
        public int ArgumentsNumValue { get; private set; }

        public ArgumentsNumAttribute(int argumentsNumValue)
        {
            ArgumentsNumValue = argumentsNumValue;
        }
    }
}