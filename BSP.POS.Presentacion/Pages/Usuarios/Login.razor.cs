using Microsoft.AspNetCore.Components;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.JSInterop;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using BSP.POS.Presentacion.Models.Licencias;
using BSP.POS.Presentacion.Pages.Usuarios.Usuarios;

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
        public mLicenciaByte licenciaByte = new mLicenciaByte();
        private ElementReference LlaveInputFile;
        private ElementReference LlaveButton;
        public mDatosLicencia licencia = new mDatosLicencia();
        private bool cargaInicial = false;
        private bool licenciaActiva = false;
        private bool licenciaProximaAVencer = false;
        private bool mismaMacAdress = true;
        private bool archivoLicenciaValido = false;
        private bool archivoLicenciaInvalido = false;

        protected override async Task OnInitializedAsync()
        {
            usuario.esquema = "BSP";
            await ValidarLicencia();
        }
        protected override void OnParametersSet()
        {


            if (usuarioLogin != null)
            {
                LoginService.UsuarioLogin = usuarioLogin;

            }
        }
        private async Task ValidarLicencia()
        {
             licenciaActiva = false;
             licenciaProximaAVencer = false;
             mismaMacAdress = true;
             archivoLicenciaValido = false;
             archivoLicenciaInvalido = false;
            await LicenciasService.ObtenerDatosDeLicencia();
            if (LicenciasService.licencia != null)
            {
                licencia = LicenciasService.licencia;
                if (licencia.FechaFin > DateTime.Now)
                {
                    licenciaActiva = true;
                    if (licencia.FechaAviso < DateTime.Now)
                    {
                        licenciaProximaAVencer = true;
                    }
                    if (!licencia.MacAddressIguales)
                    {
                        mismaMacAdress = false;
                    }
                }
            }
            cargaInicial = true;
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
                using (var stream = archivoSeleccionado.OpenReadStream())
                using (var reader = new StreamReader(stream))
                {
                    var contenido = await reader.ReadToEndAsync();
                    licenciaLlave.texto_archivo = contenido;
                }
                licenciaLlave.archivo_llave = new FormFile(archivoSeleccionado.OpenReadStream(archivoSeleccionado.Size), 0, archivoSeleccionado.Size, "name", archivoSeleccionado.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = archivoSeleccionado.ContentType
                };
                
                await SubmitLlave();
            }
        }

        private async Task EnviarLLave()
        {
            archivoLicenciaValido = false;
            archivoLicenciaInvalido = false;
                if(!string.IsNullOrEmpty(licenciaLlave.texto_archivo))
                {
                    await LicenciasService.ObtenerCodigoDeLicenciaYProducto();
                    if(LicenciasService.codigoLicenciaYProducto.codigo_licencia != null && LicenciasService.codigoLicenciaYProducto.producto != null)
                    {
                        licenciaByte.codigo_licencia = LicenciasService.codigoLicenciaYProducto.codigo_licencia;
                        licenciaByte.producto = LicenciasService.codigoLicenciaYProducto.producto;
                        licenciaByte.texto_archivo = licenciaLlave.texto_archivo;
                        var datosLicencia = await LoginService.EnviarXMLLicencia(licenciaByte);
                        if (datosLicencia != null)
                        {
                            
                            bool resultadoActualizar = await LicenciasService.ActualizarDatosLicencia(datosLicencia, licenciaByte.codigo_licencia);
                            if (resultadoActualizar)
                            {
                            archivoLicenciaValido = true;
                            StateHasChanged();
                            await Task.Delay(100);
                            if (archivoLicenciaValido)
                            {
                                await ValidarLicencia();
                            }
                        }
                            
                        }
                        else
                        {
                        StateHasChanged();
                        await Task.Delay(100);
                        archivoLicenciaInvalido = true;
                        }
                }

            }
            else
            {
                StateHasChanged();
                await Task.Delay(100);
                archivoLicenciaInvalido = true;
            }
            
        }


        private async Task Ingresar()
        {
            mensajeIntentos = string.Empty;
            mensaje = string.Empty;
            if (licenciaActiva && mismaMacAdress)
            {
                correoExistente = await UsuariosService.ValidarCorreoExistente(usuario.esquema, usuario.correo);
                if (correoExistente != null)
                {
                    intentos = await LoginService.ObtenerIntentosDeLogin(usuario.esquema, usuario.correo);
                    if (intentos >= 3)
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
                            claveActual = string.Empty;
                        }
                    }
                }
                else
                {
                    mensajeError();

                    usuario.correo = string.Empty;
                    usuario.clave = string.Empty;
                    claveActual = string.Empty;
                }
            }
            else
            {
                await ValidarLicencia();
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

        private async Task CorreoRecuperacion()
        {
            if(licenciaActiva && mismaMacAdress)
            {
                navigationManager.NavigateTo($"CorreoRecuperacion");
            }
            else
            {
                await ValidarLicencia();
            }
            
        }

        private bool mostrarClave = false;

        private void CambiarEstadoMostrarClave(bool estado)
        {
            mostrarClave = estado;
        }
    }
}
