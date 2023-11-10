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

        public List<mProyectos> ListaProyectosActivos { get; set; } = new List<mProyectos>();
        public List<mDatosProyectos> ListaDatosProyectosActivos { get; set; } = new List<mDatosProyectos>();
        public List<mDatosProyectos> ListaDatosProyectosActivosDeCliente { get; set; } = new List<mDatosProyectos>();
        public List<mProyectos> ListaProyectosTerminados { get; set; } = new List<mProyectos>();
        public async Task ObtenerListaDeProyectosActivos(string esquema)
        {
                string url = "Proyectos/ObtengaLaListaDeProyectosActivos/" + esquema;
                var response = await _http.GetAsync(url);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    var listaProyectos = await response.Content.ReadFromJsonAsync<List<mProyectos>>();
                    if (listaProyectos is not null)
                    {
                        ListaProyectosActivos = listaProyectos;
                    }
                }
        }
        public async Task ObtenerDatosDeProyectosActivos(string esquema)
        {
            string url = "Proyectos/ObtengaLosDatosDeProyectosActivos/" + esquema;
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaProyectos = await response.Content.ReadFromJsonAsync<List<mDatosProyectos>>();
                if (listaProyectos is not null)
                {
                    ListaDatosProyectosActivos = listaProyectos;
                }
            }
        }

        public async Task ObtenerDatosDeProyectosActivosDeCliente(string esquema, string cliente)
        {
            string url = "Proyectos/ObtengaLosDatosDeProyectosActivosDeCliente/" + esquema + "/" + cliente;
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaProyectos = await response.Content.ReadFromJsonAsync<List<mDatosProyectos>>();
                if (listaProyectos is not null)
                {
                    ListaDatosProyectosActivosDeCliente = listaProyectos;
                }
            }
        }
        public async Task ObtenerListaDeProyectosTerminados(string esquema)
        {
            string url = "Proyectos/ObtengaLaListaDeProyectosTerminados/" + esquema;
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaProyectos = await response.Content.ReadFromJsonAsync<List<mProyectos>>();
                if (listaProyectos is not null)
                {
                    ListaProyectosTerminados = listaProyectos;
                }
            }
        }


        public async Task<bool> ActualizarListaDeProyectos(List<mProyectos> listaProyectos, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                string url = "Proyectos/ActualizaListaDeProyectos";
                string jsonData = JsonSerializer.Serialize(listaProyectos);
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PutAsync(url, content);
                if( response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public async Task<bool> AgregarProyecto(mProyectos proyecto, string esquema)
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
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> TerminarProyecto(string numero, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                _http.DefaultRequestHeaders.Remove("X-Numero");
                string url = "Proyectos/TerminaProyecto";
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                _http.DefaultRequestHeaders.Add("X-Numero", numero);
                var content = new StringContent(esquema, Encoding.UTF8, "application/json");

                var response = await _http.PutAsync(url, content);
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
