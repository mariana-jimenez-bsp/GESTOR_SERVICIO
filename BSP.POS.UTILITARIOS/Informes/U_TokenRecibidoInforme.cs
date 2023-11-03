using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Informes
{
    public class U_TokenRecibidoInforme
    {
        public String token_recibido { get; set; }
        public String esquema { get; set; }
        public String codigo { get; set; }
        public String fecha_expiracion { get; set; }

        public U_TokenRecibidoInforme(String pTokenRecibido, String pEsquema, String pCodigo, String pFecha_expiracion)
        {
            token_recibido = pTokenRecibido;
            esquema = pEsquema;
            codigo = pCodigo;
            fecha_expiracion = pFecha_expiracion;
        }
        public U_TokenRecibidoInforme()
        {
        }
    }
}
