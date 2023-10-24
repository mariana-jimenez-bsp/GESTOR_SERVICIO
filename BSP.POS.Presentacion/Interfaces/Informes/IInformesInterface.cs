using BSP.POS.Presentacion.Models.Informes;

namespace BSP.POS.Presentacion.Interfaces.Informes
{
    public interface IInformesInterface
    {
        List<mInformes> ListaInformesAsociados { get; set; }
        Task ObtenerListaDeInformesAsociados(string cliente, string esquema);

        mInformeAsociado InformeAsociado { get; set; }
        Task<mInformeAsociado?> ObtenerInformeAsociado(string consecutivo, string esquema);
        Task ActualizarInformeAsociado(mInformeAsociado informe, string esquema);

        Task CambiarEstadoDeInforme(mInformeEstado informe, string esquema);

        Task EliminarInforme(string consecutivo, string esquema);

        Task<bool> EnviarCorreoDeAprobacionDeInforme(string esquema, string consecutivo);
        Task<mTokenAprobacionInforme> ValidarTokenAprobacionDeInforme(string esquema, string token);
        Task AprobarInforme(mTokenAprobacionInforme tokenAprobacion, string esquema);
        Task RechazarInforme(mTokenAprobacionInforme tokenAprobacion, string esquema);

        Task<string> AgregarInformeAsociado(string cliente, string esquema);
        Task<string> ValidarExistenciaConsecutivoInforme(string esquema, string consecutivo);
    }
}
