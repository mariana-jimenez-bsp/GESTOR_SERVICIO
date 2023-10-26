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
        private readonly string _urlWeb = string.Empty;
        private readonly string _tipoInicio = string.Empty;
        private readonly ICorreosInterface _correoService;
        private readonly IWhatsappInterface _whatsappService;
        public InformesController(ICorreosInterface correoService, IWhatsappInterface whatsappService)
        {
            informes = new N_Informes();
            user = new N_Usuarios();
            

            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            _tipoInicio = configuration["AppSettings:TipoInicio"];
            if (_tipoInicio == "debug")
            {
                var configurationSecrets = new ConfigurationBuilder()
                 .AddUserSecrets<Program>()
                 .Build();
                _secretKey = configurationSecrets["SecretKey"];
                _correoUsuario = configurationSecrets["SmtpFrom"];
                _claveUsuario = configurationSecrets["SmtpPassword"];
                _urlWeb = configurationSecrets["UrlWebDebug"];
            }
            else
            {
                _secretKey = configuration["AppSettings:SecretKey"];
                _correoUsuario = configuration["AppSettings:SmtpFrom"];
                _claveUsuario = configuration["AppSettings:SmtpPassword"];
                _urlWeb = configuration["AppSettings:UrlWebDeploy"];
            }

            _correoService = correoService;
            _whatsappService = whatsappService;

        }
        // GET: api/<InformesController>
        [HttpGet("ObtengaLaListaDeInformesAsociados/{cliente}/{esquema}")]
        public IActionResult ObtengaLaListaDeInformesAsociados(string cliente, string esquema)
        {
            try
            {
                string listaInformesAsociadosJson = informes.ListarInformesAsociados(esquema, cliente);
                if (string.IsNullOrEmpty(listaInformesAsociadosJson))
                {
                    return NotFound();
                }

                // Si todo está bien, devuelve la lista como JSON
                return Ok(listaInformesAsociadosJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
           
        }

        [HttpGet("ObtengaElInformeAsociado/{consecutivo}/{esquema}")]
        public IActionResult ObtengaElInformeAsociado(string consecutivo, string esquema)
        {
            try
            {
                var informeAsociadoJson = informes.ObtenerInformeAsociado(esquema, consecutivo);
                if (string.IsNullOrEmpty(informeAsociadoJson))
                {
                    return NotFound();
                }

                // Si todo está bien, devuelve la lista como JSON
                return Ok(informeAsociadoJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("ActualizaElInformeAsociado")]
        public IActionResult ActualizaElInformeAsociado([FromBody] U_InformeAsociado datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];

                string mensaje = informes.ActualizarInformeAsociado(datos, esquema);
                if (string.IsNullOrEmpty(mensaje) || mensaje == "Error")
                {
                    return BadRequest();
                }

                // Si todo está bien, devuelve la lista como JSON
                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("CambiaEstadoDeInforme")]
        public IActionResult CambiaEstadoDeInforme([FromBody] U_InformeAsociado datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];



                string mensaje = informes.CambiarEstadoDeInforme(datos, esquema);
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

        [HttpDelete("EliminaInforme")]
        public IActionResult EliminaInforme()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string consecutivo = Request.Headers["X-consecutivo"];


                string mensaje = informes.EliminarInforme(consecutivo, esquema);
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
        [HttpPost("EnviarTokenDeAprobacionDeInforme")]
        public IActionResult EnviarTokenDeAprobacionDeInforme()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string consecutivo = Request.Headers["X-Consecutivo"];
                U_Correo datos = new U_Correo();
                mObjetosParaCorreoAprobacion objetosDeAprobacion = _correoService.CrearObjetoDeCorreo(esquema, consecutivo);
                foreach (var item in objetosDeAprobacion.listadeUsuariosDeClienteDeInforme)
                {

                    U_TokenAprobacionInforme tokenAprobacionRecuperado = informes.EnviarTokenDeAprobacionDeInforme(item.codigo_usuario_cliente, item.consecutivo_informe, objetosDeAprobacion.esquema);
                    if (tokenAprobacionRecuperado != null)
                    {
                        item.token = tokenAprobacionRecuperado.token_aprobacion;

                    }
                }
                datos.correoUsuario = _correoUsuario;
                datos.claveUsuario = _claveUsuario;

                _correoService.EnviarCorreoAprobarInforme(datos, objetosDeAprobacion, _urlWeb, _tipoInicio);
                //_whatsappService.EnviarWhatsappAprobarInforme(objetosDeAprobacion, _tokenWhatsapp, _idTelefonoWhatsapp, _tipoInicio);
                return Ok();
            }

            catch (Exception)
            {
                return BadRequest();
            }
           
           

        }
        [HttpPost("AgregaInformeAsociado")]
        public IActionResult AgregaInformeAsociado([FromBody] U_ClienteAsociado cliente)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];


                string consecutivo = informes.AgregarInformeAsociado(cliente.CLIENTE, esquema);
                if (string.IsNullOrEmpty(consecutivo))
                {
                    return BadRequest();
                }
                return Ok(consecutivo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [AllowAnonymous]
        [HttpGet("ValidaTokenAprobacionDeInforme/{esquema}/{token}")]
        public IActionResult ValidaTokenAprobacionDeInforme(string esquema, string token)
        {
            try
            {
                string tokenAprobacionJson = informes.ValidarTokenAprobacionDeInforme(esquema, token);

                if (string.IsNullOrEmpty(tokenAprobacionJson))
                {
                    return BadRequest();
                }
                return Ok(tokenAprobacionJson);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
           
        }
        [AllowAnonymous]
        [HttpPut("ApruebaInforme")]
        public IActionResult ApruebaInforme([FromBody] U_TokenAprobacionInforme datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];

                string mensaje = informes.AprobarInforme(datos, esquema);
                if (string.IsNullOrEmpty(mensaje))
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
        [AllowAnonymous]
        [HttpPut("RechazaInforme")]
        public IActionResult RechazaInforme([FromBody] U_TokenAprobacionInforme datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];

                string mensaje = informes.RechazarInforme(datos, esquema);
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
        [HttpGet("ValidaExistenciaConsecutivoInforme/{esquema}/{consecutivo}")]
        public IActionResult ValidaExistenciaConsecutivoInforme(string esquema, string consecutivo)
        {
            try
            {
                string consecutivoDevuelto = informes.ValidarExistenciaConsecutivoInforme(esquema, consecutivo);
                return Ok(consecutivoDevuelto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
            
        }

    }
}
