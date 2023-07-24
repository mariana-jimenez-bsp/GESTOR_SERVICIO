using Microsoft.AspNetCore.Components;
using BSP.POS.Presentacion.Models;
namespace BSP.POS.Presentacion.Pages.Usuarios
{
    public partial class Login: ComponentBase
    {
        private string usuario { get; set; } = string.Empty;
        private string contrasena { get; set; } = string.Empty;
        private string valorSesion { get; set; } = string.Empty;
        private string mensaje { get; set; } = string.Empty;
        public mLogin usuarioLogin { get; set; } = new mLogin();
        protected override void OnParametersSet()
        {


            if (usuarioLogin != null)
            {
                UsuariosService.UsuarioLogin = usuarioLogin;

            }
        }
        private async Task Ingresar()
        {
            if (!string.IsNullOrEmpty(inputUsuario) && !string.IsNullOrEmpty(inputClave))
            {

                usuarioLogin = await UsuariosService.RealizarLogin(inputUsuario, inputClave);
                if (!string.IsNullOrEmpty(usuarioLogin.token))
                {

                    await localStorageService.SetItemAsync<string>("token", usuarioLogin.token);
                    navigationManager.NavigateTo("/index", forceLoad: true);

                }
                else
                {
                    mensajeError();
                }

            }
        }

        private string inputUsuario { get; set; } = string.Empty;
        private string inputClave { get; set; } = string.Empty;

        private void ValorUsuario(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                inputUsuario = e.Value.ToString();
            }
        }

        private void mensajeError()
        {
            mensaje = "usuario o contraseña inválidos";
        }




        private void ValorClave(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                inputClave = e.Value.ToString();
            }
        }
    }
}
