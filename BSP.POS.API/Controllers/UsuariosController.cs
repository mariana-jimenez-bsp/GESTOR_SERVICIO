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
            U_Login nuevoLogin = new U_Login();
            nuevoLogin.usuario = datos.usuario;
            nuevoLogin.contrasena = datos.clave;
            nuevoLogin.esquema = "BSP";
            nuevoLogin.key = _secretKey;

            var usuarioLogeado = login.Login(nuevoLogin);
            return usuarioLogeado;
        }

        [HttpPost("ValidarToken")]
        public string ValidarToken([FromBody] mLogin datos)
        {
            var usuarioLogeado = login.ValidarToken(datos.token);
            return usuarioLogeado;
        }

        // GET api/<UsuariosController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsuariosController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UsuariosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsuariosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
