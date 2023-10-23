
using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Actividades
{
    public class mActividades
    {
        public string Id { get; set; } = string.Empty;
        public string codigo { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del usuario es requerido")]
        public string codigo_usuario { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del nombre es requerido")]
        [StringLength(200, ErrorMessage = "Tamaño máximo de 200 caracteres")]
        public string Actividad { get; set; } = string.Empty;
        [StringLength(200, ErrorMessage = "Tamaño máximo de 200 caracteres")]
        [Required(ErrorMessage = "El campo del CI-Referencia es requerido")]
        public string CI_referencia { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de Horas es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El valor de horas debe ser un número entero válido.")]
        public string horas { get; set; } = string.Empty;
        public string fecha_actualizacion { get; set; } = string.Empty;
        public DateTime FechaActualizacionDateTime { get; set; } = DateTime.MinValue;
    }
}
