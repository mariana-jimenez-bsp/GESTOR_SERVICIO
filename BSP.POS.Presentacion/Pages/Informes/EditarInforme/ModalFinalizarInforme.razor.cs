using BSP.POS.Presentacion.Models.Informes;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Informes.EditarInforme
{
    public partial class ModalFinalizarInforme: ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public string esquema { get; set; } = string.Empty;
        [Parameter] public string consecutivo { get; set; } = string.Empty;
        public mInformeEstado informeEstado = new mInformeEstado();
        private async Task CloseModal()
        {
            await OnClose.InvokeAsync(false);

        }

        private async Task FinalizarInforme()
        {
            if (!string.IsNullOrEmpty(consecutivo) && !string.IsNullOrEmpty(esquema))
            {
                informeEstado.consecutivo = consecutivo;
                informeEstado.estado = "Finalizado";
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await InformesService.CambiarEstadoDeInforme(informeEstado, esquema);
                await CloseModal();
                navigationManager.NavigateTo("/Informe/VerInforme/" + consecutivo, forceLoad: true);
            }
        }
    }
}
