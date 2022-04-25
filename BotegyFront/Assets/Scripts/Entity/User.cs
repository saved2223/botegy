using System;

[Serializable]
public class User
{
    public string id;
    public string nickname;
    public string email;
    public int isModer;
    
    private bool loggedIn = false;
    
    public bool LoggedIn
    {
        get => loggedIn;
        set => loggedIn = value;
    }
}
