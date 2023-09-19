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
        public IActionResult ObtengaLaListaDeProyectos(string esquema)
        {
            try
            {
                string listaProyectosAsociadasJson = _proyectos.ListarProyectos(esquema);
                if (string.IsNullOrEmpty(listaProyectosAsociadasJson))
                {
                    return NotFound();
                }
                return Ok(listaProyectosAsociadasJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("ActualizaListaDeProyectos")]
        public IActionResult ActualizaListaDeActividades([FromBody] List<U_ListaProyectos> datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = _proyectos.ActualizarListaDeProyectos(datos, esquema);
                if (string.IsNullOrEmpty(mensaje))
                {
                    return NotFound();
                }
                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("AgregaProyecto")]
        public IActionResult AgregaProyecto([FromBody] U_ListaProyectos datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = _proyectos.AgregarProyecto(datos, esquema);
                if (string.IsNullOrEmpty(mensaje)) { 
                    return NotFound();
                }
                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
