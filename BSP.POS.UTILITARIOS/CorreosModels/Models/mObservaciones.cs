using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.CorreosModels.Models
{
    public class mObservaciones
    {
        public string Id { get; set; } = string.Empty;
        public string consecutivo_informe { get; set; } = string.Empty;
        public string usuario { get; set; } = string.Empty;
        public string observacion { get; set; } = string.Empty;
    }
}
