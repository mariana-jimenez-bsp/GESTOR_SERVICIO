using BSP.POS.Presentacion.Models;
using BSP.POS.UTILITARIOS.Clientes;
namespace BSP.POS.Presentacion.Interfaces.Clientes
{
    public interface IClientesInterface
    {
        List<mClientes> ListaClientes { get; set; }
        Task ObtenerListaClientes();
        List<mClientes> ListaClientesRecientes { get; set; }
        Task ObtenerListaClientesRecientes();
        mClienteAsociado ClienteAsociado { get; set; }
        Task<mClienteAsociado?> ObtenerClienteAsociado(string cliente);
        Task ActualizarListaDeClientes(List<mClientes> listaClientes);
    }
}
