

namespace BSP.POS.API.Models.Usuarios
{
    public class mUsuarioNuevaClave
    {
        public string token_recuperacion { get; set; } = string.Empty;
        public string esquema { get; set; } = string.Empty;
        public string clave { get; set; } = string.Empty;
    }
}
