using Microsoft.AspNetCore.Components;
using BSP.POS.Presentacion.Models.Usuarios;

namespace BSP.POS.Presentacion.Pages.Usuarios
{
    public partial class Login: ComponentBase
    {

        private string mensaje { get; set; } = string.Empty;
        public mLogin usuarioLogin { get; set; } = new mLogin();
        public mLogin usuario { get; set; } = new mLogin();
        protected override void OnParametersSet()
        {


            if (usuarioLogin != null)
            {
                UsuariosService.UsuarioLogin = usuarioLogin;

            }
        }
        private async Task Ingresar()
        {
            mensaje = string.Empty;

                usuarioLogin = await UsuariosService.RealizarLogin(usuario);
                if (!string.IsNullOrEmpty(usuarioLogin.token))
                {

                    await localStorageService.SetItemAsync<string>("token", usuarioLogin.token);
                    await localStorageService.SetItemAsync<string>("esquema", usuario.esquema);
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    navigationManager.NavigateTo("/index", forceLoad: true);

                }
                else
                {
                    mensajeError();

                    usuario.correo = string.Empty;
                    usuario.clave = string.Empty;
                    usuario.esquema = string.Empty;
                }

            

        }


        private void ValorCorreo(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.correo = e.Value.ToString();
            }
        }


       

        private void ValorEsquema(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.esquema = e.Value.ToString();
            }
        }

        private void mensajeError()
        { 
          if(!string.IsNullOrEmpty(usuario.correo) && !string.IsNullOrEmpty(usuario.clave) && !string.IsNullOrEmpty(usuario.esquema)){
                mensaje = "Datos inválidos";
            }

            
        }




        private void ValorClave(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.clave = e.Value.ToString();
            }
        }

        private void CorreoRecuperacion()
        {

            navigationManager.NavigateTo($"CorreoRecuperacion");
        }
    }
}
