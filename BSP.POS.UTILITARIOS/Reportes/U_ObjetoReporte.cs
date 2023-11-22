using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Reportes
{
    public class U_ObjetoReporte
    {
        public byte[] reporte { get; set; } = new byte[] { 0x00 };
        public List<string> listaCorreosExtras { get; set; } = new List<string>();
    }
}
