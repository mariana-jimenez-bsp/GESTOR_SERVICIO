using Microsoft.AspNetCore.Components;
using BSP.POS.Presentacion.Models;
namespace BSP.POS.Presentacion.Pages.Usuarios
{
    public partial class Login: ComponentBase
    {

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
            mensaje = string.Empty;
            if (!string.IsNullOrEmpty(inputUsuario) && !string.IsNullOrEmpty(inputClave) && !string.IsNullOrEmpty(inputEsquema))
            {

                usuarioLogin = await UsuariosService.RealizarLogin(inputUsuario, inputClave, inputEsquema);
                if (!string.IsNullOrEmpty(usuarioLogin.token))
                {

                    await localStorageService.SetItemAsync<string>("token", usuarioLogin.token);
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    navigationManager.NavigateTo("/index", forceLoad: true);

                }
                else
                {
                    mensajeError();

                    inputUsuario = string.Empty;
                    inputClave = string.Empty;
                    inputEsquema = string.Empty;
                }

            }
            else
            {
                mensajeError();
                inputUsuario = string.Empty;
                inputClave = string.Empty;
                inputEsquema = string.Empty;
            }
        }

        private string inputUsuario { get; set; } = string.Empty;
        private string inputClave { get; set; } = string.Empty;
        private string inputEsquema { get; set; } = string.Empty;


        private void ValorUsuario(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                inputUsuario = e.Value.ToString();
            }
        }


       

        private void ValorEsquema(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                inputEsquema = e.Value.ToString();
            }
        }

        private void mensajeError()
        { 
          if(!string.IsNullOrEmpty(inputUsuario) && !string.IsNullOrEmpty(inputClave) && !string.IsNullOrEmpty(inputEsquema)){
                mensaje = "Datos inválidos";
            }
            else
            {
                mensaje = "No dejar ningún campo vacío";
            }

            
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
