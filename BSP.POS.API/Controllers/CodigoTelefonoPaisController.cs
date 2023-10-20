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

        [HttpGet("ObtengaDatosCodigoTelefonoPais")]
        public IActionResult ObtengaDatosCodigoTelefonoPais()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string datosCodigoTelefonoPais = _datos.ObtenerDatosCodigoTelefonoPais(esquema);
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

        [HttpGet("ObtengaDatosCodigoTelefonoPaisDeUsuariosPorUsuario")]
        public IActionResult ObtengaDatosCodigoTelefonoPaisDeUsuariosPorUsuario()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string codigoUsuario = Request.Headers["X-CodigoUsuario"];
                string datosCodigoTelefonoPais = _datos.ObtenerDatosCodigoTelefonoPaisDeUsuariosPorUsuario(esquema, codigoUsuario);
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
