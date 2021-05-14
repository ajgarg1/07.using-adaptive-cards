using Microsoft.BotBuilderSamples;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptiveCardsBot.Bots.Helper.BotAPI
{
    public class BotAPIHelper
    {
        public CreateResponse ParseCreateResponse(string response)
        {
            var CreateResponse = new CreateResponse
            {
                Body = ParseCreateResponseBody(response)
            };
            return CreateResponse;
        }
        public CreateResponseBody ParseCreateResponseBody(string requestBody)
        {
            return ConvertJson<CreateResponseBody>(requestBody);
        }
        public T ConvertJson<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
