using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.ItemsCliente;
using BSP.POS.Presentacion.Models.Proyectos;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Actividades
{
    public partial class ModalAgregarActividad:ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        public string esquema = string.Empty;
        public mActividades activadNueva = new mActividades();
        
        public string mensajeError;
        [Parameter] public EventCallback<bool> actividadAgregada { get; set; }
        [Parameter] public EventCallback<bool> actividadCancelada { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
        }

        private void OpenModal()
        {
            ActivarModal = true;
        }
        private async Task CancelarCambios()
        {
            await actividadCancelada.InvokeAsync(true);
            await CloseModal();
        }
        private async Task CloseModal()
        {
            activadNueva = new mActividades();
            await OnClose.InvokeAsync(false);

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

        private async Task AgregarActividad()
        {
            mensajeError = null;
            try
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ActividadesService.AgregarActividad(activadNueva, esquema);
                await actividadAgregada.InvokeAsync(true);
                await CloseModal();
            }
            catch (Exception)
            {

                mensajeError = "Ocurrío un Error vuelva a intentarlo";
            }

        }
        private async Task SalirConLaX()
        {
            await OnClose.InvokeAsync(false);
        }
    }
}
