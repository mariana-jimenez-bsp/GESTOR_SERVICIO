using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Shared
{
    public partial class Layout: ComponentBase
    {
        private string inputValue { get; set; } = string.Empty;
        [Parameter]
        public EventCallback<string> Texto { get; set; }
        public string UsuarioActual { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {

            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            UsuarioActual = user.Identity.Name;
        }
        private void ActualizarValor(ChangeEventArgs e)
        {
            if (e.Value.ToString() != null)
            {
                inputValue = e.Value.ToString();
                EnviarTextoABuscar(inputValue);
            }
        }

        public async Task EnviarTextoABuscar(string texto)
        {

            if (texto != null)
            {
                await Texto.InvokeAsync(texto);
            }
        }

        bool activarModalPerfil = false;
        bool activarModalTiempos = false;
        bool activarModalClientes = false;

        void ClickHandlerPefil(bool activar)
        {
            activarModalPerfil = activar;
            StateHasChanged();
        }

        void ClickHandlerTiempos(bool activar)
        {
            activarModalTiempos = activar;
            StateHasChanged();
        }

        void ClickHandlerClientes(bool activar)
        {
            activarModalClientes = activar;
            StateHasChanged();
        }
    }
}
