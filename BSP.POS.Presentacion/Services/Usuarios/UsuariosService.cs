using Blazored.LocalStorage;
using BSP.POS.Presentacion.Interfaces.Usuarios;
using BSP.POS.Presentacion.Models.Usuarios;
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
        public mImagenUsuario ImagenDeUsuario { get; set; } = new mImagenUsuario();
        public List<mUsuariosDeClienteDeInforme> ListaDeInformesDeUsuarioAsociados { get; set; } = new List<mUsuariosDeClienteDeInforme>();
        public List<mUsuariosParaEditar> ListaDeUsuariosParaEditar { get; set; } = new List<mUsuariosParaEditar>();

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
            string url = "Usuarios/Login";
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
        
        public async Task ObtenerPerfil(string usuario, string esquema)
        {
            string url = "Usuarios/ObtenerPerfil/" + usuario + "/" + esquema;
            var perfilJson = await _http.GetAsync(url);
            if (perfilJson.StatusCode == HttpStatusCode.OK)
            {
                Perfil = await perfilJson.Content.ReadFromJsonAsync<mPerfil?>();

            }

        }

        public async Task ActualizarPefil(mPerfil perfil, string usuarioOriginal, string claveOriginal, string correoOriginal)
        {


                if (!string.IsNullOrEmpty(perfil.clave))
                {
                    perfil.clave = EncriptarClave(perfil.clave);
                }
                string url = "Usuarios/ActualizarPerfil";
                string jsonData = JsonSerializer.Serialize(perfil);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var mensaje = await _http.PostAsync(url, content);
                if(usuarioOriginal != perfil.usuario || correoOriginal != perfil.correo || (claveOriginal != perfil.clave && !string.IsNullOrEmpty(perfil.clave)))
                {
                    
                    _navigationManager.NavigateTo("/login", forceLoad: true);
                    await _localStorageService.RemoveItemAsync("token");
                }
            
        }
        public async Task ObtenerListaDeUsuarios(string esquema)
        {
            var listaDeUsuarios = await _http.GetFromJsonAsync<List<mPerfil>>("Usuarios/ObtengaLaListaDeUsuarios/" + esquema);
            if (listaDeUsuarios is not null)
            {
                ListaDeUsuarios = listaDeUsuarios;
            }
        }
        public async Task<List<mUsuariosDeCliente>> ObtenerListaDeUsuariosDeClienteAsociados(string esquema, string cliente)
        {
            string url = "Usuarios/ObtengaLaListaDeUsuariosDeClienteAsociados/" + esquema + "/" + cliente;
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
                string url = "Usuarios/EnviarTokenRecuperacion";
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
            string url = "Usuarios/ValidaTokenRecuperacion/" + esquema + "/" + token;
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
            string url = "Usuarios/ActualizaClaveDeUsuario";
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
            string url = "Usuarios/ValidaCorreoCambioClave/" + esquema + "/" + correo;
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
            string url = "Usuarios/ObtengaLaListaUsuariosDeClienteDeInforme/" + consecutivo + "/" + esquema;
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
                string url = "Usuarios/AgregaUsuarioDeClienteDeInforme";
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
                string url = "Usuarios/EliminaUsuarioDeClienteDeInforme";
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
        public async Task ObtenerImagenDeUsuario(string usuario, string esquema)
        {
            string url = "Usuarios/ObtengaImagenUsuario/" + usuario + "/" + esquema;
            var imagenJson = await _http.GetAsync(url);
            if (imagenJson.StatusCode == HttpStatusCode.OK)
            {
                ImagenDeUsuario = await imagenJson.Content.ReadFromJsonAsync<mImagenUsuario>();

            }
        }

        public async Task ObtenerListaDeInformesDeUsuario(string codigo, string esquema)
        {
            string url = "Usuarios/ObtengaListaDeInformesDeUsuario/" + codigo + "/" + esquema;
            var listaDeInformesDeUsuarioAsociados = await _http.GetFromJsonAsync<List<mUsuariosDeClienteDeInforme>>(url);
            if (listaDeInformesDeUsuarioAsociados is not null)
            {
                ListaDeInformesDeUsuarioAsociados = listaDeInformesDeUsuarioAsociados;
            }
        }

        public async Task ObtenerListaDeUsuariosParaEditar(string esquema)
        {
            var listaDeUsuarios = await _http.GetFromJsonAsync<List<mUsuariosParaEditar>>("Usuarios/ObtengaLaListaDeUsuariosParaEditar/" + esquema);
            if (listaDeUsuarios is not null)
            {
                ListaDeUsuariosParaEditar = listaDeUsuarios;
            }
        }

        public async Task ActualizarListaDeUsuarios(List<mUsuariosParaEditar> listaUsuarios, string esquema, string usuarioActual)
        {
            try
            {
                foreach (var usuario in listaUsuarios)
                {
                    if (!string.IsNullOrEmpty(usuario.clave))
                    {
                        usuario.clave = EncriptarClave(usuario.clave);
                    }
                }
               
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Usuarios/ActualizaListaDeUsuarios";
                string jsonData = JsonSerializer.Serialize(listaUsuarios);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var mensaje = await _http.PostAsync(url, content);
                foreach (var usuario in listaUsuarios)
                {
                    if(usuarioActual == usuario.usuarioOrignal) { 
                    if (usuario.usuarioOrignal != usuario.usuario || usuario.correoOriginal != usuario.correo || (usuario.claveOriginal != usuario.clave && !string.IsNullOrEmpty(usuario.clave)))
                    {

                        _navigationManager.NavigateTo("/login", forceLoad: true);
                        await _localStorageService.RemoveItemAsync("token");
                    }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        public async Task<string> ValidarCorreoExistente(string esquema, string correo)
        {
            string url = "Usuarios/ValidaCorreoExistente/" + esquema + "/" + correo;
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
        public async Task<string> ValidarUsuarioExistente(string esquema, string usuario)
        {
            string url = "Usuarios/ValidaUsuarioExistente/" + esquema + "/" + usuario;
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

    }
 }

