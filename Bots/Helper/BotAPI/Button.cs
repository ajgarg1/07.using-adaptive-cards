using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptiveCardsBot.Bots.Helper.BotAPI
{
    public class Button
    {
        public double id { get; set; }
        public string payload { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string intentName { get; set; }
    }
}
