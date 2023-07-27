using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Actividades
{
    public class U_ListaActividades
    {
        public string Id { get; set; }
        public string codigo { get; set; }
        public string Actividad { get; set; }
        public string CI_referencia { get; set; }
        public string horas { get; set; }


        public U_ListaActividades(string pId, string pCodigo, string pActividad, string pCI_referencia, string pHoras)
        {
            Id = pId;
            codigo = pCodigo;
            Actividad = pActividad;
            CI_referencia = pCI_referencia;
            horas = pHoras;
        }
        public U_ListaActividades() { }
    }
}
