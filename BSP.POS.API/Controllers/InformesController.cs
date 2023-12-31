﻿using BSP.POS.API.Models;
using BSP.POS.NEGOCIOS.Actividades;
using BSP.POS.NEGOCIOS.CorreosService;
using BSP.POS.NEGOCIOS.Informes;
using BSP.POS.NEGOCIOS.Usuarios;
using BSP.POS.NEGOCIOS.WhatsappService;
using BSP.POS.UTILITARIOS.Clientes;
using BSP.POS.UTILITARIOS.Correos;
using BSP.POS.UTILITARIOS.CorreosModels;
using BSP.POS.UTILITARIOS.Informes;
using BSP.POS.UTILITARIOS.Reportes;
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
        private N_Actividades _actividades;
        private readonly string _correoUsuario;
        private readonly string _claveUsuario;
        private readonly string _tokenWhatsapp;
        private readonly string _idTelefonoWhatsapp;
        private readonly string _urlWeb = string.Empty;
        private readonly string _tipoInicio = string.Empty;
        private readonly string _urlApiCrystal = string.Empty;
        private readonly ICorreosInterface _correoService;
        private readonly IWhatsappInterface _whatsappService;
        public InformesController(ICorreosInterface correoService, IWhatsappInterface whatsappService)
        {
            informes = new N_Informes();
            user = new N_Usuarios();
            _actividades = new N_Actividades();

            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            _tipoInicio = configuration["AppSettings:TipoInicio"];
            if (_tipoInicio == "debug")
            {
                var configurationSecrets = new ConfigurationBuilder()
                 .AddUserSecrets<Program>()
                 .Build();
                
                _correoUsuario = configurationSecrets["SmtpFrom"];
                _claveUsuario = configurationSecrets["SmtpPassword"];
                _urlWeb = configurationSecrets["UrlWebDebug"];
                _idTelefonoWhatsapp = configurationSecrets["idTelefonoWhatsapp"];
                _tokenWhatsapp = configurationSecrets["tokenWhatsapp"];
                _urlApiCrystal = configurationSecrets["UrlApiCrystalDebug"];
            }
            else
            {
               
                _correoUsuario = configuration["AppSettings:SmtpFrom"];
                _claveUsuario = configuration["AppSettings:SmtpPassword"];
                _urlWeb = configuration["AppSettings:UrlWebDeploy"];
                _idTelefonoWhatsapp = configuration["AppSettings:idTelefonoWhatsapp"];
                _tokenWhatsapp = configuration["AppSettings:tokenWhatsapp"];
                _urlApiCrystal = configuration["AppSettings:UrlApiCrystalDeploy"];
            }

            _correoService = correoService;
            _whatsappService = whatsappService;

        }
        // GET: api/<InformesController>
        [HttpGet("ObtengaLaListaDeInformesDeProyecto/{numero}/{esquema}")]
        public IActionResult ObtengaLaListaDeInformesDeProyecto(string numero, string esquema)
        {
            try
            {
                string listaInformesAsociadosJson = informes.ListarInformesDeProyecto(esquema, numero);
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

        [HttpGet("ObtengaLaListaDeInformes/{esquema}")]
        public IActionResult ObtengaLaListaDeInformes(string esquema)
        {
            try
            {
                string listaInformesAsociadosJson = informes.ListaInformes(esquema);
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

        [HttpGet("ObtengaLaListaDeInformesDeCliente/{cliente}/{esquema}")]
        public IActionResult ObtengaLaListaDeInformesDeCliente(string cliente, string esquema)
        {
            try
            {
                string listaInformesAsociadosJson = informes.ObtenerInformesDeCliente(esquema, cliente);
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

        [HttpGet("ObtengaLaListaDeInformesDeClienteFinalizados/{cliente}/{esquema}")]
        public IActionResult ObtengaLaListaDeInformesDeClienteFinalizados(string cliente, string esquema)
        {
            try
            {
                string listaInformesAsociadosJson = informes.ObtenerInformesDeClienteFinalizados(esquema, cliente);
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

        [HttpGet("ObtengaLaListaDeInformesDeUsuarioFinalizados/{codigo}/{esquema}")]
        public IActionResult ObtengaLaListaDeInformesDeUsuarioFinalizados(string codigo, string esquema)
        {
            try
            {
                string listaInformesAsociadosJson = informes.ObtenerInformesDeUsuarioFinalizados(esquema, codigo);
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

        [HttpGet("ObtengaLaListaDeInformesFinalizados/{esquema}")]
        public IActionResult ObtengaLaListaDeInformesFinalizados(string esquema)
        {
            try
            {
                string listaInformesAsociadosJson = informes.ObtenerInformesFinalizados(esquema);
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

        [HttpGet("ObtengaElInforme/{consecutivo}/{esquema}")]
        public IActionResult ObtengaElInforme(string consecutivo, string esquema)
        {
            try
            {
                var informeAsociadoJson = informes.ObtenerInforme(esquema, consecutivo);
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

        [HttpPut("ActualizaElInforme")]
        public IActionResult ActualizaElInforme([FromBody] U_Informe datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensajeActividad = _actividades.ActualizarListaDeActividadesAsociadas(datos.listaActividadesAsociadas, esquema);
                string mensaje = informes.ActualizarInforme(datos, esquema);
                if (string.IsNullOrEmpty(mensajeActividad) || string.IsNullOrEmpty(mensaje) || mensaje == "Error")
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
        public IActionResult CambiaEstadoDeInforme([FromBody] U_Informe datos)
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
        [HttpPost("EnviarTokenDeRecibidoDeInforme")]
        public ActionResult EnviarTokenDeRecibidoDeInforme([FromBody] U_ObjetoReporte objetoReporte)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string consecutivo = Request.Headers["X-Consecutivo"];
                U_Correo datos = new U_Correo();
                mObjetoParaCorreoInforme objetosParaInforme = _correoService.CrearObjetoDeCorreo(esquema, consecutivo);
                foreach (var item in objetosParaInforme.listadeUsuariosDeClienteDeInforme)
                {

                    U_TokenRecibidoInforme tokenRecibidoRecuperado = informes.EnviarTokenDeRecibidoDeInforme(item.codigo_usuario, item.consecutivo_informe, objetosParaInforme.esquema);
                    if (tokenRecibidoRecuperado != null)
                    {
                        item.token = tokenRecibidoRecuperado.token_recibido;

                    }
                }
                datos.correoUsuario = _correoUsuario;
                datos.claveUsuario = _claveUsuario;

                _correoService.EnviarCorreosInformes(datos, objetosParaInforme, _urlWeb, _tipoInicio, objetoReporte);
                //await _whatsappService.EnviarWhatsappReporteInforme(objetosParaInforme, _tokenWhatsapp, _idTelefonoWhatsapp, _tipoInicio);
                return Ok();
            }

            catch (Exception)
            {
                return BadRequest();
            }
           
           

        }
        [HttpPost("AgregaInformeAsociado")]
        public IActionResult AgregaInformeAsociado()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string numero = Request.Headers["X-Numero"];


                string consecutivo = informes.AgregarInformeAsociado(numero, esquema);
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
        [HttpGet("ValidaTokenRecibidoInforme/{esquema}/{token}")]
        public IActionResult ValidaTokenRecibidoInforme(string esquema, string token)
        {
            try
            {
                string tokenRecibidoJson = informes.ValidarTokenRecibidoInforme(esquema, token);

                if (string.IsNullOrEmpty(tokenRecibidoJson))
                {
                    return BadRequest();
                }
                return Ok(tokenRecibidoJson);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
           
        }
        [AllowAnonymous]
        [HttpPut("ActivaRecibidoInforme")]
        public IActionResult ActivaRecibidoInforme([FromBody] U_TokenRecibidoInforme datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];

                string mensaje = informes.ActivarRecibidoInforme(datos, esquema);
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
