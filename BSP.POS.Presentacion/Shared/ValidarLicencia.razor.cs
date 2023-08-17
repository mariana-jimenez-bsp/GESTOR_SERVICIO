using BSP.POS.Presentacion.Models.Licencias;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Shared
{
    public partial class ValidarLicencia: ComponentBase
    {
        public mLicencia licencia = new mLicencia();
        public string mensaje;
        protected override async Task OnInitializedAsync()
        {
            await LicenciasService.ObtenerEstadoDeLicencia();
            if(LicenciasService.licencia != null)
            {
                licencia = LicenciasService.licencia;
                if(licencia.estado == "Proximo")
                {
                    mensaje = "La licencia está proxima a vencer";
                }
            }

        }
    }
}
