using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Usuarios
{
    public class U_UsuarioNuevaClave
    {
        public String token_recuperacion { get; set; }
        public String esquema { get; set; }
        public String clave { get; set; }

        public U_UsuarioNuevaClave(String pToken_recuperacion, String pEsquema, String pClave)
        {
            token_recuperacion = pToken_recuperacion;
            esquema = pEsquema;
            clave = pClave;
        }
        public U_UsuarioNuevaClave()
        {
        }
    }
}
