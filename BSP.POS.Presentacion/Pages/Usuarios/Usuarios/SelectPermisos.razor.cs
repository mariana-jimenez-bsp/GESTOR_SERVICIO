using BSP.POS.Presentacion.Models.Permisos;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BSP.POS.Presentacion.Pages.Usuarios.Usuarios
{
    public partial class SelectPermisos : ComponentBase
    {
        public List<mPermisos> listaDePermisos { get; set; } = new List<mPermisos>();
        public List<mDatosPermisosDeUsuarios> listaDePermisosDeUsuario { get; set; } = new List<mDatosPermisosDeUsuarios>();
        public List<mSubPermisos> listaDeSubPermisos { get; set; } = new List<mSubPermisos>();
        public List<mDatosSubPermisosDeUsuarios> listaDeSubPermisosDeUsuario { get; set; } = new List<mDatosSubPermisosDeUsuarios>();    
        
        [Parameter] public string esquema { get; set; } = string.Empty;
        [Parameter] public string codigo { get; set; } = string.Empty;
        private List<string> permisosCambiados = new List<string>();
        private bool eventoCambioPermiso = false;
        private bool cargaInicial = false;
        protected override async Task OnInitializedAsync()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await PermisosService.ObtenerLaListaDePermisos(esquema);
            listaDePermisos = PermisosService.ListaDePermisos;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await PermisosService.ObtenerLaListaDeSubPermisos(esquema);
            listaDeSubPermisos = PermisosService.ListaDeSubPermisos;
            if (!string.IsNullOrEmpty(codigo))
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await PermisosService.ObtenerLaListaDePermisosDeUsuario(esquema, codigo);
                listaDePermisosDeUsuario = PermisosService.ListaDePermisosDeUsuario;
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await PermisosService.ObtenerLaListaDeSubPermisosDeUsuario(esquema, codigo);
                listaDeSubPermisosDeUsuario = PermisosService.ListaDeSubPermisosDeUsuario;
            }
            
            cargaInicial = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                string jsonData = "";
                if (!string.IsNullOrEmpty(codigo))
                {
                    Dictionary<string, List<string>> permisosAActivar = new Dictionary<string, List<string>>();
                    if (listaDePermisosDeUsuario.Any() && listaDeSubPermisosDeUsuario.Any())
                    {
                        foreach (var permiso in listaDePermisosDeUsuario)
                        {
                            List<string> subPermisosAActivar = new List<string>();
                            foreach (var subPermiso in listaDeSubPermisosDeUsuario)
                            {
                                if (subPermiso.id_permiso_usuario == permiso.Id)
                                {
                                    subPermisosAActivar.Add(permiso.id_permiso + "-" + subPermiso.id_sub_permiso);
                                }
                            }
                            permisosAActivar.Add(permiso.id_permiso, subPermisosAActivar);
                        }
                        jsonData = JsonSerializer.Serialize(permisosAActivar);

                    }
                }
                
                DotNetObjectReference<SelectPermisos> objRef = DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync("ActivarSelectMultiplePermisos", jsonData, objRef);


            }
            catch (Exception ex)
            {

                string error = ex.ToString();
                Console.WriteLine(error);
            }
        }

        [JSInvokable]
        public void CambioDePermisos(string[] permisosSeleccionados)
        {
            permisosCambiados = permisosSeleccionados.ToList();
            eventoCambioPermiso = true;
            
        }

        public async Task<int> ActualizarListaDePermisos(string codigoEnviado)
        {
            string codigoUsuario = "";
            if (!string.IsNullOrEmpty(codigoEnviado))
            {
                codigoUsuario = codigoEnviado;
            }
            else
            {
                codigoUsuario = codigo;
            }
            bool resultado = false;

            if (eventoCambioPermiso)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                resultado = await PermisosService.ActualizarListaPermisosDeUsuario(permisosCambiados, codigoUsuario, esquema);

            }
            else
            {
                resultado = true;
            }
            if (resultado)
            {
                if (!string.IsNullOrEmpty(codigoEnviado))
                {
                    return 1;
                }else if (eventoCambioPermiso)
                {
                    return 2;
                }
                else {
                    return 1;
                }
            }
            else
            {
                return 0;
            }
           
        }
    }
}
