using BSP.POS.API.Models.Observaciones;
using BSP.POS.NEGOCIOS.Observaciones;
using BSP.POS.UTILITARIOS.Observaciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ObservacionesController : ControllerBase
    {
        private N_Observaciones observaciones;
        public ObservacionesController()
        {
            observaciones = new N_Observaciones();
        }

        [HttpGet("ObtengaLaListaDeObservacionesDeInforme/{consecutivo}/{esquema}")]
        public string ObtengaLaListaDeObservacionesDeInforme(string consecutivo, string esquema)
        {
            try
            {
                string listaDeObservacionesJson = observaciones.ListarObservacionesDeInforme(esquema, consecutivo);
                return listaDeObservacionesJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpPost("AgregaObservacionDeInforme")]
        public string AgregaObservacionDeInforme([FromBody] mObservaciones datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                U_Observaciones observacion = new U_Observaciones();
                observacion.usuario = datos.usuario;
                observacion.consecutivo_informe = datos.consecutivo_informe;
                observacion.observacion = datos.observacion;


                string mensaje = observaciones.AgregarObservacionDeInforme(observacion, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
