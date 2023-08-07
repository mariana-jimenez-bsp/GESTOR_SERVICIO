using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.CorreosModels.Models
{
    public class mInformesAsociado
    {
        public string consecutivo { get; set; } = string.Empty;
        public string fecha_consultoria { get; set; } = string.Empty;
        public string hora_inicio { get; set; } = string.Empty;
        public string hora_final { get; set; } = string.Empty;
        public string modalidad_consultoria { get; set; } = string.Empty;
        public string cliente { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;
    }
}
