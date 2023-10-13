using BSP.POS.NEGOCIOS.Licencias;
using BSP.POS.UTILITARIOS.Licencias;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenciasController : ControllerBase
    {
        private readonly N_Licencias licencias;

        public LicenciasController()
        {
            licencias = new N_Licencias();
        }
        [HttpGet("ObtengaLosDatosDeLaLicencia")]
        public IActionResult ObtengaLosDatosDeLaLicencia()
        {
            try
            {
                string licencia = licencias.ObtenerDatosDeLicencia();
                if(string.IsNullOrEmpty(licencia))
                {
                    return NotFound();
                }
                return Ok(licencia);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpGet("ObtengaElCodigoDeLicencia")]
        public IActionResult ObtengaElCodigoDeLicencia()
        {
            try
            {
                string licencia = licencias.ObtenerCodigoDeLicencia();
                if (string.IsNullOrEmpty(licencia))
                {
                    return NotFound();
                }
                return Ok(licencia);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("ActualizaDatosDeLicencia")]
        public IActionResult ActualizaDatosDeLicencia([FromBody] U_ActualizarDatosLicencia datosLicencia)
        {
            try
            {
                string resultado = licencias.ActualizarDatosLicencia(datosLicencia);
                if (string.IsNullOrEmpty(resultado))
                {
                    return NotFound();
                }
                return Ok(resultado);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }

        }
    }
}
