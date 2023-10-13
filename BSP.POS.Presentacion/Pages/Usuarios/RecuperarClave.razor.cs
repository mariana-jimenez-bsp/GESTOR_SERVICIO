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
        public mDatosLicencia licencia = new mDatosLicencia();
        private bool licenciaActiva = false;
        private bool licenciaProximaAVencer = false;
        private bool mismaMacAdress = true;
        public string mensajeEsquema;
        public string claveActual = string.Empty;
        public string confirmarClaveActual = string.Empty;
        public bool cargaInicial = false;

        protected override async Task OnInitializedAsync()
        {
            
            if(await VerificarValidezEsquema())
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
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(esquema))
                {
                    tokenRecuperacion = await LoginService.ValidarTokenRecuperacion(esquema, token);
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
            await LoginService.ActualizarClaveDeUsuario(usuario);
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

        private bool mostrarClave = false;

        private void CambiarEstadoMostrarClave(bool estado)
        {
            mostrarClave = estado;
        }

        private bool mostrarConfirmarClave = false;

        private void CambiarEstadoMostrarConfirmarClave(bool estado)
        {
            mostrarConfirmarClave = estado;
        }
    }
}
