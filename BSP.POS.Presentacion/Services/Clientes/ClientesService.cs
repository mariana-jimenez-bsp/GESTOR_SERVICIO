﻿using BSP.POS.Presentacion.Interfaces.Clientes;
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
            var listaClientes = await _http.GetFromJsonAsync<List<mClientes>>("Clientes/ObtengaLaListaDeClientes/" + esquema);
            if (listaClientes is not null)
            {
                ListaClientes = listaClientes;
            }
        }

        public async Task ObtenerListaClientesRecientes(string esquema)
        {

            var listaClientesRecientes = await _http.GetFromJsonAsync<List<mClientes>>("Clientes/ObtengaLaListaDeClientesRecientes/" + esquema);
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
                string url = "Clientes/ActualizaListaDeClientes";
                string jsonData = JsonSerializer.Serialize(listaClientes);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var mensaje = await _http.PostAsync(url, content);
            }
            catch (Exception ex)
            {

            }


        }
        public async Task ObtenerListaClientesCorporaciones(string esquema)
        {
            var listaClientes = await _http.GetFromJsonAsync<List<mClienteContado>>("Clientes/ObtengaLaListaDeClientesCorporaciones/" + esquema);
            if (listaClientes is not null)
            {
                ListaClientesCorporaciones = listaClientes;
            }
        }

        public async Task AgregarCliente(mAgregarCliente cliente, string esquema, string usuario)
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

                var mensaje = await _http.PostAsync(url, content);
            }
            catch (Exception)
            {

            }
        }
        public async Task<string> ValidarExistenciaDeCliente(string esquema, string cliente)
        {
            string url = "Clientes/ValidaExistenciaDeCliente/" + esquema + "/" + cliente;
            string clienteDevuelto = await _http.GetStringAsync(url);
            if (!string.IsNullOrEmpty(clienteDevuelto))
            {
                return clienteDevuelto;
            }
            else
            {
                return null;
            }
        }
    }
}
