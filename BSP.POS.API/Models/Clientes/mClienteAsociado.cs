namespace BSP.POS.API.Models.Clientes
{
    public class mClienteAsociado
    {
        public string CLIENTE { get; set; } = string.Empty;
        public string NOMBRE { get; set; } = string.Empty;

        public string CONTACTO { get; set; } = string.Empty;
        public string CARGO { get; set; } = string.Empty;
        public byte[] IMAGEN { get; set; } = new byte[] { 0x00 };
    }
}
