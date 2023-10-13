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
                                                     item.TELEFONO2, item.CONTRIBUYENTE, item.MONEDA, item.NIVEL_PRECIO, item.PAIS, item.ZONA,
                                                     item.EXENTO_IMPUESTOS, item.E_MAIL, item.CODIGO_IMPUESTO, item.DIVISION_GEOGRAFICA1,
                                                     item.DIVISION_GEOGRAFICA2, item.DIVISION_GEOGRAFICA3, item.DIVISION_GEOGRAFICA4,
                                                     item.OTRAS_SENAS, item.RecordDate, item.IMAGEN);

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
                                                     item.TELEFONO2, item.CONTRIBUYENTE, item.MONEDA, item.NIVEL_PRECIO, item.PAIS, item.ZONA,
                                                     item.EXENTO_IMPUESTOS, item.E_MAIL, item.CODIGO_IMPUESTO, item.DIVISION_GEOGRAFICA1,
                                                     item.DIVISION_GEOGRAFICA2, item.DIVISION_GEOGRAFICA3, item.DIVISION_GEOGRAFICA4,
                                                     item.OTRAS_SENAS, item.RecordDate, item.CreateDate, item.IMAGEN);

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
                    U_ClienteAsociado cliente = new U_ClienteAsociado(item.CLIENTE, item.NOMBRE, item.CONTACTO, item.CARGO, item.IMAGEN);
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
                    var response = sp.GetData(esquema, cliente.CLIENTE, cliente.NOMBRE, cliente.ALIAS, cliente.CONTRIBUYENTE, cliente.TELEFONO1, cliente.TELEFONO2, cliente.E_MAIL);

                }
                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }

        public List<U_ClienteContado> ObtenerListaClientesCorporaciones(string pEsquema)
        {
            LISTAR_CORPORACIONESTableAdapter sp = new LISTAR_CORPORACIONESTableAdapter();
            var response = sp.GetData(pEsquema);
            List<U_ClienteContado> lista = new List<U_ClienteContado>();

            foreach (var item in response)
            {
                U_ClienteContado c = new U_ClienteContado();
                c.cliente = item.CLIENTE;
                c.descripcion = item.DESCRIPCION;

                lista.Add(c);
            }

            return lista;
        }

        public void AgregarCliente(U_AgregarCliente cliente, string esquema)
        {
            try
            {
                InsertarClienteTableAdapter adapter = new InsertarClienteTableAdapter();
                POSDataSet.InsertarClienteDataTable tabla = new POSDataSet.InsertarClienteDataTable();

                string APLIC_ABIERTAS = "";
                string USAR_DESC_CORP = "";
                string USAR_PRECIOS_CORP = "";

                if (cliente.ES_CORPORACION == "S")
                {

                    APLIC_ABIERTAS = "S";
                    USAR_DESC_CORP = "N";
                    USAR_PRECIOS_CORP = "N";
                }
                else
                {

                    APLIC_ABIERTAS = "S";
                    USAR_DESC_CORP = "S";
                    USAR_PRECIOS_CORP = "S";
                }


                adapter.Fill(tabla, esquema, cliente.CLIENTE, cliente.NOMBRE, cliente.ALIAS, cliente.CONTACTO, cliente.CARGO, cliente.DIRECCION,
                                cliente.TELEFONO1, cliente.TELEFONO2, cliente.CONTRIBUYENTE, cliente.TIPO_NIT, cliente.MONEDA, cliente.CONDICION_PAGO,
                                cliente.NIVEL_PRECIO, cliente.MONEDA_NIVEL, cliente.PAIS, cliente.ZONA, cliente.E_MAIL, cliente.DIVISION_GEOGRAFICA1,
                                cliente.DIVISION_GEOGRAFICA2, cliente.USUARIO_CREACION, cliente.DIVISION_GEOGRAFICA3, cliente.DIVISION_GEOGRAFICA4,
                                cliente.OTRAS_SENAS, cliente.DOC_A_GENERAR, cliente.EXENTO_IMPUESTOS, cliente.EXENCION_IMP1, cliente.EXENCION_IMP2, cliente.DESCUENTO.ToString(),
                                cliente.ES_CORPORACION, cliente.CLI_CORPORAC_ASOC, APLIC_ABIERTAS, USAR_DESC_CORP, USAR_PRECIOS_CORP, cliente.TIPO_IMPUESTO, cliente.TIPO_TARIFA,
                                decimal.Parse(cliente.PORC_TARIFA), cliente.TIPIFICACION_CLIENTE, cliente.AFECTACION_IVA, cliente.IMAGEN);
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error: ", ex.InnerException.InnerException);
            }
        }

        public string ValidarExistenciaDeCliente(string pEsquema, string pCliente)
        {
            ValidarExistenciaDeClienteTableAdapter sp = new ValidarExistenciaDeClienteTableAdapter();
            string cliente = null;

            var response = sp.GetData(pEsquema, pCliente).ToList();

            foreach (var item in response)
            {
                cliente = item.CLIENTE;
            }

            return cliente;
        }

        public string ObtenerPaisDeCliente(string cliente, string esquema)
        {
            ObtenerPaisDeClienteTableAdapter sp = new ObtenerPaisDeClienteTableAdapter();
            string pais = "";

            var response = sp.GetData(cliente, esquema);
            foreach (var item in response)
            {
                pais = item.NombrePais;
            }
            return pais;
        }

        public string ObtenerContribuyenteDeCliente(string cliente, string esquema)
        {
            ObtenerContribuyenteClienteTableAdapter sp = new ObtenerContribuyenteClienteTableAdapter();
            string contribuyente = "";

            var response = sp.GetData(esquema, cliente);
            foreach (var item in response)
            {
                contribuyente = item.CONTRIBUYENTE;
            }
            return contribuyente;
        }
    }
}

