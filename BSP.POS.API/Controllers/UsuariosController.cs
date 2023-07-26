using Microsoft.AspNetCore.Mvc;
using BSP.POS.DATOS.Usuarios;
using BSP.POS.NEGOCIOS.Usuarios;
using BSP.POS.UTILITARIOS.Usuarios;
using BSP.POS.API.Models;
using clSeguridad;
using System;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using BSP.POS.UTILITARIOS.Tiempos;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly string _secretKey;

        private N_Usuarios user;
        public UsuariosController()
        {
            user = new N_Usuarios();
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            _secretKey = configuration["AppSettings:SecretKey"];
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

       


    }
}
