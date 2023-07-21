using BSP.POS.Presentacion.Models;

namespace BSP.POS.Presentacion.Interfaces.Tiempos
{
    public interface ITiemposInterface
    {
        List<mTiempos> ListaTiempos { get; set; }
        Task ObtenerListaTIempos();
        Task ActualizarListaDeTiempo(List<mTiempos> listaTiempos);
    }
}
