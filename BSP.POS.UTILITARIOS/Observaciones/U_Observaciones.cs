using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Observaciones
{
    public class U_Observaciones
    {
        public string Id { get; set; }
        public string consecutivo_informe { get; set; }
        public string codigo_usuario { get; set; }
        public string observacion { get; set; }

        public U_Observaciones(string pId, string pConsecutivo, string pCodigo_usuario, string pObservacion) {
            Id = pId;
            consecutivo_informe = pConsecutivo;
            codigo_usuario = pCodigo_usuario;
            observacion = pObservacion;
        }

        public U_Observaciones() { }
    }
}
