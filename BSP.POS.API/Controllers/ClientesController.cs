using Microsoft.AspNetCore.Mvc;
using BSP.POS.NEGOCIOS.Clientes;
using BSP.POS.UTILITARIOS.Clientes;
using Newtonsoft.Json;
using BSP.POS.API.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private N_Clientes clientes;
        public ClientesController()
        {
            clientes = new N_Clientes();

        }
        // GET: api/<ClientesController>
        [HttpGet("ObtengaLaListaDeClientes")]
        public string ObtengaLaListaDeClientes()
        {
            try
            {
                string esquema = "BSP";
                string listaClientesJson = clientes.ListarClientes(esquema);
                return listaClientesJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }

        [HttpGet("ObtengaLaListaDeClientesRecientes")]
        public string ObtengaLaListaDeClientesRecientes()
        {
            try
            {
                string esquema = "BSP";
                string listaClientesRecientesJson = clientes.ListarClientesRecientes(esquema);
                return listaClientesRecientesJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
           
        }

        [HttpGet("ObtengaElClienteAsociado/{cliente}")]
        public string ObtengaElClienteAsociado(string cliente)
        {
            try
            {
                string esquema = "BSP";
                var clienteAsociadoJson = clientes.ObtenerClienteAsociado(esquema, cliente);
                return clienteAsociadoJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }

        
    }
}
