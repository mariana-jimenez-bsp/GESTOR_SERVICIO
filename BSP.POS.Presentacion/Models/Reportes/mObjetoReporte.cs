namespace BSP.POS.Presentacion.Models.Reportes
{
    public class mObjetoReporte
    {
        public byte[] reporte { get; set; } = new byte[] { 0x00 };
        public List<string> listaCorreosExtras { get; set; } = new List<string>();
    }
}
