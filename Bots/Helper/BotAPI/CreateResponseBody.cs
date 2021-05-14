using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptiveCardsBot.Bots.Helper.BotAPI
{
    
    public class CreateResponseBody
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "data")]
        public Data Data { get; set; }

        [JsonProperty(PropertyName = "outputStack")]
        public List<OutputStack> OutputStack { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string UserID { get; set; }

        [JsonProperty(PropertyName = "sessionId")]
        public string SessionID { get; set; }
    }
}
