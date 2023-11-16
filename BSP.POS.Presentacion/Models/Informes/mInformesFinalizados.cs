namespace BSP.POS.Presentacion.Models.Informes
{
    public class mInformesFinalizados
    {
        public string consecutivo { get; set; } = string.Empty;
        public string fecha_actualizacion { get; set; } = string.Empty;
        public string fecha_consultoria { get; set; } = string.Empty;
        public string numero_proyecto { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;
        public string codigo_cliente { get; set; } = string.Empty;
        public string nombre_cliente { get; set; } = string.Empty;
        public byte[] imagen_cliente { get; set; } = new byte[] { 0x00 };
        public string codigo_consultor { get; set; } = string.Empty;
        public string nombre_consultor { get; set; } = string.Empty;
        public DateTime FechaConsultoriaDateTime
        {
            get => DateTime.Parse(fecha_consultoria);
            set => fecha_consultoria = value.ToString("yyyy-MM-dd");
        }

        public string imagenSeleccionada { get; set; } = "eyelash-background";
        public string informeSeleccionado { get; set; } = string.Empty;
    }
}
