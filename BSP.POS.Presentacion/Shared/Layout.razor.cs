using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Shared
{
    public partial class Layout: ComponentBase
    {
        private string inputValue { get; set; } = string.Empty;
        [Parameter]
        public EventCallback<string> Texto { get; set; }
        [Parameter]
        public EventCallback<bool> ModalClientesEstado { get; set; }
        public string UsuarioActual { get; set; } = string.Empty;
        public mImagenUsuario imagenDeUsuario = new mImagenUsuario();
        public string esquema = string.Empty;
        public string rol = string.Empty;
        List<string> permisos;

        protected override async Task OnInitializedAsync()
        {

            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            permisos = user.Claims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();

            


            UsuarioActual = user.Identity.Name;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            if (!string.IsNullOrEmpty(UsuarioActual) && !string.IsNullOrEmpty(esquema))
            {
                await UsuariosService.ObtenerImagenDeUsuario(UsuarioActual, esquema);
                if(UsuariosService.ImagenDeUsuario != null)
                {
                    imagenDeUsuario = UsuariosService.ImagenDeUsuario;
                }
            }
        }
        private async Task ActualizarValor(ChangeEventArgs e)
        {
            if (e.Value.ToString() != null)
            {
                inputValue = e.Value.ToString();
                await EnviarTextoABuscar(inputValue);
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
        bool activarModalActividades = false;

        void ClickHandlerPefil(bool activar)
        {
            activarModalPerfil = activar;
            StateHasChanged();
        }


        async Task ClickHandlerClientes(bool activar)
        {
            activarModalClientes = activar;
            if(activarModalClientes == false)
            {
                await ModalClientesEstado.InvokeAsync(true);
            }
            StateHasChanged();
        }

        void ClickHandlerActividades(bool activar)
        {
            activarModalActividades = activar;
            StateHasChanged();
        }
        private void IrAlInicio()
        {

            navigationManager.NavigateTo($"Index", forceLoad: true);
        }
        private void IrAMisInformes()
        {

            navigationManager.NavigateTo($"Informes/MisInformes", forceLoad: true);
        }

        private void IrAProyectos()
        {

            navigationManager.NavigateTo($"proyectos", forceLoad: true);
        }

        private void IrAConfiguraciones()
        {

            navigationManager.NavigateTo($"Configuraciones", forceLoad: true);
        }

        private async Task CerrarSesion()
        {
            await localStorageService.RemoveItemAsync("token");
            navigationManager.NavigateTo($"login", forceLoad: true);
        }
    }
}
