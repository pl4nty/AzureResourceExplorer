using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ARMExplorer.Controllers
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> SendAsync(HttpRequest requestMessage, HttpRequestMessage sendRequest);
        Task<HttpResponseMessage> ExecuteAsync(HttpRequest requestMessage, HttpRequestMessage executeRequest);
    }
}