using BSP.POS.Presentacion.Models.Licencias;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Usuarios
{
    public partial class RecuperarClave : ComponentBase
    {

        [Parameter]
        public string token { get; set; } = string.Empty;

        [Parameter]
        public string esquema { get; set; } = string.Empty;
        public mTokenRecuperacion tokenRecuperacion = new mTokenRecuperacion();
        public mUsuarioNuevaClave usuario = new mUsuarioNuevaClave();
        public mLicencia licencia = new mLicencia();
        public string mensajeLicencia;
        public string mensajeEsquema;
        public string claveActual = string.Empty;
        public string confirmarClaveActual = string.Empty;
        public bool cargaInicial = false;

        protected override async Task OnInitializedAsync()
        {
            
            if(await VerificarValidezEsquema())
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
                    tokenRecuperacion = await UsuariosService.ValidarTokenRecuperacion(esquema, token);
                }
            }
            else
            {
                mensajeEsquema = "El esquema no existe";
            }

            cargaInicial = true;
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
        private async Task ActualizarClaveUsuario()
        {
            usuario.token_recuperacion = token;
            usuario.esquema = esquema;
            await UsuariosService.ActualizarClaveDeUsuario(usuario);
        }

        private void ValorClave(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.clave = e.Value.ToString();
            }
        }

        private void ValorClaveConfirmacion(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.confirmarClave = e.Value.ToString();
            }
        }
    }
}
