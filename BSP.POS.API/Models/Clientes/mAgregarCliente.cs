namespace BSP.POS.API.Models.Clientes
{
    public class mAgregarCliente
    {
        public string CLIENTE { get; set; } = string.Empty;

        public string CONTACTO { get; set; } = string.Empty;
        public string CARGO { get; set; } = string.Empty;

        public string NOMBRE { get; set; } = string.Empty;

        public string ALIAS { get; set; } = string.Empty;
        public string DIRECCION { get; set; } = string.Empty;

        public string TELEFONO1 { get; set; } = string.Empty;

        public string TELEFONO2 { get; set; } = string.Empty;

        public string CONTRIBUYENTE { get; set; } = string.Empty;
        public string MONEDA { get; set; } = string.Empty;
        public string PAIS { get; set; } = string.Empty;
        public string ZONA { get; set; } = string.Empty;

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
        public byte[] IMAGEN { get; set; } = new byte[] { 0x00 };
        public string CONDICION_PAGO { get; set; } = string.Empty;
        public string DOC_A_GENERAR { get; set; } = string.Empty;
        public string EXENCION_IMP1 { get; set; } = string.Empty;
        public string EXENCION_IMP2 { get; set; } = string.Empty;
        public string DESCUENTO { get; set; } = string.Empty;
        public string ES_CORPORACION { get; set; } = string.Empty;
        public string CLI_CORPORAC_ASOC { get; set; } = string.Empty;
        public string TIPO_IMPUESTO { get; set; } = string.Empty;
        public string TIPO_TARIFA { get; set; } = string.Empty;
        public string PORC_TARIFA { get; set; } = string.Empty;
        public string TIPIFICACION_CLIENTE { get; set; } = string.Empty;
        public string AFECTACION_IVA { get; set; } = string.Empty;

        public string TIPO_NIT { get; set; } = string.Empty;
    }
}
