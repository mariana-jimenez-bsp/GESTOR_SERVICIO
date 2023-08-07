using Blazored.LocalStorage;
using BSP.POS.Presentacion.Interfaces.Usuarios;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.UTILITARIOS.Informes;
using BSP.POS.UTILITARIOS.Proyectos;
using BSP.POS.UTILITARIOS.Usuarios;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
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
        public mPerfil Perfil { get; set; } = new mPerfil();
        private readonly ILocalStorageService _localStorageService;
        private readonly NavigationManager _navigationManager;
        public List<mUsuariosDeCliente> ListaDeUsuariosDeCliente { get; set; } = new List<mUsuariosDeCliente>();
        public List<mUsuariosDeClienteDeInforme> ListaUsuariosDeClienteDeInforme { get; set; } = new List<mUsuariosDeClienteDeInforme>();
        public List<mPerfil> ListaDeUsuarios { get; set; } = new List<mPerfil>();
        public mTokenRecuperacion UsuarioRecuperacion { get; set; } = new mTokenRecuperacion();
        public UsuariosService(HttpClient htpp, ILocalStorageService localStorageService, NavigationManager navigationManager)
        {
            _http = htpp;
            _localStorageService = localStorageService;
            _navigationManager = navigationManager;
        }
        public async Task<mLogin> RealizarLogin(mLogin usuarioLog)
        {

            string claveEncriptada = EncriptarClave(usuarioLog.clave);
            usuarioLog.clave = claveEncriptada;
            string url = "https://localhost:7032/api/Usuarios/Login";
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
            byte[] data = Encoding.UTF8.GetBytes(clave);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(data);
                return Convert.ToBase64String(hash);
            }
        }
        
        public async Task ObtenerPerfil(string usuario)
        {
            var perfilJson = await _http.GetAsync("https://localhost:7032/api/Usuarios/ObtenerPerfil/" + usuario);
            if (perfilJson.StatusCode == HttpStatusCode.OK)
            {
                Perfil = await perfilJson.Content.ReadFromJsonAsync<mPerfil?>();

            }

        }

        public async Task ActualizarPefil(mPerfil perfil, string usuarioOriginal, string claveOriginal)
        {


                if (!string.IsNullOrEmpty(perfil.clave))
                {
                    perfil.clave = EncriptarClave(perfil.clave);
                }
                string url = "https://localhost:7032/api/Usuarios/ActualizarPerfil";
                string jsonData = JsonSerializer.Serialize(perfil);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var mensaje = await _http.PostAsync(url, content);
                if(usuarioOriginal != perfil.usuario || (claveOriginal != perfil.clave && !string.IsNullOrEmpty(perfil.clave)))
                {
                    
                    _navigationManager.NavigateTo("/login", forceLoad: true);
                    await _localStorageService.RemoveItemAsync("token");
                }
            
        }
        public async Task ObtenerListaDeUsuarios(string esquema)
        {
            var listaDeUsuarios = await _http.GetFromJsonAsync<List<mPerfil>>("https://localhost:7032/api/Usuarios/ObtengaLaListaDeUsuarios/" + esquema);
            if (listaDeUsuarios is not null)
            {
                ListaDeUsuarios = listaDeUsuarios;
            }
        }
        public async Task<List<mUsuariosDeCliente>> ObtenerListaDeUsuariosDeClienteAsociados(string esquema, string cliente)
        {
            string url = "https://localhost:7032/api/Usuarios/ObtengaLaListaDeUsuariosDeClienteAsociados/" + esquema + "/" + cliente;
            var listaDeUsuariosDeClientesAsociados = await _http.GetFromJsonAsync<List<mUsuariosDeCliente>>(url);
            if (listaDeUsuariosDeClientesAsociados is not null)
            {
                return listaDeUsuariosDeClientesAsociados;
            }
            else
            {
                return new List<mUsuariosDeCliente>();
            }
        }

        public async Task<bool> EnviarCorreoRecuperarClave(mTokenRecuperacion tokenRecuperacion)
        {
            try
            {
                string url = "https://localhost:7032/api/Usuarios/EnviarTokenRecuperacion";
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
            string url = "https://localhost:7032/api/Usuarios/ValidaTokenRecuperacion/" + esquema + "/" + token;
            var tokenRecuperacion = await _http.GetFromJsonAsync<mTokenRecuperacion>(url);
            if (tokenRecuperacion is not null)
            {
                return tokenRecuperacion;
            }
            else
            {
                return new mTokenRecuperacion();
            }
        }

        public async Task ActualizarClaveDeUsuario(mUsuarioNuevaClave usuario)
        {
            usuario.clave = EncriptarClave(usuario.clave);
            usuario.confirmarClave = usuario.clave;
            string url = "https://localhost:7032/api/Usuarios/ActualizaClaveDeUsuario";
            string jsonData = JsonSerializer.Serialize(usuario);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync(url, content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                _navigationManager.NavigateTo("/", forceLoad: true);
            }
        }

        public async Task<string> ValidarCorreoCambioClave(string esquema, string correo)
        {
            string url = "https://localhost:7032/api/Usuarios/ValidaCorreoCambioClave/" + esquema + "/" + correo;
            string correoDevuelto = await _http.GetStringAsync(url);
            if (!string.IsNullOrEmpty(correoDevuelto))
            {
                return correoDevuelto;
            }
            else
            {
                return null;
            }
        }

        public async Task ObtenerListaUsuariosDeClienteDeInforme(string consecutivo, string esquema)
        {
            string url = "https://localhost:7032/api/Usuarios/ObtengaLaListaUsuariosDeClienteDeInforme/" + consecutivo + "/" + esquema;
            var listaInformesAsociados = await _http.GetFromJsonAsync<List<mUsuariosDeClienteDeInforme>>(url);
            if (listaInformesAsociados is not null)
            {
                ListaUsuariosDeClienteDeInforme = listaInformesAsociados;
            }
        }

        public async Task AgregarUsuarioDeClienteDeInforme(mUsuariosDeClienteDeInforme usuario, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "https://localhost:7032/api/Usuarios/AgregaUsuarioDeClienteDeInforme";
                string jsonData = JsonSerializer.Serialize(usuario);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var mensaje = await _http.PostAsync(url, content);
            }
            catch (Exception)
            {

            }
        }

        public async Task EliminarUsuarioDeClienteDeInforme(string idUsuario, string esquema)
        {
            try
            {
                string url = "https://localhost:7032/api/Usuarios/EliminaUsuarioDeClienteDeInforme";
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                _http.DefaultRequestHeaders.Remove("X-IdUsuario");
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                _http.DefaultRequestHeaders.Add("X-IdUsuario", idUsuario);
               

                var mensaje = await _http.DeleteAsync(url);
            }
            catch (Exception)
            {

            }
        }

    }
    }

