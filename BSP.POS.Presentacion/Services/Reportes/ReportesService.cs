using BSP.POS.Presentacion.Interfaces.Reportes;
using BSP.POS.Presentacion.Pages.Clientes;

namespace BSP.POS.Presentacion.Services.Reportes
{
    public class ReportesService: IReportesInterface
    {
        private readonly HttpClient _http;

        public ReportesService(HttpClient htpp)
        {
            _http = htpp;
        }
        public async Task<byte[]> GenerarReporteDeInforme(string esquema, string consecutivo)
        {

            //var response = await _http.GetAsync($"Reportes/GenerarReportePorConsecutivo/{esquema}/{consecutivo}");

            //if (response.IsSuccessStatusCode)
            //{
            //    return await response.Content.ReadAsByteArrayAsync();
            //}
            //var reponseMessage = response.RequestMessage;
            //throw new Exception("Error al obtener el PDF");
            string url = "https://cloud.bspcr.com:4443/PRUEBA_GESTOR_SERVICIOS_API_CRYSTAL/Api/GenerarReporte/";
            //string url = "https://localhost:44346/Api/GenerarReporte/";
            var response = await _http.GetAsync(url + esquema + "/" + consecutivo);
            if (response.IsSuccessStatusCode)
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();

                // Envía los bytes directamente al cliente como un archivo PDF
                return fileBytes;
            }
            else
            {
                throw new Exception("Error al obtener el PDF");
            }

        }
    }
}
