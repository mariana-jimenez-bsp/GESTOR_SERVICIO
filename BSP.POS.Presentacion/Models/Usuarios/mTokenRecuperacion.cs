using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Usuarios
{
    public class mTokenRecuperacion
    {
        public string token_recuperacion { get; set; } = string.Empty;
        [StringLength(10, ErrorMessage = "Tamaño máximo de 10 caracteres")]
        [Required(ErrorMessage = "El Esquema es requerido")]
        public string esquema { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "Ingresa una dirección de correo electrónico válida")]
        [StringLength(249, ErrorMessage = "Tamaño máximo de 249 caracteres")]
        [Required(ErrorMessage = "El correo es requerido")]
        public string correo { get; set; } = string.Empty;
        public string fecha_expiracion { get; set; } = string.Empty;
    }
}
