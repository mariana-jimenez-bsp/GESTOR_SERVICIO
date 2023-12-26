using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Actividades
{
    public class mActividadAsociadaParaAgregar
    {
        public string Id { get; set; } = string.Empty;

        public string consecutivo_informe { get; set; } = string.Empty;
        [StringLength(6, ErrorMessage = "Tamaño máximo de 6 caracteres")]
        [Required(ErrorMessage = "Debe escoger una actividad")]
        public string codigo_actividad { get; set; } = string.Empty;
       
        public string horas_cobradas { get; set; } = string.Empty;
        
        public string horas_no_cobradas { get; set; } = string.Empty;
        public string nombre_actividad { get; set; } = string.Empty;
        public string fecha { get; set; } = string.Empty;

    }
}
