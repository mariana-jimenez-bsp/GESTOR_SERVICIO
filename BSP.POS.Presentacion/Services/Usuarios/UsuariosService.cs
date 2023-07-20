using BSP.POS.Presentacion.Interfaces.Usuarios;
using BSP.POS.Presentacion.Models;
using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BSP.POS.Presentacion.Services.Usuarios
{
    public class UsuariosService : IUsuariosInterface
    {
        private readonly HttpClient _http;
        public mLogin UsuarioLogin { get; set; } = new mLogin();

        public UsuariosService(HttpClient htpp)
        {
            _http = htpp;
        }
        public async Task<mLogin> RealizarLogin(string USUARIO, string CLAVE)
        {
            mLogin enviarUsuario = new mLogin();
            string claveEncriptada = EncriptarClave(CLAVE);
            enviarUsuario.usuario = USUARIO;
            enviarUsuario.clave = claveEncriptada;
            string url = "https://localhost:7032/api/Usuarios/Login";
            string jsonData = JsonSerializer.Serialize(enviarUsuario);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var usuario = await _http.PostAsync(url, content);
            if (usuario.StatusCode == HttpStatusCode.OK)
            {
                return await usuario.Content.ReadFromJsonAsync<mLogin?>();
            }
            return null;
        }

        public string EncriptarClave(string clave)
        {
            byte[] data = Encoding.UTF8.GetBytes(clave);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(data);
                return Convert.ToBase64String(hash);
            }
        }

        public async Task<string>  ValidarToken(string token)
        {
            mLogin enviarUsuario = new mLogin();
            enviarUsuario.token = token;
            string url = "https://localhost:7032/api/Usuarios/ValidarToken";
            string jsonData = JsonSerializer.Serialize(enviarUsuario);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var usuario = await _http.PostAsync(url, content);
            if (usuario.StatusCode == HttpStatusCode.OK)
            {
                return await usuario.Content.ReadAsStringAsync();
            }
            return null;
        }
    }
    }

