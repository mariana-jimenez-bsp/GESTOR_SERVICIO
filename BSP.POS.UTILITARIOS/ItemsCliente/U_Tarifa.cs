using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.ItemsCliente
{
    public class U_Tarifa
    {
        public string tipoTarifa { get; set; }
        public string descripcion { get; set; }
        public decimal porcentaje { get; set; }

        public U_Tarifa(string pTipoTarifa, string pDescripcion, decimal pPorcentaje) { 
            tipoTarifa = pTipoTarifa;
            descripcion = pDescripcion;
            porcentaje = pPorcentaje;
        }

        public U_Tarifa() { }
    }
}
