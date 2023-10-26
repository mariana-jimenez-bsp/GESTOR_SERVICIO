
using BSP.POS.Presentacion.Models.Proyectos;

namespace BSP.POS.Presentacion.Interfaces.Proyectos
{
    public interface IProyectosInterface
    {
        List<mProyectos> ListaProyectosIniciados { get; set; }
        List<mProyectos> ListaProyectosTerminados { get; set; }
        Task ObtenerListaDeProyectosIniciados(string esquema);
        Task ObtenerListaDeProyectosTerminados(string esquema);

        Task<bool> ActualizarListaDeProyectos(List<mProyectos> listaProyectos, string esquema);
        Task<bool> TerminarProyecto(string numero, string esquema);
        Task<bool> AgregarProyecto(mProyectos proyecto, string esquema);
    }
}
