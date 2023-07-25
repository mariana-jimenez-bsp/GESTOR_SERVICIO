using BSP.POS.Presentacion.Interfaces.Actividades;
using BSP.POS.Presentacion.Models;
using BSP.POS.UTILITARIOS.Informes;
using System.Net.Http.Json;

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

        public async Task ObtenerListaDeActividadesAsociadas(string consecutivo)
        {
            var listaActividadesAsociadas = await _http.GetFromJsonAsync<List<mActividades>>("https://localhost:7032/api/Actividades/ObtengaLaListaDeActividadesAsociadas/" + consecutivo);
            if (listaActividadesAsociadas is not null)
            {
                ListaActividadesAsociadas = listaActividadesAsociadas;
            }
        }
    }
}
