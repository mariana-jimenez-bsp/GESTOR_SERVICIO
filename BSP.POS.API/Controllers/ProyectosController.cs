using BSP.POS.NEGOCIOS.Proyectos;
using BSP.POS.UTILITARIOS.Proyectos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProyectosController : ControllerBase
    {
        private N_Proyectos _proyectos;
        public ProyectosController()
        {
            _proyectos = new N_Proyectos();
        }

        [HttpGet("ObtengaLaListaDeProyectos/{esquema}")]
        public string ObtengaLaListaDeProyectos(string esquema)
        {
            try
            {
                string listaProyectosAsociadasJson = _proyectos.ListarProyectos(esquema);
                return listaProyectosAsociadasJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpPost("ActualizaListaDeProyectos")]
        public string ActualizaListaDeActividades([FromBody] List<U_ListaProyectos> datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = _proyectos.ActualizarListaDeProyectos(datos, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpPost("AgregaProyecto")]
        public string AgregaProyecto([FromBody] U_ListaProyectos datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = _proyectos.AgregarProyecto(datos, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
