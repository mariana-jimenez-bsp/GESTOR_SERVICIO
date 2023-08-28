using BSP.POS.NEGOCIOS.AcortarLinkService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcortarController : ControllerBase
    {
        private readonly IAcortarLinkInterface _acortar;
        private readonly string _tokenAcortar = string.Empty;
        public AcortarController(IAcortarLinkInterface acortar) { 
            _acortar = acortar;
            //var configuration = new ConfigurationBuilder()
            //.AddJsonFile("appsettings.json")
            //.Build();

           
            _tokenAcortar = Environment.GetEnvironmentVariable("TokenAcortarGS");
        }
        [HttpGet("AcortarLink")]
        public async Task<string> AcortarLink()
        {
            try
            {
                string response = await _acortar.AcortarLink(_tokenAcortar);
                return response;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }
    }
}
