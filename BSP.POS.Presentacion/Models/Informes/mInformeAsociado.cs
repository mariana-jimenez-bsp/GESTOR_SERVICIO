using BSP.POS.Presentacion.Models.Actividades;

namespace BSP.POS.Presentacion.Models.Informes
{
    public class mInformeAsociado
    {
        public string consecutivo { get; set; } = string.Empty;
        public string fecha_consultoria { get; set; } = string.Empty;
        public string hora_inicio { get; set; } = string.Empty;
        public string hora_final { get; set; } = string.Empty;
        public string modalidad_consultoria { get; set; } = string.Empty;
        public string cliente { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;
    }
}
