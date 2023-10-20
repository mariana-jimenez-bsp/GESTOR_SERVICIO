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

        public List<U_CodigoTelefonoPais> ObtenerDatosCodigoTelefonoPais(string esquema)
        {
            List<U_CodigoTelefonoPais> ListaDatos = new List<U_CodigoTelefonoPais>();
            ObtenerDatosCodigoTelefonoPaisTableAdapter sp = new ObtenerDatosCodigoTelefonoPaisTableAdapter();
            var response = sp.GetData(esquema);

            foreach (var item in response)
            {
                U_CodigoTelefonoPais datos = new U_CodigoTelefonoPais();
                datos.CodigoTelefono = item.CodigoTelefono;
                datos.NombrePais = item.NombrePais;
                datos.Id = item.Id;
                datos.Pais = item.Pais;
                ListaDatos.Add(datos);
            }

            return ListaDatos;
        }
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
                datos.Pais = item.Pais;
                ListaDatos.Add(datos);
            }

            return ListaDatos;
        }

        public U_CodigoTelefonoPaisUsuarios ObtenerDatosCodigoTelefonoPaisDeUsuariosPorUsuario(string esquema, string codigoUsuario)
        {
            U_CodigoTelefonoPaisUsuarios datos = new U_CodigoTelefonoPaisUsuarios();
            ObtenerDatosCodigoTelefonoPaisPorUsuarioTableAdapter sp = new ObtenerDatosCodigoTelefonoPaisPorUsuarioTableAdapter();
            var response = sp.GetData(esquema, codigoUsuario);

            foreach (var item in response)
            {
                datos.CodigoUsuario = item.CodigoUsuario;
                datos.CodigoTelefono = item.CodigoTelefono;
                datos.IdCodigoTelefono = item.IdCodigoTelefono;
                datos.NombrePais = item.NombrePais;
                datos.Id = item.Id;
                datos.Pais = item.Pais;
            }

            return datos;
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
                AgregarCodigoTelefonoPaisClienteTableAdapter sp = new AgregarCodigoTelefonoPaisClienteTableAdapter();
                var response = sp.GetData(cliente, IdCodigoTelefono, esquema).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void AgregarCodigoTelefonoPaisUsuario(string codigoUsuario, int IdCodigoTelefono, string esquema)
        {
            try
            {
                AgregarCodigoTelefonoPaisUsuarioTableAdapter sp = new AgregarCodigoTelefonoPaisUsuarioTableAdapter();
                var response = sp.GetData(codigoUsuario, IdCodigoTelefono, esquema).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void ActualizarCodigoTelefonoPaisUsuario(string codigoUsuario, int IdCodigoTelefono, string esquema)
        {
            try
            {
                ActualizarCodigoTelefonoPaisUsuarioTableAdapter sp = new ActualizarCodigoTelefonoPaisUsuarioTableAdapter();
                var response = sp.GetData(codigoUsuario, IdCodigoTelefono, esquema).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
