using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Reportes;

namespace BSP.POS.Presentacion.Interfaces.Informes
{
    public interface IInformesInterface
    {
        List<mInformesDeProyecto> ListaInformesDeProyecto { get; set; }
        List<mInformesDeProyecto> ListaInformes { get; set; }
        List<mInformesDeCliente> ListaInformesDeCliente { get; set; }
        Task ObtenerListaDeInformesDeProyecto(string numero, string esquema);
        Task ObtenerListaDeInformes(string esquema);
        Task ObtenerListaDeInformesDeCliente(string cliente, string esquema);

        mInforme Informe { get; set; }
        Task<mInforme> ObtenerInforme(string consecutivo, string esquema);
        Task<bool> ActualizarInforme(mInforme informe, string esquema);

        Task<bool> CambiarEstadoDeInforme(mInformeEstado informe, string esquema);

        Task<bool> EliminarInforme(string consecutivo, string esquema);

        Task<bool> EnviarCorreoDeReporteDeInforme(string esquema, string consecutivo, mObjetoReporte objetoReporte);
        Task<mTokenRecibidoInforme> ValidarTokenRecibidoDeInforme(string esquema, string token);
        Task<bool> ActivarRecibidoInforme(mTokenRecibidoInforme tokenRecibido, string esquema);

        Task<string> AgregarInformeAsociado(string numero, string esquema);
        Task<string> ValidarExistenciaConsecutivoInforme(string esquema, string consecutivo);
        List<mInformesFinalizados> ListaInformesFinalizados { get; set; }
        Task ObtenerListaDeInformesDeClienteFinalizados(string cliente, string esquema);
        Task ObtenerListaDeInformesDeUsuarioFinalizados(string codigo, string esquema);
        Task ObtenerListaDeInformesFinalizados(string esquema);
    }
}
