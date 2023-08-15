using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Proyectos
{
    public class mProyectos
    {
        public string Id { get; set; } = string.Empty;

        public string numero { get; set; } = string.Empty;
        [StringLength(50, ErrorMessage = "Tamaño máximo de 50 caracteres")]
        [Required(ErrorMessage = "El campo del Nombre de Consultor es requerido")]
        public string nombre_consultor { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de Fecha Inicial es requerido")]
        public string fecha_inicial { get; set; } = DateTime.Now.ToString();
        [Required(ErrorMessage = "El campo de Fecha Final es requerido")]
        [DataType(DataType.Date, ErrorMessage = "El campo tiene que ser una fecha válida")]
        public string fecha_final { get; set; } = DateTime.Now.ToString();
        [Required(ErrorMessage = "El campo de Horas Totales es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de horas debe ser un número entero válido.")]
        public string horas_totales { get; set; } = string.Empty;
        [StringLength(150, ErrorMessage = "Tamaño máximo de 150 caracteres")]
        [Required(ErrorMessage = "El campo del Nombre de la empresa es requerido")]
        public string empresa { get; set; } = string.Empty;
        [StringLength(200, ErrorMessage = "Tamaño máximo de 200 caracteres")]
        [Required(ErrorMessage = "El campo del Centro de costo es requerido")]
        public string centro_costo { get; set; } = string.Empty;
        [StringLength(200, ErrorMessage = "Tamaño máximo de 200 caracteres")]
        [Required(ErrorMessage = "El campo del nombre de proyecto es requerido")]
        public string nombre_proyecto { get; set; } = string.Empty;
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
    }
}
