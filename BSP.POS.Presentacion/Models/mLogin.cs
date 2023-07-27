using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models
{
    public class mLogin
    {
        public String token { get; set; } = string.Empty;
        [StringLength(10, ErrorMessage = "Tamaño máximo de 10 caracteres")]
        [Required(ErrorMessage = "El Esquema es requerido")]
        public String esquema { get; set; } = string.Empty;
        [StringLength(25, ErrorMessage = "Tamaño máximo de 25 caracteres")]
        [Required(ErrorMessage = "El Usuario es requerido")]
        public String usuario { get; set; } = string.Empty;
        [StringLength(100, ErrorMessage = "Tamaño máximo de 100 caracteres")]
        [Required(ErrorMessage = "La clave es requerida")]
        public String clave { get; set; } = string.Empty;
    }
}
