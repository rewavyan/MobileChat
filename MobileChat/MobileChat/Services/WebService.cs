using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MobileChat.Models;
using Newtonsoft.Json;

namespace MobileChat.Services
{
    public interface IWebService
    {
        Task<object> GetResponse<T1, T2>(object requestObj, string uri);
        Task<LastMessage> GetLastMessageAsync(Dialog dialog);
    }

    public class WebService : IWebService
    {
        public WebService()
        {

        }
        public async Task<object> GetResponse<T1, T2>(object requestObj, string uri)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            string json = JsonConvert.SerializeObject((T1)requestObj);
            request.RequestUri = new Uri(uri);
            request.Method = HttpMethod.Post;
            request.Content = new StringContent(json);
            request.Headers.Add("Accept", "application/json");
            try
            {
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContent responseContent = response.Content;
                    var responsejson = await responseContent.ReadAsStringAsync(Encoding.UTF8);
                    var data = JsonConvert.DeserializeObject<T2>(responsejson);
                    return data;
                }
            }
            catch (Exception ex){}
            return null;
        }

        public async Task<LastMessage> GetLastMessageAsync(Dialog dialog)
        {
            return (LastMessage)await GetResponse<Dialog, LastMessage>(dialog, Constants.GetLastMessagePage);
        }
    }
}
