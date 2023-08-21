using BSP.POS.UTILITARIOS.Whatsapp;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsappController : ControllerBase
    {

        [HttpGet]
        //DENTRO DE LA RUTA envia
        [Route("envia")]
        //RECIBIMOS LOS PARAMETROS QUE NOS ENVIA WHATSAPP PARA VALIDAR NUESTRA URL
        public async Task enviaAsync()
        {
            string token = "EAAJhFM6sEYEBO9bcqltDD62HOKO5fWSAWMJVOh5GZCgeUeVxQ5DIpwNKcjwCDSDiD8WfxOG3HE3WFkD0OfQDGEIcZASsYxPZAGDGNGtCcA26yKOIAVmKay50BNapwe4zAiYE9yq0kzaxpObBpCDGu0QtBNtltvybvKynejKCZCXLKL8jyqE7S1YBXw4VIP8fG7h7i28Q20fkUHstvt0ZD";
            //Identificador de número de teléfono
            string idTelefono = "118415284680964";
            //Nuestro telefono
            string telefono = "50671417642";
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://graph.facebook.com/v15.0/" + idTelefono + "/messages");

            request.Headers.Add("Authorization", "Bearer " + token);
            request.Content = new StringContent("{\"messaging_product\": \"whatsapp\",\"recipient_type\": \"individual\",\"to\": \"" + telefono + "\",\"type\": \"text\",\"text\": {\"body\": \"prueba\"}}");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
        }
    }
}
