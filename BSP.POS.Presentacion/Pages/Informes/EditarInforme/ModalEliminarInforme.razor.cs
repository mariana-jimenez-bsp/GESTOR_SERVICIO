using BSP.POS.Presentacion.Services.Actividades;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Informes.EditarInforme
{
    public partial class ModalEliminarInforme: ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public string esquema { get; set; } = string.Empty;
        [Parameter] public string consecutivo { get; set; } = string.Empty;
        public string mensajeError;
        private async Task CloseModal()
        {
            await OnClose.InvokeAsync(false);

        }

        private async Task EliminarInforme()
        {
            mensajeError = null;
            try
            {
                if (!string.IsNullOrEmpty(consecutivo) && !string.IsNullOrEmpty(esquema))
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await InformesService.EliminarInforme(consecutivo, esquema);
                    await CloseModal();
                    navigationManager.NavigateTo($"index", forceLoad: true);
                }
            }
            catch (Exception)
            {

                mensajeError = "Ocurrió un Error vuelva a intentarlo";
            }
            
        }
    }
}
