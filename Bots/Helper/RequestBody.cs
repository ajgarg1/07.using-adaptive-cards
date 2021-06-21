using Newtonsoft.Json;

namespace AdaptiveCardsBot.Bots.Helper
{
    public class RequestBody
    {
        [JsonProperty(PropertyName = "userId")]
        public string UserID { get; set; }

        [JsonProperty(PropertyName = "sessionId")]
        public string SessionID { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "data")]
        public Data Data { get; set; }
    }
    public class Data
    {
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
    }


}
