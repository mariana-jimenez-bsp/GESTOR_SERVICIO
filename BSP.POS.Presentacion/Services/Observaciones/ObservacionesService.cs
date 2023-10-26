using BSP.POS.Presentacion.Interfaces.Observaciones;
using BSP.POS.Presentacion.Models.Observaciones;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Net;

namespace BSP.POS.Presentacion.Services.Observaciones
{
    public class ObservacionesService: IObservacionesInterface
    {
        private readonly HttpClient _http;

        public ObservacionesService(HttpClient htpp)
        {
            _http = htpp;
        }
        public List<mObservaciones> ListaDeObservacionesDeInforme { get; set; } = new List<mObservaciones>();
        public async Task ObtenerListaDeObservacionesDeInforme(string consecutivo, string esquema)
        {
            string url = "Observaciones/ObtengaLaListaDeObservacionesDeInforme/" + consecutivo + "/" + esquema;
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var listaDeObservacionesDeInforme = await response.Content.ReadFromJsonAsync<List<mObservaciones>>();
                if (listaDeObservacionesDeInforme is not null)
                {
                    ListaDeObservacionesDeInforme = listaDeObservacionesDeInforme;
                }
            }
            
        }

        public async Task<bool> AgregarObservacionDeInforme(mObservaciones observacion, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Observaciones/AgregaObservacionDeInforme";
                string jsonData = JsonSerializer.Serialize(observacion);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(url, content);
                if( response.StatusCode == HttpStatusCode.OK )
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
