using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Usuarios
{
    public class mUsuarioNuevaClave
    {
        public string token_recuperacion { get; set; } = string.Empty;
        public string esquema { get; set; } = string.Empty;
        [Required(ErrorMessage = "La clave es requerida")]
        [StringLength(100, ErrorMessage = "Tamaño máximo de 100 caracteres")]
        public string clave { get; set; } = string.Empty;
        [Compare("clave", ErrorMessage = "Las contraseñas no coinciden.")]
        [StringLength(100, ErrorMessage = "Tamaño máximo de 100 caracteres")]
        public string confirmarClave { get; set; } = string.Empty;
    }
}
