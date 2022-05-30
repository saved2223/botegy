using System;

namespace Entity
{
    [Serializable]
    public class Bot
    {
        public string id;
        public string name;
        public string code;
        public User player;
    }
}