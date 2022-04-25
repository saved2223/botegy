public class GlobalVariableBlock : IExpression
{
    public enum Globals
    {
        PLAYER1,
        PLAYER2,
        FIELD
    }

    private Globals _globalVariable = Globals.FIELD;

    public Globals GlobalVariable
    {
        get => _globalVariable;
        set => _globalVariable = value;
    }

    public string toString()
    {
        return _globalVariable.ToString();
    }
}