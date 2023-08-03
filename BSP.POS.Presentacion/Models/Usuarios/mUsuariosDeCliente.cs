using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Usuarios
{
    public class mUsuariosDeCliente
    {
        public string id { get; set; } = string.Empty;

        public string codigo { get; set; } = string.Empty;
        public string cod_cliente { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debe escojer un usuario")]
        public string usuario { get; set; } = string.Empty;
        public string departamento { get; set; } = string.Empty;
        public string correo { get; set; } = string.Empty;
        public string telefono { get; set; } = string.Empty;
    }
}
