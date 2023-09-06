using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Voice;
using BSP.POS.NEGOCIOS.WhatsappService;

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsappController : ControllerBase
    {
        private readonly IWhatsappInterface _whatsappService;
        public WhatsappController(IWhatsappInterface whatsappService)
        {
            _whatsappService = whatsappService;
        }
        [HttpGet("EnviarMensaje")]
        public string EnviarMensaje()
        {
            string respuesta = _whatsappService.EnviarWhatsappConTwilio();
            return respuesta;


        }

    }
}
