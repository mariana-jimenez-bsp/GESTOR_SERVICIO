﻿using BSP.POS.Presentacion.Interfaces.Reportes;
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
            var response = await _http.GetAsync("https://cloud.bspcr.com:4443/POS_Prueba_APICrystal_Gestor_servicios/Api/GenerarReporte/" + esquema + "/" + consecutivo);
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
