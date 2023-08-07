using BSP.POS.Presentacion.Models.Informes;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Informes.EditarInforme
{
    public partial class AprobarInforme: ComponentBase
    {
        [Parameter]
        public string token { get; set; } = string.Empty;

        [Parameter]
        public string esquema { get; set; } = string.Empty;
        public bool aprobado = false;
        public bool terminaCarga = false;
        public mTokenAprobacionInforme tokenAprobacion = new mTokenAprobacionInforme();
        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(esquema))
            {
                tokenAprobacion = await InformesService.ValidarTokenAprobacionDeInforme(esquema, token);
                if(tokenAprobacion.token_aprobacion != null)
                {
                    await InformesService.AprobarInforme(tokenAprobacion, esquema);
                    aprobado = true;
                }
            }
            terminaCarga = true;

        }
    }
}
