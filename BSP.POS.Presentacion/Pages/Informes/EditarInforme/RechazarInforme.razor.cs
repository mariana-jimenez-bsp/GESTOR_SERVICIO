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
        private bool licenciaActiva = false;
        private bool licenciaProximaAVencer = false;
        private bool mismaMacAdress = true;
        public string mensajeEsquema;
        public bool cargaInicial = false;
        protected override async Task OnInitializedAsync()
        {

            if (await VerificarValidezEsquema())
            {
                
                if (LicenciasService.licencia != null)
                {
                    licencia = LicenciasService.licencia;
                    if (licencia.FechaFin > DateTime.Now)
                    {
                        licenciaActiva = true;
                        if (licencia.FechaAviso < DateTime.Now)
                        {
                            licenciaProximaAVencer = true;
                        }
                        if (!licencia.MacAddressIguales)
                        {
                            mismaMacAdress = false;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(esquema) && licenciaActiva && mismaMacAdress)
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
