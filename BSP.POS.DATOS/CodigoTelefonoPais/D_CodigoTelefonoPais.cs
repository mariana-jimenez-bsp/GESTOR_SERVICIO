using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSP.POS.DATOS.POSDataSetTableAdapters;
using BSP.POS.UTILITARIOS.CodigoTelefonoPais;

namespace BSP.POS.DATOS.CodigoTelefonoPais
{
    public class D_CodigoTelefonoPais
    {
        public List<U_CodigoTelefonoPaisClientes> ObtenerDatosCodigoTelefonoPaisDeClientes(string esquema)
        {
            List<U_CodigoTelefonoPaisClientes> ListaDatos = new List<U_CodigoTelefonoPaisClientes>();
            ObtenerDatosCodigoTelefonoPaisDeClientesTableAdapter sp = new ObtenerDatosCodigoTelefonoPaisDeClientesTableAdapter();
            var response = sp.GetData(esquema);

            foreach (var item in response)
            {
                U_CodigoTelefonoPaisClientes datos = new U_CodigoTelefonoPaisClientes();
                datos.Cliente = item.Cliente;
                datos.CodigoTelefono = item.CodigoTelefono;
                datos.IdCodigoTelefono = item.IdCodigoTelefono;
                datos.NombrePais = item.NombrePais;
                datos.Id = item.Id;
                datos.IdCodigoTelefono = item.IdCodigoTelefono;
                ListaDatos.Add(datos);
            }

            return ListaDatos;
        }

        public int ObtenerIdCodigoTelefonoPais(string esquema, string pais)
        {
            ObtenerIdCodigoTelefonoPaisTableAdapter sp = new ObtenerIdCodigoTelefonoPaisTableAdapter();
            int Id = 0;
            var response = sp.GetData(esquema, pais).ToList();
            foreach (var item in response)
            {
                Id = item.Id;
            }
            return Id;
        }

        public void AgregarCodigoTelefonoPaisCliente(string cliente, int IdCodigoTelefono, string esquema)
        {
            try
            {
                AgregarCodigoTelefoPaisClienteTableAdapter sp = new AgregarCodigoTelefoPaisClienteTableAdapter();
                var response = sp.GetData(cliente, IdCodigoTelefono, esquema).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
