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
using BSP.POS.NEGOCIOS.CodigoTelefonoPais;
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
        private readonly string _urlWeb = string.Empty;
        private readonly string _tipoInicio = string.Empty;
        private readonly ICorreosInterface _correoService;
        
        private N_Usuarios user;
        private N_CodigoTelefonoPais _datosTelefono;
        public UsuariosController(ICorreosInterface correoService)
        {
            user = new N_Usuarios();
            _datosTelefono = new N_CodigoTelefonoPais();

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

        [HttpGet("ValidaExistenciaEsquema/{esquema}")]
        public IActionResult ValidaExistenciaEsquema(string esquema)
        {
            try
            {
                string esquemaDevuelto = user.ValidarExistenciaEsquema(esquema);
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
        [HttpGet("ValidaExistenciaDeCodigoUsuario/{esquema}/{codigo}")]
        public IActionResult ValidaExistenciaDeCodigoUsuario(string esquema, string codigo)
        {
            try
            {
                string codigoDevuelto = user.ValidarExistenciaDeCodigoUsuario(esquema, codigo);
                return Ok(codigoDevuelto);
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
        [HttpPut("ActualizarPerfil")]
        public IActionResult ActualizarPerfil([FromBody] U_Perfil datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = user.ActualizarPerfil(datos, esquema);
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
        [HttpGet("ObtengaDatosListaUsuariosDeClienteDeInforme/{consecutivo}/{esquema}")]
        public IActionResult ObtengaDatosListaUsuariosDeClienteDeInforme(string consecutivo, string esquema)
        {
            try
            {
                string listaInformesDeUsuarioDeClienteJson = user.ListarDatosUsuariosDeClienteDeInforme(esquema, consecutivo);
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
        public IActionResult AgregaUsuarioDeClienteDeInforme([FromBody] U_UsuariosDeInforme datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];


                string mensaje = user.AgregarUsuarioDeClienteDeInforme(datos, esquema);
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
        [Authorize]
        [HttpDelete("EliminaUsuarioDeClienteDeInforme")]
        public IActionResult EliminaUsuarioDeClienteDeInforme()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string codigo = Request.Headers["X-Codigo"];


                string mensaje = user.EliminarUsuarioDeClienteDeInforme(codigo, esquema);
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
        [HttpPost("AgregaUsuario")]
        public IActionResult AgregaUsuario([FromBody] U_UsuariosParaEditar datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                int IdCodigoTelefono = int.Parse(Request.Headers["X-IdCodigoTelefono"]);

                string mensaje = user.AgregarUsuario(datos, esquema, IdCodigoTelefono);
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
        [HttpPut("ActualizaElUsuario")]
        public IActionResult ActualizaElUsuario([FromBody] U_UsuariosParaEditar datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                int IdCodigoTelefono = int.Parse(Request.Headers["X-IdCodigoTelefono"]);
                string mensaje = user.ActualizarUsuario(datos, esquema);
                if (string.IsNullOrEmpty(mensaje) || mensaje == "Error")
                {
                    return BadRequest();
                }
                _datosTelefono.ActualizarCodigoTelefonoPaisUsuario(datos.codigo, IdCodigoTelefono, esquema);
                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

    }
}
