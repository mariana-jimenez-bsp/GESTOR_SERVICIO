using BSP.POS.Presentacion.Models.Actividades;

namespace BSP.POS.Presentacion.Interfaces.Actividades
{
    public interface IActividadesInterface
    {
        List<mActividadesAsociadas> ListaActividadesAsociadas { get; set; }
        Task ObtenerListaDeActividadesAsociadas(string consecutivo, string esquema);

        List<mActividades> ListaActividades { get; set; }
        Task ObtenerListaDeActividades(string esquema);

        Task<bool> ActualizarListaDeActividades(List<mActividades> listaActividades, string esquema);
        Task ActualizarListaDeActividadesAsociadas(List<mActividadesAsociadas> listaActividades, string esquema);
        Task AgregarActividadDeInforme(mActividadAsociadaParaAgregar actividad, string esquema);
        Task EliminarActividadDeInforme(string idActividad, string esquema);
        Task<bool> AgregarActividad(mActividades actividad, string esquema);
    }
}
