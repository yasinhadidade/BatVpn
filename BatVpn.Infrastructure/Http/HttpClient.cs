using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace BatVpn.Infrastructure.Http
{
    public class HttpClient : IHttpClient
    {
        private readonly IConfiguration conf;

        public HttpClient(IConfiguration conf)
        {
            this.conf = conf;
        }

        private System.Net.Http.HttpClient client = new System.Net.Http.HttpClient()
        {
            Timeout = TimeSpan.FromMilliseconds(10000),
        };

        public async Task<T> Get<T>(string url) where T : class
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var output = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
                return output;
            }
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                return null;
            }
        }
        public async Task<T> Send<T>(HttpRequestMessage request) where T : class
        {
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var output = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
                return output;
            }
            else return null;
        }
        public async Task<string> Download(string url, long? pageId = null, long? postId = null)
        {
            var response = await client.GetAsync(url);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return null;
            var path = Path.Combine(conf.GetSection("UploadAddress").Value, pageId?.ToString() ?? "general", postId?.ToString() ?? "general");
            var file = Guid.NewGuid().ToString() + "" + Path.GetExtension(url.Split("?")[0]);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            using (var fs = new FileStream(Path.Combine(path, file), FileMode.CreateNew))
            {
                await response.Content.CopyToAsync(fs);
                return Path.Combine(pageId?.ToString() ?? "general", postId?.ToString() ?? "general", file).Replace("\\", "/");
            }
        }
    }
}
