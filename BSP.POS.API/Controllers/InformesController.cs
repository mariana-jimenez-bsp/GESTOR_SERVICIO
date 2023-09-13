using BSP.POS.API.Models;
using BSP.POS.NEGOCIOS.CorreosService;
using BSP.POS.NEGOCIOS.Informes;
using BSP.POS.NEGOCIOS.Usuarios;
using BSP.POS.NEGOCIOS.WhatsappService;
using BSP.POS.UTILITARIOS.Clientes;
using BSP.POS.UTILITARIOS.Correos;
using BSP.POS.UTILITARIOS.CorreosModels;
using BSP.POS.UTILITARIOS.Informes;
using BSP.POS.UTILITARIOS.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private readonly string _tokenWhatsapp;
        private readonly string _idTelefonoWhatsapp;
        private readonly ICorreosInterface _correoService;
        private readonly IWhatsappInterface _whatsappService;
        public InformesController(ICorreosInterface correoService, IWhatsappInterface whatsappService)
        {
            informes = new N_Informes();
            user = new N_Usuarios();
            //var configuration = new ConfigurationBuilder()
            // .AddUserSecrets<Program>()
            // .Build();
            // var configuration = new ConfigurationBuilder()
            //.AddJsonFile("appsettings.json")
            //.Build();
            _secretKey = Environment.GetEnvironmentVariable("SecretKeyGS");
            _correoUsuario = Environment.GetEnvironmentVariable("SmtpFromGS");
            _claveUsuario = Environment.GetEnvironmentVariable("SmtpPasswordGS");
            _tokenWhatsapp = Environment.GetEnvironmentVariable("tokenWhatsappGS");
            _idTelefonoWhatsapp = Environment.GetEnvironmentVariable("idTelefonoWhatsappGS");
            //_secretKey = configuration["AppSettings:SecretKey"];
            //_correoUsuario = configuration["AppSettings:SmtpFrom"];
            //_claveUsuario = configuration["AppSettings:SmtpPassword"];
            //_tokenWhatsapp = configuration["AppSettings:tokenWhatsapp"];
            //_idTelefonoWhatsapp = configuration["AppSettings:idTelefonoWhatsapp"];
            _correoService = correoService;
            _whatsappService = whatsappService;

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
        public string ActualizaElInformeAsociado([FromBody] U_InformeAsociado datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];

                string mensaje = informes.ActualizarInformeAsociado(datos, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpPost("CambiaEstadoDeInforme")]
        public string CambiaEstadoDeInforme([FromBody] U_InformeAsociado datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];



                string mensaje = informes.CambiarEstadoDeInforme(datos, esquema);
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

                    U_TokenAprobacionInforme tokenAprobacionRecuperado = informes.EnviarTokenDeAprobacionDeInforme(item.codigo_usuario_cliente, item.consecutivo_informe, objetosDeAprobacion.esquema);
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

                //_correoService.EnviarCorreoAprobarInforme(datos, objetosDeAprobacion);
                _whatsappService.EnviarWhatsappAprobarInforme(objetosDeAprobacion, _tokenWhatsapp, _idTelefonoWhatsapp);
                return Ok();
            }

            catch (Exception)
            {
                return BadRequest();
            }
           
           

        }
        [HttpPost("AgregaInformeAsociado")]
        public string AgregaInformeAsociado([FromBody] U_ClienteAsociado cliente)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];


                string consecutivo = informes.AgregarInformeAsociado(cliente.CLIENTE, esquema);
                return consecutivo;
            }
            catch (Exception ex)
            {
                return ex.Message;
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
        public string ApruebaInforme([FromBody] U_TokenAprobacionInforme datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];

                string mensaje = informes.AprobarInforme(datos, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        [HttpGet("ValidaExistenciaConsecutivoInforme/{esquema}/{consecutivo}")]
        public string ValidaExistenciaConsecutivoInforme(string esquema, string consecutivo)
        {

            string consecutivoDevuelto = informes.ValidarExistenciaConsecutivoInforme(esquema, consecutivo);

            return consecutivoDevuelto;
        }

    }
}
