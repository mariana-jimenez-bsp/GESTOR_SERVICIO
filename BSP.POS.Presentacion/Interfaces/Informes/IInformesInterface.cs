using BSP.POS.Presentacion.Models;

namespace BSP.POS.Presentacion.Interfaces.Informes
{
    public interface IInformesInterface
    {
        List<mInformes> ListaInformesAsociados { get; set; }
        Task ObtenerListaDeInformesAsociados(string cliente);


    }
}
