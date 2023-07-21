using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Tiempos
{
    public class U_ListaTiempos
    {
        public string Id { get; set; }
        public string nombre_servicio { get; set; }
        public string horas { get; set; }




        public U_ListaTiempos(string pId, string pNombre_servicio, string pHoras)
        {
            nombre_servicio = pNombre_servicio;
            horas = pHoras;
            Id = pId;

        }
        public U_ListaTiempos() { }
    }
}
