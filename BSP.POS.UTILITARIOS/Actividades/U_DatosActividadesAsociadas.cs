using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Actividades
{
    public class U_DatosActividadesAsociadas
    {
        public string Id { get; set; }
        public string consecutivo_informe { get; set; }
        public string codigo_actividad { get; set; }
        public string horas_cobradas { get; set; }
        public string horas_no_cobradas { get; set; }
        public string nombre_actividad { get; set; }

        public U_DatosActividadesAsociadas(string pId, string pConsecutivo, string pCodigo, string pHorasCobradas, string pHoraNoCobradas, string pNombreActividad)
        {
            Id = pId;
            consecutivo_informe = pConsecutivo;
            codigo_actividad = pCodigo;
            horas_cobradas = pHorasCobradas;
            horas_no_cobradas = pHoraNoCobradas;
            nombre_actividad = pNombreActividad;
        }
        public U_DatosActividadesAsociadas() { }
    }
}
