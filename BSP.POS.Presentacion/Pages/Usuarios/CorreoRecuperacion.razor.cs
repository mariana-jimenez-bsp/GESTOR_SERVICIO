using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Usuarios
{
    public partial class CorreoRecuperacion: ComponentBase
    {
        public mTokenRecuperacion tokenRecuperacion { get; set; } = new mTokenRecuperacion();

        private void ValorCorreo(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                tokenRecuperacion.correo = e.Value.ToString();
            }
        }

        private void ValorEsquema(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                tokenRecuperacion.esquema = e.Value.ToString();
            }
        }

        private async Task EnviarCorreo()
        {
            await UsuariosService.EnviarCorreoRecuperarClave(tokenRecuperacion);
        }
        }
}
