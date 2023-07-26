using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Permisos
{
    public class U_PermisosAsociados
    {
        public string Id { get; set; }

        public string id_permiso { get; set; }

        public U_PermisosAsociados(string pId, string pId_permiso)
        {
            Id = pId;
            id_permiso = pId_permiso;
        }

        public U_PermisosAsociados()
        {
        }
    }
}
