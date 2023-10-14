namespace BSP.POS.Presentacion.Models.Licencias
{
    public class mCodigoLicenciaYProducto
    {
        public byte[] codigo_licencia { get; set; } = new byte[] { 0x00 };
        public string producto { get; set; } = string.Empty;
    }
}
