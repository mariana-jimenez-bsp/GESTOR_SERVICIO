
using BSP.POS.Presentacion.Models.Proyectos;

namespace BSP.POS.Presentacion.Interfaces.Proyectos
{
    public interface IProyectosInterface
    {
        List<mProyectos> ListaProyectos { get; set; }
        Task ObtenerListaDeProyectos(string esquema);

        Task ActualizarListaDeProyectos(List<mProyectos> listaProyectos, string esquema);
        Task AgregarProyecto(mProyectos proyecto, string esquema);
    }
}
