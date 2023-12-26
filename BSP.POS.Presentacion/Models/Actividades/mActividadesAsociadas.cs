using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Actividades
{
    public class mActividadesAsociadas
    {
        public string Id { get; set; } = string.Empty;

        public string consecutivo_informe { get; set; } = string.Empty;
        public string codigo_actividad { get; set; } = string.Empty;
        [Range(0, int.MaxValue, ErrorMessage = "El valor de horas debe ser un número entero válido.")]
        [Required(ErrorMessage = "Las horas cobradas son requeridas")]
        public string horas_cobradas { get; set; } = string.Empty;
        [Required(ErrorMessage = "Las horas no cobradas son requeridas")]
        [Range(0, int.MaxValue, ErrorMessage = "El valor de horas debe ser un número entero válido.")]
        public string horas_no_cobradas { get; set; } = string.Empty;
        [StringLength(200, ErrorMessage = "Tamaño máximo de 200 caracteres")]
        [Required(ErrorMessage = "El nombre de la actividad es requerida")]
        public string nombre_actividad { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de Fecha es requerido")]
        [DataType(DataType.Date, ErrorMessage = "El campo tiene que ser una fecha válida")]
        public string fecha { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de Fecha es requerido")]
        [DataType(DataType.Date, ErrorMessage = "El campo tiene que ser una fecha válida")]
        public DateTime FechaDateTime
        {
            get => DateTime.Parse(fecha);
            set => fecha = value.ToString("yyyy-MM-dd");
        }

    }
}
