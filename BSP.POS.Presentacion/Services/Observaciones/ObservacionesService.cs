using BSP.POS.Presentacion.Interfaces.Observaciones;
using BSP.POS.Presentacion.Models.Observaciones;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

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
            string url = "https://localhost:7032/api/Observaciones/ObtengaLaListaDeObservacionesDeInforme/" + consecutivo + "/" + esquema;
            var listaDeObservacionesDeInforme = await _http.GetFromJsonAsync<List<mObservaciones>>(url);
            if (listaDeObservacionesDeInforme is not null)
            {
                ListaDeObservacionesDeInforme = listaDeObservacionesDeInforme;
            }
        }

        public async Task AgregarObservacionDeInforme(mObservaciones observacion, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "https://localhost:7032/api/Observaciones/AgregaObservacionDeInforme";
                string jsonData = JsonSerializer.Serialize(observacion);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var mensaje = await _http.PostAsync(url, content);
            }
            catch (Exception)
            {

            }
        }
    }
}
