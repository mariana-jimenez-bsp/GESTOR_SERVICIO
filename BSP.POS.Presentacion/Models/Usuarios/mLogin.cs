using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Usuarios
{
    public class mLogin
    {
        public string token { get; set; } = string.Empty;
        [StringLength(10, ErrorMessage = "Tamaño máximo de 10 caracteres")]
        [Required(ErrorMessage = "El Esquema es requerido")]
        public string esquema { get; set; } = string.Empty;
        [StringLength(25, ErrorMessage = "Tamaño máximo de 25 caracteres")]
        [Required(ErrorMessage = "El Usuario es requerido")]
        public string usuario { get; set; } = string.Empty;
        [StringLength(100, ErrorMessage = "Tamaño máximo de 100 caracteres")]
        [Required(ErrorMessage = "La clave es requerida")]
        public string clave { get; set; } = string.Empty;
    }
}
