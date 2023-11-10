using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Informes
{
    public class U_InformesDeProyecto
    {
        public string consecutivo { get; set; }
        public string fecha_actualizacion { get; set; }
        public string fecha_consultoria { get; set; }
        public string numero_proyecto { get; set; }
        public string estado { get; set; }



        public U_InformesDeProyecto(string pConsecutivo, string pFecha_actualizacion, string pFecha_consultoria, string pNumero, string pEstado)
        {
            consecutivo = pConsecutivo;
            fecha_actualizacion = pFecha_actualizacion;
            fecha_consultoria = pFecha_consultoria;
            numero_proyecto = pNumero;
            estado = pEstado;
        }
        public U_InformesDeProyecto() { }
    }
}
