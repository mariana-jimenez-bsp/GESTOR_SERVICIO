﻿using BSP.POS.API.Models.Usuarios;
using BSP.POS.NEGOCIOS.CorreosService;
using BSP.POS.NEGOCIOS.Usuarios;
using BSP.POS.UTILITARIOS.Correos;
using BSP.POS.UTILITARIOS.Usuarios;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly string _secretKey = string.Empty;
        private readonly string _correoUsuario = string.Empty;
        private readonly string _claveUsuario = string.Empty;
        private readonly string _urlWeb = string.Empty;
        private readonly string _tipoInicio = string.Empty;
        private readonly ICorreosInterface _correoService;
        
        private N_Usuarios user;
        private N_Login login;

        public LoginController(ICorreosInterface correoService)
        {
            user = new N_Usuarios();
            login = new N_Login();

            

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

        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] mLogin datos)
        {
            try
            {
                U_Login nuevoLogin = new U_Login();
                nuevoLogin.correo = datos.correo;
                nuevoLogin.contrasena = datos.clave;
                nuevoLogin.esquema = datos.esquema;
                nuevoLogin.key = _secretKey;

                var usuarioLogeado = login.Login(nuevoLogin);
                if (string.IsNullOrEmpty(usuarioLogeado))
                {
                    return NotFound();
                }
                return Ok(usuarioLogeado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("ValidarToken")]
        public IActionResult ValidarToken([FromBody] U_LoginToken datos)
        {
            try
            {
                string token = datos.token.Trim('"');
                string esquema = datos.esquema.Trim('"');
                var tokenValidacion = login.ValidarToken(token, esquema);

                return Ok(tokenValidacion);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("EnviarTokenRecuperacion")]
        public IActionResult EnviarTokenRecuperacion(U_TokenRecuperacion tokenRecuperacion)
        {
            U_Correo datos = new U_Correo();
            U_TokenRecuperacion tokenRecuperado = login.EnviarTokenRecuperacion(tokenRecuperacion.correo, tokenRecuperacion.esquema);
            try
            {
                if (tokenRecuperado != null)
                {
                    datos.para = tokenRecuperado.correo;
                    datos.correoUsuario = _correoUsuario;
                    datos.claveUsuario = _claveUsuario;
                    string token = tokenRecuperado.token_recuperacion;

                    _correoService.EnviarCorreoRecuperarClave(datos, token, tokenRecuperacion.esquema, _urlWeb, _tipoInicio);
                    return Ok();
                }
                return BadRequest();
            }

            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet("ValidaTokenRecuperacion/{esquema}/{token}")]
        public IActionResult ValidaTokenRecuperacion(string esquema, string token)
        {
            try
            {
                string tokenRecuperadoJson = login.ValidarTokenRecuperacion(esquema, token);
                if (string.IsNullOrEmpty(tokenRecuperadoJson))
                {
                    return NotFound();
                }
                return Ok(tokenRecuperadoJson);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("ActualizaClaveDeUsuario")]
        public IActionResult ActualizaClaveDeUsuario([FromBody] U_UsuarioNuevaClave datos)
        {
            try
            {



                string mensaje = login.ActualizarClaveDeUsuario(datos);
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

        [HttpGet("ValidaCorreoCambioClave/{esquema}/{correo}")]
        public IActionResult ValidaCorreoCambioClave(string esquema, string correo)
        {
            try
            {
                string correoDevuelto = user.ValidarCorreoExistente(esquema, correo);
                return Ok(correoDevuelto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("AumentaIntentosDeLogin")]
        public IActionResult AumentaIntentosDeLogin()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string correo = Request.Headers["X-Correo"];
                string mensaje = login.AumentarIntentosDeLogin(esquema, correo);
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

        [HttpGet("ObtengaLosIntentosDeLogin")]
        public IActionResult ObtengaLosIntentosDeLogin()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string correo = Request.Headers["X-Correo"];
                int intentos = login.ObtenerIntentosDeLogin(esquema, correo);
                if (string.IsNullOrEmpty(intentos.ToString()))
                {
                    return NotFound();
                }
                return Ok(intentos.ToString());
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }

        }

       
    }
}
