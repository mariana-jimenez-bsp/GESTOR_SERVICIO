using BSP.POS.NEGOCIOS.Actividades;
using BSP.POS.UTILITARIOS.Actividades;
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

        [HttpGet("ObtengaLaListaDeActividadesAsociadas/{consecutivo}/{esquema}")]
        public string ObtengaLaListaDeActividadesAsociadas(string consecutivo, string esquema)
        {
            try
            {
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

        [HttpPost("ActualizaListaDeActividades")]
        public string ActualizaListaDeActividades([FromBody] List<U_ListaActividades> datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = actividades.ActualizarListaDeActividades(datos, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpPost("ActualizaListaDeActividadesAsociadas")]
        public string ActualizaListaDeActividadesAsociadas([FromBody] List<U_ListaActividadesAsociadas> datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = actividades.ActualizarListaDeActividadesAsociadas(datos, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpPost("AgregaActividadDeInforme")]
        public string AgregaActividadDeInforme([FromBody] U_ListaActividadesAsociadas datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = actividades.AgregarActividadDeInforme(datos, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpDelete("EliminaActividadDeInforme")]
        public string EliminaActividadDeInforme()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string idActividad = Request.Headers["X-IdActividad"];


                string mensaje = actividades.EliminarActividadDeInforme(idActividad, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpPost("AgregaActividad")]
        public string AgregaActividad([FromBody] U_ListaActividades datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = actividades.AgregarActividad(datos, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
