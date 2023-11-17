using BSP.POS.Presentacion.Models.Clientes;

namespace BSP.POS.Presentacion.Interfaces.Clientes
{
    public interface IClientesInterface
    {
        List<mClientes> ListaClientes { get; set; }
        Task ObtenerListaClientes(string esquema);
        List<mClientes> ListaClientesRecientes { get; set; }
        Task ObtenerListaClientesRecientes(string esquema);
        mClienteAsociado ClienteAsociado { get; set; }
        Task<mClienteAsociado?> ObtenerClienteAsociado(string cliente, string esquema);
        Task<bool> ActualizarListaDeClientes(List<mClientes> listaClientes, string esquema);

        List<mClienteContado> ListaClientesCorporaciones { get; set; }
        Task ObtenerListaClientesCorporaciones(string esquema);

        Task<bool> AgregarCliente(mAgregarCliente cliente, string esquema, string usuario);
        Task<string> ValidarExistenciaDeCliente(string esquema, string cliente);
        Task<string> ObtenerPaisDeCliente(string esquema, string cliente);
        Task<string> ObtenerContribuyenteDeCliente(string esquema, string cliente);
        Task<bool> EliminarCliente(string esquema, string cliente);
    }
}
