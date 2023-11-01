using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Shared
{
    public partial class Layout: ComponentBase
    {
        private string inputValue { get; set; } = string.Empty;
        private string filtroValue { get; set; } = "clientes";
        [Parameter]
        public EventCallback<string> Texto { get; set; }
        [Parameter]
        public EventCallback<string> Filtro { get; set; }
        [Parameter]
        public EventCallback<DateTime> ActualizaFechaInicio { get; set; }
        [Parameter]
        public EventCallback<DateTime> ActualizaFechaFin { get; set; }
        [Parameter]
        public DateTime fechaInicioDateTime { get; set; } = DateTime.MinValue;
        [Parameter]
        public DateTime fechaFinalDateTime { get; set; } = DateTime.MinValue;
        public string UsuarioActual { get; set; } = string.Empty;
        public mImagenUsuario imagenDeUsuario = new mImagenUsuario();
        public string esquema = string.Empty;
        public string rol = string.Empty;
        
        List<string> permisos;
        private bool estadoPerfilActualizado = false;
        private bool estadoPerfilDescartado = false;

        
        private string fechaInicio = string.Empty;
        private string fechaFinal = string.Empty;

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

        private async Task ActualizarFiltro(ChangeEventArgs e)
        {
            if (e.Value.ToString() != null)
            {
                filtroValue = e.Value.ToString();
                await Filtro.InvokeAsync(filtroValue);
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

        void ClickHandlerPefil(bool activar)
        {
            if (activar)
            {
                estadoPerfilActualizado = false;
                estadoPerfilDescartado = false;
            }
            activarModalPerfil = activar;
            StateHasChanged();
        }

        private void IrAlInicio()
        {
            navigationManager.NavigateTo($"Index");
        }
        private void IrAMisInformes()
        {
            navigationManager.NavigateTo($"Informes/MisInformes");
        }

        private void IrAProyectos()
        {
            navigationManager.NavigateTo($"proyectos");
        }

        private void IrAClientes()
        {
            navigationManager.NavigateTo($"clientes");
        }

        private void IrAActividades()
        {

            navigationManager.NavigateTo($"actividades");
        }

        private void IrAConfiguraciones()
        {

            navigationManager.NavigateTo($"Configuraciones");
        }

        private async Task IrAtras()
        {
            await JSRuntime.InvokeVoidAsync("history.back");
        }

        private async Task CerrarSesion()
        {
            await localStorageService.RemoveItemAsync("token");
            navigationManager.NavigateTo($"login", forceLoad: true);
        }

        private void CambiarEstadoPerfilActualizado(bool estado)
        {
            estadoPerfilActualizado = estado;
        }

        private void CambiarEstadoPerfilDescartado(bool estado)
        {
            estadoPerfilDescartado = estado;
        }
        private async Task CambioFechaInicio(ChangeEventArgs e)
        {
            fechaInicio = e.Value.ToString();
            if (!string.IsNullOrEmpty(fechaInicio))
            {
                fechaInicioDateTime = DateTime.ParseExact(fechaInicio, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (fechaInicioDateTime > fechaFinalDateTime)
                {
                    fechaFinal = fechaInicio;
                    fechaFinalDateTime = fechaInicioDateTime;
                }
                await ActualizaFechaInicio.InvokeAsync(fechaInicioDateTime);
            }
        }

        private async Task CambioFechaFinal(ChangeEventArgs e)
        {
            fechaFinal = e.Value.ToString();
            if (!string.IsNullOrEmpty(fechaFinal))
            {
                fechaFinalDateTime = DateTime.ParseExact(fechaFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (fechaFinalDateTime < fechaInicioDateTime)
                {
                    fechaInicio = fechaFinal;
                    fechaInicioDateTime = fechaFinalDateTime;
                }
                await ActualizaFechaFin.InvokeAsync(fechaFinalDateTime);
            }
        }

    }
}
