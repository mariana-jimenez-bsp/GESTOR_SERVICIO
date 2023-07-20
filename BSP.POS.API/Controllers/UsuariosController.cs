using Microsoft.AspNetCore.Mvc;
using BSP.POS.DATOS.Usuarios;
using BSP.POS.NEGOCIOS.Usuarios;
using BSP.POS.UTILITARIOS.Usuarios;
using BSP.POS.API.Models;
using clSeguridad;
using System;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly string _secretKey;

        Cryptografia _Cryptografia = new Cryptografia();
        private N_Login login;
        public UsuariosController()
        {
            login = new N_Login();
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
                nuevoLogin.esquema = "BSP";
                nuevoLogin.key = _secretKey;

                var usuarioLogeado = login.Login(nuevoLogin);
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
                var usuarioLogeado = login.ValidarToken(token);

                return usuarioLogeado;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        } 

       
    }
}
