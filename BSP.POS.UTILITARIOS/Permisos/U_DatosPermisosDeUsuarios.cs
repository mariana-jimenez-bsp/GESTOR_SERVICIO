using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Permisos
{
    public class U_DatosPermisosDeUsuarios
    {
        public string Id { get; set; }
        public string id_permiso { get; set; }
        public string permiso { get; set; }

        public U_DatosPermisosDeUsuarios(string pId, string pIdPermiso, string pPermiso) 
        { 
            Id = pId;
            id_permiso = pIdPermiso;
            permiso = pPermiso;
        }

        public U_DatosPermisosDeUsuarios() { }
    }
}
