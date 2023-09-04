using BSP.POS.Presentacion.Models.Licencias;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Usuarios
{
    public partial class CorreoEnviadoMensaje: ComponentBase
    {
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

        }
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
