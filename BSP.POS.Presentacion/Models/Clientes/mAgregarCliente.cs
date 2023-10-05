using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Clientes
{
    public class mAgregarCliente
    {
        public string CLIENTE { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del Responsable es requerido")]
        [StringLength(30, ErrorMessage = "Tamaño máximo de 30 caracteres")]
        public string CONTACTO { get; set; } = string.Empty;
        public string CARGO { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del Nombre es requerido")]
        [StringLength(150, ErrorMessage = "Tamaño máximo de 150 caracteres")]
        public string NOMBRE { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del Nombre es requerido")]
        [StringLength(150, ErrorMessage = "Tamaño máximo de 150 caracteres")]
        public string ALIAS { get; set; } = string.Empty;
        public string DIRECCION { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del Teléfono 1 es requerido")]
        [StringLength(50, ErrorMessage = "Tamaño máximo de 50 caracteres")]
        [RegularExpression(@"^\d+(-\d+)*$", ErrorMessage = "El campo tiene que ser un teléfono válido")]
        public string TELEFONO1 { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del Teléfono 2 es requerido")]
        [StringLength(50, ErrorMessage = "Tamaño máximo de 50 caracteres")]
        [RegularExpression(@"^\d+(-\d+)*$", ErrorMessage = "El campo tiene que ser un teléfono válido")]
        public string TELEFONO2 { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de la Cédula es requerido")]
        [StringLength(20, ErrorMessage = "Tamaño máximo de 20 caracteres")]
        public string CONTRIBUYENTE { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de la Moneda es requerido")]
        [StringLength(4, ErrorMessage = "Tamaño máximo de 4 caracteres")]
        public string MONEDA { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del País es requerido")]
        public string PAIS { get; set; } = string.Empty;
        public string ZONA { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del Correo es requerido")]
        [StringLength(249, ErrorMessage = "Tamaño máximo de 249 caracteres")]
        public string? E_MAIL { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de la Provincia es requerido")]
        public string DIVISION_GEOGRAFICA1 { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del Cantón es requerido")]
        public string DIVISION_GEOGRAFICA2 { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del Distrito es requerido")]
        public string DIVISION_GEOGRAFICA3 { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del Barrio es requerido")]
        public string DIVISION_GEOGRAFICA4 { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de Otras señas es requerido")]
        [StringLength(160, ErrorMessage = "Tamaño máximo de 160 caracteres")]
        public string OTRAS_SENAS { get; set; } = string.Empty;
        public string NIVEL_PRECIO { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debe escoger una de la opciones")]
        public string EXENTO_IMPUESTOS { get; set; } = string.Empty;
        public string CODIGO_IMPUESTO { get; set; } = string.Empty;
        public string RecordDate { get; set; } = string.Empty;
        public string CreateDate { get; set; } = string.Empty;
        public byte[] IMAGEN { get; set; } = new byte[] { 0x00 };
        [Required(ErrorMessage = "El campo de la Condición de pago es requerido")]
        public string CONDICION_PAGO { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debe escoger una de la opciones")]
        public string DOC_A_GENERAR { get; set; } = string.Empty;
        [RegularExpression(@"^(0|[1-9]\d*)(\.\d{1,8})?$", ErrorMessage = "Solo se permiten valores decimales positivos con un máximo de 8 decimales con el formato 0.00")]
        public string EXENCION_IMP1 { get; set; } = string.Empty;
        [RegularExpression(@"^(0|[1-9]\d*)(\.\d{1,8})?$", ErrorMessage = "Solo se permiten valores decimales positivos con un máximo de 8 decimales con el formato 0.00")]
        public string EXENCION_IMP2 { get; set; } = string.Empty;
        [RegularExpression(@"^(0|[1-9]\d*)(\.\d{1,8})?$", ErrorMessage = "Solo se permiten valores decimales positivos con un máximo de 8 decimales con el formato 0.00")]
        public string DESCUENTO { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debe escoger una de la opciones")]
        public string ES_CORPORACION { get; set; } = string.Empty;
        public string CLI_CORPORAC_ASOC { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del Tipo de Impuesto es requerido")]
        public string TIPO_IMPUESTO { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del Tipo de Tarifa es requerido")]
        public string TIPO_TARIFA { get; set; } = string.Empty;
        public string PORC_TARIFA { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del Tipo de Cliente es requerido")]
        public string TIPIFICACION_CLIENTE { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de la Afectación del IVA es requerido")]
        public string AFECTACION_IVA { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del Tipo de Nit es requerido")]
        public string TIPO_NIT { get; set; } = string.Empty;

        public bool readonlyExento = true;
        public bool readonlyCorporacion = true;
        [RegularExpression(@"\.(png|jpg|jpeg)$", ErrorMessage = "La imagen debe ser un archivo PNG o JPG.")]
        public IFormFile? ImagenFile { get; set; }
    }
}
