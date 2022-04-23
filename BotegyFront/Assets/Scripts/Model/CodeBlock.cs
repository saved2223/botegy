public abstract class CodeBlock : ICode
{
    public abstract string toString();
    
    public string insertTabs(string str)
    {
        if (str.Substring(str.Length - 1) == "\n")
        {
            str = str.Substring(0, str.Length - 1);
        }
        
        string[] subs = str.Split('\n');

        string newString = "";
        
        foreach (var sub in subs)
        {
            newString += "\t" + sub + "\n";
        }

        newString = newString.Substring(0, newString.Length - 1);
        
        return newString;
    }
}