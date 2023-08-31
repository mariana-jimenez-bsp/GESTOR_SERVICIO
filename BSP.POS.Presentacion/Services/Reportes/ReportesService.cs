using BSP.POS.Presentacion.Interfaces.Reportes;

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

                var response = await _http.GetAsync($"Reportes/GenerarReportePorConsecutivo/{esquema}/{consecutivo}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }

                throw new Exception("Error al obtener el PDF");
            
        }
    }
}
