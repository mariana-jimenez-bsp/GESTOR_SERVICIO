using BSP.POS.Presentacion.Models.CodigoTelefonoPais;

namespace BSP.POS.Presentacion.Interfaces.CodigoTelefonoPais
{
    public interface ICodigoTelefonoPaisInterface
    {
        List<mCodigoTelefonoPais> listaDatosCodigoTelefonoPais { get; set; }
        Task ObtenerDatosCodigoTelefonoPais(string esquema);
        List<mCodigoTelefonoPaisClientes> listaDatosCodigoTelefonoPaisDeCliente { get; set; }
        Task ObtenerDatosCodigoTelefonoPaisDeClientes(string esquema);
        mCodigoTelefonoPaisUsuarios datosCodigoTelefonoPaisDeUsuario { get; set; }
        Task ObtenerDatosCodigoTelefonoPaisDeUsuarioPorUsuario(string esquema, string codigoUsuario);
    }
}
