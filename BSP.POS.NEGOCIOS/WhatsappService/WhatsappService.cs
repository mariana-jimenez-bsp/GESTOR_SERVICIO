using BSP.POS.UTILITARIOS.Correos;
using BSP.POS.UTILITARIOS.CorreosModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.WhatsappService
{
    public class WhatsappService : IWhatsappInterface
    {
        public async Task EnviarWhatsappAprobarInforme()
        {
            try
            {
                string token = "EAAJhFM6sEYEBO980J6WdZClvQ5VBVfxF1T0QEBUXWzQfzFZBtfRUYs1FPwJ7BePwGTiYbjUiWpy8jSmzYwbQEZBOWftKHdsG1tbdRZC1O9Fsv5BAJbRZCx3Lu16yCC69E7HrV7plVsV3UFu1dh68G8f6GFAaqZChAWvsZBp0lUwiDaPzlmYb6IMTupiExaGb7yfrbve3f9Se58Evw02MSgZD";
                //Identificador de número de teléfono
                string idTelefono = "118415284680964";
                //Nuestro telefono
                string telefono = "50671417642";
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://graph.facebook.com/v17.0/" + idTelefono + "/messages");
                request.Headers.Add("Authorization", "Bearer " + token);
                request.Content = new StringContent("{\"messaging_product\": \"whatsapp\",\"recipient_type\": \"individual\",\"to\": \"" + telefono + "\",\"type\": \"text\",\"text\": {\"body\": \"prueba\"}}");
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {

                throw;
            }
          
        }
    }
}
