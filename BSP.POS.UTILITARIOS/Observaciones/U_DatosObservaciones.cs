using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Observaciones
{
    public class U_DatosObservaciones
    {
        public string Id { get; set; } = string.Empty;
        public string consecutivo_informe { get; set; } = string.Empty;
        public string codigo_usuario { get; set; } = string.Empty;
        public string observacion { get; set; } = string.Empty;
        public string nombre_usuario { get; set; } = string.Empty;

        public U_DatosObservaciones(string pId, string pConsecutivo, string pCodigo, string pObservacion, string pNombreUsuario)
        {
            Id = pId;
            consecutivo_informe = pConsecutivo;
            codigo_usuario = pCodigo;
            observacion = pObservacion;
            nombre_usuario = pNombreUsuario;
        }
        public U_DatosObservaciones() { }
    }
}
