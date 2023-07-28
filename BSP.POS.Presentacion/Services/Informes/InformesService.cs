using BSP.POS.Presentacion.Interfaces.Informes;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Pages.Home;
using System.Net;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace BSP.POS.Presentacion.Services.Informes
{
    public class InformesService : IInformesInterface
    {
        private readonly HttpClient _http;

        public InformesService(HttpClient htpp)
        {
            _http = htpp;
        }
        public List<mInformes> ListaInformesAsociados { get; set; } = new List<mInformes>();
        public mInformeAsociado InformeAsociado { get; set; } = new mInformeAsociado();
        

        public async Task ObtenerListaDeInformesAsociados(string cliente, string esquema)
        {
            string url = "https://localhost:7032/api/Informes/ObtengaLaListaDeInformesAsociados/" + cliente + "/" + esquema;
            var listaInformesAsociados = await _http.GetFromJsonAsync<List<mInformes>>(url);
            if (listaInformesAsociados is not null)
            {
                ListaInformesAsociados = listaInformesAsociados;
            }
        }

        public async Task<mInformeAsociado?> ObtenerInformeAsociado(string consecutivo, string esquema)
        {
            string url = "https://localhost:7032/api/Informes/ObtengaElInformeAsociado/" + consecutivo + "/" + esquema;
            var informeAsociadoJson = await _http.GetAsync(url);
            if (informeAsociadoJson.StatusCode == HttpStatusCode.OK)
            {
                return await informeAsociadoJson.Content.ReadFromJsonAsync<mInformeAsociado?>();
            }
            return null;
        }

    }
}
