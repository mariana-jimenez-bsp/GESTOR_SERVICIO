namespace BSP.POS.Presentacion.Models.Licencias
{
    public class mDatosLicencia
    {
        public DateTime FechaInicio { get; set; } = DateTime.Now;
        public DateTime FechaFin { get; set; } = DateTime.Now;
        public DateTime FechaAviso { get; set; } = DateTime.Now;
        public int CantidadUsuarios { get; set; } = 0;
        public bool MacAddressIguales { get; set; } = false;
        public string Pais { get; set; } = string.Empty;
        public string CedulaJuridica { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
    }
}
