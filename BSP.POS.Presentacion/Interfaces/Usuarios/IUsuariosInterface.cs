using BSP.POS.Presentacion.Models.Usuarios;

namespace BSP.POS.Presentacion.Interfaces.Usuarios
{
    public interface IUsuariosInterface
    {
        mLogin UsuarioLogin { get; set; }
        Task<mLogin> RealizarLogin(mLogin usuarioLog);
        string EncriptarClave(string clave);

        mPerfil Perfil { get; set; }
        Task ObtenerPerfil(string usuario);

        Task ActualizarPefil(mPerfil perfil, string usuarioOriginal, string claveOriginal);

        Task<List<mUsuariosDeCliente>> ObtenerListaDeUsuariosDeClienteAsociados(string esquema, string cliente);
        List<mUsuariosDeCliente> ListaDeUsuariosDeCliente { get; set; }

        Task<bool> EnviarCorreoRecuperarClave(mTokenRecuperacion tokenRecuperacion);
        mTokenRecuperacion UsuarioRecuperacion { get; set; }

        Task<mTokenRecuperacion> ValidarTokenRecuperacion(string esquema, string token);

        Task ActualizarClaveDeUsuario(mUsuarioNuevaClave usuario);

        Task<string> ValidarCorreoCambioClave(string esquema, string correo);

        List<mUsuariosDeClienteDeInforme> ListaUsuariosDeClienteDeInforme { get; set; }
        Task ObtenerListaUsuariosDeClienteDeInforme(string consecutivo, string esquema);
        Task AgregarUsuarioDeClienteDeInforme(mUsuariosDeClienteDeInforme usuario, string esquema);
    }
}
