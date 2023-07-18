using BSP.POS.Presentacion.Interfaces.Informes;
using BSP.POS.Presentacion.Models;
using BSP.POS.Presentacion.Pages.Clientes;
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


        public async Task ObtenerListaDeInformesAsociados(string cliente)
        {
            var listaInformesAsociados = await _http.GetFromJsonAsync<List<mInformes>>("https://localhost:7032/api/Informes/ObtengaLaListaDeInformesAsociados/" + cliente);
            if (listaInformesAsociados is not null)
            {
                ListaInformesAsociados = listaInformesAsociados;
            }
        }

    }
}
