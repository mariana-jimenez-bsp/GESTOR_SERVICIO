using BSP.POS.Presentacion.Models.Licencias;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Http;

namespace BSP.POS.Presentacion.Interfaces.Usuarios
{
    public interface ILoginInterface
    {
        mLogin UsuarioLogin { get; set; }
        Task<mLogin> RealizarLogin(mLogin usuarioLog);
        string EncriptarClave(string clave);
        Task<bool> EnviarCorreoRecuperarClave(mTokenRecuperacion tokenRecuperacion);
        mTokenRecuperacion UsuarioRecuperacion { get; set; }
        Task ActualizarClaveDeUsuario(mUsuarioNuevaClave usuario);
        Task<mTokenRecuperacion> ValidarTokenRecuperacion(string esquema, string token);
        Task<string> ValidarCorreoCambioClave(string esquema, string correo);
        Task AumentarIntentosDeLogin(string esquema, string correo);
        Task<int> ObtenerIntentosDeLogin(string esquema, string correo);
        Task<bool> EnviarLlaveLicencia(mLicenciaByte licenciaLlave);
    }
}
