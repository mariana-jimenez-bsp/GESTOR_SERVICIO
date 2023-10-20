using BSP.POS.DATOS.CodigoTelefonoPais;
using BSP.POS.UTILITARIOS.CodigoTelefonoPais;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.CodigoTelefonoPais
{
    public class N_CodigoTelefonoPais
    {
        D_CodigoTelefonoPais _datosTelefono = new D_CodigoTelefonoPais();


        public string ObtenerDatosCodigoTelefonoPais(string esquema)
        {
            List<U_CodigoTelefonoPais> listaDatos = new List<U_CodigoTelefonoPais>();
            listaDatos = _datosTelefono.ObtenerDatosCodigoTelefonoPais(esquema);
            string listaDatosJson = JsonConvert.SerializeObject(listaDatos);
            return listaDatosJson;
        }
        public string ObtenerDatosCodigoTelefonoPaisDeClientes(string esquema){
            List<U_CodigoTelefonoPaisClientes> listaDatos = new List<U_CodigoTelefonoPaisClientes>();
            listaDatos = _datosTelefono.ObtenerDatosCodigoTelefonoPaisDeClientes(esquema);
            string listaDatosJson = JsonConvert.SerializeObject(listaDatos);
            return listaDatosJson;
        }

        public string ObtenerDatosCodigoTelefonoPaisDeUsuariosPorUsuario(string esquema, string codigoUsuario)
        {
            U_CodigoTelefonoPaisUsuarios datos = new U_CodigoTelefonoPaisUsuarios();
            datos = _datosTelefono.ObtenerDatosCodigoTelefonoPaisDeUsuariosPorUsuario(esquema, codigoUsuario);
            string datosJson = JsonConvert.SerializeObject(datos);
            return datosJson;
        }

        public int ObtenerIdCodigoTelefonoPais(string esquema, string pais)
        {
            int Id = _datosTelefono.ObtenerIdCodigoTelefonoPais(esquema, pais);
            return Id;
            
        }

        public void AgregarCodigoTelefonoPaisCliente(string cliente, int idCodigoTelefono, string esquema)
        {
            _datosTelefono.AgregarCodigoTelefonoPaisCliente(cliente, idCodigoTelefono, esquema);

        }

        public void AgregarCodigoTelefonoPaisUsuario(string codigoUsuario, int idCodigoTelefono, string esquema)
        {
            _datosTelefono.AgregarCodigoTelefonoPaisUsuario(codigoUsuario, idCodigoTelefono, esquema);

        }

        public void ActualizarCodigoTelefonoPaisUsuario(string codigoUsuario, int idCodigoTelefono, string esquema)
        {
            _datosTelefono.ActualizarCodigoTelefonoPaisUsuario(codigoUsuario, idCodigoTelefono, esquema);

        }
    }
}
