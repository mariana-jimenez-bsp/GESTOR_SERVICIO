using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Permisos
{
    public class U_DatosSubPermisosDeUsuario
    {
        public string Id { get; set; }
        public string id_permiso_usuario { get; set; }
        public string id_sub_permiso { get; set; }
        public string sub_permiso { get; set; }

        public U_DatosSubPermisosDeUsuario(string pId, string pIdPermisoUsuario, string pIdSubPermiso, string pSubPermiso) 
        { 
            Id = pId;
            id_permiso_usuario = pIdPermisoUsuario;
            id_sub_permiso = pIdSubPermiso;
            sub_permiso = pSubPermiso;
        }
    }
}
