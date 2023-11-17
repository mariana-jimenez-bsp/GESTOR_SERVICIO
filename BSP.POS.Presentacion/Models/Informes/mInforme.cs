using BSP.POS.Presentacion.Models.Actividades;
using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Informes
{
    public class mInforme : IValidatableObject
    {
        public string consecutivo { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de Fecha de consultoría es requerido")]
        public string fecha_consultoria { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo de Hora de Inicio es requerido")]
        public string hora_inicio { get; set; } = DateTime.Now.ToString("HH:mm");
        [Required(ErrorMessage = "El campo de Hora Final es requerido")]
        public string hora_final { get; set; } = DateTime.Now.ToString("HH:mm");
        [Required(ErrorMessage = "El campo de la Modalidad es requerido")]
        public string modalidad_consultoria { get; set; } = string.Empty;
        public string numero_proyecto { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;

        [DataType(DataType.Date, ErrorMessage = "El campo tiene que ser una fecha válida")]
        public DateTime FechaConsultoriaDateTime
        {
            get => DateTime.Parse(fecha_consultoria);
            set => fecha_consultoria = value.ToString("yyyy-MM-dd");
        }

        [DataType(DataType.Time, ErrorMessage = "El campo tiene que ser una hora válida")]
        public DateTime HoraInicioTime
        {
            get => DateTime.Parse(hora_inicio);
            set => hora_inicio = value.ToString("HH:mm");
        }


        [DataType(DataType.Time, ErrorMessage = "El campo tiene que ser una hora válida")]
        public DateTime HoraFinalTime
        {
            get => DateTime.Parse(hora_final);
            set => hora_final = value.ToString(@"HH\:mm");
        }
        public TimeSpan HoraInicioTimeSpan
        {
            get => TimeSpan.Parse(hora_inicio);
            set => hora_inicio = value.ToString(@"HH\:mm");
        }

        public TimeSpan HoraFinalTimeSpan
        {
            get => TimeSpan.Parse(hora_final);
        }
        [ValidateComplexType]
        public List<mActividadesAsociadas> listaActividadesAsociadas { get; set; } = new List<mActividadesAsociadas>();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HoraInicioTimeSpan > HoraFinalTimeSpan)
            {
                yield return new ValidationResult("La Hora de Inicio no puede ser mayor que la Hora Final.", new[] { nameof(HoraInicioTimeSpan) });
            }
            else if (HoraFinalTimeSpan < HoraInicioTimeSpan)
            {
                yield return new ValidationResult("La Hora Final no puede ser menor que la Hora de Inicio.", new[] { nameof(HoraFinalTimeSpan) });
            }
        }
    }
}
