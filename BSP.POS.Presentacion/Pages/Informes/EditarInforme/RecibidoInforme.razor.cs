using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Licencias;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Informes.EditarInforme
{
    public partial class RecibidoInforme: ComponentBase
    {
        [Parameter]
        public string token { get; set; } = string.Empty;

        [Parameter]
        public string esquema { get; set; } = string.Empty;
        public bool recibido = false;
        public bool terminaCarga = false;
        public mTokenRecibidoInforme tokenRecibido = new mTokenRecibidoInforme();
        public mDatosLicencia licencia = new mDatosLicencia();
        private bool licenciaActiva = false;
        private bool licenciaProximaAVencer = false;
        private bool mismaMacAdress = true;
        public string mensajeEsquema;
        public bool cargaInicial = false;
        protected override async Task OnInitializedAsync()
        {
            licenciaActiva = true;
            if (await VerificarValidezEsquema())
            {

                await LicenciasService.ObtenerDatosDeLicencia();
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
                            mismaMacAdress = true;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(esquema) && licenciaActiva && mismaMacAdress)
                {
                    tokenRecibido = await InformesService.ValidarTokenRecibidoDeInforme(esquema, token);
                    if (tokenRecibido.token_recibido != null)
                    {
                        bool resultadoInforme = await InformesService.ActivarRecibidoInforme(tokenRecibido, esquema);
                        if (resultadoInforme)
                        {
                            recibido = true;
                        }
                        else
                        {
                            mensajeEsquema = "Ocurrio un error vuelva a intentarlo";
                        }

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
