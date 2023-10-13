namespace BSP.POS.Presentacion.Models.Licencias
{
    public class mActualizarDatosLicencia
    {
        public string FechaInicio { get; set; } = string.Empty;
        public string FechaFin { get; set; } = string.Empty;
        public string FechaAviso { get; set; } = string.Empty;
        public string CantidadCajas { get; set; } = string.Empty;
        public string CantidadUsuarios { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
        public byte[] Codigo { get; set; } = new byte[] { 0x00 };
    }
}
