
namespace LCManager.JWT
{
    using System;
    using Site.Infrastrucure;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading;
    using System.Threading.Tasks;

    public class HttpClientService
    {
        //private static readonly string ConnectionString = Config.GetApiUrl();

        public static Task<HttpResponseMessage> PostAsync<T>(string requestUri, T value)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Config.GetApiUrl());
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JwtProps.GetToken());
            client.DefaultRequestHeaders.Add(Config.GetCustomHeaderAuthorization(), "Bearer " + JwtProps.GetToken());
            return client.PostAsJsonAsync<T>(requestUri, value, CancellationToken.None);
        }

        public static Task<HttpResponseMessage> PostAsyncWoBearer<T>(string requestUri, T value)
        {
            HttpClient client = new HttpClient {BaseAddress = new Uri(Config.GetApiUrl()) };
            client.DefaultRequestHeaders.Accept.Clear();
            return client.PostAsync(requestUri, value,new JsonMediaTypeFormatter());
        }
    }
}