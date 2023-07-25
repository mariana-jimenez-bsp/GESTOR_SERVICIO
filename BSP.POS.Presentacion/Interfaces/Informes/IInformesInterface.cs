using BSP.POS.Presentacion.Models;

namespace BSP.POS.Presentacion.Interfaces.Informes
{
    public interface IInformesInterface
    {
        List<mInformes> ListaInformesAsociados { get; set; }
        Task ObtenerListaDeInformesAsociados(string cliente);

        mInformeAsociado InformeAsociado { get; set; }
        Task<mInformeAsociado?> ObtenerInformeAsociado(string consecutivo);
    }
}
