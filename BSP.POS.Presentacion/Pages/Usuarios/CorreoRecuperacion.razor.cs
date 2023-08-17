using BSP.POS.Presentacion.Models.Licencias;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Usuarios
{
    public partial class CorreoRecuperacion: ComponentBase
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
            string verificar = await UsuariosService.ValidarCorreoCambioClave(tokenRecuperacion.esquema, tokenRecuperacion.correo);
            if(verificar != null)
            {
                bool validar = await UsuariosService.EnviarCorreoRecuperarClave(tokenRecuperacion);
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
