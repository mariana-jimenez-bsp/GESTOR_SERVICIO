using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Proyectos
{
    public class mProyectos : IValidatableObject
    {
        public string Id { get; set; } = string.Empty;

        public string numero { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del Nombre del Cliente es requerido")]
        public string codigo_cliente { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de Fecha Inicial es requerido")]
        public string fecha_inicial { get; set; } = DateTime.Now.ToString();
        [Required(ErrorMessage = "El campo de Fecha Final es requerido")]
        [DataType(DataType.Date, ErrorMessage = "El campo tiene que ser una fecha válida")]
        public string fecha_final { get; set; } = DateTime.Now.ToString();
        [Required(ErrorMessage = "El campo de Horas Totales es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de horas debe ser un número entero válido.")]
        public string horas_totales { get; set; } = string.Empty;
        [StringLength(200, ErrorMessage = "Tamaño máximo de 200 caracteres")]
        [Required(ErrorMessage = "El campo del Centro de costo es requerido")]
        public string centro_costo { get; set; } = string.Empty;
        [StringLength(200, ErrorMessage = "Tamaño máximo de 200 caracteres")]
        [Required(ErrorMessage = "El campo del nombre de proyecto es requerido")]
        public string nombre_proyecto { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de Fecha Inicial es requerido")]
        [DataType(DataType.Date, ErrorMessage = "El campo tiene que ser una fecha válida")]
        public DateTime FechaInicialDateTime
        {
            get => DateTime.Parse(fecha_inicial);
            set => fecha_inicial = value.ToString("yyyy-MM-dd");
        }
        [Required(ErrorMessage = "El campo de Fecha Final es requerido")]
        [DataType(DataType.Date, ErrorMessage = "El campo tiene que ser una fecha válida")]
        
        public DateTime FechaFinalDateTime
        {
            get => DateTime.Parse(fecha_final);
            set => fecha_final = value.ToString("yyyy-MM-dd");
        }
        
        public string nombre_consultor { get; set; } = string.Empty;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaInicialDateTime > FechaFinalDateTime)
            {
                yield return new ValidationResult("La Fecha de Inicio no puede ser mayor que la Fecha Final.", new[] { nameof(FechaInicialDateTime) });
            }
            else if (FechaFinalDateTime < FechaInicialDateTime)
            {
                yield return new ValidationResult("La Fecha Final no puede ser menor que la Fecha de Inicio.", new[] { nameof(FechaFinalDateTime) });
            }
        }
    }
}
