using BSP.POS.Presentacion.Interfaces.Actividades;
using BSP.POS.Presentacion.Pages.Home;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using BSP.POS.Presentacion.Models.Actividades;
using System.Net;

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
        public List<mActividades> ListaActividadesDeUsuario { get; set; } = new List<mActividades>();
        public async Task ObtenerListaDeActividadesAsociadas(string consecutivo, string esquema)
        {
            string url = "Actividades/ObtengaLaListaDeActividadesAsociadas/" + consecutivo + "/" + esquema;
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var listaActividadesAsociadas = await response.Content.ReadFromJsonAsync<List<mActividadesAsociadas>>();
                if (listaActividadesAsociadas is not null)
                {
                    ListaActividadesAsociadas = listaActividadesAsociadas;
                }
            }
            
        }

        public async Task ObtenerListaDeActividades(string esquema)
        {
            string url = "Actividades/ObtengaLaListaDeActividades/" + esquema;
            var response  = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var listaActividades = await response.Content.ReadFromJsonAsync<List<mActividades>>();
                if (listaActividades is not null)
                {
                    ListaActividades = listaActividades;
                }
            }
            
        }

        public async Task ObtenerListaDeActividadesActivas(string esquema)
        {
            string url = "Actividades/ObtengaLaListaDeActividadesActivas/" + esquema;
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaActividades = await response.Content.ReadFromJsonAsync<List<mActividades>>();
                if (listaActividades is not null)
                {
                    ListaActividades = listaActividades;
                }
            }

        }
        public async Task ObtenerListaDeActividadesPorUsuario(string esquema, string codigo)
        {
            string url = "Actividades/ObtengaLaListaDeActividadesPorUsuario";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Remove("X-CodigoUsuario");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Add("X-CodigoUsuario", codigo);
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaActividades = await response.Content.ReadFromJsonAsync<List<mActividades>>();
                if (listaActividades is not null)
                {
                    ListaActividadesDeUsuario = listaActividades;
                }
            }
        }

        public async Task ObtenerListaDeActividadesActivasPorUsuario(string esquema, string codigo)
        {
            string url = "Actividades/ObtengaLaListaDeActividadesActivasPorUsuario";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Remove("X-CodigoUsuario");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Add("X-CodigoUsuario", codigo);
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaActividades = await response.Content.ReadFromJsonAsync<List<mActividades>>();
                if (listaActividades is not null)
                {
                    ListaActividadesDeUsuario = listaActividades;
                }
            }
        }
        public async Task<bool> ActualizarListaDeActividades(List<mActividades> listaActividades, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Actividades/ActualizaListaDeActividades";
                string jsonData = JsonSerializer.Serialize(listaActividades);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PutAsync(url, content);
                if( response.StatusCode == HttpStatusCode.OK )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ActualizarListaDeActividadesAsociadas(List<mActividadesAsociadas> listaActividades, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Actividades/ActualizaListaDeActividadesAsociadas";
                string jsonData = JsonSerializer.Serialize(listaActividades);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PutAsync(url, content);
                if(response.StatusCode == HttpStatusCode.OK )
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

        public async Task AgregarActividadDeInforme(mActividadAsociadaParaAgregar actividad, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Actividades/AgregaActividadDeInforme";
                string jsonData = JsonSerializer.Serialize(actividad);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(url, content);
                if( response.StatusCode == HttpStatusCode.OK )
                {

                }
            }
            catch (Exception)
            {

            }
        }

        public async Task<bool> EliminarActividadDeInforme(string idActividad, string esquema)
        {
            try
            {
                string url = "Actividades/EliminaActividadDeInforme";
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                _http.DefaultRequestHeaders.Remove("X-IdActividad");
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                _http.DefaultRequestHeaders.Add("X-IdActividad", idActividad);


                var response = await _http.DeleteAsync(url);
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

        public async Task<bool> AgregarActividad(mActividades actividad, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Actividades/AgregaActividad";
                string jsonData = JsonSerializer.Serialize(actividad);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(url, content);
                if(response.StatusCode == HttpStatusCode.OK )
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

        public async Task<bool> EliminarActividad(string esquema, string codigo)
        {
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Remove("X-Codigo");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Add("X-Codigo", codigo);
            string url = "Actividades/EliminaLaActividad";
            var response = await _http.DeleteAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CambiarEstadoActividad(string esquema, string codigo, string estado)
        {
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Remove("X-Codigo");
            _http.DefaultRequestHeaders.Remove("X-Estado");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Add("X-Codigo", codigo);
            _http.DefaultRequestHeaders.Add("X-Estado", estado);
            string url = "Actividades/CambiaEstadoActividad";
            var content = new StringContent(esquema, Encoding.UTF8, "application/json");
            var response = await _http.PutAsync(url, content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ValidarActividadAsociadaInforme(string esquema, string codigo)
        {
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Remove("X-Codigo");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Add("X-Codigo", codigo);
            string url = "Actividades/ValidaActividadAsociadaInforme";
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string resultado = await response.Content.ReadAsStringAsync();
                if(resultado == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
               
            }
            return false;
        }
    }
}
