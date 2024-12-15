using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ARMExplorer.Controllers
{
    public interface IArmRepository
    {
        Task<IList<string>> GetSubscriptionIdsAsync(HttpRequest requestMessage);
        Task<HashSet<string>> GetProviderNamesFor(HttpRequest requestMessage, string subscriptionId);
        Task<Dictionary<string, Dictionary<string, HashSet<string>>>> GetProvidersFor(HttpRequest requestMessage, string subscriptionId);
        Task<HttpResponseMessage> InvokeAsync(HttpRequest requestMessage, HttpRequestMessage info);
    }
}