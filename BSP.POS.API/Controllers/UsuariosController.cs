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
        public string Login([FromBody] mLogin datos)
        {
            try
            {
                U_Login nuevoLogin = new U_Login();
                nuevoLogin.correo = datos.correo;
                nuevoLogin.contrasena = datos.clave;
                nuevoLogin.esquema = datos.esquema;
                nuevoLogin.key = _secretKey;

                var usuarioLogeado = user.Login(nuevoLogin);
                return usuarioLogeado;
            }
            catch(Exception ex) 
            {
                return ex.Message;
            }
           
        }

        [HttpPost("ValidarToken")]
        public string ValidarToken([FromBody] U_LoginToken datos)
        {
            try
            {
                string token = datos.token.Trim('"');
                string esquema = datos.esquema.Trim('"');
                var usuarioLogeado = user.ValidarToken(token, esquema);

                return usuarioLogeado;
            }

            catch (Exception ex)
            {
                return ex.Message;
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
        public string ValidaTokenRecuperacion(string esquema, string token)
        {

            string tokenRecuperadoJson = user.ValidarTokenRecuperacion(esquema, token);

            return tokenRecuperadoJson;
        }

        [HttpPost("ActualizaClaveDeUsuario")]
        public string ActualizaClaveDeUsuario([FromBody] U_UsuarioNuevaClave datos)
        {
            try
            {



                string mensaje = user.ActualizarClaveDeUsuario(datos);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpGet("ValidaCorreoCambioClave/{esquema}/{correo}")]
        public string ValidaCorreoCambioClave(string esquema, string correo)
        {

            string correoDevuelto = user.ValidarCorreoExistente(esquema, correo);

            return correoDevuelto;
        }

        [HttpGet("ValidaExistenciaEsquema/{esquema}")]
        public string ValidaExistenciaEsquema(string esquema)
        {

            string esquemaDevuelto = user.ValidarExistenciaEsquema(esquema);

            return esquemaDevuelto;
        }
        [HttpGet("ValidaCorreoExistente/{esquema}/{correo}")]
        public string ValidaCorreoExistente(string esquema, string correo)
        {

            string correoDevuelto = user.ValidarCorreoExistente(esquema, correo);

            return correoDevuelto;
        }
        [HttpPost("AumentaIntentosDeLogin")]
        public string AumentaIntentosDeLogin()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string correo = Request.Headers["X-Correo"];
                string mensaje = user.AumentarIntentosDeLogin(esquema, correo);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpGet("ObtengaLosIntentosDeLogin")]
        public string ObtengaLosIntentosDeLogin()
        {
                string esquema = Request.Headers["X-Esquema"];
                string correo = Request.Headers["X-Correo"];
                int intentos = user.ObtenerIntentosDeLogin(esquema, correo);
                return intentos.ToString();

        }
        [Authorize]
        [HttpGet("ValidaUsuarioExistente/{esquema}/{usuario}")]
        public string ValidaUsuarioExistente(string esquema, string usuario)
        {

            string usuarioDevuelto = user.ValidarUsuarioExistente(esquema, usuario);

            return usuarioDevuelto;
        }

        [Authorize]
        [HttpGet("ObtenerPerfil/{usuario}/{esquema}")]
        public string ObtenerPerfil(string usuario, string esquema)
        {
            try
            {
                var perfil = user.ObtenerPerfil(esquema, usuario);
                return perfil;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        [Authorize]
        [HttpPost("ActualizarPerfil")]
        public string ActualizarPerfil([FromBody] U_Perfil datos)
        {
            try
            {
                

                string mensaje = user.ActualizarPerfil(datos);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        [Authorize]
        [HttpGet("ObtengaLaListaDeUsuarios/{esquema}")]
        public string ObtengaLaListaDeUsuarios(string esquema)
        {
            try
            {
                string listaDeUsuariosJson = user.ListarUsuarios(esquema);
                return listaDeUsuariosJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        [Authorize]
        [HttpGet("ObtengaLaListaDeUsuariosDeClienteAsociados/{esquema}/{cliente}")]
        public string ObtengaLaListaDeUsuariosDeClienteAsociados(string esquema,string cliente)
        {
            try
            {
                string listaUsuariosDeClienteAsociadosJson = user.ListarUsuariosDeClienteAsociados(esquema, cliente);
                return listaUsuariosDeClienteAsociadosJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        [Authorize]
        [HttpGet("ObtengaLaListaUsuariosDeClienteDeInforme/{consecutivo}/{esquema}")]
        public string ObtengaLaListaUsuariosDeClienteDeInforme(string consecutivo, string esquema)
        {
            try
            {
                string listaInformesDeUsuarioDeClienteJson = user.ListarUsuariosDeClienteDeInforme(esquema, consecutivo);
                return listaInformesDeUsuarioDeClienteJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        [Authorize]
        [HttpPost("AgregaUsuarioDeClienteDeInforme")]
        public string AgregaUsuarioDeClienteDeInforme([FromBody] U_UsuariosDeClienteDeInforme datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];


                string mensaje = user.AgregarUsuarioDeClienteDeInforme(datos, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        [Authorize]
        [HttpDelete("EliminaUsuarioDeClienteDeInforme")]
        public string EliminaUsuarioDeClienteDeInforme()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string idUsuario = Request.Headers["X-IdUsuario"];


                string mensaje = user.EliminarUsuarioDeClienteDeInforme(idUsuario, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [Authorize]
        [HttpGet("ObtengaImagenUsuario/{usuario}/{esquema}")]
        public string ObtengaImagenUsuario(string usuario, string esquema)
        {
            try
            {
                var imagen = user.ObtenerImagenDeUsuario(esquema, usuario);
                return imagen;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        [Authorize]
        [HttpGet("ObtengaListaDeInformesDeUsuario/{codigo}/{esquema}")]
        public string ObtengaListaDeInformesDeUsuario(string codigo, string esquema)
        {
            try
            {
                string listaInformesDeUsuarioDeClienteJson = user.ObtenerListaDeInformesDeUsuarioDeInforme(esquema, codigo);
                return listaInformesDeUsuarioDeClienteJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [Authorize]
        [HttpGet("ObtengaLaListaDeUsuariosParaEditar/{esquema}")]
        public string ObtengaLaListaDeUsuariosParaEditar(string esquema)
        {
            try
            {
                string listaDeUsuariosJson = user.ListarUsuariosParaEditar(esquema);
                return listaDeUsuariosJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        [Authorize]
        [HttpPost("ActualizaListaDeUsuarios")]
        public string ActualizaListaDeUsuarios([FromBody] List<U_UsuariosParaEditar> datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = user.ActualizarListaDeUsuarios(datos, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        [Authorize]
        [HttpPost("AgregaUsuario")]
        public string AgregaUsuario([FromBody] U_UsuariosParaEditar datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];

                string mensaje = user.AgregarUsuario(datos, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [Authorize]
        [HttpGet("ObtengaElUsuarioParaEditar/{esquema}/{codigo}")]
        public string ObtengaElUsuarioParaEditar(string esquema, string codigo)
        {
            try
            {
                string usuarioParaEditarJson = user.ObtenerUsuarioParaEditar(esquema, codigo);
                return usuarioParaEditarJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [Authorize]
        [HttpPost("ActualizaElUsuario")]
        public string ActualizaElUsuario([FromBody] U_UsuariosParaEditar datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = user.ActualizarUsuario(datos, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

    }
}
