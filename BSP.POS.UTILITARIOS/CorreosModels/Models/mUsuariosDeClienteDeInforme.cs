using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.CorreosModels.Models
{
    public class mUsuariosDeClienteDeInforme
    {
        public string id { get; set; } = string.Empty;
        public string consecutivo_informe { get; set; } = string.Empty;
        public string codigo_usuario_cliente { get; set; } = string.Empty;
        public string aceptacion { get; set; } = string.Empty;

        public string nombre_usuario { get; set; } = string.Empty;
        public string departamento_usuario { get; set; } = string.Empty;
        public string correo_usuario { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
    }
}
