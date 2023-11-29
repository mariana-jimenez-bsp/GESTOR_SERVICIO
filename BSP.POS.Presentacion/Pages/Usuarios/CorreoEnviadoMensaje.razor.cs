using BSP.POS.Presentacion.Models.Licencias;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Usuarios
{
    public partial class CorreoEnviadoMensaje: ComponentBase
    {

       private void IrACorreoRecuperacion()
       {
            navigationManager.NavigateTo("CorreoRecuperacion", forceLoad: true);
       }

    }
}
