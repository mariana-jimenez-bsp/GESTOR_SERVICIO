using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Esquemas
{
    public class U_Esquemas
    {
        public string Id { get; set; }

        public string esquema { get; set; }

        public U_Esquemas(string pId, string pEsquema)
        {
            Id = pId;
            esquema = pEsquema;
        }

        public U_Esquemas()
        {
        }
    }
}
