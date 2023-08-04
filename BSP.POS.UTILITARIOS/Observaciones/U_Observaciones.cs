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
        public string usuario { get; set; }
        public string observacion { get; set; }

        public U_Observaciones(string pId, string pConsecutivo, string pUsuario, string pObservacion) {
            Id = pId;
            consecutivo_informe = pConsecutivo;
            usuario = pUsuario;
            observacion = pObservacion;
        }

        public U_Observaciones() { }
    }
}
