using BSP.POS.NEGOCIOS.Lugares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LugaresController : ControllerBase
    {
        private N_Lugares _lugares;

        public LugaresController()
        {
            _lugares = new N_Lugares();
        }

        [HttpGet("ObtengaLaListaDePaises")]
        public IActionResult ObtengaLaListaDePaises()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string listaJson = _lugares.ObtenerPaises(esquema);
                if(string.IsNullOrEmpty(listaJson))
                {
                    return NotFound();
                }
                return Ok(listaJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("ObtengaLaListaDeProvinciasPorPais")]
        public IActionResult ObtengaLaListaDeProvinciasPorPais()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string pais = Request.Headers["X-Pais"];
                string listaJson = _lugares.ObtenerProvinciasPorPais(esquema, pais);
                if(string.IsNullOrEmpty(listaJson))
                {
                    return NotFound();
                }
                return Ok(listaJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("ObtengaLaListaDeCantonesPorProvincia")]
        public IActionResult ObtengaLaListaDeCantonesPorProvincia()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string pais = Request.Headers["X-Pais"];
                string provincia = Request.Headers["X-Provincia"];
                string listaJson = _lugares.ObtenerCantonesPorProvincia(esquema, pais, provincia);
                if( string.IsNullOrEmpty(listaJson))
                {
                    return NotFound();
                }
                return Ok(listaJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("ObtengaLaListaDeDistritosPorCanton")]
        public IActionResult ObtengaLaListaDeDistritosPorCanton()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string pais = Request.Headers["X-Pais"];
                string provincia = Request.Headers["X-Provincia"];
                string canton = Request.Headers["X-Canton"];
                string listaJson = _lugares.ObtenerDistritosPorCanton(esquema, pais, provincia, canton);
                if(string.IsNullOrEmpty(listaJson))
                {
                    return NotFound();
                }
                return Ok(listaJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("ObtengaLaListaDeBarriosPorDistrito")]
        public IActionResult ObtengaLaListaDeBarriosPorDistrito()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string pais = Request.Headers["X-Pais"];
                string provincia = Request.Headers["X-Provincia"];
                string canton = Request.Headers["X-Canton"];
                string distrito = Request.Headers["X-Distrito"];
                string listaJson = _lugares.ObtenerBarriosPorDistrito(esquema, pais, provincia, canton, distrito);
                if( string.IsNullOrEmpty(listaJson))
                {
                    return NotFound();
                }
                return Ok(listaJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
