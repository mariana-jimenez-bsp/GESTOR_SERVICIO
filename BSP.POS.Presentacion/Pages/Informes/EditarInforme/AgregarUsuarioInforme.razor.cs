using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Services.Alertas;
using BSP.POS.Presentacion.Services.Usuarios;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Informes.EditarInforme
{
    public partial class AgregarUsuarioInforme : ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public string Consecutivo { get; set; } = string.Empty;
        [Parameter]public List<mUsuariosDeCliente> listaDeUsuariosParaAgregar { get; set; } = new List<mUsuariosDeCliente>();
        public mUsuariosDeClienteDeInforme usuarioAAgregar = new mUsuariosDeClienteDeInforme();
        public string esquema = string.Empty;
        private string mensajeError;

        protected override async Task OnInitializedAsync()
        {

            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
        }

        private async Task AgregarUsuarioDeClienteDeInforme()
        {
            mensajeError = null;
            if (usuarioAAgregar.codigo_usuario_cliente != null)
            {
                usuarioAAgregar.consecutivo_informe = Consecutivo;
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                bool resultadoAgregar = await UsuariosService.AgregarUsuarioDeClienteDeInforme(usuarioAAgregar, esquema);
                if (resultadoAgregar)
                {
                    usuarioAAgregar = new mUsuariosDeClienteDeInforme();
                    await OnClose.InvokeAsync(false);
                }
                else
                {
                    mensajeError = "Ocurrió un error vuelva a intentarlo";
                }

            }
        }

        private void CambioCodigoDeUsuario(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuarioAAgregar.codigo_usuario_cliente = e.Value.ToString();

            }
        }

        private async Task SalirConLaX()
        {
            await OnClose.InvokeAsync(false);
        }

        private async Task DescartarCambios()
        {
            await CloseModal();
        }

        private async Task CloseModal()
        {
            await OnClose.InvokeAsync(false);

        }
    }
}
