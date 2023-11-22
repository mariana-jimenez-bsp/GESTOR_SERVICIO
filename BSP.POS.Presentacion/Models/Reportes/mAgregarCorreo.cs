using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Reportes
{
    public class mAgregarCorreo
    {
        [StringLength(249, ErrorMessage = "Tamaño máximo de 249 caracteres")]
        [EmailAddress(ErrorMessage = "Ingresa una dirección de correo electrónico válida")]
        [Required(ErrorMessage = "El Correo es requerido")]
        public string correo { get; set; } = string.Empty;
    }
}
