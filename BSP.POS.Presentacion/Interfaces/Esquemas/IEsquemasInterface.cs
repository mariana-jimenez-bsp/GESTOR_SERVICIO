using BSP.POS.Presentacion.Models.Esquemas;

namespace BSP.POS.Presentacion.Interfaces.Esquemas
{
    public interface IEsquemasInterface
    {
        List<mEsquemas> ListaEsquemas { get; set;}
        List<mDatosEsquemasDeUsuario> ListaEsquemasDeUsuario { get; set;}
        Task ObtenerListaDeEsquemas();
        Task ObtenerListaDeEsquemasDeUsuario(string codigo);
        Task<bool> ActualizarListaDeEsquemasDeUsuario(List<string> listaEsquemas, string codigo);
    }
}
