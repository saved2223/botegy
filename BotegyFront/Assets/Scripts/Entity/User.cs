using System;

namespace Entity
{
    [Serializable]
    public class User
    {
        public string id;
        public string nickname;
        public string email;
        public int isModer;
        public int isGoogle;
    
        private bool loggedIn = false;
    
        public bool LoggedIn
        {
            get => loggedIn;
            set => loggedIn = value;
        }
    }
}
