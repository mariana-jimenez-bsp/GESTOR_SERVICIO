

using BSP.POS.Presentacion.Models;

namespace BSP.POS.Presentacion.Interfaces.Usuarios
{
    public interface IUsuariosInterface
    {
        mLogin UsuarioLogin { get; set; }
        Task<mLogin> RealizarLogin(string USUARIO, string CLAVE, string ESQUEMA);
        string EncriptarClave(string clave);

        mPerfil Perfil { get; set; }
        Task ObtenerPerfil(string usuario);

        Task ActualizarPefil(mPerfil perfil, string usuarioOriginal, string claveOriginal);



    }
}
