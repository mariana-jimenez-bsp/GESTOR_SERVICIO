using BSP.POS.Presentacion.Models.Usuarios;

namespace BSP.POS.Presentacion.Interfaces.Usuarios
{
    public interface IUsuariosInterface
    {
       
        string EncriptarClave(string clave);
        string DesencriptarClave(string clave);

        mPerfil Perfil { get; set; }
        Task ObtenerPerfil(string usuario, string esquema);

        Task<bool> ActualizarPefil(mPerfil perfil, string usuarioOriginal, string claveOriginal, string correoOriginal, string esquema);

        Task<List<mUsuariosDeCliente>> ObtenerListaDeUsuariosDeClienteAsociados(string esquema, string cliente);
        List<mUsuariosDeCliente> ListaDeUsuariosDeCliente { get; set; }
        
        Task<string> ValidarCorreoExistente(string esquema, string correo);
        Task<string> ValidarUsuarioExistente(string esquema, string usuario);
        Task<string> ValidarExistenciaDeCodigoUsuario(string esquema, string codigo);
        List<mDatosUsuariosDeClienteDeInforme> ListaDatosUsuariosDeClienteDeInforme { get; set; }
        Task ObtenerDatosListaUsuariosDeClienteDeInforme(string consecutivo, string esquema);
        Task<bool> AgregarUsuarioDeClienteDeInforme(mUsuariosDeInforme usuario, string esquema);
        Task<bool> EliminarUsuarioDeClienteDeInforme(string codigo, string esquema);
        List<mPerfil> ListaDeUsuarios { get; set; }
        Task ObtenerListaDeUsuarios(string esquema);

        mImagenUsuario ImagenDeUsuario { get; set; }
        Task ObtenerImagenDeUsuario(string usuario, string esquema);

        List<mUsuariosDeInforme> ListaUsuariosDeInformeAsociados { get; set; }
        List<mUsuariosDeInforme> ListaUsuariosDeInformeDeCliente { get; set; }
        Task ObtenerListaDeInformesDeUsuario(string codigo, string esquema);
        Task ObtenerListaDeInformesDeUsuarioDeCliente(string cliente, string esquema);

        List<mUsuariosParaEditar> ListaDeUsuariosParaEditar { get; set; }
        Task ObtenerListaDeUsuariosParaEditar(string esquema);
        
        Task<bool> AgregarUsuario(mUsuarioParaAgregar usuario, string esquema);

        mUsuariosParaEditar UsuarioParaEditar { get; set; }
        Task ObtenerElUsuarioParaEditar(string esquema, string codigo);
        Task<bool> ActualizarUsuario(mUsuariosParaEditar usuario, string esquema, string usuarioActual);
        Task<string> ValidarExistenciaEsquema(string esquema); 
    }
}
