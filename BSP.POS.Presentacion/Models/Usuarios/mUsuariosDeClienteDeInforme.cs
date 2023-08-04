using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Usuarios
{
    public class mUsuariosDeClienteDeInforme
    {
        public string id { get; set; } = string.Empty;
        public string consecutivo_informe { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debe escojer un usuario")]
        public string codigo_usuario_cliente { get; set; } = string.Empty;
        public string aceptacion { get; set; } = string.Empty;

        public string nombre_usuario { get; set;} = string.Empty;
        public string departamento_usuario { get; set; } = string.Empty;
    }
}
