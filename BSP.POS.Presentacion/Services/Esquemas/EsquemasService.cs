using BSP.POS.Presentacion.Interfaces.Esquemas;
using BSP.POS.Presentacion.Models.Esquemas;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BSP.POS.Presentacion.Services.Esquemas
{
    public class EsquemasService : IEsquemasInterface
    {

        private readonly HttpClient _http;
        public EsquemasService(HttpClient htpp)
        {
            _http = htpp;
        }
        public List<mEsquemas> ListaEsquemas { get; set; } = new List<mEsquemas>();
        public List<mDatosEsquemasDeUsuario> ListaEsquemasDeUsuario { get; set; } = new List<mDatosEsquemasDeUsuario>(); 

        public async Task ObtenerListaDeEsquemas()
        {
            string url = "Esquemas/ObtengaListaDeEsquemas";
           
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaEsquemas = await response.Content.ReadFromJsonAsync<List<mEsquemas>>();
                if (listaEsquemas is not null)
                {
                    ListaEsquemas = listaEsquemas;
                }
            }
        }

        public async Task ObtenerListaDeEsquemasDeUsuario(string codigo)
        {
            string url = "Esquemas/ObtengaListaDeEsquemasDeUsuario";
            _http.DefaultRequestHeaders.Remove("X-Codigo");
            _http.DefaultRequestHeaders.Add("X-Codigo", codigo);
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaEsquemas = await response.Content.ReadFromJsonAsync<List<mDatosEsquemasDeUsuario>>();
                if (listaEsquemas is not null)
                {
                    ListaEsquemasDeUsuario = listaEsquemas;
                }
            }
        }

        public async Task<bool> ActualizarListaDeEsquemasDeUsuario(List<string> listaEsquemas, string codigo)
        {
            string url = "Esquemas/ActualizaListaDeEsquemasDeUsuario";
            string jsonData = JsonSerializer.Serialize(listaEsquemas);
            _http.DefaultRequestHeaders.Remove("X-Codigo");
            _http.DefaultRequestHeaders.Add("X-Codigo", codigo);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _http.PutAsync(url, content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
    }
}
