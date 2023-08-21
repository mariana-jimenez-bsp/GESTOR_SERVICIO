namespace BSP.POS.API.Models.Usuarios
{
    public class mUsuariosParaEditar
    {
        public string id { get; set; } = string.Empty;
        public string codigo { get; set; } = string.Empty;
        public string cod_cliente { get; set; } = string.Empty;

        public string usuario { get; set; } = string.Empty;
        public string correo { get; set; } = string.Empty;

        public string clave { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string rol { get; set; } = string.Empty;
        public string telefono { get; set; } = string.Empty;
        public string departamento { get; set; } = string.Empty;
        public byte[] imagen { get; set; } = new byte[] { 0x00 };
        public string esquema { get; set; } = string.Empty;
    }
}
