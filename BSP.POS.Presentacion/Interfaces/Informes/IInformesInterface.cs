using BSP.POS.Presentacion.Models.Informes;

namespace BSP.POS.Presentacion.Interfaces.Informes
{
    public interface IInformesInterface
    {
        List<mInformesDeProyecto> ListaInformesDeProyecto { get; set; }
        Task ObtenerListaDeInformesDeProyecto(string cliente, string esquema);

        mInforme Informe { get; set; }
        Task<mInforme> ObtenerInforme(string consecutivo, string esquema);
        Task<bool> ActualizarInforme(mInforme informe, string esquema);

        Task<bool> CambiarEstadoDeInforme(mInformeEstado informe, string esquema);

        Task<bool> EliminarInforme(string consecutivo, string esquema);

        Task<bool> EnviarCorreoDeReporteDeInforme(string esquema, string consecutivo);
        Task<mTokenRecibidoInforme> ValidarTokenRecibidoDeInforme(string esquema, string token);
        Task<bool> ActivarRecibidoInforme(mTokenRecibidoInforme tokenRecibido, string esquema);

        Task<string> AgregarInformeAsociado(string numero, string esquema);
        Task<string> ValidarExistenciaConsecutivoInforme(string esquema, string consecutivo);
    }
}
