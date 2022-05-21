using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class AssignBlock : IStatement
    {
        private VariableBlock _variableBlock = new VariableBlock();
        private IExpression _expressionBlock = null;

        public AssignBlock(VariableBlock variableBlock, IExpression expressionBlock)
        {
            _variableBlock = variableBlock;
            _expressionBlock = expressionBlock;
        }

        public AssignBlock()
        {
        }

        public string GetString(Dictionary<string, VariableBlock> b)
        {
            string str = "";
            if (!b.ContainsKey(_variableBlock.Name))
            {
                b.Add(_variableBlock.Name, _variableBlock);
                str += "let ";
            }

            return str + (_variableBlock != null ? _variableBlock.GetString(b) : "")
                       + " = " + (_expressionBlock != null ? _expressionBlock.GetString(b) : "") + ";";
        }

        public VariableBlock VariableBlock
        {
            get => _variableBlock;
            set => _variableBlock = value;
        }

        public IExpression ExpressionBlock
        {
            get => _expressionBlock;
            set => _expressionBlock = value;
        }
    }
}