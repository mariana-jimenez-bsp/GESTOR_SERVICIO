using BSP.POS.Presentacion.Interfaces.CodigoTelefonoPais;
using BSP.POS.Presentacion.Models.Clientes;
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
        public List<mCodigoTelefonoPaisClientes> listaDatosCodigoClientePaisDeCliente { get; set; } = new List<mCodigoTelefonoPaisClientes>();
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
                    listaDatosCodigoClientePaisDeCliente = listaDatos;
                }
            }
        }
    }
}
