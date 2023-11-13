namespace BSP.POS.Presentacion.Models.Informes
{
    public class mInformesDeCliente
    {
        public string consecutivo { get; set; } = string.Empty;
        public string fecha_actualizacion { get; set; } = string.Empty;
        public string fecha_consultoria { get; set; } = string.Empty;
        public string numero_proyecto { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;
        public DateTime FechaActualizacionDateTime
        {
            get => DateTime.Parse(fecha_actualizacion);
            set { }
        }
    }
}
