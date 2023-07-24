using Microsoft.AspNetCore.Components;
using BSP.POS.Presentacion.Models;
namespace BSP.POS.Presentacion.Pages.Modals
{
    public partial class ModalPerfil : ComponentBase
    {


        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public mPerfil perfil { get; set; } = new mPerfil();
        public string tipo { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {

            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            Usuario = user.Identity.Name;
            await UsuariosService.ObtenerPerfil(Usuario);

            perfil = UsuariosService.Perfil;



        }

        private void CambioUsuario(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                perfil.usuario = e.Value.ToString();
            }
        }
        private void CambioClave(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                perfil.clave = e.Value.ToString();
            }
        }
        private void CambioCorreo(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                perfil.correo = e.Value.ToString();
            }
        }
        private void CambioRol(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                perfil.rol = e.Value.ToString();
            }
        }
        private void CambioNombreEmpresa(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                perfil.nombre = e.Value.ToString();
            }
        }

        private async Task ActualizarPerfil()
        {

            await UsuariosService.ActualizarPefil(perfil);
            await UsuariosService.ObtenerPerfil(perfil.usuario);
            if (UsuariosService.Perfil != null)
            {
                perfil = UsuariosService.Perfil;
            }
            await CloseModal();
        }
        private void OpenModal()
        {
            ActivarModal = true;
        }

        private async Task CloseModal()
        {
            await UsuariosService.ObtenerPerfil(Usuario);
            perfil = UsuariosService.Perfil;
            await OnClose.InvokeAsync(false);

        }

    }
}
