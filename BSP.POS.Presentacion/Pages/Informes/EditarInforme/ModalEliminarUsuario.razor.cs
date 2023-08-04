using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Informes.EditarInforme
{
    public partial class ModalEliminarUsuario: ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public string esquema { get; set; } = string.Empty;
        [Parameter] public string idUsuario { get; set; } = string.Empty;
        [Parameter] public string nombreUsuario { get; set; } = string.Empty;


        private async Task CloseModal()
        {
            await OnClose.InvokeAsync(false);

        }

        private async Task EliminarUsuarioDeClienteDeInforme()
        {
            if (!string.IsNullOrEmpty(idUsuario) && !string.IsNullOrEmpty(esquema))
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await UsuariosService.EliminarUsuarioDeClienteDeInforme(idUsuario, esquema);
                await CloseModal();
            }
        }
    }
}
