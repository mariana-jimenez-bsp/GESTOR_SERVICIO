using BSP.POS.Presentacion.Interfaces.Actividades;
using BSP.POS.Presentacion.Models;
using BSP.POS.Presentacion.Pages.Home;
using BSP.POS.UTILITARIOS.Informes;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace BSP.POS.Presentacion.Services.Actividades
{
    public class ActividadesService : IActividadesInterface
    {
        private readonly HttpClient _http;

        public ActividadesService(HttpClient htpp)
        {
            _http = htpp;
        }

        public List<mActividades> ListaActividadesAsociadas { get; set; } = new List<mActividades>();
        public List<mActividades> ListaActividades { get; set; } = new List<mActividades>();

        public async Task ObtenerListaDeActividadesAsociadas(string consecutivo)
        {
            var listaActividadesAsociadas = await _http.GetFromJsonAsync<List<mActividades>>("https://localhost:7032/api/Actividades/ObtengaLaListaDeActividadesAsociadas/" + consecutivo);
            if (listaActividadesAsociadas is not null)
            {
                ListaActividadesAsociadas = listaActividadesAsociadas;
            }
        }

        public async Task ObtenerListaDeActividades(string esquema)
        {
            var listaActividades = await _http.GetFromJsonAsync<List<mActividades>>("https://localhost:7032/api/Actividades/ObtengaLaListaDeActividades/" + esquema);
            if (listaActividades is not null)
            {
                ListaActividades = listaActividades;
            }
        }

        public async Task ActualizarListaDeActividades(List<mActividades> listaActividades, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "https://localhost:7032/api/Actividades/ActualizaListaDeActividades";
                string jsonData = JsonSerializer.Serialize(listaActividades);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var mensaje = await _http.PostAsync(url, content);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
