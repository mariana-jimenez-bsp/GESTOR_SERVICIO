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
        public List<mProyectos> ListaProyectosTerminadosYCancelados { get; set; } = new List<mProyectos>();
        public mProyectos ProyectoAsociado { get; set; } = new mProyectos();
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
        public async Task ObtenerListaDeProyectosTerminadosYCancelados(string esquema)
        {
            string url = "Proyectos/ObtengaLaListaDeProyectosTerminadosYCancelados/" + esquema;
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listaProyectos = await response.Content.ReadFromJsonAsync<List<mProyectos>>();
                if (listaProyectos is not null)
                {
                    ListaProyectosTerminadosYCancelados = listaProyectos;
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
        public async Task<bool> CambiarEstadoProyecto(string numero, string estado, string esquema)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("X-Esquema");
                _http.DefaultRequestHeaders.Remove("X-Estado");
                _http.DefaultRequestHeaders.Remove("X-Numero");
                string url = "Proyectos/CambiaEstadoProyecto";
                _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
                _http.DefaultRequestHeaders.Add("X-Estado", estado);
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

        public async Task ObtenerProyecto(string esquema, string numero)
        {
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Remove("X-Numero");
            string url = "Proyectos/ObtengaElProyecto";
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Add("X-Numero", numero);
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var proyecto = await response.Content.ReadFromJsonAsync<mProyectos>();
                if (proyecto is not null)
                {
                    ProyectoAsociado = proyecto;
                }
            }
        }
        public async Task<bool> EliminarProyecto(string esquema, string numero)
        {
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Remove("X-Numero");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Add("X-Numero", numero);
            string url = "Proyectos/EliminaElProyecto";
            var response = await _http.DeleteAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
    }
}
