using BSP.POS.Presentacion.Interfaces.Actividades;
using BSP.POS.Presentacion.Pages.Home;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using BSP.POS.Presentacion.Models.Actividades;

namespace BSP.POS.Presentacion.Services.Actividades
{
    public class ActividadesService : IActividadesInterface
    {
        private readonly HttpClient _http;

        public ActividadesService(HttpClient htpp)
        {
            _http = htpp;
        }

        public List<mActividadesAsociadas> ListaActividadesAsociadas { get; set; } = new List<mActividadesAsociadas>();
        public List<mActividades> ListaActividades { get; set; } = new List<mActividades>();

        public async Task ObtenerListaDeActividadesAsociadas(string consecutivo, string esquema)
        {
            string url = "Actividades/ObtengaLaListaDeActividadesAsociadas/" + consecutivo + "/" + esquema;
            var listaActividadesAsociadas = await _http.GetFromJsonAsync<List<mActividadesAsociadas>>(url);
            if (listaActividadesAsociadas is not null)
            {
                ListaActividadesAsociadas = listaActividadesAsociadas;
            }
        }

        public async Task ObtenerListaDeActividades(string esquema)
        {
            var listaActividades = await _http.GetFromJsonAsync<List<mActividades>>("Actividades/ObtengaLaListaDeActividades/" + esquema);
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
                string url = "Actividades/ActualizaListaDeActividades";
                string jsonData = JsonSerializer.Serialize(listaActividades);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var mensaje = await _http.PostAsync(url, content);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task ActualizarListaDeActividadesAsociadas(List<mActividadesAsociadas> listaActividades, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Actividades/ActualizaListaDeActividadesAsociadas";
                string jsonData = JsonSerializer.Serialize(listaActividades);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var mensaje = await _http.PostAsync(url, content);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task AgregarActividadDeInforme(mActividadAsociadaParaAgregar actividad, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Actividades/AgregaActividadDeInforme";
                string jsonData = JsonSerializer.Serialize(actividad);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var mensaje = await _http.PostAsync(url, content);
            }
            catch (Exception)
            {

            }
        }

        public async Task EliminarActividadDeInforme(string idActividad, string esquema)
        {
            try
            {
                string url = "Actividades/EliminaActividadDeInforme";
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                _http.DefaultRequestHeaders.Remove("X-IdActividad");
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                _http.DefaultRequestHeaders.Add("X-IdActividad", idActividad);


                var mensaje = await _http.DeleteAsync(url);
            }
            catch (Exception)
            {

            }
        }
    }
}
