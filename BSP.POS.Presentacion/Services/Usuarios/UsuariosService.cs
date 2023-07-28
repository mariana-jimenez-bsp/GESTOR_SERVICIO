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

            try
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
            catch (Exception ex)
            {

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

    }
    }

