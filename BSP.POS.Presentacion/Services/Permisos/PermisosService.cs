using BSP.POS.Presentacion.Models;
using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BSP.POS.Presentacion.Interfaces.Permisos;
using BSP.POS.Presentacion.Pages.Home;

namespace BSP.POS.Presentacion.Services.Permisos
{
    public class PermisosService: IPermisosInterface
    {
        private readonly HttpClient _http;
        public PermisosService(HttpClient htpp)
        {
            _http = htpp;
        }
        public List<mPermisosAsociados> ListaPermisosAsociadados { get; set; } = new List<mPermisosAsociados>();
        public List<mPermisos> ListaPermisos { get; set; } = new List<mPermisos>();
        public async Task ObtenerListaDePermisosAsociados(string esquema, string id)
        {
            string url = "https://localhost:7032/api/Permisos/ObtengaLaListaDePermisosAsociados/" + esquema + "/" + id;
            var listaPermisosAsociados = await _http.GetFromJsonAsync<List<mPermisosAsociados>>(url);
            if (listaPermisosAsociados is not null)
            {
                ListaPermisosAsociadados = listaPermisosAsociados;
            }
        }

        public async Task ObtenerListaDePermisos(string esquema)
        {
            string url = "https://localhost:7032/api/Permisos/ObtengaLaListaDePermisos/" + esquema;
            var listaPermisos = await _http.GetFromJsonAsync<List<mPermisos>>(url);
            if (listaPermisos is not null)
            {
                ListaPermisos = listaPermisos;
            }
        }

        public async Task ActualizarListaPermisosAsociados(List<mPermisosAsociados> listaPermisos, string idUsuario, string esquema)
        {
            try
            {
                string url = "https://localhost:7032/api/Permisos/ActualizaListaDePermisosAsociados";
                string jsonData = JsonSerializer.Serialize(listaPermisos);
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                _http.DefaultRequestHeaders.Remove("X-IdUsuario");
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                _http.DefaultRequestHeaders.Add("X-IdUsuario", idUsuario);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var mensaje = await _http.PostAsync(url, content);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
