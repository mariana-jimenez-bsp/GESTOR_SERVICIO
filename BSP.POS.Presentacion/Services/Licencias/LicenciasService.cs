using BSP.POS.Presentacion.Interfaces.Licencias;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Licencias;
using System.Net;
using System.Net.Http.Json;

namespace BSP.POS.Presentacion.Services.Licencias
{
    public class LicenciasService: ILicenciasInterface
    {
        private readonly HttpClient _http;

        public LicenciasService(HttpClient htpp)
        {
            _http = htpp;
        }
        public mLicencia licencia { get; set; } = new mLicencia();
        public async Task ObtenerEstadoDeLicencia()
        {

            string url = "Licencias/ObtengaElEstadoDeLaLicencia/";
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var licenciaResponse = await response.Content.ReadFromJsonAsync<mLicencia>();
                if (licenciaResponse is not null)
                {
                    licencia = licenciaResponse;
                }
            }
            
        }
    }
}
