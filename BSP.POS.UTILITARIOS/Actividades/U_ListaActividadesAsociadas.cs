using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Actividades
{
    public class U_ListaActividadesAsociadas
    {
        public string Id { get; set; }
        public string consecutivo_informe { get; set; }
        public string codigo_actividad { get; set; }
        public string horas_cobradas { get; set; }
        public string horas_no_cobradas { get; set; }


        public U_ListaActividadesAsociadas(string pId, string pConsecutivo_informe, string pCodigo_actividad, string pHoras_cobradas, string pHoras_no_cobradas)
        { 
            Id = pId;
            consecutivo_informe = pConsecutivo_informe;
            codigo_actividad = pCodigo_actividad;
            horas_cobradas = pHoras_cobradas;
            horas_no_cobradas = pHoras_no_cobradas;
        }
        public U_ListaActividadesAsociadas() { }
    }
}
