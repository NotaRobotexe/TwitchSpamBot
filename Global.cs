using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchSpamBot
{
    class Global
    {
        public static string twitch = "https://www.twitch.tv/directory/game/";
        public static string twitch_ = "https://www.twitch.tv/";
    }

    struct Targets
    {
        public string name;
        public int Views;
    }

    struct Ledger
    {
        public string Name;
        public DateTime? AcessTime;
        public int viewers;
        public bool visited;
     }
}
