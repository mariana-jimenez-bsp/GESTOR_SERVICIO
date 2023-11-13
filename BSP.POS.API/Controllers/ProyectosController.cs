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

        [HttpGet("ObtengaLaListaDeProyectosActivos/{esquema}")]
        public IActionResult ObtengaLaListaDeProyectosActivos(string esquema)
        {
            try
            {
                string listaProyectosJson = _proyectos.ListarProyectosActivos(esquema);
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

        [HttpGet("ObtengaElProyecto")]
        public IActionResult ObtengaElProyecto()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string numero = Request.Headers["X-Numero"];
                string proyectoJson = _proyectos.ObtenerProyecto(esquema, numero);
                if (string.IsNullOrEmpty(proyectoJson))
                {
                    return NotFound();
                }
                return Ok(proyectoJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("ObtengaLosDatosDeProyectosActivos/{esquema}")]
        public IActionResult ObtengaLosDatosDeProyectosActivos(string esquema)
        {
            try
            {
                string listaProyectosJson = _proyectos.ListarDatosProyectosActivos(esquema);
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

        [HttpGet("ObtengaLosDatosDeProyectosActivosDeCliente/{esquema}/{cliente}")]
        public IActionResult ObtengaLosDatosDeProyectosActivosDeCliente(string esquema, string cliente)
        {
            try
            {
                string listaProyectosJson = _proyectos.ListarDatosProyectosActivosDeCliente(esquema, cliente);
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

        [HttpPut("ActualizaListaDeProyectos")]
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

        [HttpPut("TerminaProyecto")]
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
