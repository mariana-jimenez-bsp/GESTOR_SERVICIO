using BSP.POS.Presentacion.Models.Informes;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
namespace BSP.POS.Presentacion.Pages.Home
{
    public partial class ConsecutivoInforme: ComponentBase
    {
        [Parameter]
        public string consecutivo { get; set; } = string.Empty;
        [Parameter]
        public string estado { get; set; } = string.Empty;

        [Parameter] public EventCallback<bool> RefrescarListaInformes { get; set; }
        public string esquema = string.Empty;
        public string mensajeError = string.Empty;

        protected async override Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
        }

       
       
       
        private async Task FinalizarInforme(string consecutivo)
        {
            mensajeError = null;
            if (!string.IsNullOrEmpty(consecutivo) && !string.IsNullOrEmpty(esquema))
            {
                mInformeEstado informeEstado = new mInformeEstado();
                informeEstado.consecutivo = consecutivo;
                informeEstado.estado = "Finalizado";
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                bool resultadoEstado = await InformesService.CambiarEstadoDeInforme(informeEstado, esquema);
                if (resultadoEstado)
                {
                    await RefrescarListaInformes.InvokeAsync(true);
                    await SwalAviso("El informe ha sido finalizado", "Finalizar");
                }
                else
                {
                    mensajeError = "Ocurrió un error vuelva a intentarlo";
                }
                
            }
        }

        private async Task SwalAdvertencia(string mensajeAlerta, string accion, string identificador)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Advertencia!",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Aceptar",
                CancelButtonText = "Cancelar"
            }).ContinueWith(async swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (result.IsConfirmed)
                {
                    if (accion == "Finalizar")
                    {
                        await FinalizarInforme(identificador);
                    }
                }
            });
        }

        private async Task SwalAviso(string mensajeAlerta, string accion)
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
                    
                }
            });
        }
    }
}
