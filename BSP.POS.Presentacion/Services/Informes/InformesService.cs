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
        public List<mInformesDeProyecto> ListaInformesDeProyecto { get; set; } = new List<mInformesDeProyecto>();
        public List<mInformesDeCliente> ListaInformesDeCliente { get; set; } = new List<mInformesDeCliente>();
        public mInforme Informe { get; set; } = new mInforme();
        

        public async Task ObtenerListaDeInformesDeProyecto(string numero, string esquema)
        {
            string url = "Informes/ObtengaLaListaDeInformesDeProyecto/" + numero + "/" + esquema;
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaInformes = await response.Content.ReadFromJsonAsync<List<mInformesDeProyecto>>();
                if (listaInformes is not null)
                {
                    ListaInformesDeProyecto = listaInformes;
                }
            }
            else
            {
                
            }
            
        }
        public async Task ObtenerListaDeInformesDeCliente(string cliente, string esquema)
        {
            string url = "Informes/ObtengaLaListaDeInformesDeProyecto/" + cliente + "/" + esquema;
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaInformes = await response.Content.ReadFromJsonAsync<List<mInformesDeCliente>>();
                if (listaInformes is not null)
                {
                    ListaInformesDeCliente = listaInformes;
                }
            }
            else
            {

            }

        }

        public async Task<mInforme> ObtenerInforme(string consecutivo, string esquema)
        {
            string url = "Informes/ObtengaElInforme/" + consecutivo + "/" + esquema;
            var informeAsociadoJson = await _http.GetAsync(url);
            if (informeAsociadoJson.StatusCode == HttpStatusCode.OK)
            {
                return await informeAsociadoJson.Content.ReadFromJsonAsync<mInforme>();
            }
            return null;
        }

        public async Task<bool> ActualizarInforme(mInforme informe, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Informes/ActualizaElInforme";
                string jsonData = JsonSerializer.Serialize(informe);
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
        public async Task<bool> CambiarEstadoDeInforme(mInformeEstado informe, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Informes/CambiaEstadoDeInforme";
                string jsonData = JsonSerializer.Serialize(informe);
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

        public async Task<bool> EliminarInforme(string consecutivo, string esquema)
        {
            try
            {
                string url = "Informes/EliminaInforme";
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                _http.DefaultRequestHeaders.Remove("X-consecutivo");
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                _http.DefaultRequestHeaders.Add("X-consecutivo", consecutivo);


                var response = await _http.DeleteAsync(url);
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
        public async Task<bool> EnviarCorreoDeReporteDeInforme(string esquema, string consecutivo)
        {
            try
            {
                string url = "Informes/EnviarTokenDeRecibidoDeInforme";
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
        public async Task<mTokenRecibidoInforme> ValidarTokenRecibidoDeInforme(string esquema, string token)
        {
            string url = "Informes/ValidaTokenRecibidoInforme/" + esquema + "/" + token;
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var tokenRecibido = await response.Content.ReadFromJsonAsync<mTokenRecibidoInforme>();
                if (tokenRecibido is not null)
                {
                    return tokenRecibido;
                }
                return new mTokenRecibidoInforme();
            }
            else
            {
                return new mTokenRecibidoInforme();
            }
        }

        public async Task<bool> ActivarRecibidoInforme(mTokenRecibidoInforme tokenRecibido, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Informes/ActivaRecibidoInforme";
                string jsonData = JsonSerializer.Serialize(tokenRecibido);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PutAsync(url, content);
                if( response.StatusCode == HttpStatusCode.OK)
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

        public async Task<string> AgregarInformeAsociado(string numero, string esquema)
        {
            try
            {
                
                string url = "Informes/AgregaInformeAsociado";
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                _http.DefaultRequestHeaders.Remove("X-Numero");
                _http.DefaultRequestHeaders.Add("X-Numero", numero);
                string jsonData = JsonSerializer.Serialize(numero);
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
