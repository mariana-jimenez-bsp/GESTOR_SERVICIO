using BSP.POS.NEGOCIOS.Tiempos;
using BSP.POS.UTILITARIOS.Usuarios;
using Microsoft.AspNetCore.Mvc;
using BSP.POS.API.Models;
using BSP.POS.UTILITARIOS.Tiempos;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TiemposController : ControllerBase
    {
        private N_Tiempos tiempos;
        public TiemposController()
        {
            tiempos = new N_Tiempos();

        }
        // GET: api/<TiemposController>
        [HttpGet("ObtengaLaListaDeTiempos")]
        public string ObtengaLaListaDeTiempos()
        {
            try
            {
                string esquema = "BSP";
                string listaTiemposJson = tiempos.ListarTiempos(esquema);
                return listaTiemposJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        [HttpPost("ActualizaLaListaDeTiempos")]
        public string ActualizaLaListaDeTiempos([FromBody] List<mTiempos> datos)
        {
            try
            {
                List<U_ListaTiempos> listaTiempos = new List<U_ListaTiempos>();
                foreach (var item in datos)
                {
                    U_ListaTiempos tiempo = new U_ListaTiempos();
                    tiempo.Id = item.Id;
                    tiempo.horas = item.horas;
                    tiempo.nombre_servicio = item.nombre_servicio;
                    listaTiempos.Add(tiempo);
                }

                string mensaje = tiempos.ActualizarListaDeTiempos(listaTiempos);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


    }
}
