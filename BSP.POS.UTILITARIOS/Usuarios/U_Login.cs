using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Usuarios
{
    public class U_Login
    {
        public String usuario { get; set; }

        public String contrasena { get; set; }
        public String esquema { get; set; }

        public String accion { get; set; }
        public String key { get; set; }
        public String correo { get; set; }

        public U_Login(String pUsuario, String pContrasena, String pEsquema, String pAccion, String pKey, String pCorreo)
        {
            usuario = pUsuario;
            contrasena = pContrasena;
            esquema = pEsquema;
            accion = pAccion;
            key = pKey;
            correo = pCorreo;
        }
        public U_Login()
        {
        }
    }
}
