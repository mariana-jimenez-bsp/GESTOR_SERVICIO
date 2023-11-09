using BSP.POS.NEGOCIOS.Esquemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twilio.TwiML.Messaging;

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EsquemasController : ControllerBase
    {
        private N_Esquemas _esquemas;

        public EsquemasController()
        {
            _esquemas = new N_Esquemas();
        }

        [HttpGet("ObtengaListaDeEsquemas")]
        public IActionResult ObtengaListaDeEsquemas()
        {
            try
            {
                string listaEsquemasJson = _esquemas.ObtenerListaDeEsquemas();
                if (string.IsNullOrEmpty(listaEsquemasJson))
                {
                    return NotFound();
                }
                return Ok(listaEsquemasJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("ObtengaListaDeEsquemasDeUsuario")]
        public IActionResult ObtengaListaDeEsquemasDeUsuario()
        {
            try
            {
                string codigo = Request.Headers["X-Codigo"];
                string listaEsquemasJson = _esquemas.ObtenerListaDeEsquemasDeUsuario(codigo);
                if (string.IsNullOrEmpty(listaEsquemasJson))
                {
                    return NotFound();
                }
                return Ok(listaEsquemasJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("ActualizaListaDeEsquemasDeUsuario")]
        public IActionResult ActualizaListaDeEsquemasDeUsuario([FromBody] List<string> datos)
        {
            
            try
            {
                string codigoUsuario = Request.Headers["X-Codigo"];
                _esquemas.ActualizarEsquemasDeUsuario(datos, codigoUsuario);
                return Ok();
            }
            catch (Exception ex) 
            {

                return StatusCode(500, ex.Message);
            }
        }

    }
}
