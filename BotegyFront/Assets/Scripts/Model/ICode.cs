using System.Collections.Generic;

namespace Model
{
    public interface ICode
    {
        string GetString(Dictionary<string, VariableBlock> b);

    }
}
