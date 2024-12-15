using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.SystemWebAdapters;

namespace ARMExplorer.Controllers
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private HttpClient GetHttpClient(HttpRequest requestMessage)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Utils.GetCSMUrl(requestMessage.Host.Value));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",requestMessage.AsSystemWeb().Headers.GetValues(Utils.X_MS_OAUTH_TOKEN).FirstOrDefault());
            client.DefaultRequestHeaders.Add("User-Agent", requestMessage.Host.Value);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }


        public async Task<HttpResponseMessage> SendAsync(HttpRequest requestMessage, HttpRequestMessage sendRequest)
        {
            using (var client = GetHttpClient(requestMessage))
            {
                return await client.SendAsync(sendRequest);
            }
        }

        public async Task<HttpResponseMessage> ExecuteAsync(HttpRequest requestMessage, HttpRequestMessage executeRequest)
        {
            using (var client = GetHttpClient(requestMessage))
            {
                executeRequest.Headers.Add("x-ms-request-id", Guid.NewGuid().ToString("N"));
                return await Utils.Execute(client.SendAsync(executeRequest));
            }
        }
    }
}