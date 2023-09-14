using Microsoft.AspNetCore.Components;
namespace BSP.POS.Presentacion.Pages.Home
{
    public partial class ConsecutivoInforme: ComponentBase
    {
        [Parameter]
        public string consecutivo { get; set; } = string.Empty;
        [Parameter]
        public string estado { get; set; } = string.Empty;

        public string Consecutivo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string esquema = string.Empty;

        protected async override Task OnParametersSetAsync()
        {



                var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authenticationState.User;
                esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
                Consecutivo = consecutivo;
                Estado = estado;
            
        }

        private void IrAEditar()
        {

            navigationManager.NavigateTo($"Informe/Editar/{Consecutivo}");
        }

        private void VerInforme()
        {

            navigationManager.NavigateTo($"Informe/VerInforme/{Consecutivo}");
        }

        bool activarModalFinalizarInforme = false;
        void ClickHandlerFinalizarInforme(bool activar)
        {
            activarModalFinalizarInforme = activar;
            StateHasChanged();
        }
    }
}
