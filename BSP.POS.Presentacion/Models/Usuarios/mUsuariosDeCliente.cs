using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Usuarios
{
    public class mUsuariosDeCliente
    {
        public string id { get; set; } = string.Empty;

        public string codigo { get; set; } = string.Empty;
        public string cod_cliente { get; set; } = string.Empty;
        public string usuario { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string codigo_departamento { get; set; } = string.Empty;
        public string correo { get; set; } = string.Empty;
        public string telefono { get; set; } = string.Empty;

        public string nombre_departamento { get; set; } = string.Empty;
    }
}
