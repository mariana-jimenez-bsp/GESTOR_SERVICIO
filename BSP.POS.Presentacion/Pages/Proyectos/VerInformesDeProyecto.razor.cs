using BSP.POS.Presentacion.Models.Informes;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Proyectos
{
    public partial class VerInformesDeProyecto : ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public string Numero_Proyecto { get; set; } = string.Empty;
        public List<mInformesDeProyecto> InformesDeProyecto { get; set; } = new List<mInformesDeProyecto>();
        public string esquema = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await InformesService.ObtenerListaDeInformesDeProyecto(Numero_Proyecto, esquema);
            InformesDeProyecto = InformesService.ListaInformesDeProyecto;
        }

        private async Task SalirConLaX()
        {
            await OnClose.InvokeAsync(false);
        }

        private async Task Cerrar()
        {
            await CloseModal();
        }

        private async Task CloseModal()
        {
            await OnClose.InvokeAsync(false);

        }

        private void IrAEditar(string consecutivo)
        {

            navigationManager.NavigateTo($"Informe/Editar/{consecutivo}");
        }

        private void VerInforme(string consecutivo)
        {

            navigationManager.NavigateTo($"Informe/VerInforme/{consecutivo}");
        }
    }
}
