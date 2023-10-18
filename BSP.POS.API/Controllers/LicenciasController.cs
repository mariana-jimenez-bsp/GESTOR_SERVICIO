using BSP.POS.NEGOCIOS.Licencias;
using BSP.POS.UTILITARIOS.Licencias;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;
using System.Text;
using System.Text.Json;
using System.Net;

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenciasController : ControllerBase
    {
        private readonly N_Licencias licencias;

        public LicenciasController()
        {
            licencias = new N_Licencias();
        }
        [HttpGet("ObtengaLosDatosDeLaLicencia")]
        public IActionResult ObtengaLosDatosDeLaLicencia()
        {
            try
            {
                string licencia = licencias.ObtenerDatosDeLicencia();
                if(string.IsNullOrEmpty(licencia))
                {
                    return NotFound();
                }
                return Ok(licencia);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpGet("ObtengaElCodigoDeLicenciaYProducto")]
        public IActionResult ObtengaElCodigoDeLicenciaYProducto()
        {
            try
            {
                string licencia = licencias.ObtenerCodigoDeLicenciaYProducto();
                if (string.IsNullOrEmpty(licencia))
                {
                    return NotFound();
                }
                return Ok(licencia);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet("ObtengaElCodigoDeLicenciaDesencriptado")]
        public IActionResult ObtengaElCodigoDeLicenciaDesencriptado()
        {
            try
            {
                string licencia = licencias.ObtenerCodigoDeLicenciaDescencriptado();
                if (string.IsNullOrEmpty(licencia))
                {
                    return NotFound();
                }
                return Ok(licencia);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("ActualizaDatosDeLicencia")]
        public IActionResult ActualizaDatosDeLicencia([FromBody] U_ActualizarDatosLicencia datosLicencia)
        {
            try
            {
                string resultado = licencias.ActualizarDatosLicencia(datosLicencia);
                if (string.IsNullOrEmpty(resultado))
                {
                    return NotFound();
                }
                return Ok(resultado);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("ConectaApiEnviaXML")]
        public async Task<IActionResult> ConectaApiEnviaXML([FromBody] U_LicenciaByte licenciaLlave)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient http = new HttpClient(clientHandler);

            try
            {
                //string url = "https://localhost:7121/api/Licencias/EnviaXML";
                //string url = "http://localhost/Prueba_API_POS_Licencia/api/Licencias/EnviaXML";
                string url = "https://192.168.2.21/Prueba_API_POS_Licencia/api/Licencias/EnviaXML";
                string jsonData = JsonSerializer.Serialize(licenciaLlave);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await http.PostAsync(url, content);
                if (response.StatusCode == HttpStatusCode.OK)
                {

                    var datosLicencia = await response.Content.ReadFromJsonAsync<U_Licencia>();
                    if (datosLicencia != null)
                    {
                        return Ok(datosLicencia);
                    }
                    return BadRequest();


                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }

        }
    }
}
