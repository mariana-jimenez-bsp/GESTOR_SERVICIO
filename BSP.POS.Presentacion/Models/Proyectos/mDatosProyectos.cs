namespace BSP.POS.Presentacion.Models.Proyectos
{
    public class mDatosProyectos
    {
        public string Id { get; set; } = string.Empty;

        public string numero { get; set; } = string.Empty;
        public string codigo_cliente { get; set; } = string.Empty;
        public string fecha_inicial { get; set; } = string.Empty;
        public string fecha_final { get; set; } = string.Empty;
        public string horas_totales { get; set; } = string.Empty;
        public string centro_costo { get; set; } = string.Empty;
        public string nombre_proyecto { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;
        public string nombre_cliente { get; set; } = string.Empty;
        public string contacto { get; set; } = string.Empty;
        public string cargo { get; set; } = string.Empty;
        public byte[] imagen { get; set; } = new byte[] { 0x00 };
    }
}
