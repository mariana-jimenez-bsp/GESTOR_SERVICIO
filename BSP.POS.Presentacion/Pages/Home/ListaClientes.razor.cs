using BSP.POS.Presentacion.Models;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Home
{
    public partial class ListaClientes : ComponentBase
    {
        public List<mInformes> InformesAsociados = new List<mInformes>();
        public mClienteAsociado ClienteAsociado = new mClienteAsociado();
        public string usuarioActual { get; set; } = string.Empty;
        protected override async Task OnInitializedAsync()
        {

            await ClientesService.ObtenerListaClientes();
            await ClientesService.ObtenerListaClientesRecientes();
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            usuarioActual = user.Identity.Name;

        }
        private string activeTab = "recent"; // Pestaña activa inicialmente

        private void ChangeTab(string tabId)
        {
            activeTab = tabId;
        }

        private string GetTabLinkClass(string tabId)
        {
            return activeTab == tabId ? "active" : "";
        }

        private async Task EnviarInformesAsociados(string cliente)
        {
            await InformesService.ObtenerListaDeInformesAsociados(cliente);
            if (await ClientesService.ObtenerClienteAsociado(cliente) != null)
            {
                ClienteAsociado = await ClientesService.ObtenerClienteAsociado(cliente);
            }

            if (InformesService.ListaInformesAsociados != null)
            {
                InformesAsociados = InformesService.ListaInformesAsociados;
            }
        }

        [Parameter]
        public string textoRecibido { get; set; } = string.Empty;

        private Task RecibirTexto(string texto)
        {
            textoRecibido = texto;
            return Task.CompletedTask;
        }
    }
}
