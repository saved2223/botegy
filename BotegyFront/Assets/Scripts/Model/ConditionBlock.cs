using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    [Serializable]
    public class ConditionBlock : IStatement
    {
        private IExpression _cond;
        private List<ICode> _scope = new List<ICode>();
        private List<ICode> _elseScope = new List<ICode>();

        public ConditionBlock()
        {
        }

        public string GetString(Dictionary<string, VariableBlock> b)
        {
            string str = "if(";

            str += _cond.GetString(b) + "){\n";

            str += InsertTabs(ListToString(_scope, b.ToDictionary(entry => entry.Key,
                entry => entry.Value)));
            str += "\n}";

            if (_elseScope.Count > 0)
            {
                str += "\nelse {\n    ";
                str += InsertTabs(ListToString(_elseScope, b.ToDictionary(entry => entry.Key,
                    entry => entry.Value)));
                str += "\n}";
            }

            return str;
        }

        private string ListToString(List<ICode> scope, Dictionary<string, VariableBlock> b)
        {
            string str = "";

            foreach (var i in scope)
            {
                string line = i.GetString(b);

                if (i.GetType() == typeof(FunctionBlock))
                    line += ";";

                str += line + "\n";
            }

            return str;
        }


        public IExpression Condition
        {
            get => _cond;
            set => _cond = value;
        }

        public List<ICode> Scope
        {
            get => _scope;
            set => _scope = value;
        }

        public List<ICode> ElseScope
        {
            get => _elseScope;
            set => _elseScope = value;
        }

        public string InsertTabs(string str)
        {
            if (str.Length > 0 && str.Substring(str.Length - 1) == "\n")
            {
                str = str.Substring(0, str.Length - 1);
            }

            string[] subs = str.Split('\n');

            string newString = "";

            foreach (var sub in subs)
            {
                newString += "\t" + sub + "\n";
            }

            if (newString.Length > 0)
                newString = newString.Substring(0, newString.Length - 1);

            return newString;
        }
    }
}