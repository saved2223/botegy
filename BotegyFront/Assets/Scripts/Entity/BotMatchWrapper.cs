using System;
using System.Collections.Generic;

namespace Entity
{
    [Serializable]
    class BotMatchWrapper
    {
        public Bot bot;
        public List<Match> matches;
    }
}