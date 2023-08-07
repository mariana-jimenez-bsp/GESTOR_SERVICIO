using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Actividades
{
    public class mActividadesAsociadas
    {
        public string Id { get; set; } = string.Empty;

        public string consecutivo_informe { get; set; } = string.Empty;
        [StringLength(6, ErrorMessage = "Tamaño máximo de 6 caracteres")]
        [Required(ErrorMessage = "El nombre de la actividad es requerida")]
        public string codigo_actividad { get; set; } = string.Empty;
        [Range(0, int.MaxValue, ErrorMessage = "El valor de horas debe ser un número entero válido.")]
        public string horas_cobradas { get; set; } = string.Empty;
        [Range(0, int.MaxValue, ErrorMessage = "El valor de horas debe ser un número entero válido.")]
        public string horas_no_cobradas { get; set; } = string.Empty;
        public string nombre_actividad { get; set; } = string.Empty;

    }
}
