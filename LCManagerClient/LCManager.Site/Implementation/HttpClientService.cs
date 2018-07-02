using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using LC_Manager.Models;

namespace LC_Manager.Implementation
{
    public class HttpClientService
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;

        public static Task<HttpResponseMessage> PostAsync<T>(string requestUri, T value)
        {
            var t = JwtProps.GetToken();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConnectionString);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JwtProps.GetToken());
            return client.PostAsJsonAsync<T>(requestUri, value, CancellationToken.None);
        }

        public static Task<HttpResponseMessage> PostAsyncWoBearer<T>(string requestUri, T value)
        {
            HttpClient client = new HttpClient {BaseAddress = new Uri(ConnectionString)};
            client.DefaultRequestHeaders.Accept.Clear();
            return client.PostAsync(requestUri, value,new JsonMediaTypeFormatter());
        }
    }
}