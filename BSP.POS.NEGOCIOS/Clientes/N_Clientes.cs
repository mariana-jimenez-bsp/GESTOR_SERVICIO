using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSP.POS.UTILITARIOS.Clientes;
using BSP.POS.DATOS.Clientes;
using Newtonsoft.Json;
using System.Security.Permissions;

namespace BSP.POS.NEGOCIOS.Clientes
{
    public class N_Clientes
    {
        D_Clientes objCliente = new D_Clientes();

        public string ListarClientes(String pEsquema)
        {
            try
            {
                List<U_ListaClientes> list = new List<U_ListaClientes>();

                list = objCliente.ListaClientes(pEsquema);

                string cliente = JsonConvert.SerializeObject(list);
                return cliente;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ListarClientesRecientes(String pEsquema)
        {
            try
            {
                List<U_ListarClientesRecientes> list = new List<U_ListarClientesRecientes>();

                list = objCliente.ListaClientesRecientes(pEsquema);

                string cliente = JsonConvert.SerializeObject(list);
                return cliente;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ObtenerClienteAsociado(String pEsquema, String pCliente)
        {
            try
            {
                U_ClienteAsociado clienteAso = new U_ClienteAsociado();

                clienteAso = objCliente.ClienteAsociado(pEsquema, pCliente);

                string cliente = JsonConvert.SerializeObject(clienteAso);
                return cliente;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
    }
}
