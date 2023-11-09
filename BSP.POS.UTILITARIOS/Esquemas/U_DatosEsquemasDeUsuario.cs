using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Esquemas
{
    public class U_DatosEsquemasDeUsuario
    {
        public string Id { get; set; }
        public string id_esquema { get; set; }
        public string esquema { get; set; }

        public U_DatosEsquemasDeUsuario(string pId, string pIdEsquema, string pEsquema)
        {
            Id = pId;
            id_esquema = pIdEsquema;
            esquema = pEsquema;
        }

        public U_DatosEsquemasDeUsuario() { }
    }
}
