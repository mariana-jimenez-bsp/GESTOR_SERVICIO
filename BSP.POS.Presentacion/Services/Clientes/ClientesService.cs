using BSP.POS.Presentacion.Interfaces.Clientes;
using BSP.POS.Presentacion.Models;
using BSP.POS.UTILITARIOS.Clientes;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
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

        public async Task<mClienteAsociado?> ObtenerClienteAsociado(string cliente)
        {
            var clienteAsociadoJson = await _http.GetAsync("https://localhost:7032/api/Clientes/ObtengaElClienteAsociado/" + cliente);
            if (clienteAsociadoJson.StatusCode == HttpStatusCode.OK)
            {
                return await clienteAsociadoJson.Content.ReadFromJsonAsync<mClienteAsociado?>();
            }
            return null;
        }

        public async Task ObtenerListaClientes()
        {
            var listaClientes = await _http.GetFromJsonAsync<List<mClientes>>("https://localhost:7032/api/Clientes/ObtengaLaListaDeClientes");
            if (listaClientes is not null)
            {
                ListaClientes = listaClientes;
            }
        }

        public async Task ObtenerListaClientesRecientes()
        {

            var listaClientesRecientes = await _http.GetFromJsonAsync<List<mClientes>>("https://localhost:7032/api/Clientes/ObtengaLaListaDeClientesRecientes");
            if (listaClientesRecientes is not null)
            {
                ListaClientesRecientes = listaClientesRecientes;
            }
        }

        public async Task ActualizarListaDeClientes(List<mClientes> listaClientes, string esquema)
        {
            try
            {
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
