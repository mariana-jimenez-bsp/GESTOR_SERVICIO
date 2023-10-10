using Microsoft.AspNetCore.Components;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.JSInterop;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using BSP.POS.Presentacion.Models.Licencias;

namespace BSP.POS.Presentacion.Pages.Usuarios
{
    public partial class Login: ComponentBase
    {

        private string mensaje { get; set; } = string.Empty;
        private string correoExistente { get; set; } = string.Empty;
        private string claveActual { get; set; } = string.Empty;
        private int intentos = 0;
        private string mensajeIntentos { get; set; } = string.Empty;
        public mLogin usuarioLogin { get; set; } = new mLogin();
        public mLogin usuario { get; set; } = new mLogin();
        public mLicenciaLlave licenciaLlave = new mLicenciaLlave();
        private ElementReference LlaveInputFile;
        private ElementReference LlaveButton;
        private string llaveSeccionada = string.Empty;
        protected override void OnParametersSet()
        {


            if (usuarioLogin != null)
            {
                LoginService.UsuarioLogin = usuarioLogin;

            }
        }

        private async Task ActivarInputFileLLave()
        {
            await JS.InvokeVoidAsync("clickInput", LlaveInputFile);
        }

        private async Task SubmitLlave()
        {
            await JS.InvokeVoidAsync("clickButton", LlaveButton);
        }
        private async void CambiarArchivoLlave(InputFileChangeEventArgs e)
        {
            // Aquí puedes manejar el archivo seleccionado.
            var archivoSeleccionado = e.File;
            
            if (archivoSeleccionado != null)
            {
                licenciaLlave.archivo_llave = new FormFile(archivoSeleccionado.OpenReadStream(archivoSeleccionado.Size), 0, archivoSeleccionado.Size, "name", archivoSeleccionado.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = archivoSeleccionado.ContentType
                };
                await SubmitLlave();
            }
        }

        private void EnviarLLave()
        {
            navigationManager.NavigateTo("https://www.google.com/");
        }


        private async Task Ingresar()
        {
            mensajeIntentos = string.Empty;
            mensaje = string.Empty;
            correoExistente = await UsuariosService.ValidarCorreoExistente(usuario.esquema, usuario.correo);
            if(correoExistente != null)
            {
                intentos = await LoginService.ObtenerIntentosDeLogin(usuario.esquema, usuario.correo);
                if(intentos >= 3)
                {
                    mensajeIntentos = "Se excedió el limite de intentos, oprima la opción recuperar contraseña";
                }
                else
                {
                    usuarioLogin = await LoginService.RealizarLogin(usuario);

                    if (!string.IsNullOrEmpty(usuarioLogin.token))
                    {

                        await localStorageService.SetItemAsync<string>("token", usuarioLogin.token);
                        await localStorageService.SetItemAsync<string>("esquema", usuario.esquema);
                        await AuthenticationStateProvider.GetAuthenticationStateAsync();
                        navigationManager.NavigateTo($"index", forceLoad: true);

                    }
                    else
                    {
                        await LoginService.AumentarIntentosDeLogin(usuario.esquema, usuario.correo);
                        mensajeError();

                        usuario.correo = string.Empty;
                        usuario.clave = string.Empty;
                        usuario.esquema = string.Empty;
                        claveActual = string.Empty;
                    }
                }
            }
            else
            {
                mensajeError();

                usuario.correo = string.Empty;
                usuario.clave = string.Empty;
                usuario.esquema = string.Empty;
                claveActual = string.Empty;
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
            if(!string.IsNullOrEmpty(correoExistente))
            {
                mensaje = "Contraseña Incorrecta";
            }else
            {
                mensaje = "El correo no existe en el esquema";
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

        private bool mostrarClave = false;

        private void CambiarEstadoMostrarClave(bool estado)
        {
            mostrarClave = estado;
        }
    }
}
