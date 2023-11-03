using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BSP.POS.Presentacion.Models.Usuarios
{
    public class mUsuariosDeClienteDeInforme
    {
        public string id { get; set; } = string.Empty;
        public string consecutivo_informe { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debe escojer un usuario")]
        public string codigo_usuario_cliente { get; set; } = string.Empty;
        public string recibido { get; set; } = string.Empty;

        public string nombre_usuario { get; set;} = string.Empty;
        public string departamento_usuario { get; set; } = string.Empty;

        public string fecha_consultoria { get; set; } = string.Empty;

        public string imagenSeleccionada { get; set; } = "eyelash-background";
        public string informeSeleccionado { get; set; } = string.Empty;
        public DateTime FechaConsultoriaDateTime { get; set; } = DateTime.MinValue;
        
    }
}
