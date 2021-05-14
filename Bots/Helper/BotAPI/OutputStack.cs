using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptiveCardsBot.Bots.Helper.BotAPI
{
    public class OutputStack
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "data")]
        public Data Data { get; set; }
        [JsonProperty(PropertyName = "traceId")]
        public string TraceID { get; set; }

        [JsonProperty(PropertyName = "disableSensitiveLogging")]
        public bool DisableSensitiveLogging { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }
    }
}
