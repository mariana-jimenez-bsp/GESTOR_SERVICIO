using BSP.POS.Presentacion.Interfaces.Proyectos;
using BSP.POS.Presentacion.Models.Proyectos;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Net;

namespace BSP.POS.Presentacion.Services.Proyectos
{
    public class ProyectosService: IProyectosInterface
    {
        private readonly HttpClient _http;

        public ProyectosService(HttpClient http)
        {
            _http = http;
        }

        public List<mProyectos> ListaProyectos { get; set; } = new List<mProyectos>();
        public async Task ObtenerListaDeProyectos(string esquema)
        {
                string url = "Proyectos/ObtengaLaListaDeProyectos/" + esquema;
                var response = await _http.GetAsync(url);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    var listaProyectos = await response.Content.ReadFromJsonAsync<List<mProyectos>>();
                    if (listaProyectos is not null)
                    {
                        ListaProyectos = listaProyectos;
                    }
                }
        }

        public async Task ActualizarListaDeProyectos(List<mProyectos> listaProyectos, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Proyectos/ActualizaListaDeProyectos";
                string jsonData = JsonSerializer.Serialize(listaProyectos);
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

        public async Task AgregarProyecto(mProyectos proyecto, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Proyectos/AgregaProyecto";
                string jsonData = JsonSerializer.Serialize(proyecto);
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
    }
}
