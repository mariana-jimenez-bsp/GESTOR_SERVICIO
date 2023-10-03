using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Usuarios
{
    public class mUsuarioNuevaClave
    {
        public string token_recuperacion { get; set; } = string.Empty;
        public string esquema { get; set; } = string.Empty;
        [Required(ErrorMessage = "La clave es requerida")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "El tamaño debe ser entre 8 a 100 caracteres")]
        [RegularExpression(@"^(?=.*\d)(?=.*[!@#$%^&*()_+\-=?.])(?=.*[A-Z]).+$", ErrorMessage = "La contraseña debe contener al menos un dígito, al menos un carácter de la lista !@#$%^&*()_+-=?. y al menos una letra mayúscula.")]
        public string clave { get; set; } = string.Empty;
        [Compare("clave", ErrorMessage = "Las contraseñas no coinciden.")]
        public string confirmarClave { get; set; } = string.Empty;
    }
}
