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
        public string codigo { get; set; }
        public string esquema { get; set; }
        public string estado { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_vencimiento { get; set; }

        public U_Licencia(string pId, string pCodigo, string pEsquema, string pEstado, string pFecha_Inicio, string pFecha_Vencimiento) { 
           Id = pId;
            estado = pEstado;
            esquema = pEsquema;
            fecha_inicio = pFecha_Inicio;
            fecha_vencimiento = pFecha_Vencimiento;
            codigo = pCodigo;
        
        }

        public U_Licencia() { }
    }
}
