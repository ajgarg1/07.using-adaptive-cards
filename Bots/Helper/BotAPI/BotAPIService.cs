using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AdaptiveCardsBot.Bots.Helper.BotAPI
{
    public static class BotAPIService
    {
        public static Tuple<HttpWebRequest, ErrorDTO> ManageConfigurationForAPIPostCall(RequestBody Body)
        {
            HttpWebRequest request = null;
            ErrorDTO pageError = null;
            try
            {
                string oceaneWebApiUrl = "https://endpoint-trial.cognigy.ai/53db5906f63a38927aa4c6bed1fb763d90557198c140d86ba10e999597952d44";
                Uri requestUri = new Uri(oceaneWebApiUrl);
                request = WebRequest.Create(requestUri) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                CredentialCache myCredentials = new CredentialCache();
                NetworkCredential netCred = new NetworkCredential();
                myCredentials.Add(requestUri, "Basic", netCred);
                request.Credentials = myCredentials;
                request.PreAuthenticate = true;
                //handling of json body for post api call to Oceane
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(Body);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                //string jsonData = string.Empty;
                //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                //{
                //    jsonData = ApiStreamReaderForJsonBody(response.GetResponseStream());

                //}
            }
            catch (Exception exception)
            {
                var faultMessage = "some error in the code";
                pageError = SetPageError(faultMessage, "test123");

            }
            return new Tuple<HttpWebRequest, ErrorDTO>(request, pageError);

        }
        public static ErrorDTO SetPageError(string message, string errorCode)
        {
            ErrorDTO pageError = new ErrorDTO();
            pageError.Message = message;
            pageError.HelpId = errorCode;
            pageError.ErrorCode = errorCode;
            return pageError;
        }
        public static Tuple<ErrorDTO, string> GetResponse(WebRequest req)
        {
            string jsonData = string.Empty;
            ErrorDTO pageError = null;
            if (req != null)
            {
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                    {
                        jsonData = ApiStreamReaderForJsonBody(response.GetResponseStream());

                    }
                }
                catch (WebException exception)
                {
                    using (WebResponse webResponse = exception.Response)
                    {
                        HttpWebResponse httpWebResponse = (HttpWebResponse)webResponse;

                        jsonData = ApiStreamReaderForJsonBody(httpWebResponse.GetResponseStream());
                    }

                }
            }
            return new Tuple<ErrorDTO, string>(pageError, jsonData);
        }
        public static string ApiStreamReaderForJsonBody(Stream getResponseStream)
        {
            string jsonData = string.Empty;

            using (StreamReader sr = new StreamReader(getResponseStream))
            {
                jsonData = sr.ReadToEnd();
            }

            return jsonData;

        }
        public static RequestBody CreateBody()
        {
            var requestBody = new RequestBody
            {
                UserID = Guid.NewGuid().ToString(),//1234, 123 etc            
                SessionID = "someUniqueId",
                //Body.Text = text;
                Data = null
            };
            return requestBody;
        }
    }

}
