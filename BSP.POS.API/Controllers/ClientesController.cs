using Microsoft.AspNetCore.Mvc;
using BSP.POS.NEGOCIOS.Clientes;
using BSP.POS.UTILITARIOS.Clientes;
using Newtonsoft.Json;
using BSP.POS.API.Models;
using BSP.POS.UTILITARIOS.Tiempos;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpPost("ActualizaListaDeClientes")]
        public string ActualizaListaDeClientes([FromBody] List<mClientes> datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];

                List<U_ListaClientes> listaClientes = new List<U_ListaClientes>();
                foreach (var item in datos)
                {
                    U_ListaClientes cliente = new U_ListaClientes();
                    cliente.CLIENTE = item.CLIENTE;
                    cliente.NOMBRE = item.NOMBRE;
                    cliente.ALIAS = item.ALIAS;
                    cliente.CONTRIBUYENTE = item.CONTRIBUYENTE;
                    cliente.TELEFONO1 = item.TELEFONO1;
                    cliente.TELEFONO2 = item.TELEFONO2;
                    cliente.E_MAIL = item.E_MAIL;
                    listaClientes.Add(cliente);
                }

                string mensaje = clientes.ActualizarListaDeClientes(listaClientes, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


    }
}
