using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class ValueBlock : IExpression
    {
        private string _value = "0";
        private ValueType _valueType = ValueType.INT;

        public ValueBlock(string value, ValueType valueType)
        {
            _value = value;
            _valueType = valueType;
        }

        public ValueBlock()
        {
        }

        public string GetString(Dictionary<string, VariableBlock> b)
        {
            if (_valueType == ValueType.UNITE)
                return "'" + _value + "'";

            return _value;
        }

        public string Value
        {
            get => _value;
            set => _value = value;
        }


        public ValueType Type
        {
            get => _valueType;
            set => _valueType = value;
        }
    }
}