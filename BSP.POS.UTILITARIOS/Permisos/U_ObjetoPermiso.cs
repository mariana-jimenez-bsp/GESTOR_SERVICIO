using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Permisos
{
    public class U_ObjetoPermiso
    {
        public string permiso { get; set; } = string.Empty;
        public List<string> subpermisos { get; set; } = new List<string>();
    }
}
