using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Permisos
{
    public class U_SubPermisos
    {
        public string Id { get; set; }

        public string sub_permiso { get; set; }

        public U_SubPermisos(string pId, string pSub_permiso)
        {
            Id = pId;
            sub_permiso = pSub_permiso;
        }

        public U_SubPermisos()
        {
        }
    }
}
