using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Observaciones;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BSP.POS.Presentacion.Pages.Informes.CrearInforme
{
    public partial class CrearInforme: ComponentBase
    {
        [Parameter]
        public string Cliente { get; set; } = string.Empty;
        public string Consecutivo { get; set; } = string.Empty;
        public string esquema = string.Empty;
        private bool cargaInicial = false;
        private string mensajeCliente;
        public string mensajeError;
        protected override async Task OnInitializedAsync()
        {

            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            if(await VerificarValidezCliente()) { 
            if (!string.IsNullOrEmpty(Cliente)) {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                Consecutivo =  await InformesService.AgregarInformeAsociado(Cliente, esquema);
            if (!string.IsNullOrEmpty(Consecutivo))
            {
                 navigationManager.NavigateTo($"Informe/Editar/{Consecutivo}");
            }
            }
            }
            else
            {
                mensajeCliente = "Cliente no existe";
            }
            cargaInicial = true;
        }
        private async Task<bool> VerificarValidezCliente()
        {
            if (Cliente.Length > 20)
            {
                return false;
            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            string clienteVerficado = await ClientesService.ValidarExistenciaDeCliente(esquema, Cliente);
            if (!string.IsNullOrEmpty(clienteVerficado))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
