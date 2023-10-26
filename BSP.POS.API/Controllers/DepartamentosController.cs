using BSP.POS.NEGOCIOS.Departamentos;
using BSP.POS.UTILITARIOS.Departamentos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartamentosController : ControllerBase
    {
        private N_Departamentos _departamentos;

        public DepartamentosController()
        {
            _departamentos = new N_Departamentos();
        }

        [HttpGet("ObtengaListaDeDepartamentos")]
        public IActionResult ObtengaListaDeDepartamentos()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string lista = _departamentos.ObtenerListaDeDepartamentos(esquema);
                if (!string.IsNullOrEmpty(lista))
                {
                    return Ok(lista);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("ActualizaListaDepartamentos")]
        public IActionResult ActualizaListaDepartamentos([FromBody] List<U_Departamentos> datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                _departamentos.ActualizarListaDepartamentos(datos, esquema);
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("AgregaDepartamento")]
        public IActionResult AgregaDepartamento([FromBody] U_Departamentos datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                _departamentos.AgregarDepartamento(datos, esquema);
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
    }
}
