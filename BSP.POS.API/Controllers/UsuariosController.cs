using Microsoft.AspNetCore.Mvc;

using BSP.POS.NEGOCIOS.Usuarios;
using BSP.POS.UTILITARIOS.Usuarios;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using BSP.POS.API.Models.Usuarios;
using BSP.POS.NEGOCIOS.CorreosService;
using BSP.POS.UTILITARIOS.Correos;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata.Ecma335;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly string _secretKey = string.Empty;
        private readonly string _correoUsuario = string.Empty;
        private readonly string _claveUsuario = string.Empty;
        private readonly ICorreosInterface _correoService;

        private N_Usuarios user;
        public UsuariosController(ICorreosInterface correoService)
        {
            user = new N_Usuarios();
            //var configuration = new ConfigurationBuilder()
            // .AddUserSecrets<Program>()
            // .Build();


            _secretKey = Environment.GetEnvironmentVariable("SecretKeyGS");
            _correoUsuario = Environment.GetEnvironmentVariable("SmtpFromGS");
            _claveUsuario = Environment.GetEnvironmentVariable("SmtpPasswordGS");

            //var configuration = new ConfigurationBuilder()
            //.AddJsonFile("appsettings.json")
            //.Build();

            //_secretKey = configuration["AppSettings:SecretKey"];
            //_correoUsuario = configuration["AppSettings:SmtpFrom"];
            //_claveUsuario = configuration["AppSettings:SmtpPassword"];
            _correoService = correoService;

        }
        // GET: api/<UsuariosController>
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

                var usuarioLogeado = user.Login(nuevoLogin);
                if (string.IsNullOrEmpty(usuarioLogeado))
                {
                    return NotFound();
                }
                return Ok(usuarioLogeado);
            }
            catch(Exception ex) 
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
                var tokenValidacion = user.ValidarToken(token, esquema);

                if (string.IsNullOrEmpty(tokenValidacion))
                {
                    return NotFound();
                }
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
            U_TokenRecuperacion tokenRecuperado = user.EnviarTokenRecuperacion(tokenRecuperacion.correo, tokenRecuperacion.esquema);
            try
            {
                if (tokenRecuperado != null)
                {
                    datos.para = tokenRecuperado.correo;
                    datos.correoUsuario = _correoUsuario;
                    datos.claveUsuario = _claveUsuario;
                    string token = tokenRecuperado.token_recuperacion;
                    
                    _correoService.EnviarCorreoRecuperarClave(datos, token, tokenRecuperacion.esquema);
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
                string tokenRecuperadoJson = user.ValidarTokenRecuperacion(esquema, token);
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

        [HttpPost("ActualizaClaveDeUsuario")]
        public IActionResult ActualizaClaveDeUsuario([FromBody] U_UsuarioNuevaClave datos)
        {
            try
            {



                string mensaje = user.ActualizarClaveDeUsuario(datos);
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

        [HttpGet("ValidaExistenciaEsquema/{esquema}")]
        public IActionResult ValidaExistenciaEsquema(string esquema)
        {
            try
            {
                string esquemaDevuelto = user.ValidarExistenciaEsquema(esquema);
                if (string.IsNullOrEmpty(esquemaDevuelto))
                {
                    return NotFound();
                }
                return Ok(esquemaDevuelto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
            
        }
        [HttpGet("ValidaCorreoExistente/{esquema}/{correo}")]
        public IActionResult ValidaCorreoExistente(string esquema, string correo)
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
        [HttpPost("AumentaIntentosDeLogin")]
        public IActionResult AumentaIntentosDeLogin()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string correo = Request.Headers["X-Correo"];
                string mensaje = user.AumentarIntentosDeLogin(esquema, correo);
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

        [HttpGet("ObtengaLosIntentosDeLogin")]
        public IActionResult ObtengaLosIntentosDeLogin()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string correo = Request.Headers["X-Correo"];
                int intentos = user.ObtenerIntentosDeLogin(esquema, correo);
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
        [Authorize]
        [HttpGet("ValidaUsuarioExistente/{esquema}/{usuario}")]
        public IActionResult ValidaUsuarioExistente(string esquema, string usuario)
        {
            try
            {
                string usuarioDevuelto = user.ValidarUsuarioExistente(esquema, usuario);
                return Ok(usuarioDevuelto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }

        }

        [Authorize]
        [HttpGet("ObtenerPerfil/{usuario}/{esquema}")]
        public IActionResult ObtenerPerfil(string usuario, string esquema)
        {
            try
            {
                var perfil = user.ObtenerPerfil(esquema, usuario);
                if (string.IsNullOrEmpty(perfil))
                {
                    return NotFound();
                }
                return Ok(perfil);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [Authorize]
        [HttpPost("ActualizarPerfil")]
        public IActionResult ActualizarPerfil([FromBody] U_Perfil datos)
        {
            try
            {
                string mensaje = user.ActualizarPerfil(datos);
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
        [Authorize]
        [HttpGet("ObtengaLaListaDeUsuarios/{esquema}")]
        public IActionResult ObtengaLaListaDeUsuarios(string esquema)
        {
            try
            {
                string listaDeUsuariosJson = user.ListarUsuarios(esquema);
                if (string.IsNullOrEmpty(listaDeUsuariosJson))
                {
                    return NotFound();
                }
                return Ok(listaDeUsuariosJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [Authorize]
        [HttpGet("ObtengaLaListaDeUsuariosDeClienteAsociados/{esquema}/{cliente}")]
        public IActionResult ObtengaLaListaDeUsuariosDeClienteAsociados(string esquema,string cliente)
        {
            try
            {
                string listaUsuariosDeClienteAsociadosJson = user.ListarUsuariosDeClienteAsociados(esquema, cliente);
                if (string.IsNullOrEmpty(listaUsuariosDeClienteAsociadosJson))
                {
                    return NotFound();
                }
                return Ok(listaUsuariosDeClienteAsociadosJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [Authorize]
        [HttpGet("ObtengaLaListaUsuariosDeClienteDeInforme/{consecutivo}/{esquema}")]
        public IActionResult ObtengaLaListaUsuariosDeClienteDeInforme(string consecutivo, string esquema)
        {
            try
            {
                string listaInformesDeUsuarioDeClienteJson = user.ListarUsuariosDeClienteDeInforme(esquema, consecutivo);
                if (string.IsNullOrEmpty(listaInformesDeUsuarioDeClienteJson))
                {
                    return NotFound();
                }
                return Ok(listaInformesDeUsuarioDeClienteJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [Authorize]
        [HttpPost("AgregaUsuarioDeClienteDeInforme")]
        public IActionResult AgregaUsuarioDeClienteDeInforme([FromBody] U_UsuariosDeClienteDeInforme datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];


                string mensaje = user.AgregarUsuarioDeClienteDeInforme(datos, esquema);
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
        [Authorize]
        [HttpDelete("EliminaUsuarioDeClienteDeInforme")]
        public IActionResult EliminaUsuarioDeClienteDeInforme()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string idUsuario = Request.Headers["X-IdUsuario"];


                string mensaje = user.EliminarUsuarioDeClienteDeInforme(idUsuario, esquema);
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

        [Authorize]
        [HttpGet("ObtengaImagenUsuario/{usuario}/{esquema}")]
        public IActionResult ObtengaImagenUsuario(string usuario, string esquema)
        {
            try
            {
                var imagen = user.ObtenerImagenDeUsuario(esquema, usuario);
                if (string.IsNullOrEmpty(imagen))
                {
                    return NotFound();
                }
                return Ok(imagen);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [Authorize]
        [HttpGet("ObtengaListaDeInformesDeUsuario/{codigo}/{esquema}")]
        public IActionResult ObtengaListaDeInformesDeUsuario(string codigo, string esquema)
        {
            try
            {
                string listaInformesDeUsuarioDeClienteJson = user.ObtenerListaDeInformesDeUsuarioDeInforme(esquema, codigo);
                if (string.IsNullOrEmpty(listaInformesDeUsuarioDeClienteJson))
                {
                    return NotFound();
                }
                return Ok(listaInformesDeUsuarioDeClienteJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Authorize]
        [HttpGet("ObtengaLaListaDeUsuariosParaEditar/{esquema}")]
        public IActionResult ObtengaLaListaDeUsuariosParaEditar(string esquema)
        {
            try
            {
                string listaDeUsuariosJson = user.ListarUsuariosParaEditar(esquema);
                if (string.IsNullOrEmpty(listaDeUsuariosJson))
                {
                    return NotFound();
                }
                return Ok(listaDeUsuariosJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [Authorize]
        [HttpPost("ActualizaListaDeUsuarios")]
        public IActionResult ActualizaListaDeUsuarios([FromBody] List<U_UsuariosParaEditar> datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = user.ActualizarListaDeUsuarios(datos, esquema);
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
        [Authorize]
        [HttpPost("AgregaUsuario")]
        public IActionResult AgregaUsuario([FromBody] U_UsuariosParaEditar datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];

                string mensaje = user.AgregarUsuario(datos, esquema);
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

        [Authorize]
        [HttpGet("ObtengaElUsuarioParaEditar/{esquema}/{codigo}")]
        public IActionResult ObtengaElUsuarioParaEditar(string esquema, string codigo)
        {
            try
            {
                string usuarioParaEditarJson = user.ObtenerUsuarioParaEditar(esquema, codigo);
                if (string.IsNullOrEmpty(usuarioParaEditarJson))
                {
                    return NotFound();
                }
                return Ok(usuarioParaEditarJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Authorize]
        [HttpPost("ActualizaElUsuario")]
        public IActionResult ActualizaElUsuario([FromBody] U_UsuariosParaEditar datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = user.ActualizarUsuario(datos, esquema);
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
