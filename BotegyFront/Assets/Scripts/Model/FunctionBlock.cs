using System.Collections.Generic;

namespace Model
{
    public class FunctionBlock : IStatement, IExpression
    {
        public FunctionBlock()
        {
        }

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

        public string GetString(Dictionary<string, VariableBlock> b)
        {
            string str = "";
            foreach (var arg in arguments)
            {
                str += arg == null ? "" : arg.GetString(b) + ", ";
            }

            return "this.field." + function.ToString().ToLower() + "(" +
                   (str.Length > 0 ? str.Substring(0, str.Length - 2) : "") + ")";
        }
    }
}