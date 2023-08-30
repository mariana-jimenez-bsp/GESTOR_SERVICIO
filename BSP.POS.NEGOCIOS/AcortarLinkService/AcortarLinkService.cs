using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.AcortarLinkService
{
    public class AcortarLinkService : IAcortarLinkInterface
    {
        public async Task<string> AcortarLink(string token)
        {
            string pathToJson = "../BSP.POS.NEGOCIOS/AcortarLinkService/AcortarLinkJson/AcortarLink.json";
            string jsonString = File.ReadAllText(pathToJson);
            string url = "https://127.0.0.1:7200/ValidarAprobacionInforme/89B6092AC1B6ACD68A8CC1FE6F030F3604E643D0AFE467F94AA99499ACE3B0E52128444E0F51A1D0612931C385A75244D58763CB47951CE6C8D571D73409BC3F/BSP";
            jsonString = jsonString.Replace("{url}", url);
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://api-ssl.bitly.com/v4/shorten");
            
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
