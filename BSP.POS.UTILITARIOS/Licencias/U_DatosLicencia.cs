using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Licencias
{
    public class U_DatosLicencia
    {
        public DateTime FechaInicio { get; set; } = DateTime.Now;
        public DateTime FechaFin { get; set; } = DateTime.Now;
        public DateTime FechaAviso { get; set; } = DateTime.Now;
        public int CantidadUsuarios { get; set; } = 0;
        public bool MacAddressIguales { get; set; } = false;

    }
}
