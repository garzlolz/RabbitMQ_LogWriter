using MQConsumer.Class;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MQConsumer.Service
{
    public class CallAPIService
    {
        public static async void RemoteDBLog(LogInfo logInfo)
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress
                = new Uri(ConfigurationManager.AppSettings.Get("ClientSettingsProvider.ServiceUri"))
            };
            string json = JsonConvert.SerializeObject(logInfo);
            HttpContent contentPost = new StringContent(json, Encoding.UTF8, "application/json");
            client.Timeout = TimeSpan.FromSeconds(2000);
            HttpResponseMessage response = await client.PostAsync("WriteOracle", contentPost);
            //var result = .GetAwaiter().GetResult();
            var result = response.Content.ReadAsStringAsync();
        }
    }
}
