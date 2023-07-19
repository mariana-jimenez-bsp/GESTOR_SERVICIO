

namespace BSP.POS.Presentacion.Interfaces.Usuarios
{
    public interface IUsuariosInterface
    {
        Task<string> RealizarLogin(string USUARIO, string CLAVE);
    }
}
