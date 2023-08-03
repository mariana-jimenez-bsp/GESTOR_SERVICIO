using BSP.POS.Presentacion.Models.Actividades;
using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Informes
{
    public class mInformeAsociado
    {
        public string consecutivo { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de Fecha de consultoría es requerido")]
        public string fecha_consultoria { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo de Hora de Inicio es requerido")]
        public string hora_inicio { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de Hora Final es requerido")]
        public string hora_final { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de la Modalidad es requerido")]
        public string modalidad_consultoria { get; set; } = string.Empty;
        public string cliente { get; set; } = string.Empty;
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
            set => hora_final = value.ToString("HH:mm");
        }
    }
}
