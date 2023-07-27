using BSP.POS.NEGOCIOS.Actividades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActividadesController : ControllerBase
    {
        private N_Actividades actividades;

        public ActividadesController()
        {
            actividades = new N_Actividades();

        }

        [HttpGet("ObtengaLaListaDeActividadesAsociadas/{consecutivo}")]
        public string ObtengaLaListaDeActividadesAsociadas(string consecutivo)
        {
            try
            {
                string esquema = "BSP";
                string listaActividadesAsociadasJson = actividades.ListarActividadesAsociadas(esquema, consecutivo);
                return listaActividadesAsociadasJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpGet("ObtengaLaListaDeActividades/{esquema}")]
        public string ObtengaLaListaDeActividades(string esquema)
        {
            try
            {
                string listaActividadesAsociadasJson = actividades.ListarActividades(esquema);
                return listaActividadesAsociadasJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
