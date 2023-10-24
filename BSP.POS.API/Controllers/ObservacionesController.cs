using BSP.POS.NEGOCIOS.Observaciones;
using BSP.POS.UTILITARIOS.Observaciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ObservacionesController : ControllerBase
    {
        private N_Observaciones observaciones;
        public ObservacionesController()
        {
            observaciones = new N_Observaciones();
        }

        [HttpGet("ObtengaLaListaDeObservacionesDeInforme/{consecutivo}/{esquema}")]
        public IActionResult ObtengaLaListaDeObservacionesDeInforme(string consecutivo, string esquema)
        {
            try
            {
                string listaDeObservacionesJson = observaciones.ListarObservacionesDeInforme(esquema, consecutivo);
                if (string.IsNullOrEmpty(listaDeObservacionesJson))
                {
                    return NotFound();
                }
                return Ok(listaDeObservacionesJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("AgregaObservacionDeInforme")]
        public IActionResult AgregaObservacionDeInforme([FromBody] U_Observaciones datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = observaciones.AgregarObservacionDeInforme(datos, esquema);
                if (string.IsNullOrEmpty(mensaje) || mensaje == "Error")
                {
                    return BadRequest();
                }
                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
