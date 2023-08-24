﻿using BSP.POS.Presentacion.Interfaces.ItemsCliente;
using BSP.POS.Presentacion.Models.ItemsCliente;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using static System.Net.WebRequestMethods;

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
        public async Task ObtenerLaListaDeCondicionesDePago(string esquema)
        {
            string url = "ItemsCliente/ObtengaLaListaDeCondicionesDePago";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            var listaObtenida = await _http.GetFromJsonAsync<List<mItemsCliente>>(url);
            if (listaObtenida is not null)
            {
                listaCondicionesDePago = listaObtenida;
            }
        }
        public async Task ObtenerLaListaDeNivelesDePrecio(string esquema, string moneda)
        {
            string url = "ItemsCliente/ObtengaLaListaDeNivelesDePrecio";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Remove("X-Moneda");
            _http.DefaultRequestHeaders.Add("X-Moneda", moneda);
            var listaObtenida = await _http.GetFromJsonAsync<List<mItemsCliente>>(url);
            if (listaObtenida is not null)
            {
                listaNivelesDePrecio = listaObtenida;
            }
        }

        public async Task ObtenerLosTiposDeImpuestos(string esquema)
        {
            string url = "ItemsCliente/ObtengaLosTiposDeImpuestos";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            var listaObtenida = await _http.GetFromJsonAsync<List<mItemsCliente>>(url);
            if (listaObtenida is not null)
            {
                listaTiposDeImpuestos = listaObtenida;
            }
        }

        public async Task ObtenerLosTiposDeTarifasDeImpuesto(string esquema, string impuesto)
        {
            string url = "ItemsCliente/ObtengaLosTiposDeTarifasDeImpuesto";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Remove("X-Impuesto");
            _http.DefaultRequestHeaders.Add("X-Impuesto", impuesto);
            var listaObtenida = await _http.GetFromJsonAsync<List<mTarifa>>(url);
            if (listaObtenida is not null)
            {
                listaDeTarifasDeImpuesto = listaObtenida;
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
                string porcentajeString = await _http.GetStringAsync(url);
                decimal porcentaje = decimal.Parse(porcentajeString);
                return porcentaje;
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
                string codigo = await _http.GetStringAsync(url);
                return codigo;
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
            var listaObtenida = await _http.GetFromJsonAsync<List<mItemsCliente>>(url);
            if (listaObtenida is not null)
            {
                listaTiposDeNit = listaObtenida;
            }
        }





    }
}
