using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Licencias
{
    public class U_CodigoLicenciaYProducto
    {
        public byte[] codigo_licencia { get; set; } = new byte[] { 0x00 };
        public string producto { get; set; } = string.Empty;
    }
}
