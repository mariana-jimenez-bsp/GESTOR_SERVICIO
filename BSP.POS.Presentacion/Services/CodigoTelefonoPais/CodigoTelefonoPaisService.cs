using BSP.POS.Presentacion.Interfaces.CodigoTelefonoPais;
using BSP.POS.Presentacion.Models.CodigoTelefonoPais;
using BSP.POS.Presentacion.Pages.Home;
using System.Net.Http.Json;
using System.Net;

namespace BSP.POS.Presentacion.Services.CodigoTelefonoPais
{
    public class CodigoTelefonoPaisService : ICodigoTelefonoPaisInterface
    {
        private readonly HttpClient _http;

        public CodigoTelefonoPaisService(HttpClient htpp)
        {
            _http = htpp;
        }
        public List<mCodigoTelefonoPais> listaDatosCodigoTelefonoPais { get; set; } = new List<mCodigoTelefonoPais>();
        public List<mCodigoTelefonoPaisClientes> listaDatosCodigoTelefonoPaisDeCliente { get; set; } = new List<mCodigoTelefonoPaisClientes>();
        public mCodigoTelefonoPaisUsuarios datosCodigoTelefonoPaisDeUsuario { get; set; } = new mCodigoTelefonoPaisUsuarios();
        public async Task ObtenerDatosCodigoTelefonoPais(string esquema)
        {
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            string url = "CodigoTelefonoPais/ObtengaDatosCodigoTelefonoPais";
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaDatos = await response.Content.ReadFromJsonAsync<List<mCodigoTelefonoPais>>();
                if (listaDatos is not null)
                {
                    listaDatosCodigoTelefonoPais = listaDatos;
                }
            }
        }
        public async Task ObtenerDatosCodigoTelefonoPaisDeClientes(string esquema)
        {
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            string url = "CodigoTelefonoPais/ObtengaDatosCodigoTelefonoPaisDeClientes";
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaDatos = await response.Content.ReadFromJsonAsync<List<mCodigoTelefonoPaisClientes>>();
                if (listaDatos is not null)
                {
                    listaDatosCodigoTelefonoPaisDeCliente = listaDatos;
                }
            }
        }
        public async Task ObtenerDatosCodigoTelefonoPaisDeUsuarioPorUsuario(string esquema, string codigoUsuario)
        {
            _http.DefaultRequestHeaders.Remove("X-CodigoUsuario");
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Add("X-CodigoUsuario", codigoUsuario);
            string url = "CodigoTelefonoPais/ObtengaDatosCodigoTelefonoPaisDeUsuariosPorUsuario";
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var datos = await response.Content.ReadFromJsonAsync<mCodigoTelefonoPaisUsuarios>();
                if (datos is not null)
                {
                    datosCodigoTelefonoPaisDeUsuario = datos;
                }
            }
        }
    }
}
