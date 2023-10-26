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
        public mCodigoLicenciaYProducto codigoLicenciaYProducto { get; set; } = new mCodigoLicenciaYProducto();
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

        public async Task ObtenerCodigoDeLicenciaYProducto()
        {
            string url = "Licencias/ObtengaElCodigoDeLicenciaYProducto";
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var licenciaResponse = await response.Content.ReadFromJsonAsync<mCodigoLicenciaYProducto>();
                if (licenciaResponse is not null)
                {
                    codigoLicenciaYProducto = licenciaResponse;
                }
            }
        }

        public async Task<string> ObtenerCodigoDeLicenciaDesencriptado()
        {
            string url = "Licencias/ObtengaElCodigoDeLicenciaDesencriptado";
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var licenciaResponse = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(licenciaResponse))
                {
                    return licenciaResponse;
                }
                return null;
            }
            return null;
        }
        public async Task<bool> ActualizarDatosLicencia(mLicencia datosLicencia, byte[] codigoLicencia)
        {
            mActualizarDatosLicencia datosAActualizar = new mActualizarDatosLicencia();
            datosAActualizar.FechaInicio = datosLicencia.FechaInicio;
            datosAActualizar.FechaFin = datosLicencia.FechaFin;
            datosAActualizar.FechaAviso = datosLicencia.FechaAviso;
            datosAActualizar.CantidadCajas = datosLicencia.CantidadCajas;
            datosAActualizar.CantidadUsuarios = datosLicencia.CantidadUsuarios;
            datosAActualizar.MacAddress = datosLicencia.MacAddress;
            datosAActualizar.Pais = datosLicencia.Pais;
            datosAActualizar.CedulaJuridica = datosLicencia.CedulaJuridica;
            datosAActualizar.NombreCliente = datosLicencia.NombreCliente;
            datosAActualizar.Codigo = codigoLicencia;
            string url = "Licencias/ActualizaDatosDeLicencia";
                string jsonData = JsonSerializer.Serialize(datosAActualizar);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _http.PutAsync(url, content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string respuesta = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(respuesta))
                {
                    return bool.Parse(respuesta);
                }
                return false;
                }
                else
                {
                    return false;
                }
            
        }
    }
}
