using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Licencias;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Informes.EditarInforme
{
    public partial class RechazarInforme : ComponentBase
    {
        [Parameter]
        public string token { get; set; } = string.Empty;

        [Parameter]
        public string esquema { get; set; } = string.Empty;
        public bool rechazado = false;
        public bool terminaCarga = false;
        public mTokenAprobacionInforme tokenAprobacion = new mTokenAprobacionInforme();
        public mLicencia licencia = new mLicencia();
        public string mensajeLicencia;
        public string mensajeEsquema;
        public bool cargaInicial = false;
        protected override async Task OnInitializedAsync()
        {

            if (await VerificarValidezEsquema())
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
                    if (tokenAprobacion.token_aprobacion != null)
                    {
                        await InformesService.RechazarInforme(tokenAprobacion, esquema);
                        rechazado = true;
                    }
                }
            }
            else
            {
                mensajeEsquema = "El esquema no existe";
            }
            terminaCarga = true;

        }

        private async Task<bool> VerificarValidezEsquema()
        {
            if (esquema.Length > 10)
            {
                return false;
            }
            string esquemaVerficado = await UsuariosService.ValidarExistenciaEsquema(esquema);
            if (!string.IsNullOrEmpty(esquemaVerficado))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
