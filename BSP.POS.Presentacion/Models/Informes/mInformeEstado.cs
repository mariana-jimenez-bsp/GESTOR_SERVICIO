using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Informes
{
    public class mInformeEstado
    {
        public string consecutivo { get; set; } = string.Empty;
      
        public string fecha_consultoria { get; set; } = string.Empty;


        public string hora_inicio { get; set; } = string.Empty;

        public string hora_final { get; set; } = string.Empty;

        public string modalidad_consultoria { get; set; } = string.Empty;
        public string numero_proyecto { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;
    }
}
