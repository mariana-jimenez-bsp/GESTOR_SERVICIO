using BSP.POS.NEGOCIOS.Permisos;
using Microsoft.AspNetCore.Mvc;
using BSP.POS.UTILITARIOS.Permisos;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PermisosController : ControllerBase
    {
        private N_Permisos _permisos;

        public PermisosController()
        {
            _permisos = new N_Permisos();
        }

        [HttpGet("ObtengaLaListaDePermisosAsociados/{esquema}/{id}")]
        public IActionResult ObtengaLaListaDePermisosAsociados(string esquema, string id)
        {
            try
            {

                string listaPermisosAsociadosJson = _permisos.ListarPermisosAsociados(esquema, id);
                if (string.IsNullOrEmpty(listaPermisosAsociadosJson))
                {
                    return NotFound();
                }
                return Ok(listaPermisosAsociadosJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("ObtengaLaListaDePermisos/{esquema}")]
        public IActionResult ObtengaLaListaDePermisos(string esquema)
        {
            try
            {

                string listaPermisosJson = _permisos.ListarPermisos(esquema);
                if (string.IsNullOrEmpty(listaPermisosJson))
                {
                    return NotFound();
                }
                return Ok(listaPermisosJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("ActualizaListaDePermisosAsociados")]
        public IActionResult ActualizaListaDePermisosAsociados([FromBody] List<U_PermisosAsociados> datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string idUsuario = Request.Headers["X-IdUsuario"];
                string mensaje = _permisos.ActualizarPermisosAsociados(datos, idUsuario, esquema);
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
    }
}
