using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Usuarios
{
    public class U_PermisosAsociados
    {
        public String Id { get; set; }

        public String id_permiso { get; set; }

        public U_PermisosAsociados(String pId, String pId_permiso)
        {
            Id = pId;
            id_permiso = pId_permiso;
        }

        public U_PermisosAsociados()
        {
        }
    }
}
