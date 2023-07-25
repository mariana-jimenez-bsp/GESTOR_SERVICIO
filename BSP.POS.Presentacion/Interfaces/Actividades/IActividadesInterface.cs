using BSP.POS.Presentacion.Models;

namespace BSP.POS.Presentacion.Interfaces.Actividades
{
    public interface IActividadesInterface
    {
        List<mActividades> ListaActividadesAsociadas { get; set; }
        Task ObtenerListaDeActividadesAsociadas(string consecutivo);
    }
}
