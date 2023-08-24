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
        public string ObtengaLaListaDePaises()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string listaInformesAsociadosJson = _lugares.ObtenerPaises(esquema);
                return listaInformesAsociadosJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpGet("ObtengaLaListaDeProvinciasPorPais")]
        public string ObtengaLaListaDeProvinciasPorPais()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string pais = Request.Headers["X-Pais"];
                string listaInformesAsociadosJson = _lugares.ObtenerProvinciasPorPais(esquema, pais);
                return listaInformesAsociadosJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpGet("ObtengaLaListaDeCantonesPorProvincia")]
        public string ObtengaLaListaDeCantonesPorProvincia()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string pais = Request.Headers["X-Pais"];
                string provincia = Request.Headers["X-Provincia"];
                string listaInformesAsociadosJson = _lugares.ObtenerCantonesPorProvincia(esquema, pais, provincia);
                return listaInformesAsociadosJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpGet("ObtengaLaListaDeDistritosPorCanton")]
        public string ObtengaLaListaDeDistritosPorCanton()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string pais = Request.Headers["X-Pais"];
                string provincia = Request.Headers["X-Provincia"];
                string canton = Request.Headers["X-Canton"];
                string listaInformesAsociadosJson = _lugares.ObtenerDistritosPorCanton(esquema, pais, provincia, canton);
                return listaInformesAsociadosJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpGet("ObtengaLaListaDeBarriosPorDistrito")]
        public string ObtengaLaListaDeBarriosPorDistrito()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string pais = Request.Headers["X-Pais"];
                string provincia = Request.Headers["X-Provincia"];
                string canton = Request.Headers["X-Canton"];
                string distrito = Request.Headers["X-Distrito"];
                string listaInformesAsociadosJson = _lugares.ObtenerBarriosPorDistrito(esquema, pais, provincia, canton, distrito);
                return listaInformesAsociadosJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
