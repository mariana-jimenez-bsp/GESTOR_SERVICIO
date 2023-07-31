using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Usuarios
{
    public partial class RecuperarClave : ComponentBase
    {

        [Parameter]
        public string token { get; set; } = string.Empty;

        [Parameter]
        public string esquema { get; set; } = string.Empty;
        public mTokenRecuperacion tokenRecuperacion = new mTokenRecuperacion();
        public mUsuarioNuevaClave usuario = new mUsuarioNuevaClave();

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(esquema))
            {
                tokenRecuperacion = await UsuariosService.ValidarTokenRecuperacion(esquema, token);
            }

        }

        private async Task ActualizarClaveUsuario()
        {
            usuario.token_recuperacion = token;
            usuario.esquema = esquema;
            await UsuariosService.ActualizarClaveDeUsuario(usuario);
        }

        private void ValorClave(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.clave = e.Value.ToString();
            }
        }

        private void ValorClaveConfirmacion(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.confirmarClave = e.Value.ToString();
            }
        }
    }
}
