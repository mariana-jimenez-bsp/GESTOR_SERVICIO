using BSP.POS.API.Models;
using BSP.POS.DATOS.Informes;
using BSP.POS.NEGOCIOS.Informes;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformesController : ControllerBase
    {
        private N_Informes informes;
        public InformesController()
        {
            informes = new N_Informes();

        }
        // GET: api/<InformesController>
        [HttpGet("ObtengaLaListaDeInformesAsociados/{cliente}")]
        public string ObtengaLaListaDeInformesAsociados(string cliente)
        {
            string esquema = "BSP";
            string listaInformesAsociadosJson = informes.ListarInformesAsociados(esquema, cliente);
            return listaInformesAsociadosJson;
        }

       
    }
}
