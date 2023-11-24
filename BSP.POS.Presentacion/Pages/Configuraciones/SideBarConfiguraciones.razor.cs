using BSP.POS.Presentacion.Models.Permisos;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace BSP.POS.Presentacion.Pages.Configuraciones
{
    public partial class SideBarConfiguraciones : ComponentBase
    {
        List<mObjetoPermiso> permisos = new List<mObjetoPermiso>();
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            var PermisosClaim = user.Claims.FirstOrDefault(c => c.Type == "permisos");
            if (PermisosClaim != null)
            {
                permisos = JsonConvert.DeserializeObject<List<mObjetoPermiso>>(PermisosClaim.Value);
            }
        }
    }
}
