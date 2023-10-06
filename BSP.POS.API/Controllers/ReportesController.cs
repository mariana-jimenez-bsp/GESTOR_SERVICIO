using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportesController : ControllerBase
    {
        private readonly string _tipoInicio = string.Empty;
        private readonly string _urlApiCrystal = string.Empty;
        public ReportesController() {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _tipoInicio = configuration["AppSettings:TipoInicio"];
            if (_tipoInicio == "debug")
            {
                _urlApiCrystal = "https://localhost:44346/";
            }
            else
            {
                _urlApiCrystal = "http://localhost/POS_Prueba_APICrystal_Gestor_servicios/";
            }

        }
        [HttpGet("GenerarReportePorConsecutivo/{esquema}/{consecutivo}")]
        public async Task<IActionResult> GenerarReportePorConsecutivo(string esquema, string consecutivo)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(_urlApiCrystal + "Api/GenerarReporte/" + esquema + "/" + consecutivo);

                    if (response.IsSuccessStatusCode)
                    {
                        var fileBytes = await response.Content.ReadAsByteArrayAsync();

                        // Envía los bytes directamente al cliente como un archivo PDF
                        return File(fileBytes, "application/pdf", "ReporteInforme.pdf");
                    }
                    else
                    {
                        return BadRequest(await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error Interno de servidor: {ex.Message}");
            }
        }
    }
}
