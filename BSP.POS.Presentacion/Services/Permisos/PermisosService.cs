using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BSP.POS.Presentacion.Interfaces.Permisos;
using BSP.POS.Presentacion.Pages.Home;
using BSP.POS.Presentacion.Models.Permisos;

namespace BSP.POS.Presentacion.Services.Permisos
{
    public class PermisosService: IPermisosInterface
    {
        private readonly HttpClient _http;
        public PermisosService(HttpClient htpp)
        {
            _http = htpp;
        }
        public List<mPermisos> ListaDePermisos { get; set; } = new List<mPermisos>();
        public List<mSubPermisos> ListaDeSubPermisos { get; set; } = new List<mSubPermisos>();
        public List<mDatosPermisosDeUsuarios> ListaDePermisosDeUsuario { get; set; } = new List<mDatosPermisosDeUsuarios>();
        public List<mDatosSubPermisosDeUsuarios> ListaDeSubPermisosDeUsuario { get; set; } = new List<mDatosSubPermisosDeUsuarios>();

        public async Task ObtenerLaListaDePermisos(string esquema)
        {
            string url = "Permisos/ObtengaListaDePermisos";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaPermisos = await response.Content.ReadFromJsonAsync<List<mPermisos>>();
                if (listaPermisos is not null)
                {
                    ListaDePermisos = listaPermisos;
                }
            }
        }
        public async Task ObtenerLaListaDeSubPermisos(string esquema)
        {
            string url = "Permisos/ObtengaListaDeSubPermisos";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaPermisos = await response.Content.ReadFromJsonAsync<List<mSubPermisos>>();
                if (listaPermisos is not null)
                {
                    ListaDeSubPermisos = listaPermisos;
                }
            }
        }
        public async Task ObtenerLaListaDePermisosDeUsuario(string esquema, string codigo)
        {
            string url = "Permisos/ObtengaListaDePermisosDeUsuario";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Remove("X-Codigo");
            _http.DefaultRequestHeaders.Add("X-Codigo", codigo);
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaPermisos = await response.Content.ReadFromJsonAsync<List<mDatosPermisosDeUsuarios>>();
                if (listaPermisos is not null)
                {
                    ListaDePermisosDeUsuario = listaPermisos;
                }
            }
        }
        public async Task ObtenerLaListaDeSubPermisosDeUsuario(string esquema, string codigo)
        {
            string url = "Permisos/ObtengaListaDeSubPermisosDeUsuario";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Remove("X-Codigo");
            _http.DefaultRequestHeaders.Add("X-Codigo", codigo);
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaPermisos = await response.Content.ReadFromJsonAsync<List<mDatosSubPermisosDeUsuarios>>();
                if (listaPermisos is not null)
                {
                    ListaDeSubPermisosDeUsuario = listaPermisos;
                }
            }
        }

        public async Task<bool> ActualizarListaPermisosDeUsuario(List<string> listaPermisos, string codigo,string esquema)
        {
            try
            {
                string url = "Permisos/ActualizaListaDePermisosDeUsuario";
                string jsonData = JsonSerializer.Serialize(listaPermisos);
                _http.DefaultRequestHeaders.Remove("X-Codigo");
                _http.DefaultRequestHeaders.Add("X-Codigo", codigo);
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PutAsync(url, content);
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
    }
}
