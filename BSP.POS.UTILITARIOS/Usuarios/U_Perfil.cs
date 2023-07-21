using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Usuarios
{
    public class U_Perfil
    {
        public String id { get; set; }

        public String usuario { get; set; }
        public String correo { get; set; }

        public String clave { get; set; }
        public String nombre { get; set; }
        public String rol { get; set; }

        public U_Perfil(String pId, String pUsuario, String pCorreo, String pClave, String pNombre, String pRol)
        {
            id = pId;
            usuario = pUsuario;
            correo = pCorreo;
            clave = pClave;
            nombre = pNombre;
            rol = pRol;
        }
        public U_Perfil()
        {
        }
    }
}
