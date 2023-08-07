using BSP.POS.Presentacion.Interfaces.Informes;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Pages.Home;
using BSP.POS.UTILITARIOS.Proyectos;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
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
        public mInformeAsociado InformeAsociado { get; set; } = new mInformeAsociado();
        

        public async Task ObtenerListaDeInformesAsociados(string cliente, string esquema)
        {
            string url = "https://localhost:7032/api/Informes/ObtengaLaListaDeInformesAsociados/" + cliente + "/" + esquema;
            var listaInformesAsociados = await _http.GetFromJsonAsync<List<mInformes>>(url);
            if (listaInformesAsociados is not null)
            {
                ListaInformesAsociados = listaInformesAsociados;
            }
        }

        public async Task<mInformeAsociado?> ObtenerInformeAsociado(string consecutivo, string esquema)
        {
            string url = "https://localhost:7032/api/Informes/ObtengaElInformeAsociado/" + consecutivo + "/" + esquema;
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
                string url = "https://localhost:7032/api/Informes/ActualizaElInformeAsociado";
                string jsonData = JsonSerializer.Serialize(informe);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var mensaje = await _http.PostAsync(url, content);
            }
            catch (Exception ex)
            {

            }
        }
        public async Task CambiarEstadoDeInforme(mInformeEstado informe, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "https://localhost:7032/api/Informes/CambiaEstadoDeInforme";
                string jsonData = JsonSerializer.Serialize(informe);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var mensaje = await _http.PostAsync(url, content);
            }
            catch (Exception)
            {

            }
        }

        public async Task EliminarInforme(string consecutivo, string esquema)
        {
            try
            {
                string url = "https://localhost:7032/api/Informes/EliminaInforme";
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
        public async Task<bool> EnviarCorreoDeAprobacionDeInforme(mObjetosParaCorreoAprobacion objetosParaCorreo)
        {
            try
            {
                string url = "https://localhost:7032/api/Informes/EnviarTokenDeAprobacionDeInforme";
                string jsonData = JsonSerializer.Serialize(objetosParaCorreo);
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

    }
}
