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

        [HttpGet("ObtengaLaListaDeProyectosIniciados/{esquema}")]
        public IActionResult ObtengaLaListaDeProyectosIniciados(string esquema)
        {
            try
            {
                string listaProyectosJson = _proyectos.ListarProyectosIniciados(esquema);
                if (string.IsNullOrEmpty(listaProyectosJson))
                {
                    return NotFound();
                }
                return Ok(listaProyectosJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("ObtengaLaListaDeProyectosTerminados/{esquema}")]
        public IActionResult ObtengaLaListaDeProyectosTerminados(string esquema)
        {
            try
            {
                string listaProyectosJson = _proyectos.ListarProyectosTerminados(esquema);
                if (string.IsNullOrEmpty(listaProyectosJson))
                {
                    return NotFound();
                }
                return Ok(listaProyectosJson);
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
                if (string.IsNullOrEmpty(mensaje) || mensaje == "Error")
                {
                    return BadRequest();
                }
                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("TerminaProyecto")]
        public IActionResult TerminaProyecto()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string numero = Request.Headers["X-Numero"];
                string mensaje = _proyectos.TerminarProyecto(numero, esquema);
                if (string.IsNullOrEmpty(mensaje) || mensaje == "Error")
                {
                    return BadRequest();
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
                if (string.IsNullOrEmpty(mensaje) || mensaje == "Error")
                {
                    return BadRequest();
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
