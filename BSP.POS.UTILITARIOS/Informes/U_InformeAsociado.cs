using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Informes
{
    public class U_InformeAsociado
    {
            public string consecutivo { get; set; }
            public string fecha_consultoria { get; set; }
             public string hora_inicio { get; set; }
            public string hora_final { get; set; }
            public string modalidad_consultoria { get; set; }
            public string cliente { get; set; }
            public string estado { get; set; }



            public U_InformeAsociado(string pConsecutivo, string pFecha_Consultoria, string pHora_Inicio, string pHora_Final, string pModalidad, string pCliente, string pEstado)
            {
                consecutivo = pConsecutivo;
            fecha_consultoria = pFecha_Consultoria;
            hora_inicio = pHora_Inicio;
            hora_final = pHora_Final;
            modalidad_consultoria = pModalidad;
            cliente = pCliente;
                estado = pEstado;
            }
            public U_InformeAsociado() { }

        }
}
