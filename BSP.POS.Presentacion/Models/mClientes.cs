using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models
{
    public class mClientes
    {
        public string CLIENTE { get; set; } = string.Empty;
        public string CONTACTO { get; set; } = string.Empty;
        public string CARGO { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del Nombre es requerido")]
        [StringLength(150, ErrorMessage = "Tamaño máximo de 150 caracteres")]
        public string NOMBRE { get; set; } = string.Empty;
        [StringLength(150, ErrorMessage = "Tamaño máximo de 150 caracteres")]
        public string ALIAS { get; set; } = string.Empty;
        public string DIRECCION { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del Teléfono 1 es requerido")]
        [StringLength(50, ErrorMessage = "Tamaño máximo de 50 caracteres")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "El campo tiene que ser un teléfono")]
        public string TELEFONO1 { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del Teléfono 2 es requerido")]
        [StringLength(50, ErrorMessage = "Tamaño máximo de 50 caracteres")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "El campo tiene que ser un teléfono")]
        public string TELEFONO2 { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de la Cédula es requerido")]
        [StringLength(20, ErrorMessage = "Tamaño máximo de 20 caracteres")]
        public string CONTRIBUYENTE { get; set; } = string.Empty;
        public string MONEDA { get; set; } = string.Empty;      
        public string PAIS { get; set; } = string.Empty;
        public string ZONA { get; set; } = string.Empty;
        [DataType(DataType.EmailAddress, ErrorMessage = "El campo tiene que ser un correo")]
        [StringLength(249, ErrorMessage = "Tamaño máximo de 249 caracteres")]
        public string E_MAIL { get; set; } = string.Empty;
        public string DIVISION_GEOGRAFICA1 { get; set; } = string.Empty;
        public string DIVISION_GEOGRAFICA2 { get; set; } = string.Empty;
        public string DIVISION_GEOGRAFICA3 { get; set; } = string.Empty;
        public string DIVISION_GEOGRAFICA4 { get; set; } = string.Empty;
        public string OTRAS_SENAS { get; set; } = string.Empty;
        public string NIVEL_PRECIO { get; set; } = string.Empty;
        public string EXENTO_IMPUESTOS { get; set; } = string.Empty;
        public string CODIGO_IMPUESTO { get; set; } = string.Empty;
        public string RecordDate { get; set; } = string.Empty;
        public string CreateDate { get; set; } = string.Empty;

        public List<mUsuariosDeCliente> listaDeUsuarios { get; set; } = new List<mUsuariosDeCliente>();
    }
}
