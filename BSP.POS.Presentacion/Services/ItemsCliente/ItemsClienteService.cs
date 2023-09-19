using BSP.POS.Presentacion.Interfaces.ItemsCliente;
using BSP.POS.Presentacion.Models.ItemsCliente;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace BSP.POS.Presentacion.Services.ItemsCliente
{
    public class ItemsClienteService:IItemsClienteInterface
    {
        private readonly HttpClient _http;

        public ItemsClienteService(HttpClient htpp)
        {
            _http = htpp;
        }

        public List<mItemsCliente> listaCondicionesDePago { get; set; } = new List<mItemsCliente>();
        public List<mItemsCliente> listaNivelesDePrecio { get; set; } = new List<mItemsCliente>();
        public List<mItemsCliente> listaTiposDeImpuestos { get; set; } = new List<mItemsCliente>();
        public List<mTarifa> listaDeTarifasDeImpuesto { get; set; } = new List<mTarifa>();
        public List<mItemsCliente> listaTiposDeNit { get; set; } = new List<mItemsCliente>();
        public List<mItemsCliente> listaCentrosDeCosto { get; set; } = new List<mItemsCliente>();
        public async Task ObtenerLaListaDeCondicionesDePago(string esquema)
        {
            string url = "ItemsCliente/ObtengaLaListaDeCondicionesDePago";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var listaObtenida = await response.Content.ReadFromJsonAsync<List<mItemsCliente>>();
                if (listaObtenida is not null)
                {
                    listaCondicionesDePago = listaObtenida;
                }
            }
            
        }
        public async Task ObtenerLaListaDeNivelesDePrecio(string esquema, string moneda)
        {
            string url = "ItemsCliente/ObtengaLaListaDeNivelesDePrecio";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Remove("X-Moneda");
            _http.DefaultRequestHeaders.Add("X-Moneda", moneda);
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var listaObtenida = await response.Content.ReadFromJsonAsync<List<mItemsCliente>>();
                if (listaObtenida is not null)
                {
                    listaNivelesDePrecio = listaObtenida;
                }
            }
            
        }

        public async Task ObtenerLosTiposDeImpuestos(string esquema)
        {
            string url = "ItemsCliente/ObtengaLosTiposDeImpuestos";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var listaObtenida = await response.Content.ReadFromJsonAsync<List<mItemsCliente>>();
                if (listaObtenida is not null)
                {
                    listaTiposDeImpuestos = listaObtenida;
                }
            }
            
        }

        public async Task ObtenerLosTiposDeTarifasDeImpuesto(string esquema, string impuesto)
        {
            string url = "ItemsCliente/ObtengaLosTiposDeTarifasDeImpuesto";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Remove("X-Impuesto");
            _http.DefaultRequestHeaders.Add("X-Impuesto", impuesto);
            var response = await _http.GetAsync(url);
            if( response.StatusCode == HttpStatusCode.OK)
            {
                var listaObtenida = await response.Content.ReadFromJsonAsync<List<mTarifa>>();
                if (listaObtenida is not null)
                {
                    listaDeTarifasDeImpuesto = listaObtenida;
                }
            }
            
        }

        public async Task<decimal> ObtenerElPorcentajeDeTarifa(string esquema, string impuesto, string tipoTarifa)
        {
            try
            {
                string url = "ItemsCliente/ObtengaElPorcentajeDeTarifa";
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                _http.DefaultRequestHeaders.Remove("X-Impuesto");
                _http.DefaultRequestHeaders.Add("X-Impuesto", impuesto);
                _http.DefaultRequestHeaders.Remove("X-TipoTarifa");
                _http.DefaultRequestHeaders.Add("X-TipoTarifa", tipoTarifa);
                var response = await _http.GetAsync(url);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    string porcentajeString = await response.Content.ReadAsStringAsync();
                    try
                    {
                        decimal porcentaje = decimal.Parse(porcentajeString);
                        return porcentaje;
                    }
                    catch (Exception)
                    {

                        throw new Exception("Error al obtener el porcentaje de Tarifa");
                    }
                    
                    
                }
                return 0;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        public async Task<string> ObtenerElSiguienteCodigoCliente(string esquema, string letra)
        {
            try
            {
                string url = "ItemsCliente/ObtengaElSiguienteCodigoDeCliente";
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                _http.DefaultRequestHeaders.Remove("X-Letra");
                _http.DefaultRequestHeaders.Add("X-Letra", letra);
                var response = await _http.GetAsync(url);
                if( response.StatusCode == HttpStatusCode.OK)
                {
                    string codigo = await response.Content.ReadAsStringAsync();
                    if(string.IsNullOrEmpty(codigo))
                    {
                        return codigo;
                    }
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
        public async Task ObtenerLosTiposDeNit(string esquema)
        {
            string url = "ItemsCliente/ObtengaLosTiposDeNit";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var listaObtenida = await response.Content.ReadFromJsonAsync<List<mItemsCliente>>();
                if (listaObtenida is not null)
                {
                    listaTiposDeNit = listaObtenida;
                }
            }
            
        }


        public async Task ObtenerListaDeCentrosDeCosto(string esquema)
        {
            string url = "ItemsCliente/ObtengaLaListaDeCentrosDeCosto";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            var response = await _http.GetAsync(url);
            if( response.StatusCode == HttpStatusCode.OK)
            {
                var listaObtenida = await response.Content.ReadFromJsonAsync<List<mItemsCliente>>();
                if (listaObtenida is not null)
                {
                    listaCentrosDeCosto = listaObtenida;
                }
            }
            
        }


    }
}
