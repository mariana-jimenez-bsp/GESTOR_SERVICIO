namespace BSP.POS.Presentacion.Interfaces.Reportes
{
    public interface IReportesInterface
    {
        Task<byte[]> GenerarReporteDeInforme(string esquema, string consecutivo);
    }
}
