using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.ItemsCliente
{
    public class U_ItemsCliente
    {
        public string valor { get; set; }
        public string descripcion { get; set; }

        public U_ItemsCliente(string pValor, string pDescripcion)
        {
            valor = pValor;
            descripcion = pDescripcion;
        }

        public U_ItemsCliente() { }
    }
}
