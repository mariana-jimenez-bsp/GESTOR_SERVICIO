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
        [HttpGet("GenerarReportePorConsecutivo/{esquema}/{consecutivo}")]
        public async Task<IActionResult> GenerarReportePorConsecutivo(string esquema, string consecutivo)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync("https://localhost:44346/Api/GenerarReporte/" + esquema + "/" + consecutivo);

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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
