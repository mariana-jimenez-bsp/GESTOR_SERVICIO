using BSP.POS.Presentacion.Interfaces.Proyectos;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.UTILITARIOS.Actividades;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

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
            var listaProyectos = await _http.GetFromJsonAsync<List<mProyectos>>("Proyectos/ObtengaLaListaDeProyectos/" + esquema);
            if (listaProyectos is not null)
            {
                ListaProyectos = listaProyectos;
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

                var mensaje = await _http.PostAsync(url, content);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
