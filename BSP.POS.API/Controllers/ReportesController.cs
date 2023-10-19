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
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ReportesController(IWebHostEnvironment hostingEnvironment) {

            
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            _hostingEnvironment = hostingEnvironment;
            _tipoInicio = configuration["AppSettings:TipoInicio"];
            if (_tipoInicio == "debug")
            {
                var configurationSecrets = new ConfigurationBuilder()
                 .AddUserSecrets<Program>()
                 .Build();
                _urlApiCrystal = configurationSecrets["UrlApiCrystalDebug"];
            }
            else
            {
                _urlApiCrystal = configuration["AppSettings:UrlApiCrystalDeploy"];
            }

        }
        [HttpGet("GenerarReportePorConsecutivo/{esquema}/{consecutivo}")]
        public async Task<IActionResult> GenerarReportePorConsecutivo(string esquema, string consecutivo)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using (HttpClient client = new HttpClient(clientHandler))
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
                if (_tipoInicio == "deploy")
                {
                    string pathError = Path.Combine(_hostingEnvironment.ContentRootPath, "WhatsappService", "MensajesJson", "TextError.txt");
                    System.IO.File.WriteAllText(pathError, ex.ToString());
                    if (ex.InnerException != null)
                    {
                        System.IO.File.AppendAllText(pathError, "\nInner Exception: " + ex.InnerException.ToString());
                    }
                }
                return StatusCode(500, $"Error Interno de servidor: {ex.Message}");
            }
        }
    }
}
