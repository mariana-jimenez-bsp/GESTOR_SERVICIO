using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Usuarios;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Actividades
{
    public partial class AgregarActividad: ComponentBase
    {
        
        public string esquema = string.Empty;
        public mActividades activadNueva = new mActividades();
        public List<mUsuariosParaEditar> listaUsuarios = new List<mUsuariosParaEditar>();
        public mPerfil perfilActual = new mPerfil();
        private bool cargaInicial = false;
        public string usuarioActual = string.Empty;
        public string rol = string.Empty;
        List<string> permisos;

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            permisos = user.Claims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();
            usuarioActual = user.Identity.Name;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerListaDeUsuariosParaEditar(esquema);
            if (UsuariosService.ListaDeUsuariosParaEditar != null)
            {
                listaUsuarios = UsuariosService.ListaDeUsuariosParaEditar;
            }
            if (rol != "Admin")
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await UsuariosService.ObtenerPerfil(usuarioActual, esquema);
                if (UsuariosService.Perfil != null)
                {
                    perfilActual = UsuariosService.Perfil;
                    activadNueva.codigo_usuario = perfilActual.codigo;
                }
            }
            
            cargaInicial = true;
        }

        private void CambioCodigoUsuario(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()) && rol == "Admin")
            {

                activadNueva.codigo_usuario = e.Value.ToString();

            }

        }


        private void CambioActividad(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                activadNueva.Actividad = e.Value.ToString();

            }

        }
        private void CambioCIReferencia(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                activadNueva.CI_referencia = e.Value.ToString();

            }

        }
        private void CambioHoras(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                activadNueva.horas = e.Value.ToString();

            }

        }
        private async Task DescartarCambios()
        {
            await AlertasService.SwalAvisoCancelado("Se han cancelado los cambios");
        }
        private async Task AgregarActividadNueva()
        {
            try
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                bool resultadoActividad = await ActividadesService.AgregarActividad(activadNueva, esquema);
                if (resultadoActividad)
                {
                    await AlertasService.SwalExitoHecho("La actividad se ha agregado");
                }
                else
                {
                    await AlertasService.SwalError("Ocurrío un Error vuelva a intentarlo");
                }
                
            }
            catch (Exception)
            {

                await AlertasService.SwalError("Ocurrío un Error vuelva a intentarlo");
            }

        }
    }
}
