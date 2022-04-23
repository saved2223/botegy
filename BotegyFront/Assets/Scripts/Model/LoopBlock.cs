using System;
using System.Collections.Generic;

public class LoopBlock : IStatement
{
    public enum LoopType
    {
        WHILE,
        FOR
    }
    
    private IExpression _cond = null;
    private List<ICode> _code = new List<ICode>();
    private LoopType _type = LoopType.FOR; 
    
    public LoopBlock(IExpression cond, ICode code, LoopType type)
    {
        _cond = cond;
        _code.Add(code);
        _type = type;
    }

    public LoopBlock(IExpression cond, List<ICode> code, LoopType type)
    {
        _cond = cond;
        _code = code;
        _type = type;
    }

    public LoopBlock()
    {
        
    }
    public string toString()
    {
        return _type.ToString().ToLower() + " " + ( _cond == null ? "" : _cond.toString() ) + ":\n" +  insertTabs( listToString());
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

    public LoopType Type
    {
        get => _type;
        set => _type = value;
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