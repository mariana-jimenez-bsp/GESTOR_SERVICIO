using BSP.POS.NEGOCIOS.Permisos;
using Microsoft.AspNetCore.Mvc;
using BSP.POS.UTILITARIOS.Permisos;
using Microsoft.AspNetCore.Authorization;
using BSP.POS.API.Models.Permisos;

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
        public string ObtengaLaListaDePermisosAsociados(string esquema, string id)
        {
            try
            {

                string listaPermisosAsociadosJson = _permisos.ListarPermisosAsociados(esquema, id);
                return listaPermisosAsociadosJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpGet("ObtengaLaListaDePermisos/{esquema}")]
        public string ObtengaLaListaDePermisos(string esquema)
        {
            try
            {

                string listaPermisosJson = _permisos.ListarPermisos(esquema);
                return listaPermisosJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpPost("ActualizaListaDePermisosAsociados")]
        public string ActualizaListaDePermisosAsociados([FromBody] List<mPermisosAsociados> datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string idUsuario = Request.Headers["X-IdUsuario"];

                List<U_PermisosAsociados> listaPermisos = new List<U_PermisosAsociados>();
                foreach (var item in datos)
                {
                    U_PermisosAsociados permiso = new U_PermisosAsociados();
                    permiso.Id = item.Id;
                    permiso.id_permiso = item.id_permiso;

                    listaPermisos.Add(permiso);
                }

                string mensaje = _permisos.ActualizarPermisosAsociados(listaPermisos, idUsuario, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
