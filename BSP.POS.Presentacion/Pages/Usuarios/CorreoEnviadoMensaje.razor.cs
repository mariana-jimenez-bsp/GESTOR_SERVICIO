using BSP.POS.Presentacion.Models.Licencias;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Usuarios
{
    public partial class CorreoEnviadoMensaje: ComponentBase
    {

       
        private void IrAlInicio()
        {

            navigationManager.NavigateTo($"", forceLoad: true);
        }

        private void IrAEnviarCorreo()
        {

            navigationManager.NavigateTo($"CorreoRecuperacion", forceLoad: true);
        }
    }
}
