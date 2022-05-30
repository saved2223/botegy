using System;

namespace Entity
{
    [Serializable]
    public class Match
    {
        public string id;
        public Bot bot1;
        public Bot bot2;
        public Bot winnerBot;
    }
}
