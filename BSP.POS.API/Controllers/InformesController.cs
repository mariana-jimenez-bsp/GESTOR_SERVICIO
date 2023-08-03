using BSP.POS.API.Models;
using BSP.POS.API.Models.Informes;
using BSP.POS.DATOS.Informes;
using BSP.POS.NEGOCIOS.Informes;
using BSP.POS.UTILITARIOS.Informes;
using BSP.POS.UTILITARIOS.Proyectos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InformesController : ControllerBase
    {
        private N_Informes informes;
        public InformesController()
        {
            informes = new N_Informes();

        }
        // GET: api/<InformesController>
        [HttpGet("ObtengaLaListaDeInformesAsociados/{cliente}/{esquema}")]
        public string ObtengaLaListaDeInformesAsociados(string cliente, string esquema)
        {
            try
            {
                string listaInformesAsociadosJson = informes.ListarInformesAsociados(esquema, cliente);
                return listaInformesAsociadosJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
           
        }

        [HttpGet("ObtengaElInformeAsociado/{consecutivo}/{esquema}")]
        public string ObtengaElInformeAsociado(string consecutivo, string esquema)
        {
            try
            {
                var informeAsociadoJson = informes.ObtenerInformeAsociado(esquema, consecutivo);
                return informeAsociadoJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpPost("ActualizaElInformeAsociado")]
        public string ActualizaElInformeAsociado([FromBody] mInformeAsociado datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];

                U_InformeAsociado informeAsociado = new U_InformeAsociado();
                informeAsociado.consecutivo = datos.consecutivo;
                informeAsociado.cliente = datos.cliente;
                informeAsociado.estado = datos.estado;
                informeAsociado.modalidad_consultoria = datos.modalidad_consultoria;
                informeAsociado.hora_final = datos.hora_final;
                informeAsociado.hora_inicio = datos.hora_inicio;
                informeAsociado.fecha_consultoria = datos.fecha_consultoria;

                string mensaje = informes.ActualizarInformeAsociado(informeAsociado, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }



    }
}
