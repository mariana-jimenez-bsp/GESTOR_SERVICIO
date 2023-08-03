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
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly string _secretKey;
        private readonly string _correoUsuario;
        private readonly string _claveUsuario;
        private readonly ICorreosInterface _correoService;

        private N_Usuarios user;
        public UsuariosController(ICorreosInterface correoService)
        {
            user = new N_Usuarios();
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            _secretKey = configuration["AppSettings:SecretKey"];
            _correoUsuario = configuration["AppSettings:SmtpFrom"];
            _claveUsuario = configuration["AppSettings:SmtpPassword"];
            _correoService = correoService;
        }
        // GET: api/<UsuariosController>
        [HttpPost("Login")]
        public string Login([FromBody] mLogin datos)
        {
            try
            {
                U_Login nuevoLogin = new U_Login();
                nuevoLogin.usuario = datos.usuario;
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
        public string ValidarToken([FromBody] mLogin datos)
        {
            try
            {
                string token = datos.token.Trim('"');
                var usuarioLogeado = user.ValidarToken(token);

                return usuarioLogeado;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [HttpPost("EnviarCorreo")]
        public IActionResult EnviarCorreo(U_TokenRecuperacion tokenRecuperacion)
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

                    _correoService.EnviarCorreo(datos, token, tokenRecuperacion.esquema);
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
        public string ActualizaClaveDeUsuario([FromBody] mUsuarioNuevaClave datos)
        {
            try
            {
                U_UsuarioNuevaClave usuario = new U_UsuarioNuevaClave();
                usuario.token_recuperacion = datos.token_recuperacion;
                usuario.clave = datos.clave;
                usuario.esquema = datos.esquema;


                string mensaje = user.ActualizarClaveDeUsuario(usuario);
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

            string correoDevuelto = user.ValidarCorreoExistenteCambioClave(esquema, correo);

            return correoDevuelto;
        }

        [Authorize]
        [HttpGet("ObtenerPerfil/{usuario}")]
        public string ObtenerPerfil(string usuario)
        {
            try
            {
                string esquema = "BSP";
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
        public string ActualizarPerfil([FromBody] mPerfil datos)
        {
            try
            {
                U_Perfil perfil = new U_Perfil();
                    perfil.id = datos.id;
                    perfil.correo = datos.correo;
                    perfil.clave = datos.clave;
                    perfil.usuario = datos.usuario;
                    perfil.nombre = datos.nombre;
                    perfil.rol = datos.rol;
                    perfil.telefono = datos.telefono;
                    perfil.esquema = datos.esquema;


                string mensaje = user.ActualizarPerfil(perfil);
                return mensaje;
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
        public string AgregaUsuarioDeClienteDeInforme([FromBody] mUsuarioDeClienteDeInforme datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                U_UsuariosDeClienteDeInforme usuario = new U_UsuariosDeClienteDeInforme();
                usuario.consecutivo_informe = datos.consecutivo_informe;
                usuario.codigo_usuario_cliente = datos.codigo_usuario_cliente;


                string mensaje = user.AgregarUsuarioDeClienteDeInforme(usuario, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


    }
}
