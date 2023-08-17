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
            string token = "EAAJhFM6sEYEBO980J6WdZClvQ5VBVfxF1T0QEBUXWzQfzFZBtfRUYs1FPwJ7BePwGTiYbjUiWpy8jSmzYwbQEZBOWftKHdsG1tbdRZC1O9Fsv5BAJbRZCx3Lu16yCC69E7HrV7plVsV3UFu1dh68G8f6GFAaqZChAWvsZBp0lUwiDaPzlmYb6IMTupiExaGb7yfrbve3f9Se58Evw02MSgZD";
            //Identificador de número de teléfono
            string idTelefono = "118415284680964";
            //Nuestro telefono
            string telefono = "50671417642";
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://graph.facebook.com/v15.0/" + idTelefono + "/messages");

            request.Headers.Add("Authorization", "Bearer " + token);
            request.Content = new StringContent("{ \"messaging_product\": \"whatsapp\", \"to\": \"50671417642\", \"type\": \"text\", \"text\": {\"preview_url\": false, \"body\": \"hello_world\" }}");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
        }
    }
}
