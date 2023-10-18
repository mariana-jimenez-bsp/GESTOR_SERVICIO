using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Licencias
{
    public class U_LicenciaByte
    {
        public string texto_archivo { get; set; } = string.Empty;
        public string producto { get; set; } = string.Empty;
        public byte[] codigo_licencia { get; set; } = new byte[] { 0x00 };
    }
}
