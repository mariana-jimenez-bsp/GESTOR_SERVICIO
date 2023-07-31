using Microsoft.AspNetCore.Components;
using System.Security.Claims;

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
            string rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            var permisos = user.Claims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();
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
        bool activarModalClientes = false;
        bool activarModalProyectos = false;
        bool activarModalActividades = false;

        void ClickHandlerPefil(bool activar)
        {
            activarModalPerfil = activar;
            StateHasChanged();
        }


        void ClickHandlerClientes(bool activar)
        {
            activarModalClientes = activar;
            StateHasChanged();
        }

        void ClickHandlerProyectos(bool activar)
        {
            activarModalProyectos = activar;
            StateHasChanged();
        }
        void ClickHandlerActividades(bool activar)
        {
            activarModalActividades = activar;
            StateHasChanged();
        }
        private void IrAlInicio()
        {

            navigationManager.NavigateTo($"Index");
        }
    }
}
