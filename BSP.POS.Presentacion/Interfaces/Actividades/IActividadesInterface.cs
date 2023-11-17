using BSP.POS.Presentacion.Models.Actividades;

namespace BSP.POS.Presentacion.Interfaces.Actividades
{
    public interface IActividadesInterface
    {
        List<mActividadesAsociadas> ListaActividadesAsociadas { get; set; }
        Task ObtenerListaDeActividadesAsociadas(string consecutivo, string esquema);

        List<mActividades> ListaActividades { get; set; }
        Task ObtenerListaDeActividades(string esquema);
        List<mActividades> ListaActividadesDeUsuario { get; set; }
        Task ObtenerListaDeActividadesPorUsuario(string esquema, string codigo);

        Task<bool> ActualizarListaDeActividades(List<mActividades> listaActividades, string esquema);
        Task<bool> ActualizarListaDeActividadesAsociadas(List<mActividadesAsociadas> listaActividades, string esquema);
        Task AgregarActividadDeInforme(mActividadAsociadaParaAgregar actividad, string esquema);
        Task<bool> EliminarActividadDeInforme(string idActividad, string esquema);
        Task<bool> AgregarActividad(mActividades actividad, string esquema);
        Task<bool> EliminarActividad(string esquema, string codigo);
    }
}
