using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Usuarios
{
    public class U_TokenRecuperacion
    {
        public String token_recuperacion { get; set; }
        public String esquema { get; set; }
        public String correo { get; set; }
        public String fecha_expiracion { get; set; }

        public U_TokenRecuperacion(String pToken_recuperacion, String pEsquema, String pCorreo, String pFecha_expiracion)
        {
            token_recuperacion = pToken_recuperacion;
            esquema = pEsquema;
            correo = pCorreo;
            fecha_expiracion = pFecha_expiracion;
        }
        public U_TokenRecuperacion()
        {
        }

    }
}
