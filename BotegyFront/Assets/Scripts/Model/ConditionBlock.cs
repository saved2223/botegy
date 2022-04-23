using System.Collections.Generic;

public class ConditionBlock : IStatement
{
    private IExpression _cond = null;
    private List<IExpression> _conditions = new List<IExpression>();
    private List<List<ICode>> _scopes = new List<List<ICode>>();
    private List<ICode> _elseScope = new List<ICode>();
    
    private List<ICode> _code = new List<ICode>();

    public ConditionBlock(IExpression cond, ICode code)
    {
        _cond = cond;
        _code.Add(code);
    }
    
    public ConditionBlock(IExpression cond, List<ICode> code)
    {
        _cond = cond;
        _code = code;
    }
    
    public ConditionBlock(){}
    public string toString()
    {
        return "if " +( _cond == null ? "" : _cond.toString() )+ ":\n" +  insertTabs(listToString());
    }
    
    public void addCodeBlock(ICode code)
    {
        _code.Add(code);
    }

    private string listToString()
    {
        string str = "";

        foreach (var i in this._code)
        {
            str += i.toString() + "\n";
        }
        
        if (str.Length > 0)
            str = str.Substring(0, str.Length - 1);
        
        return str;
    }

    public IExpression Cond
    {
        get => _cond;
        set => _cond = value;
    }

    public List<ICode> Code
    {
        get => _code;
        set => _code = value;
    }

    public List<IExpression> Conditions
    {
        get => _conditions;
        set => _conditions = value;
    }

    public List<List<ICode>> Scopes
    {
        get => _scopes;
        set => _scopes = value;
    }

    public List<ICode> ElseScope
    {
        get => _elseScope;
        set => _elseScope = value;
    }

    public string insertTabs(string str)
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