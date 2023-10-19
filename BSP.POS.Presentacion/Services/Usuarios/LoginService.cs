using Blazored.LocalStorage;
using BSP.POS.Presentacion.Interfaces.Usuarios;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using BSP.POS.Presentacion.Pages.Usuarios.Usuarios;
using BSP.POS.Presentacion.Models.Licencias;
using clSeguridad;

namespace BSP.POS.Presentacion.Services.Usuarios
{
    public class LoginService : ILoginInterface
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorageService;
        private readonly NavigationManager _navigationManager;
        public mLogin UsuarioLogin { get; set; } = new mLogin();
        public mTokenRecuperacion UsuarioRecuperacion { get; set; } = new mTokenRecuperacion();

        public LoginService(HttpClient htpp, ILocalStorageService localStorageService, NavigationManager navigationManager)
        {
            _http = htpp;
            _localStorageService = localStorageService;
            _navigationManager = navigationManager;
        }

        public async Task<mLogin> RealizarLogin(mLogin usuarioLog)
        {

            string claveEncriptada = EncriptarClave(usuarioLog.clave);
            usuarioLog.clave = claveEncriptada;
            string url = "Login/Login";
            string jsonData = JsonSerializer.Serialize(usuarioLog);
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
            Cryptografia _cryptografia = new Cryptografia();
            //byte[] data = Encoding.UTF8.GetBytes(clave);
            //using (SHA256 sha256 = SHA256.Create())
            //{
            //    byte[] hash = sha256.ComputeHash(data);
            //    return Convert.ToBase64String(hash);
            //}
            return _cryptografia.EncryptString(clave, "BSP");
        }

        public async Task<bool> EnviarCorreoRecuperarClave(mTokenRecuperacion tokenRecuperacion)
        {
            try
            {
                string url = "Login/EnviarTokenRecuperacion";
                string jsonData = JsonSerializer.Serialize(tokenRecuperacion);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync(url, content);

                if (response.StatusCode == HttpStatusCode.OK)
                {


                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }


        }
        public async Task<mTokenRecuperacion> ValidarTokenRecuperacion(string esquema, string token)
        {
            string url = "Login/ValidaTokenRecuperacion/" + esquema + "/" + token;
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var tokenRecuperacion = await response.Content.ReadFromJsonAsync<mTokenRecuperacion>();
                if (tokenRecuperacion is not null)
                {
                    return tokenRecuperacion;
                }
                else
                {
                    return new mTokenRecuperacion();
                }
            }
            return new mTokenRecuperacion();
        }

        public async Task ActualizarClaveDeUsuario(mUsuarioNuevaClave usuario)
        {
            usuario.clave = EncriptarClave(usuario.clave);
            usuario.confirmarClave = usuario.clave;
            string url = "Login/ActualizaClaveDeUsuario";
            string jsonData = JsonSerializer.Serialize(usuario);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync(url, content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                _navigationManager.NavigateTo($"", forceLoad: true);
            }
        }

        public async Task<string> ValidarCorreoCambioClave(string esquema, string correo)
        {
            string url = "Login/ValidaCorreoCambioClave/" + esquema + "/" + correo;
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string correoDevuelto = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(correoDevuelto))
                {
                    return correoDevuelto;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public async Task AumentarIntentosDeLogin(string esquema, string usuario)
        {
            string url = "Login/AumentaIntentosDeLogin";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Remove("X-Usuario");
            _http.DefaultRequestHeaders.Add("X-Usuario", usuario);

            var content = new StringContent("");

            var response = await _http.PostAsync(url, content);
            if (response.StatusCode == HttpStatusCode.OK)
            {

            }

        }
        public async Task<int> ObtenerIntentosDeLogin(string esquema, string usuario)
        {
            string url = "Login/ObtengaLosIntentosDeLogin";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Remove("X-Usuario");
            _http.DefaultRequestHeaders.Add("X-Usuario", usuario);
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string intentosString = await response.Content.ReadAsStringAsync();
                int intentos = int.Parse(intentosString);
                return intentos;
            }
            else
            {
                throw new Exception("Error al obtener los intentos");
            }

        }

        public async Task<mLicencia> EnviarXMLLicencia(mLicenciaByte licenciaLlave)
        {
            string url = "Licencias/ConectaApiEnviaXML";
            
            string jsonData = JsonSerializer.Serialize(licenciaLlave);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(url, content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                    
                    var datosLicencia = await response.Content.ReadFromJsonAsync<mLicencia>();
                    if (datosLicencia != null)
                    {
                        return datosLicencia;
                    }
                    return null;
                
              
            }
            else
            {
                return null;
            }
        }
    }
}
