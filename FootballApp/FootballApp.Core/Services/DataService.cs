using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace FootballApp.Core.Services
{
    public class DataService : IDataService
    {

        public async Task<string> MakeJsonWebRequestAsync(string url)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
                var httpResponse = await client.SendAsync(request);
                string response = await httpResponse.Content.ReadAsStringAsync();
                return response;
            }            
        }
    }
}
