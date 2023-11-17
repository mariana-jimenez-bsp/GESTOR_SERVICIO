using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSP.POS.UTILITARIOS.Clientes;
using BSP.POS.DATOS.Clientes;
using Newtonsoft.Json;


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

                string clientes = JsonConvert.SerializeObject(list);
                return clientes;
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

                string clientes = JsonConvert.SerializeObject(list);
                return clientes;
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

        public string ActualizarListaDeClientes(List<U_ListaClientes> pClientes, string esquema)
        {
            string mensaje = string.Empty;
            mensaje = objCliente.ActualizarListaDeClientes(pClientes, esquema);
            return mensaje;
        }

        public string ObtenerListaClientesCorporaciones(string pEsquema)
        {
            var lista = new List<U_ClienteContado>();
            lista = objCliente.ObtenerListaClientesCorporaciones(pEsquema);
            string listaJson = JsonConvert.SerializeObject(lista);

            return listaJson;
        }

        public void AgregarCliente(U_AgregarCliente cliente, string pEsquema)
        {
            try
            {

                string monedaNivel = calculaMoneda(cliente.MONEDA);




                cliente.MONEDA_NIVEL = monedaNivel;
               
                objCliente.AgregarCliente(cliente, pEsquema);
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error: ", ex.InnerException.InnerException);
            }
        }

        public string calculaMoneda(string moneda)
        {
            if (moneda == "CRC")
            {
                moneda = "L";
            }
            else
            {
                moneda = "D";
            }
            return moneda;
        }

        public string calculaNivelPrecio(string nivel)
        {
            if (nivel == "CRC")
            {
                nivel = "ND-LOCAL";
            }
            else
            {
                nivel = "ND-DOLAR";
            }
            return nivel;
        }

        public string calculaDocGenerar(string dGenerar)
        {
            if (dGenerar == "Factura")
            {
                dGenerar = "F";
            }
            else
            {
                dGenerar = "B";
            }
            return dGenerar;
        }

        public string ValidarExistenciaDeCliente(string pEsquema, string pCliente)
        {
            string cliente = objCliente.ValidarExistenciaDeCliente(pEsquema, pCliente);
            return cliente;
        }

        public string ObtenerPaisDeCliente(string cliente, string esquema)
        {
            string pais = objCliente.ObtenerPaisDeCliente(cliente, esquema);
            return pais;
        }

        public string ObtenerContribuyenteDeCliente(string cliente, string esquema)
        {
            string contribuyente = objCliente.ObtenerContribuyenteDeCliente(cliente, esquema);
            return contribuyente;
        }

        public void EliminarCliente(string esquema, string cliente)
        {
            objCliente.EliminarCliente(esquema, cliente);
        }
    }
}
