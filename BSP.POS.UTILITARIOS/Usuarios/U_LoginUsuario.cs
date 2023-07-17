using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Usuarios
{
    public class U_LoginUsuario
    {
        public String usuario { get; set; }

        public String pin { get; set; }
        public string rol { get; set; }
        public U_LoginUsuario(String pUsuario, String pPin, string pRol)
        {
            usuario = pUsuario;
            pin = pPin;
            rol = pRol;
        }
        public U_LoginUsuario()
        {
        }
    }
}
