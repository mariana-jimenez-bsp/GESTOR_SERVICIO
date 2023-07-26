using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSP.POS.UTILITARIOS.Clientes;
using BSP.POS.DATOS.POSDataSetTableAdapters;


namespace BSP.POS.DATOS.Clientes
{
    public class D_Clientes
    {
        public List<U_ListaClientes> ListaClientes(String pEsquema)
        {
            var LstClientes = new List<U_ListaClientes>();

            ListarClientesTableAdapter sp = new ListarClientesTableAdapter();

            var response = sp.GetData(pEsquema).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_ListaClientes cliente = new U_ListaClientes(item.CLIENTE, item.NOMBRE, item.ALIAS, item.CONTACTO, item.CARGO, item.DIRECCION, item.TELEFONO1,
                                                     item.CONTRIBUYENTE, item.MONEDA, item.NIVEL_PRECIO, item.PAIS, item.ZONA,
                                                     item.EXENTO_IMPUESTOS, item.E_MAIL, item.CODIGO_IMPUESTO, item.DIVISION_GEOGRAFICA1,
                                                     item.DIVISION_GEOGRAFICA2, item.DIVISION_GEOGRAFICA3, item.DIVISION_GEOGRAFICA4,
                                                     item.OTRAS_SENAS, item.RecordDate);

                    LstClientes.Add(cliente);
                }
                return LstClientes;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
        public List<U_ListarClientesRecientes> ListaClientesRecientes(String pEsquema)
        {
            var LstClientes = new List<U_ListarClientesRecientes>();

            ListarClientesRecientesTableAdapter sp = new ListarClientesRecientesTableAdapter();

            var response = sp.GetData(pEsquema).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_ListarClientesRecientes cliente = new U_ListarClientesRecientes(item.CLIENTE, item.NOMBRE, item.ALIAS, item.CONTACTO, item.CARGO, item.DIRECCION, item.TELEFONO1,
                                                     item.CONTRIBUYENTE, item.MONEDA, item.NIVEL_PRECIO, item.PAIS, item.ZONA,
                                                     item.EXENTO_IMPUESTOS, item.E_MAIL, item.CODIGO_IMPUESTO, item.DIVISION_GEOGRAFICA1,
                                                     item.DIVISION_GEOGRAFICA2, item.DIVISION_GEOGRAFICA3, item.DIVISION_GEOGRAFICA4,
                                                     item.OTRAS_SENAS, item.RecordDate, item.CreateDate);

                    LstClientes.Add(cliente);
                }
                return LstClientes;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public U_ClienteAsociado ClienteAsociado(String pEsquema, String pCliente)
        {
            var clienteAso = new U_ClienteAsociado();

            ObtenerCienteAsociadoTableAdapter sp = new ObtenerCienteAsociadoTableAdapter();

            var response = sp.GetData(pEsquema, pCliente).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_ClienteAsociado cliente = new U_ClienteAsociado(item.CLIENTE, item.NOMBRE, item.CONTACTO, item.CARGO);
                    clienteAso = cliente;
                }
                return clienteAso;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ActualizarListaDeClientes(List<U_ListaClientes> pClientes, string esquema)
        {
            POSDataSet.ActualizarListaDeClientesDataTable bTabla = new POSDataSet.ActualizarListaDeClientesDataTable();
            ActualizarListaDeClientesTableAdapter sp = new ActualizarListaDeClientesTableAdapter();
            try
            {
                foreach (var cliente in pClientes)
                {
                    var response = sp.GetData(esquema, cliente.CLIENTE, cliente.NOMBRE, cliente.ALIAS, cliente.CONTRIBUYENTE, cliente.TELEFONO);

                }
                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }


    }
}

