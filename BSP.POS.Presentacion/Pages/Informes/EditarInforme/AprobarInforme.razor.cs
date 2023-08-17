using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Licencias;
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
        public mLicencia licencia = new mLicencia();
        public string mensajeLicencia;

        protected override async Task OnInitializedAsync()
        {
            await LicenciasService.ObtenerEstadoDeLicencia();
            if (LicenciasService.licencia != null)
            {
                licencia = LicenciasService.licencia;
                if (licencia.estado == "Proximo")
                {
                    mensajeLicencia = "La licencia está proxima a vencer";
                }
            }
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
