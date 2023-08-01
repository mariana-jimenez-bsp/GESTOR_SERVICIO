using BSP.POS.API.Models.Actividades;
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
        public string ActualizaListaDeActividades([FromBody] List<mActividades> datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];

                List<U_ListaActividades> listaActividades = new List<U_ListaActividades>();
                foreach (var item in datos)
                {
                    U_ListaActividades atividad = new U_ListaActividades();
                    atividad.Id = item.Id;
                    atividad.codigo = item.codigo;
                    atividad.Actividad = item.Actividad;
                    atividad.CI_referencia = item.CI_referencia;
                    atividad.horas = item.horas;

                    listaActividades.Add(atividad);
                }

                string mensaje = actividades.ActualizarListaDeActividades(listaActividades, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpPost("ActualizaListaDeActividadesAsociadas")]
        public string ActualizaListaDeActividadesAsociadas([FromBody] List<mActividadesAsociadas> datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];

                List<U_ListaActividadesAsociadas> listaActividades = new List<U_ListaActividadesAsociadas>();
                foreach (var item in datos)
                {
                    U_ListaActividadesAsociadas atividad = new U_ListaActividadesAsociadas();
                    atividad.Id = item.Id;
                    atividad.codigo_actividad = item.codigo_actividad;
                    atividad.consecutivo_informe = item.consecutivo_informe;
                    atividad.horas_cobradas = item.horas_cobradas;
                    atividad.horas_no_cobradas = item.horas_no_cobradas;

                    listaActividades.Add(atividad);
                }

                string mensaje = actividades.ActualizarListaDeActividadesAsociadas(listaActividades, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
