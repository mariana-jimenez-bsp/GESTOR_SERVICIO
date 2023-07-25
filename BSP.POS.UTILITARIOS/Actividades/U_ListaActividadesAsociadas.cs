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
        public string consecutivo { get; set; }
        public string Actividad { get; set; }
        public string horas_cobradas { get; set; }
        public string horas_no_cobradas { get; set; }


        public U_ListaActividadesAsociadas(string pId, string pConsecutivo, string pActividad, string pHoras_cobradas, string pHoras_no_cobradas)
        { 
            Id = pId;
            consecutivo = pConsecutivo;
            Actividad = pActividad;
            horas_cobradas = pHoras_cobradas;
            horas_no_cobradas = pHoras_no_cobradas;
        }
        public U_ListaActividadesAsociadas() { }
    }
}
