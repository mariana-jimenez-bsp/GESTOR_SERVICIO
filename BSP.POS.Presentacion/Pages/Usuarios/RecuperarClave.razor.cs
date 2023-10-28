using BSP.POS.Presentacion.Models.Licencias;
using BSP.POS.Presentacion.Models.Usuarios;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Usuarios
{
    public partial class RecuperarClave : ComponentBase
    {

        [Parameter]
        public string token { get; set; } = string.Empty;

        [Parameter]
        public string esquema { get; set; } = string.Empty;
        public mTokenRecuperacion tokenRecuperacion = new mTokenRecuperacion();
        public mUsuarioNuevaClave usuario = new mUsuarioNuevaClave();
        public mDatosLicencia licencia = new mDatosLicencia();
        private bool licenciaActiva = false;
        private bool mismaMacAdress = true;
        public string mensajeEsquema;
        public string claveActual = string.Empty;
        public string confirmarClaveActual = string.Empty;
        public bool cargaInicial = false;

        protected override async Task OnInitializedAsync()
        {
            
            if(await VerificarValidezEsquema())
            {

                await ValidarLicencia();
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(esquema))
                {
                    tokenRecuperacion = await LoginService.ValidarTokenRecuperacion(esquema, token);
                }
            }
            else
            {
                mensajeEsquema = "El esquema no existe";
            }

            cargaInicial = true;
        }

        private async Task ValidarLicencia()
        {
            licenciaActiva = false;
            mismaMacAdress = true;
            await LicenciasService.ObtenerDatosDeLicencia();
            if (LicenciasService.licencia != null)
            {
                licencia = LicenciasService.licencia;
                if (licencia.FechaFin > DateTime.Now)
                {
                    licenciaActiva = true;
                    StateHasChanged();
                    if (licencia.FechaAviso < DateTime.Now)
                    {
                        await AlertasService.SwalAdvertencia("Licencia Próxima a vencer");
                    }
                    if (!licencia.MacAddressIguales)
                    {
                        mismaMacAdress = false;
                        StateHasChanged();
                        await AlertasService.SwalError("La MacAddress no es la misma registrada");
                    }
                }
                else
                {
                    await AlertasService.SwalError("Licencia no activa, debe renovarla");
                }
            }
        }
        private async Task<bool> VerificarValidezEsquema()
        {
            if (esquema.Length > 10)
            {
                return false;
            }
            string esquemaVerficado = await UsuariosService.ValidarExistenciaEsquema(esquema);
            if (!string.IsNullOrEmpty(esquemaVerficado))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task ActualizarClaveUsuario()
        {
            bool resultado = false;
            if(mismaMacAdress && licenciaActiva)
            {
                usuario.token_recuperacion = token;
                usuario.esquema = esquema;
                resultado = await LoginService.ActualizarClaveDeUsuario(usuario);
                if (resultado)
                {
                    await SwalExitoCambio("La clave se ha actualizado");
                }
            }
            else
            {
                await ValidarLicencia();
            }
        }

        private void ValorClave(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.clave = e.Value.ToString();
            }
        }

        private void ValorClaveConfirmacion(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.confirmarClave = e.Value.ToString();
            }
        }

        private bool mostrarClave = false;

        private void CambiarEstadoMostrarClave(bool estado)
        {
            mostrarClave = estado;
        }

        private bool mostrarConfirmarClave = false;

        private void CambiarEstadoMostrarConfirmarClave(bool estado)
        {
            mostrarConfirmarClave = estado;
        }

        private async Task SwalExitoCambio(string mensajeAlerta)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Éxito",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Success,
                ShowCancelButton = false,
                ConfirmButtonText = "Ok"
            }).ContinueWith(swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (result.IsConfirmed || result.IsDismissed)
                {
                    navigationManager.NavigateTo($"");
                }
            });
        }
    }
}
