using BSP.POS.Presentacion.Interfaces.Clientes;
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
        public List<mClienteContado> ListaClientesCorporaciones { get; set; } = new List<mClienteContado>();
        
        public async Task<mClienteAsociado?> ObtenerClienteAsociado(string cliente, string esquema)
        {
            string url = "Clientes/ObtengaElClienteAsociado/" + cliente + "/" + esquema;
            var clienteAsociadoJson = await _http.GetAsync(url);
            if (clienteAsociadoJson.StatusCode == HttpStatusCode.OK)
            {
                return await clienteAsociadoJson.Content.ReadFromJsonAsync<mClienteAsociado?>();
            }
            return null;
        }

        public async Task ObtenerListaClientes(string esquema)
        {
            string url = "Clientes/ObtengaLaListaDeClientes/" + esquema;
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var listaClientes = await response.Content.ReadFromJsonAsync<List<mClientes>>();
                if (listaClientes is not null)
                {
                    ListaClientes = listaClientes;
                }
            }
            
        }

        public async Task ObtenerListaClientesRecientes(string esquema)
        {
            string url = "Clientes/ObtengaLaListaDeClientesRecientes/" + esquema;
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var listaClientesRecientes = await response.Content.ReadFromJsonAsync<List<mClientes>>();
                if (listaClientesRecientes is not null)
                {
                    ListaClientesRecientes = listaClientesRecientes;
                }
            }
            
        }

        public async Task<bool> ActualizarListaDeClientes(List<mClientes> listaClientes, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Clientes/ActualizaListaDeClientes";
                string jsonData = JsonSerializer.Serialize(listaClientes);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PutAsync(url, content);
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
        public async Task ObtenerListaClientesCorporaciones(string esquema)
        {
            string url = "Clientes/ObtengaLaListaDeClientesCorporaciones/" + esquema;
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var listaClientes = await response.Content.ReadFromJsonAsync<List<mClienteContado>>();
                if (listaClientes is not null)
                {
                    ListaClientesCorporaciones = listaClientes;
                }
            }
            
        }

        public async Task<bool> AgregarCliente(mAgregarCliente cliente, string esquema, string usuario)
        {
            try
            {
                
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                _http.DefaultRequestHeaders.Remove("X-Usuario");
                string url = "Clientes/AgregaCliente";
                string jsonData = JsonSerializer.Serialize(cliente);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                _http.DefaultRequestHeaders.Add("X-Usuario", usuario);
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
        public async Task<string> ValidarExistenciaDeCliente(string esquema, string cliente)
        {
            string url = "Clientes/ValidaExistenciaDeCliente/" + esquema + "/" + cliente;
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                string clienteDevuelto = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(clienteDevuelto))
                {
                    return clienteDevuelto;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public async Task<string> ObtenerPaisDeCliente(string esquema, string cliente)
        {
            string url = "Clientes/ObtengaPaisDeCliente/" + esquema + "/" + cliente;
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string paisDevuelto = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(paisDevuelto))
                {
                    return paisDevuelto;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public async Task<string> ObtenerContribuyenteDeCliente(string esquema, string cliente)
        {
            string url = "Clientes/ObtengaContribuyenteDeCliente/" + esquema + "/" + cliente;
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string contribuyente = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(contribuyente))
                {
                    return contribuyente;
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
