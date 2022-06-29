using Clientix.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace Clientix.REST
{
    public class BaseAPI
    {
        private const string BASE_URL = "https://klientiks.ru/clientix/Restapi/list/";

        private const int DEFAULT_PAGE_SIZE = 200;

        public string AccountID { get; set; }

        public string UserID { get; set; }

        public string AccessToken { get; set; }

        public int PageSize { get; set; }


        public BaseAPI()
        {
            PageSize = DEFAULT_PAGE_SIZE;
        }

        private void AssertEmpty(string value, string errorMessage)
        {
            if (value.Equals(string.Empty))
            {
                throw new ArgumentException(errorMessage);
            }
        }


        public List<T> GetCollection<T>(string method, List<UrlParam> urlParameters, params KeyValuePair<string, string>[] queryParameters)
        {
            List<T> results = new List<T>();

            int offset = 0;

           
            Response <T> response;
            do
            {
                List<UrlParam> newUrlParams = (urlParameters != null) ? new List<UrlParam>(urlParameters) : new List<UrlParam>();

                newUrlParams.Add(new UrlParam("offset", offset.ToString()));
                newUrlParams.Add(new UrlParam("limit", "100"));

                response = Get<Response<T>>(method, newUrlParams, queryParameters.ToArray());
                if (response.HasError)
                {
                    throw new APIException(string.Format("Server raised exception. Details: {0} ",
                                                                response.Error)
                        );
                }

                results.AddRange(response.Data);

                offset = response.Offset;

                //Requires by API Clientix. 
                Thread.Sleep(500);

            } while ((response.Count > 0) && (response.Count == response.Limit));
             

            return results;
        }

        private string ConvertUrlParamsToString(ICollection<UrlParam> urlParams)
        {
            string result = string.Join("/", urlParams.Select(x => x.Key + "/" + x.Value));
            if (result.Length > 0)
            {
                result += '/';
            }

            return result;
        }

        private string ConvertUrlParamsToString(UrlParam urlParam)
        {
            return urlParam.Key + "/" + urlParam.Value + "/";
        }

        private List<UrlParam> GetAuthLine()
        {
            List<UrlParam> lines = new List<UrlParam>();

            lines.Add(new UrlParam("a", AccountID));
            lines.Add(new UrlParam("u", UserID));
            lines.Add(new UrlParam("t", AccessToken));

            return lines;
        }

        private T Get<T>(string method, List<UrlParam> urlParameters, params KeyValuePair<string, string>[] queryParameters)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            //Creates parameters of request
            var uriBuilder = new UriBuilder(BASE_URL);
            uriBuilder.Path += ConvertUrlParamsToString(GetAuthLine());
            uriBuilder.Path += ConvertUrlParamsToString(new UrlParam("m", method));
            uriBuilder.Path += ConvertUrlParamsToString(urlParameters);
            //uriBuilder.Path += ConvertUrlParamsToString(new UrlParam("limit", "100"));

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            for (int i = 0; i < queryParameters.Length; i++)
            {
                query[queryParameters[i].Key] = queryParameters[i].Value;
            }
            uriBuilder.Query = query.ToString();



            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriBuilder.ToString());
            //request.ContentType = "application/json";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                string responseString = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(responseString);
            }
        }
    }
}
