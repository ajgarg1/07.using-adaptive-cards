using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptiveCardsBot.Bots.Helper
{
    //public class RequestBody
    //{
    //}
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 

    public class RequestBody
    {
        [JsonProperty(PropertyName = "userId")]
        public string UserID { get; set; }
        public string sessionId { get; set; }
        public string text { get; set; }
        public Data data { get; set; }
    }
    public class Data
    {
        public string key { get; set; }
    }


}
