using BSP.POS.API.Models;
using BSP.POS.API.Models.Informes;
using BSP.POS.NEGOCIOS.CorreosService;
using BSP.POS.NEGOCIOS.Informes;
using BSP.POS.NEGOCIOS.Usuarios;
using BSP.POS.UTILITARIOS.Correos;
using BSP.POS.UTILITARIOS.CorreosModels;
using BSP.POS.UTILITARIOS.Informes;
using BSP.POS.UTILITARIOS.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InformesController : ControllerBase
    {
        private N_Informes informes;
        private N_Usuarios user;
        private readonly string _secretKey;
        private readonly string _correoUsuario;
        private readonly string _claveUsuario;
        private readonly ICorreosInterface _correoService;
        public InformesController(ICorreosInterface correoService)
        {
            informes = new N_Informes();
            user = new N_Usuarios();
            var configuration = new ConfigurationBuilder()
             .AddUserSecrets<Program>()
             .Build();

            _secretKey = configuration["SecretKey"];
            _correoUsuario = configuration["SmtpFrom"];
            _claveUsuario = configuration["SmtpPassword"];
            _correoService = correoService;
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

        [HttpPost("CambiaEstadoDeInforme")]
        public string CambiaEstadoDeInforme([FromBody] mInformeAsociado datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];

                U_InformeAsociado informeAsociado = new U_InformeAsociado();
                informeAsociado.consecutivo = datos.consecutivo;
                informeAsociado.estado = datos.estado;


                string mensaje = informes.CambiarEstadoDeInforme(informeAsociado, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpDelete("EliminaInforme")]
        public string EliminaInforme()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string consecutivo = Request.Headers["X-consecutivo"];


                string mensaje = informes.EliminarInforme(consecutivo, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        [HttpPost("EnviarTokenDeAprobacionDeInforme")]
        public IActionResult EnviarTokenDeAprobacionDeInforme(mObjetosParaCorreoAprobacion objetosDeAprobacion)
        {
            try
            {
                U_Correo datos = new U_Correo();

                foreach (var item in objetosDeAprobacion.listadeUsuariosDeClienteDeInforme)
                {

                    U_TokenAprobacionInforme tokenAprobacionRecuperado = informes.EnviarTokenDeAprobacionDeInforme(item.codigo_usuario_cliente, objetosDeAprobacion.esquema);
                    if (tokenAprobacionRecuperado != null)
                    {
                        U_ListaDeUsuariosDeCliente usuario = new U_ListaDeUsuariosDeCliente();
                        usuario = user.ObtenerUsuarioDeClientePorCodigo(objetosDeAprobacion.esquema, tokenAprobacionRecuperado.codigo);

                        item.token = tokenAprobacionRecuperado.token_aprobacion;
                        item.correo_usuario = usuario.correo;

                    }
                }
                datos.correoUsuario = _correoUsuario;
                datos.claveUsuario = _claveUsuario;
                _correoService.EnviarCorreoAprobarInforme(datos, objetosDeAprobacion);
                return Ok();
            }

            catch (Exception)
            {
                return BadRequest();
            }
           
           

        }
        [AllowAnonymous]
        [HttpGet("ValidaTokenAprobacionDeInforme/{esquema}/{token}")]
        public string ValidaTokenAprobacionDeInforme(string esquema, string token)
        {

            string tokenAprobacionJson = informes.ValidarTokenAprobacionDeInforme(esquema, token);

            return tokenAprobacionJson;
        }
        [AllowAnonymous]
        [HttpPost("ApruebaInforme")]
        public string ApruebaInforme([FromBody] mTokenAprobacionInforme datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];

                U_TokenAprobacionInforme tokenAprobacion = new U_TokenAprobacionInforme();
                tokenAprobacion.token_aprobacion = datos.token_aprobacion;


                string mensaje = informes.AprobarInforme(tokenAprobacion, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }



    }
}
