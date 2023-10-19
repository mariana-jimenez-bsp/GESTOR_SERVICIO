using BSP.POS.DATOS.CodigoTelefonoPais;
using BSP.POS.UTILITARIOS.CodigoTelefonoPais;
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

        public string ObtenerDatosCodigoTelefonoPaisDeClientes(string esquema){
            List<U_CodigoTelefonoPaisClientes> listaDatos = new List<U_CodigoTelefonoPaisClientes>();
            listaDatos = _datosTelefono.ObtenerDatosCodigoTelefonoPaisDeClientes(esquema);
            string listaDatosJson = JsonSerializer.Serialize(listaDatos);
            return listaDatosJson;
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
    }
}
