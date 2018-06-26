using Client.Core.Model;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebClient
{
    public class DeveloperService: IDeveloperService
    {         

        private HttpClient Init()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:50106")
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public async Task<IEnumerable<IDeveloper>> FindAllAsync()
        {
            using (var client = Init())
            {
                HttpResponseMessage response = await client.GetAsync("api/Developer");
                if (response.IsSuccessStatusCode)
                {
                    var items = await response.Content.ReadAsAsync<DeveloperItem[]>();
                    return items;
                }

                return null;
            }
        }
    }
}
