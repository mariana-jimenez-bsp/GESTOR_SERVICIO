using Microsoft.AspNetCore.Components;
namespace BSP.POS.Presentacion.Pages.Home
{
    public partial class ConsecutivoInforme: ComponentBase
    {
        [Parameter]
        public string consecutivo { get; set; } = string.Empty;
        [Parameter]
        public string estado { get; set; } = string.Empty;

       
        public string esquema = string.Empty;

        protected async override Task OnParametersSetAsync()
        {



                var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authenticationState.User;
                esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();

            
        }

        private void IrAEditar()
        {

            navigationManager.NavigateTo($"Informe/Editar/{consecutivo}");
        }

        private void VerInforme()
        {

            navigationManager.NavigateTo($"Informe/VerInforme/{consecutivo}");
        }

        bool activarModalFinalizarInforme = false;
        void ClickHandlerFinalizarInforme(bool activar)
        {
            activarModalFinalizarInforme = activar;
            StateHasChanged();
        }
    }
}
