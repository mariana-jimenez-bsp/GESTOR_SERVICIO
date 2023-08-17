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
        public string ObtengaElEstadoDeLaLicencia()
        {
            string licencia = licencias.ObtenerEstadoDeLicencia();
            return licencia;
        }
    }
}
