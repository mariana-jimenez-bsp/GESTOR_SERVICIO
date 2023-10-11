using BSP.POS.Presentacion.Models.Licencias;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Usuarios
{
    public partial class CorreoRecuperacion: ComponentBase
    {
        public mLicencia licencia = new mLicencia();
        private bool cargaInicial = false;
        private bool licenciaActiva = false;
        private bool licenciaProximaAVencer = false;
        private bool mismaMacAdress = true;
        protected override async Task OnInitializedAsync()
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
                        mismaMacAdress = false;
                    }
                }
            }
            cargaInicial = true;

        }
        public mTokenRecuperacion tokenRecuperacion { get; set; } = new mTokenRecuperacion();
        public string mensaje { get; set; } = string.Empty;
        public bool CorreoEnviado = false; 
        private void ValorCorreo(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                tokenRecuperacion.correo = e.Value.ToString();
            }
        }

        private void ValorEsquema(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                tokenRecuperacion.esquema = e.Value.ToString();
            }
        }

        private async Task EnviarCorreo()
        {
            string verificar = await LoginService.ValidarCorreoCambioClave(tokenRecuperacion.esquema, tokenRecuperacion.correo);
            if(verificar != null)
            {
                bool validar = await LoginService.EnviarCorreoRecuperarClave(tokenRecuperacion);
                if (validar)
                {
                    mensaje = string.Empty;
                    CorreoEnviado = true;
                }
              
            }
            else
            {
                mensaje = "El correo no existe";
            }
           
        }
        }
}
