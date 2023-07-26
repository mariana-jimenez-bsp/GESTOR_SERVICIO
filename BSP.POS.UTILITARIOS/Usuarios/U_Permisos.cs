using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Usuarios
{
    public class U_Permisos
    {
        public String Id { get; set; }

        public String permiso { get; set; }

        public U_Permisos(String pId, String pPermiso)
        {
            Id = pId;
            permiso = pPermiso;
        }

        public U_Permisos()
        {
        }

    }
}
