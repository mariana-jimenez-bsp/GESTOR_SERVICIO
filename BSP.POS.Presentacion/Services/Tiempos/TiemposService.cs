using BSP.POS.Presentacion.Interfaces.Tiempos;
using BSP.POS.Presentacion.Models;
using BSP.POS.Presentacion.Pages.Home;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace BSP.POS.Presentacion.Services.Tiempos
{
    public class TiemposService : ITiemposInterface
    {
        private readonly HttpClient _http;

        public TiemposService(HttpClient htpp)
        {
            _http = htpp;
        }
        public List<mTiempos> ListaTiempos { get; set; } = new List<mTiempos>();

        public async Task ObtenerListaTIempos()
        {
            var listaTiempos = await _http.GetFromJsonAsync<List<mTiempos>>("https://localhost:7032/api/Tiempos/ObtengaLaListaDeTiempos");
            if (listaTiempos is not null)
            {
                ListaTiempos = listaTiempos;
            }
        }

        public async Task ActualizarListaDeTiempo(List<mTiempos> listaTiempos)
        {
            try
            {
                string url = "https://localhost:7032/api/Tiempos/ActualizaLaListaDeTiempos";
                string jsonData = JsonSerializer.Serialize(listaTiempos);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var mensaje = await _http.PostAsync(url, content);
            }
            catch (Exception ex)
            {
              
            }
            

        }
    }
}
