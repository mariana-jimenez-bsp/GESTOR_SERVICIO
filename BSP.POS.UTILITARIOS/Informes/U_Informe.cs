using BSP.POS.UTILITARIOS.Actividades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Informes
{
    public class U_Informe
    {
        public string consecutivo { get; set; }
        public string fecha_consultoria { get; set; }
        public string hora_inicio { get; set; }
        public string hora_final { get; set; }
        public string modalidad_consultoria { get; set; }
        public string numero_proyecto { get; set; }
        public string estado { get; set; }
        public List<U_ListaActividadesAsociadas> listaActividadesAsociadas { get; set; } = new List<U_ListaActividadesAsociadas>();


        public U_Informe(string pConsecutivo, string pFecha_Consultoria, string pHora_Inicio, string pHora_Final, string pModalidad, string pNumero, string pEstado)
        {
            consecutivo = pConsecutivo;
            fecha_consultoria = pFecha_Consultoria;
            hora_inicio = pHora_Inicio;
            hora_final = pHora_Final;
            modalidad_consultoria = pModalidad;
            numero_proyecto = pNumero;
            estado = pEstado;
        }
        public U_Informe() { }

    }
}
