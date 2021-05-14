using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptiveCardsBot.Bots.Helper.BotAPI
{
    public class Data
    {
        public string data { get; set; }
        public string type { get; set; }
        public bool? linear { get; set; }
        public bool? loop { get; set; }
        public List<string> text { get; set; }
        public Cognigy _cognigy { get; set; }
        public Data _data { get; set; }
    }
}
