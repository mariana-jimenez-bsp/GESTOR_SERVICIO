using BSP.POS.Presentacion.Models.CodigoTelefonoPais;

namespace BSP.POS.Presentacion.Interfaces.CodigoTelefonoPais
{
    public interface ICodigoTelefonoPaisInterface
    {
        List<mCodigoTelefonoPaisClientes> listaDatosCodigoClientePaisDeCliente { get; set; }
        Task ObtenerDatosCodigoTelefonoPaisDeClientes(string esquema);
    }
}
