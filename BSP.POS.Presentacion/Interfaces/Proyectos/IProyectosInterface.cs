
using BSP.POS.Presentacion.Models.Proyectos;

namespace BSP.POS.Presentacion.Interfaces.Proyectos
{
    public interface IProyectosInterface
    {
        List<mProyectos> ListaProyectosActivos { get; set; }
        List<mDatosProyectos> ListaDatosProyectosActivos { get; set; }
        List<mDatosProyectos> ListaDatosProyectosActivosDeCliente { get; set; }
        List<mProyectos> ListaProyectosTerminadosYCancelados { get; set; }
        mProyectos ProyectoAsociado { get; set; }
        Task ObtenerListaDeProyectosActivos(string esquema);
        Task ObtenerDatosDeProyectosActivos(string esquema);
        Task ObtenerDatosDeProyectosActivosDeCliente(string esquema, string cliente);
        Task ObtenerListaDeProyectosTerminadosYCancelados(string esquema);

        Task<bool> ActualizarListaDeProyectos(List<mProyectos> listaProyectos, string esquema);
        Task<bool> CambiarEstadoProyecto(string numero, string estado, string esquema);
        Task<bool> AgregarProyecto(mProyectos proyecto, string esquema);
        Task ObtenerProyecto(string esquema, string numero);
        Task<bool> EliminarProyecto(string esquema, string numero);
    }
}
