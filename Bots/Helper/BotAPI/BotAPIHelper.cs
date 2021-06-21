using Microsoft.BotBuilderSamples;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public CreateResponseBody CreateApiPostCall(RequestBody Body)
        {
            ErrorDTO pageError = new ErrorDTO();
            Tuple<HttpWebRequest, ErrorDTO> ReqVar;
            CreateResponseBody responseBody = null;
            try
            {
                ReqVar = BotAPIService.ManageConfigurationForAPIPostCall(Body);
                responseBody = GetResponseFromApi(ReqVar.Item1);
            }
            catch (Exception exception)
            {
                //handle error
            }
            return responseBody;

        }
        public CreateResponseBody GetResponseFromApi(WebRequest request)
        {
            bool ApiUnavailable=true;
            string response = string.Empty;
            ErrorDTO pageError = new ErrorDTO();
            var Error = "test123";
            Tuple<ErrorDTO, string> responseFromAPI = new Tuple<ErrorDTO, string>(pageError, response);
            responseFromAPI = BotAPIService.GetResponse(request);

            CreateResponseBody responseBody = null;
            if (!string.IsNullOrEmpty(responseFromAPI.Item2))
            {
                BotAPIHelper botAPIHelper = new BotAPIHelper();
                var responseAfterParse = botAPIHelper.ParseCreateResponse(responseFromAPI.Item2);
                if (responseAfterParse.Body != null)
                {
                    responseBody = responseAfterParse.Body;
                    ApiUnavailable = false;
                }
            }
            return responseBody;
        }
    }
}
