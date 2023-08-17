using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Licencias
{
    public class U_Licencia
    {
        public string Id { get; set; }
        public string esquema { get; set; }
        public string estado { get; set; }
        public string fecha_vencimiento { get; set; }

        public U_Licencia(string pId, string pEsquema, string pEstado, string pFecha_Vencimiento) { 
           Id = pId;
            estado = pEstado;
            esquema = pEsquema;
            fecha_vencimiento = pFecha_Vencimiento;
        
        }

        public U_Licencia() { }
    }
}
