using BSP.POS.NEGOCIOS.Licencias;
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
        [HttpGet("ObtengaElEstadoDeLaLicencia")]
        public IActionResult ObtengaElEstadoDeLaLicencia()
        {
            try
            {
                string licencia = licencias.ObtenerEstadoDeLicencia();
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
    }
}
