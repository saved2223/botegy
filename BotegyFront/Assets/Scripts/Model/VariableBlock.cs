using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class VariableBlock : IExpression
    {
        private string _name = "new_var";

        public VariableBlock(string name)
        {
            _name = name;
        }

        public VariableBlock()
        {
        }

        public string GetString(Dictionary<string, VariableBlock> b)
        {
            return _name;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }
    }
}