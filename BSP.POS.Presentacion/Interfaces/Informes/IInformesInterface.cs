using BSP.POS.Presentacion.Models.Informes;

namespace BSP.POS.Presentacion.Interfaces.Informes
{
    public interface IInformesInterface
    {
        List<mInformes> ListaInformesAsociados { get; set; }
        Task ObtenerListaDeInformesAsociados(string cliente, string esquema);

        mInformeAsociado InformeAsociado { get; set; }
        Task<mInformeAsociado?> ObtenerInformeAsociado(string consecutivo, string esquema);
    }
}
