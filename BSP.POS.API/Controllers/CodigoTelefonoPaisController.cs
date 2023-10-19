using BSP.POS.NEGOCIOS.CodigoTelefonoPais;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CodigoTelefonoPaisController : ControllerBase
    {
        private N_CodigoTelefonoPais _datos;

        public CodigoTelefonoPaisController()
        {
            _datos = new N_CodigoTelefonoPais();
        }

        [HttpGet("ObtengaDatosCodigoTelefonoPaisDeClientes")]
        public IActionResult ObtengaDatosCodigoTelefonoPaisDeClientes()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string datosCodigoTelefonoPais = _datos.ObtenerDatosCodigoTelefonoPaisDeClientes(esquema);
                if (!string.IsNullOrEmpty(datosCodigoTelefonoPais))
                {
                    return Ok(datosCodigoTelefonoPais);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
    }
}
