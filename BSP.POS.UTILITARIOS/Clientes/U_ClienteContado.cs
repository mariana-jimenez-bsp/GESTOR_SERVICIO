using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Clientes
{
    public class U_ClienteContado
    {
        public string cliente { get; set; }
        public string descripcion { get; set; }

        public U_ClienteContado(string pCliente, string pDescripcion) { 
            cliente = pCliente;
            descripcion = pDescripcion;
        }

        public U_ClienteContado() { }
    }
}
