using BSP.POS.Presentacion.Services.Usuarios;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Informes.EditarInforme
{
    public partial class ModalEliminarActividad: ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public string esquema { get; set; } = string.Empty;
        [Parameter] public string idActividad { get; set; } = string.Empty;
        [Parameter] public string nombreActividad { get; set; } = string.Empty;

        private async Task CloseModal()
        {
            await OnClose.InvokeAsync(false);

        }

        private async Task EliminarActividadDeInforme()
        {
            if (!string.IsNullOrEmpty(idActividad) && !string.IsNullOrEmpty(esquema))
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ActividadesService.EliminarActividadDeInforme(idActividad, esquema);
                await CloseModal();
            }
        }
    }
}
