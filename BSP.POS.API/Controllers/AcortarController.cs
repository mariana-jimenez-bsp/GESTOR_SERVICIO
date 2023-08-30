using BSP.POS.NEGOCIOS.AcortarLinkService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Web;

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

           
            _tokenAcortar = "93e1b3f98e7a7aa74056267d0490ca198b9fe6fb";
        }
        [HttpGet("AcortarLink")]
        public async Task<string> AcortarLink()
        {
            return await _acortar.AcortarLink(_tokenAcortar);

        }
    }
}
