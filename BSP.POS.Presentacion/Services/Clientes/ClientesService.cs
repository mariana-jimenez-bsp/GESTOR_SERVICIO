using BSP.POS.Presentacion.Interfaces.Clientes;
using BSP.POS.UTILITARIOS.Clientes;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using BSP.POS.Presentacion.Models.Clientes;

namespace BSP.POS.Presentacion.Services.Clientes
{
    public class ClientesService : IClientesInterface
    {
        private readonly HttpClient _http;

        public ClientesService(HttpClient htpp)
        {
            _http = htpp;
        }

        public List<mClientes> ListaClientes { get; set; } = new List<mClientes>();
        public List<mClientes> ListaClientesRecientes { get; set; } = new List<mClientes>();
        public mClienteAsociado ClienteAsociado { get; set; } = new mClienteAsociado();

        public async Task<mClienteAsociado?> ObtenerClienteAsociado(string cliente, string esquema)
        {
            string url = "https://localhost:7032/api/Clientes/ObtengaElClienteAsociado/" + cliente + "/" + esquema;
            var clienteAsociadoJson = await _http.GetAsync(url);
            if (clienteAsociadoJson.StatusCode == HttpStatusCode.OK)
            {
                return await clienteAsociadoJson.Content.ReadFromJsonAsync<mClienteAsociado?>();
            }
            return null;
        }

        public async Task ObtenerListaClientes(string esquema)
        {
            var listaClientes = await _http.GetFromJsonAsync<List<mClientes>>("https://localhost:7032/api/Clientes/ObtengaLaListaDeClientes/" + esquema);
            if (listaClientes is not null)
            {
                ListaClientes = listaClientes;
            }
        }

        public async Task ObtenerListaClientesRecientes(string esquema)
        {

            var listaClientesRecientes = await _http.GetFromJsonAsync<List<mClientes>>("https://localhost:7032/api/Clientes/ObtengaLaListaDeClientesRecientes/" + esquema);
            if (listaClientesRecientes is not null)
            {
                ListaClientesRecientes = listaClientesRecientes;
            }
        }

        public async Task ActualizarListaDeClientes(List<mClientes> listaClientes, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "https://localhost:7032/api/Clientes/ActualizaListaDeClientes";
                string jsonData = JsonSerializer.Serialize(listaClientes);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var mensaje = await _http.PostAsync(url, content);
            }
            catch (Exception ex)
            {

            }


        }
    }
}
