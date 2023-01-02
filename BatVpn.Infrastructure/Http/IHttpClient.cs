using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BatVpn.Infrastructure.Http
{
    public interface IHttpClient
    {
        Task<T> Get<T>(string url) where T : class;
        Task<T> Send<T>(HttpRequestMessage request) where T : class;
        Task<string> Download(string url, long? pageId = null, long? postId = null);
    }
}
