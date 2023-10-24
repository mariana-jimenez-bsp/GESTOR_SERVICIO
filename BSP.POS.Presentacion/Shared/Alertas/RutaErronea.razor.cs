using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

namespace BSP.POS.Presentacion.Shared.Alertas
{
    public partial class RutaErronea : ComponentBase
    {
        [Parameter]
        public string mensaje { get; set; } = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            await ActivarAlertaRutaErronea();
        }
        private async Task ActivarAlertaRutaErronea()
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Error!",
                Text = mensaje,
                Icon = SweetAlertIcon.Error,
                ShowCancelButton = false,
                ConfirmButtonText = "Ok"
            }).ContinueWith(async swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (result.IsConfirmed || result.IsDismissed)
                {
                    await IrAtras();
                }
            });
        }

        private async Task IrAtras()
        {

            await JSRuntime.InvokeVoidAsync("history.back");
        }
    }
}
