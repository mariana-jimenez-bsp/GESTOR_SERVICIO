using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Lugares
{
    public class U_Lugares
    {
        public string lugar { get; set; }
        public string descripcion { get; set; }

        public U_Lugares(string pLugar, string pDescripcion)
        {
            lugar = pLugar;
            descripcion = pDescripcion;
        }

        public U_Lugares() { }
    }
}
