using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.CorreosModels.Models
{
    public class mLasActividadesAsociadas
    {
        public string Id { get; set; } = string.Empty;
        public string consecutivo_informe { get; set; } = string.Empty;
        public string codigo_actividad { get; set; } = string.Empty;
        public string horas_cobradas { get; set; } = string.Empty;
        public string horas_no_cobradas { get; set; } = string.Empty;
        public string nombre_actividad { get; set; } = string.Empty;
    }
}
