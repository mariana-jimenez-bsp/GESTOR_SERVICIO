namespace BSP.POS.Presentacion.Models.Licencias
{
    public class mLicenciaByte
    {
        public string texto_archivo { get; set; } = string.Empty;
        public string producto { get; set; } = string.Empty;
        public byte[] codigo_licencia { get; set; } = new byte[] { 0x00 };
    }
}
