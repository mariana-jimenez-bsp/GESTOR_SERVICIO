using Blazored.LocalStorage;
using BSP.POS.Presentacion.Interfaces.Usuarios;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Pages.Usuarios.Usuarios;
using clSeguridad;
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
        
        public mPerfil Perfil { get; set; } = new mPerfil();
        private readonly ILocalStorageService _localStorageService;
        private readonly NavigationManager _navigationManager;
        public List<mUsuariosDeCliente> ListaDeUsuariosDeCliente { get; set; } = new List<mUsuariosDeCliente>();
        public List<mDatosUsuariosDeClienteDeInforme> ListaDatosUsuariosDeClienteDeInforme { get; set; } = new List<mDatosUsuariosDeClienteDeInforme>();
        public List<mPerfil> ListaDeUsuarios { get; set; } = new List<mPerfil>();
        
        public mImagenUsuario ImagenDeUsuario { get; set; } = new mImagenUsuario();
        public List<mUsuariosDeInforme> ListaUsuariosDeInformeAsociados { get; set; } = new List<mUsuariosDeInforme>();
        public List<mUsuariosDeInforme> ListaUsuariosDeInformeDeCliente { get; set; } = new List<mUsuariosDeInforme>();
        public List<mUsuariosParaEditar> ListaDeUsuariosParaEditar { get; set; } = new List<mUsuariosParaEditar>();
        public List<mUsuariosParaEditar> ListaDeUsuariosConsultores { get; set; } = new List<mUsuariosParaEditar>();
        public mUsuariosParaEditar UsuarioParaEditar { get; set; } = new mUsuariosParaEditar();
        
        public UsuariosService(HttpClient htpp, ILocalStorageService localStorageService, NavigationManager navigationManager)
        {
            _http = htpp;
            _localStorageService = localStorageService;
            _navigationManager = navigationManager;
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

        public string DesencriptarClave(string clave)
        {
            Cryptografia _cryptografia = new Cryptografia();
            return _cryptografia.DecryptString(clave, "BSP");
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

        public async Task<bool> ActualizarPefil(mPerfil perfil, string usuarioOriginal, string claveOriginal, string correoOriginal, string esquema)
        {
                perfil.claveDesencriptada = null;
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                string url = "Usuarios/ActualizarPerfil";
                string jsonData = JsonSerializer.Serialize(perfil);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PutAsync(url, content);
                if(response.StatusCode == HttpStatusCode.OK) {
                return true;

            }
            return false;
            
        }
        public async Task ObtenerListaDeUsuarios(string esquema)
        {
            string url = "Usuarios/ObtengaLaListaDeUsuarios/" + esquema;
            var response = await _http.GetAsync(url);
            if( response.StatusCode == HttpStatusCode.OK )
            {
                var listaDeUsuarios = await response.Content.ReadFromJsonAsync<List<mPerfil>>();
                if (listaDeUsuarios is not null)
                {
                    ListaDeUsuarios = listaDeUsuarios;
                }
            }
            
        }
        public async Task<List<mUsuariosDeCliente>> ObtenerListaDeUsuariosDeClienteAsociados(string esquema, string cliente)
        {
            string url = "Usuarios/ObtengaLaListaDeUsuariosDeClienteAsociados/" + esquema + "/" + cliente;
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK ) {
                var listaDeUsuariosDeClientesAsociados = await response.Content.ReadFromJsonAsync<List<mUsuariosDeCliente>>();
                if (listaDeUsuariosDeClientesAsociados is not null)
                {
                    return listaDeUsuariosDeClientesAsociados;
                }
                else
                {
                    return new List<mUsuariosDeCliente>();
                }
            }
            return new List<mUsuariosDeCliente>();
        }

        public async Task ObtenerDatosListaUsuariosDeClienteDeInforme(string consecutivo, string esquema)
        {
            string url = "Usuarios/ObtengaDatosListaUsuariosDeClienteDeInforme/" + consecutivo + "/" + esquema;
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaInformesAsociados = await response.Content.ReadFromJsonAsync<List<mDatosUsuariosDeClienteDeInforme>>();
                if (listaInformesAsociados is not null)
                {
                    ListaDatosUsuariosDeClienteDeInforme = listaInformesAsociados;
                }
            }
        }
        public async Task<bool> AgregarUsuarioDeClienteDeInforme(mUsuariosDeInforme usuario, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Usuarios/AgregaUsuarioDeClienteDeInforme";
                string jsonData = JsonSerializer.Serialize(usuario);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(url, content);
                if(response.StatusCode == HttpStatusCode.OK)
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

        public async Task<bool> EliminarUsuarioDeClienteDeInforme(string codigo, string esquema)
        {
            try
            {
                string url = "Usuarios/EliminaUsuarioDeClienteDeInforme";
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                _http.DefaultRequestHeaders.Remove("X-Codigo");
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                _http.DefaultRequestHeaders.Add("X-Codigo", codigo);
               

                var response = await _http.DeleteAsync(url);
                if( response.StatusCode == HttpStatusCode.OK)
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
        public async Task ObtenerImagenDeUsuario(string usuario, string esquema)
        {
            string url = "Usuarios/ObtengaImagenUsuario/" + usuario + "/" + esquema;
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                ImagenDeUsuario = await response.Content.ReadFromJsonAsync<mImagenUsuario>();

            }
        }

        public async Task ObtenerListaDeInformesDeUsuario(string codigo, string esquema)
        {
            string url = "Usuarios/ObtengaListaDeInformesDeUsuario/" + codigo + "/" + esquema;
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var listaUsuariosDeInformeAsociados = await response.Content.ReadFromJsonAsync<List<mUsuariosDeInforme>>();
                if (listaUsuariosDeInformeAsociados is not null)
                {
                    ListaUsuariosDeInformeAsociados = listaUsuariosDeInformeAsociados;
                }
            }
            
        }

        public async Task ObtenerListaDeInformesDeUsuarioDeCliente(string cliente, string esquema)
        {
            string url = "Usuarios/ObtengaListaDeInformesDeUsuarioDeCliente/" + cliente + "/" + esquema;
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaUsuariosDeInformeAsociados = await response.Content.ReadFromJsonAsync<List<mUsuariosDeInforme>>();
                if (listaUsuariosDeInformeAsociados is not null)
                {
                    ListaUsuariosDeInformeDeCliente = listaUsuariosDeInformeAsociados;
                }
            }

        }

        public async Task ObtenerListaDeUsuariosParaEditar(string esquema)
        {
            string url = "Usuarios/ObtengaLaListaDeUsuariosParaEditar/" + esquema;
            var response = await _http.GetAsync(url);
            if( response.StatusCode == HttpStatusCode.OK)
            {
                var listaDeUsuarios = await response.Content.ReadFromJsonAsync<List<mUsuariosParaEditar>>();
                if (listaDeUsuarios is not null)
                {
                    ListaDeUsuariosParaEditar = listaDeUsuarios;
                }
            }
            
        }
        public async Task ObtenerListaDeUsuariosConsultores(string esquema)
        {
            string url = "Usuarios/ObtengaLaListaDeUsuariosConsultores/" + esquema;
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaDeUsuarios = await response.Content.ReadFromJsonAsync<List<mUsuariosParaEditar>>();
                if (listaDeUsuarios is not null)
                {
                    ListaDeUsuariosConsultores = listaDeUsuarios;
                }
            }
        }
        public async Task<string> ValidarCorreoExistente(string esquema, string correo)
        {
            string url = "Usuarios/ValidaCorreoExistente/" + esquema + "/" + correo;
            var response = await _http.GetAsync(url);
            if( response.StatusCode == HttpStatusCode.OK )
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
        public async Task<string> ValidarUsuarioExistente(string esquema, string usuario)
        {
            string url = "Usuarios/ValidaUsuarioExistente/" + esquema + "/" + usuario;
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK )
            {
                string usuarioDevuelto = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(usuarioDevuelto))
                {
                    return usuarioDevuelto;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
        public async Task<string> ValidarExistenciaDeCodigoUsuario(string esquema, string codigo)
        {
            string url = "Usuarios/ValidaExistenciaDeCodigoUsuario/" + esquema + "/" + codigo;
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string codigoDevuelto = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(codigoDevuelto))
                {
                    return codigoDevuelto;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
        public async Task<bool> AgregarUsuario(mUsuarioParaAgregar usuario, string esquema)
        {
            try
            {
                usuario.claveDesencriptada = null;
                _http.DefaultRequestHeaders.Remove("X-IdCodigoTelefono");
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Usuarios/AgregaUsuario";
                string jsonData = JsonSerializer.Serialize(usuario);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                _http.DefaultRequestHeaders.Add("X-IdCodigoTelefono", usuario.IdCodigoTelefono.ToString());
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(url, content);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task ObtenerElUsuarioParaEditar(string esquema, string codigo)
        {
            string url = "Usuarios/ObtengaElUsuarioParaEditar/" + esquema + "/" + codigo;
            var response = await _http.GetAsync(url);
            if( response.StatusCode == HttpStatusCode.OK )
            {
                var usuarioParaEditar = await response.Content.ReadFromJsonAsync<mUsuariosParaEditar>();
                if (usuarioParaEditar is not null)
                {
                    UsuarioParaEditar = usuarioParaEditar;
                }
            }
            
        }

        public async Task<bool> ActualizarUsuario(mUsuariosParaEditar usuario, string esquema, string usuarioActual)
        {
            try
            { 
                usuario.claveDesencriptada = null;
                usuario.claveOriginal = null;

                _http.DefaultRequestHeaders.Remove("X-Esquema");
                _http.DefaultRequestHeaders.Remove("X-IdCodigoTelefono");
                string url = "Usuarios/ActualizaElUsuario";
                string jsonData = JsonSerializer.Serialize(usuario);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                _http.DefaultRequestHeaders.Add("X-IdCodigoTelefono", usuario.IdCodigoTelefono.ToString());
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PutAsync(url, content);
                if(response.StatusCode == HttpStatusCode.OK )
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

        public async Task<string> ValidarExistenciaEsquema(string esquema)
        {
            string url = "Usuarios/ValidaExistenciaEsquema/" + esquema;
            var response = await _http.GetAsync(url);
            if( response.StatusCode == HttpStatusCode.OK )
            {
                string esquemaDevuelto = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(esquemaDevuelto))
                {
                    return esquemaDevuelto;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        

    }
 }

