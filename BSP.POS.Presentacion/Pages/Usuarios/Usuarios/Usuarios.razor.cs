using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Licencias;
using BSP.POS.Presentacion.Models.Permisos;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Services.Proyectos;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Usuarios.Usuarios
{
    public partial class Usuarios : ComponentBase
    {
        public string esquema = string.Empty;
        public List<mUsuariosParaEditar> usuarios = new List<mUsuariosParaEditar>();
        public List<mClientes> listaClientes = new List<mClientes>();
        public mDatosLicencia licencia = new mDatosLicencia();
        public bool cargaInicial = false;
        public string rol = string.Empty;
        List<mObjetoPermiso> permisos = new List<mObjetoPermiso>();

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            var PermisosClaim = user.Claims.FirstOrDefault(c => c.Type == "permisos");
            if (PermisosClaim != null)
            {
                permisos = JsonConvert.DeserializeObject<List<mObjetoPermiso>>(PermisosClaim.Value);
            }
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await RefrescarListaDeUsuarios();
            await LicenciasService.ObtenerDatosDeLicencia();
            if (LicenciasService.licencia != null)
            {
                licencia = LicenciasService.licencia;

            }
                cargaInicial = true;


        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            // Inicializa los tooltips de Bootstrap
            try
            {
                await JSRuntime.InvokeVoidAsync("initTooltips");
            }
            catch (Exception ex)
            {

                string error = ex.ToString();
                Console.WriteLine(error);
            }


        }


        [Parameter]
        public string textoRecibido { get; set; } = string.Empty;

        private Task RecibirTexto(string texto)
        {
            textoRecibido = texto;
            return Task.CompletedTask;
        }
        private async Task RefrescarListaDeUsuarios()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerListaDeUsuariosParaEditar(esquema);
            if (UsuariosService.ListaDeUsuariosParaEditar != null)
            {
                usuarios = UsuariosService.ListaDeUsuariosParaEditar;
            }
        }
        
        
        private async Task MensajeLimiteUsuarios()
        {
            await AlertasService.SwalAdvertencia("Límite de Cantidad de Usuarios Alcanzado");  
        }

       

        private async Task SwalEliminarUsuario(string mensajeAlerta, string codigoUsuario)
        {
            if (permisos.Any(p => p.permiso == "Usuarios" && !p.subpermisos.Contains("Eliminar")))
            {
                await AlertasService.SwalAdvertencia("No tienes permisos para eliminar usuarios");
            }
            else
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = mensajeAlerta,
                    Icon = SweetAlertIcon.Question,
                    ShowCancelButton = true,
                    ConfirmButtonText = "Eliminar",
                    CancelButtonText = "Cancelar"
                }).ContinueWith(async swalTask =>
                {
                    SweetAlertResult result = swalTask.Result;
                    if (result.IsConfirmed)
                    {
                        bool resultadoEliminar = await EliminarUsuario(codigoUsuario);
                        if (resultadoEliminar)
                        {
                            await AlertasService.SwalExito("Se ha eliminado el usuario con el código " + codigoUsuario);
                            await RefrescarListaDeUsuarios();
                            StateHasChanged();
                        }
                        else
                        {
                            await AlertasService.SwalError("Ocurrió un error vuelva a intentarlo");
                        }
                    }
                });
            }

        }

        private async Task<bool> EliminarUsuario(string codigo)
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            bool resultadoEliminar = await UsuariosService.EliminarUsuario(esquema, codigo);
            return resultadoEliminar;
        }
    }
}
