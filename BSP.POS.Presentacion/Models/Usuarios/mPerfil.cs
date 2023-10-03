using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Usuarios
{
    public class mPerfil
    {
        public string id { get; set; } = string.Empty;
        public string codigo { get; set; } = string.Empty;
        public string cod_cliente { get; set; } = string.Empty;
        [StringLength(25, ErrorMessage = "Tamaño máximo de 25 caracteres")]
        [Required(ErrorMessage = "El Usuario es requerido")]
        public string usuario { get; set; } = string.Empty;
        [StringLength(249, ErrorMessage = "Tamaño máximo de 249 caracteres")]
        [EmailAddress(ErrorMessage = "Ingresa una dirección de correo electrónico válida")]
        [Required(ErrorMessage = "El Correo es requerido")]
        public string correo { get; set; } = string.Empty;
        [StringLength(100, MinimumLength = 8, ErrorMessage = "El tamaño debe ser entre 8 a 100 caracteres")]
        [RegularExpression(@"^(?=.*\d)(?=.*[!@#$%^&*()_+\-=?.])(?=.*[A-Z]).+$", ErrorMessage = "La contraseña debe contener al menos un dígito, al menos un carácter de la lista !@#$%^&*()_+-=?. y al menos una letra mayúscula.")]
        public string? clave { get; set; }
        [StringLength(100, ErrorMessage = "Tamaño máximo de 100 caracteres")]
        [Required(ErrorMessage = "El Nombre de la empresa es requerido")]
        public string nombre { get; set; } = string.Empty;
        [Required(ErrorMessage = "El rol es requerido")]
        public string rol { get; set; } = string.Empty;
        [Required(ErrorMessage = "El teléfono es requerido")]
        [StringLength(50, ErrorMessage = "Tamaño máximo de 50 caracteres")]
        public string telefono { get; set; } = string.Empty;
        [Required(ErrorMessage = "El esquema es requerido")]
        public string esquema { get; set; } = string.Empty;

    }
}
