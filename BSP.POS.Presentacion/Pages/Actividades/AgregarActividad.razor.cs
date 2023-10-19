using BSP.POS.Presentacion.Models.Actividades;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Actividades
{
    public partial class AgregarActividad: ComponentBase
    {
        
        public string esquema = string.Empty;
        public mActividades activadNueva = new mActividades();

        public string mensajeError;
        private bool cargaInicial = false;
        List<string> permisos;

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            permisos = user.Claims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();
            cargaInicial = true;
        }

        
       

        private void CambioActividad(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                activadNueva.Actividad = e.Value.ToString();

            }

        }
        private void CambioCIReferencia(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                activadNueva.CI_referencia = e.Value.ToString();

            }

        }
        private void CambioHoras(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                activadNueva.horas = e.Value.ToString();

            }

        }
        private async Task DescartarCambios()
        {
            await SwalAviso("Se han cancelado los cambios");
        }
        private async Task AgregarActividadNueva()
        {
            mensajeError = null;
            bool resultadoActividad = false;
            try
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                resultadoActividad = await ActividadesService.AgregarActividad(activadNueva, esquema);
                if (resultadoActividad)
                {
                    await SwalExito("La actividad se ha agregado");
                }
                else
                {
                    mensajeError = "Ocurrío un Error vuelva a intentarlo";
                }
                
            }
            catch (Exception)
            {

                mensajeError = "Ocurrío un Error vuelva a intentarlo";
            }

        }

        private void IrAActividades()
        {
            navigationManager.NavigateTo($"actividades");
        }

        private async Task SwalExito(string mensajeAlerta)
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
                    IrAActividades();
                }
            });
        }

        private async Task SwalAviso(string mensajeAlerta)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Aviso!",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Info,
                ShowCancelButton = false,
                ConfirmButtonText = "Ok"
            }).ContinueWith(swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (result.IsConfirmed || result.IsDismissed)
                {
                    IrAActividades();
                }
            });
        }
    }
}
