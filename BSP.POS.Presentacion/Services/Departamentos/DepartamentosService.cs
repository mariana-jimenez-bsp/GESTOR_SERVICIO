using BSP.POS.Presentacion.Interfaces.Departamentos;
using BSP.POS.Presentacion.Models.Departamentos;
using System.Net.Http.Json;
using System.Net;
using BSP.POS.Presentacion.Pages.Home;
using System.Text.Json;
using System.Text;

namespace BSP.POS.Presentacion.Services.Departamentos
{
    public class DepartamentosService : IDepartamentosInterface
    {
        private readonly HttpClient _http;

        public DepartamentosService(HttpClient htpp)
        {
            _http = htpp;
        }
        public List<mDepartamentos> listaDepartamentos { get; set; } = new List<mDepartamentos>();
        public async Task ObtenerListaDeDepartamentos(string esquema)
        {
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            string url = "Departamentos/ObtengaListaDeDepartamentos";
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var lista = await response.Content.ReadFromJsonAsync<List<mDepartamentos>>();
                if (lista is not null)
                {
                    listaDepartamentos = lista;
                }
            }
        }

        public async Task<bool> ActualizarListaDeDepartamentos(List<mDepartamentos> listaDepartamentosActualizar, string esquema)
        {
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            string url = "Departamentos/ActualizaListaDepartamentos";
            string jsonData = JsonSerializer.Serialize(listaDepartamentosActualizar);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _http.PutAsync(url, content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> AgregarDepartamento(mDepartamentos departamento, string esquema)
        {
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            string url = "Departamentos/AgregaDepartamento";
            string jsonData = JsonSerializer.Serialize(departamento);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync(url, content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> EliminarDepartamento(string esquema, string codigo)
        {
            _http.DefaultRequestHeaders.Remove("X-Esquema");
            _http.DefaultRequestHeaders.Remove("X-Codigo");
            _http.DefaultRequestHeaders.Add("X-Esquema", esquema);
            _http.DefaultRequestHeaders.Add("X-Codigo", codigo);
            string url = "Departamentos/EliminaElDepartamento";
            var response = await _http.DeleteAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
    }
}
