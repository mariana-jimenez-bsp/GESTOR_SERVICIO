using BSP.POS.Presentacion.Interfaces.Licencias;
using BSP.POS.Presentacion.Models.Licencias;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace BSP.POS.Presentacion.Services.Licencias
{
    public class LicenciasService: ILicenciasInterface
    {
        private readonly HttpClient _http;

        public LicenciasService(HttpClient htpp)
        {
            _http = htpp;
        }
        public mDatosLicencia licencia { get; set; } = new mDatosLicencia();
        public mCodigoLicencia codigoLicencia { get; set; } = new mCodigoLicencia();
        public async Task ObtenerDatosDeLicencia()
        {

            string url = "Licencias/ObtengaLosDatosDeLaLicencia";
            var response = await _http.GetAsync(url);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var licenciaResponse = await response.Content.ReadFromJsonAsync<mDatosLicencia>();
                if (licenciaResponse is not null)
                {
                    licencia = licenciaResponse;
                }
            }
            
        }

        public async Task ObtenerCodigoDeLicencia()
        {
            string url = "Licencias/ObtengaElCodigoDeLicencia";
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var licenciaResponse = await response.Content.ReadFromJsonAsync<mCodigoLicencia>();
                if (licenciaResponse is not null)
                {
                    codigoLicencia = licenciaResponse;
                }
            }
        }
        public async Task<bool> ActualizarDatosLicencia(mActualizarDatosLicencia datosLicencia)
        {

                string url = "Licencias/ActualizaDatosDeLicencia";
                string jsonData = JsonSerializer.Serialize(datosLicencia);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(url, content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string respuesta = await response.Content.ReadAsStringAsync();
                    
                    return bool.Parse(respuesta);
                }
                else
                {
                    return false;
                }
            
        }
    }
}
