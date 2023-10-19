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
    }
}
