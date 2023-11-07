using BSP.POS.Presentacion.Models.Permisos;

namespace BSP.POS.Presentacion.Interfaces.Permisos
{
    public interface IPermisosInterface
    {
        List<mPermisosAsociados> ListaPermisosAsociadados { get; set; }
        Task ObtenerListaDePermisosAsociados(string esquema, string id);

        List<mPermisos> ListaPermisos { get; set; }
        Task ObtenerListaDePermisos(string esquema);
        Task<bool> ActualizarListaPermisosAsociados(List<mPermisosAsociados> listaPermisos, string idUsuario, string esquema);
        List<mPermisos> ListaDePermisos { get; set; }
        List<mSubPermisos> ListaDeSubPermisos { get; set; }
        List<mDatosPermisosDeUsuarios> ListaDePermisosDeUsuario { get; set; }
        List<mDatosSubPermisosDeUsuarios> ListaDeSubPermisosDeUsuario { get; set; }
        Task ObtenerLaListaDePermisos(string esquema);
        Task ObtenerLaListaDeSubPermisos(string esquema);
        Task ObtenerLaListaDePermisosDeUsuario(string esquema, string codigo);
        Task ObtenerLaListaDeSubPermisosDeUsuario(string esquema, string codigo);
        Task<bool> ActualizarListaPermisosDeUsuario(List<string> listaPermisos, string codigo, string esquema);
    }
}
