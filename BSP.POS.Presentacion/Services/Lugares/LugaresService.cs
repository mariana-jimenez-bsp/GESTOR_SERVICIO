using BSP.POS.Presentacion.Interfaces.Lugares;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Lugares;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace BSP.POS.Presentacion.Services.Lugares
{
    public class LugaresService:ILugaresInterface
    {
        private readonly HttpClient _http;

        public LugaresService(HttpClient htpp)
        {
            _http = htpp;
        }

        public List<mLugares> listaDePaises { get; set; } = new List<mLugares>();
        public List<mLugares> listaDeProvincias { get; set; } = new List<mLugares>();
        public List<mLugares> ListaDeCantones { get; set; } = new List<mLugares>();
        public List<mLugares> listaDeDistritos { get; set; } = new List<mLugares>();
        public List<mLugares> listaDeBarrios { get; set; } = new List<mLugares>();

        public async Task ObtenerListaDePaises(string esquema)
        {
            string url = "Lugares/ObtengaLaListaDePaises";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var listaObtenida = await response.Content.ReadFromJsonAsync<List<mLugares>>();
                if (listaObtenida is not null)
                {
                    listaDePaises = listaObtenida;
                }
            }
            
        }
        public async Task ObtenerListaDeProvinciasPorPais(string esquema, string pais)
        {
            string url = "Lugares/ObtengaLaListaDeProvinciasPorPais";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Remove("X-Pais");
            _http.DefaultRequestHeaders.Add("X-Pais", pais);
            var response = await _http.GetAsync(url);
            if( response.StatusCode == HttpStatusCode.OK)
            {
                var listaObtenida = await response.Content.ReadFromJsonAsync<List<mLugares>>();
                if (listaObtenida is not null)
                {
                    listaDeProvincias = listaObtenida;
                }
            }
            
        }
        public async Task ObtenerListaDeCantonesPorProvincia(string esquema, string pais, string provincia)
        {
            string url = "Lugares/ObtengaLaListaDeCantonesPorProvincia";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Remove("X-Pais");
            _http.DefaultRequestHeaders.Add("X-Pais", pais);
            _http.DefaultRequestHeaders.Remove("X-Provincia");
            _http.DefaultRequestHeaders.Add("X-Provincia", provincia);
            var response =  await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var listaObtenida = await response.Content.ReadFromJsonAsync<List<mLugares>>();
                if (listaObtenida is not null)
                {
                    ListaDeCantones = listaObtenida;
                }
            }
            
        }
        public async Task ObtenerListaDeDistritosPorCanton(string esquema, string pais, string provincia, string canton)
        {
            string url = "Lugares/ObtengaLaListaDeDistritosPorCanton";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Remove("X-Pais");
            _http.DefaultRequestHeaders.Add("X-Pais", pais);
            _http.DefaultRequestHeaders.Remove("X-Provincia");
            _http.DefaultRequestHeaders.Add("X-Provincia", provincia);
            _http.DefaultRequestHeaders.Remove("X-Canton");
            _http.DefaultRequestHeaders.Add("X-Canton", canton);
            var response = await _http.GetAsync(url);
            if( response.StatusCode == HttpStatusCode.OK)
            {
                var listaObtenida = await response.Content.ReadFromJsonAsync<List<mLugares>>();
                if (listaObtenida is not null)
                {
                    listaDeDistritos = listaObtenida;
                }
            }
            
        }
        public async Task ObtenerListaDeBarriosPorDistrito(string esquema, string pais, string provincia, string canton, string distrito)
        {
            string url = "Lugares/ObtengaLaListaDeBarriosPorDistrito";
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Remove("X-Pais");
            _http.DefaultRequestHeaders.Add("X-Pais", pais);
            _http.DefaultRequestHeaders.Remove("X-Provincia");
            _http.DefaultRequestHeaders.Add("X-Provincia", provincia);
            _http.DefaultRequestHeaders.Remove("X-Canton");
            _http.DefaultRequestHeaders.Add("X-Canton", canton);
            _http.DefaultRequestHeaders.Remove("X-Distrito");
            _http.DefaultRequestHeaders.Add("X-Distrito", distrito);
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var listaObtenida = await response.Content.ReadFromJsonAsync<List<mLugares>>();
                if (listaObtenida is not null)
                {
                    listaDeBarrios = listaObtenida;
                }
            }
            
        }
    }
}
