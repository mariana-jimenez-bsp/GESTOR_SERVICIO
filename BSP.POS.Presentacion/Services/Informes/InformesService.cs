using BSP.POS.Presentacion.Interfaces.Informes;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Pages.Home;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using BSP.POS.Presentacion.Models.Clientes;

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
            string url = "Informes/ObtengaLaListaDeInformesAsociados/" + cliente + "/" + esquema;
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaInformesAsociados = await response.Content.ReadFromJsonAsync<List<mInformes>>();
                if (listaInformesAsociados is not null)
                {
                    ListaInformesAsociados = listaInformesAsociados;
                }
            }
            else
            {
                
            }
            
        }

        public async Task<mInformeAsociado?> ObtenerInformeAsociado(string consecutivo, string esquema)
        {
            string url = "Informes/ObtengaElInformeAsociado/" + consecutivo + "/" + esquema;
            var informeAsociadoJson = await _http.GetAsync(url);
            if (informeAsociadoJson.StatusCode == HttpStatusCode.OK)
            {
                return await informeAsociadoJson.Content.ReadFromJsonAsync<mInformeAsociado?>();
            }
            return null;
        }

        public async Task ActualizarInformeAsociado(mInformeAsociado informe, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Informes/ActualizaElInformeAsociado";
                string jsonData = JsonSerializer.Serialize(informe);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(url, content);
                if(response.StatusCode == HttpStatusCode.OK)
                {

                }
            }
            catch (Exception)
            {

            }
        }
        public async Task CambiarEstadoDeInforme(mInformeEstado informe, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Informes/CambiaEstadoDeInforme";
                string jsonData = JsonSerializer.Serialize(informe);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(url, content);
                if(response.StatusCode == HttpStatusCode.OK)
                {

                }
            }
            catch (Exception)
            {

            }
        }

        public async Task EliminarInforme(string consecutivo, string esquema)
        {
            try
            {
                string url = "Informes/EliminaInforme";
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                _http.DefaultRequestHeaders.Remove("X-consecutivo");
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                _http.DefaultRequestHeaders.Add("X-consecutivo", consecutivo);


                var mensaje = await _http.DeleteAsync(url);
            }
            catch (Exception)
            {

            }
        }
        public async Task<bool> EnviarCorreoDeAprobacionDeInforme(string esquema, string consecutivo)
        {
            try
            {
                string url = "Informes/EnviarTokenDeAprobacionDeInforme";
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                _http.DefaultRequestHeaders.Remove("X-Consecutivo");
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                _http.DefaultRequestHeaders.Add("X-Consecutivo", consecutivo);
                string jsonData = JsonSerializer.Serialize(esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync(url, content);

                if (response.StatusCode == HttpStatusCode.OK)
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
        public async Task<mTokenAprobacionInforme> ValidarTokenAprobacionDeInforme(string esquema, string token)
        {
            string url = "Informes/ValidaTokenAprobacionDeInforme/" + esquema + "/" + token;
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var tokenAprobacion = await response.Content.ReadFromJsonAsync<mTokenAprobacionInforme>();
                if (tokenAprobacion is not null)
                {
                    return tokenAprobacion;
                }
                return new mTokenAprobacionInforme();
            }
            else
            {
                return new mTokenAprobacionInforme();
            }
        }

        public async Task AprobarInforme(mTokenAprobacionInforme tokenAprobacion, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Informes/ApruebaInforme";
                string jsonData = JsonSerializer.Serialize(tokenAprobacion);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(url, content);
                if( response.StatusCode == HttpStatusCode.OK)
                {

                }
            }
            catch (Exception)
            {

            }
        }

        public async Task RechazarInforme(mTokenAprobacionInforme tokenAprobacion, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Informes/RechazaInforme";
                string jsonData = JsonSerializer.Serialize(tokenAprobacion);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(url, content);
                if(response.StatusCode == HttpStatusCode.OK)
                {

                }
            }
            catch (Exception)
            {

            }
        }

        public async Task<string> AgregarInformeAsociado(string cliente, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Informes/AgregaInformeAsociado";
                mClienteAsociado clienteAso = new mClienteAsociado();
                clienteAso.CLIENTE = cliente;
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                string jsonData = JsonSerializer.Serialize(clienteAso);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(url, content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public async Task<string> ValidarExistenciaConsecutivoInforme(string esquema, string consecutivo)
        {
            string url = "Informes/ValidaExistenciaConsecutivoInforme/" + esquema + "/" + consecutivo;
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                string consecutivoDevuelto = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(consecutivoDevuelto))
                {
                    return consecutivoDevuelto;
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
