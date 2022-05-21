using System.Collections.Generic;

namespace Model
{
    public class BracesBlock : IExpression
    {
        private IExpression _expressionBlock = null;

        public BracesBlock(IExpression expressionBlock)
        {
            _expressionBlock = expressionBlock;
        }

        public BracesBlock()
        {
        }

        public string GetString(Dictionary<string, VariableBlock> b)
        {
            return "(" + (_expressionBlock == null ? "" : _expressionBlock.GetString(b)) + ")";
        }

        public IExpression ExpressionBlock
        {
            get => _expressionBlock;
            set => _expressionBlock = value;
        }
    }
}