using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Informes
{
    public class U_TokenAprobacionInforme
    {
        public String token_aprobacion { get; set; }
        public String esquema { get; set; }
        public String codigo { get; set; }
        public String fecha_expiracion { get; set; }

        public U_TokenAprobacionInforme(String pToken_aprobacion, String pEsquema, String pCodigo, String pFecha_expiracion)
        {
            token_aprobacion = pToken_aprobacion;
            esquema = pEsquema;
            codigo = pCodigo;
            fecha_expiracion = pFecha_expiracion;
        }
        public U_TokenAprobacionInforme()
        {
        }
    }
}
