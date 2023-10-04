using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Services.Informes;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Proyectos
{
    public partial class ModalTerminarProyecto: ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public EventCallback<bool> proyectoTerminado { get; set; }
        [Parameter] public string esquema { get; set; } = string.Empty;
        [Parameter] public string numero { get; set; } = string.Empty;
        public string mensajeError;
        private async Task CloseModal()
        {
            await OnClose.InvokeAsync(false);

        }
        private async Task TerminarProyecto()
        {
            try
            {
                if (!string.IsNullOrEmpty(numero) && !string.IsNullOrEmpty(esquema))
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await ProyectosService.TerminarProyecto(numero, esquema);
                    await proyectoTerminado.InvokeAsync(true);
                    await CloseModal();
                }
            }
            catch (Exception)
            {

                mensajeError = "Ocurrió un Error vuelva a intentarlo";
            }

        }
    }
}
