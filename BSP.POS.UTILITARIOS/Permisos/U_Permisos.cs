using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Permisos
{
    public class U_Permisos
    {
        public string Id { get; set; }

        public string permiso { get; set; }

        public U_Permisos(string pId, string pPermiso)
        {
            Id = pId;
            permiso = pPermiso;
        }

        public U_Permisos()
        {
        }

    }
}
