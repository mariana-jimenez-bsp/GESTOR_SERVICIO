using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.ItemsCliente;
using BSP.POS.Presentacion.Models.Permisos;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Services.Alertas;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Proyectos
{
    public partial class ProyectosTerminados: ComponentBase
    {
        public string esquema = string.Empty;
        public List<mProyectos> proyectos = new List<mProyectos>();
        public List<mItemsCliente> listaCentrosDeCosto = new List<mItemsCliente>();
        public List<mClientes> listaDeClientes = new List<mClientes>();
        public bool cargaInicial = false;
        public string rol = string.Empty;
        public List<mUsuariosParaEditar> listaUsuariosConsultores = new List<mUsuariosParaEditar>();
        List<mObjetoPermiso> permisos = new List<mObjetoPermiso>();

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            var PermisosClaim = user.Claims.FirstOrDefault(c => c.Type == "permisos");
            if (PermisosClaim != null)
            {
                permisos = JsonConvert.DeserializeObject<List<mObjetoPermiso>>(PermisosClaim.Value);
            }
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ClientesService.ObtenerListaClientes(esquema);
            if (ClientesService.ListaClientes != null)
            {
                listaDeClientes = ClientesService.ListaClientes;
            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerListaDeUsuariosConsultores(esquema);
            if (UsuariosService.ListaDeUsuariosConsultores != null)
            {
                listaUsuariosConsultores = UsuariosService.ListaDeUsuariosConsultores;
            }

            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ItemsClienteService.ObtenerListaDeCentrosDeCosto(esquema);
            
            if (ItemsClienteService.listaCentrosDeCosto != null)
            {
                listaCentrosDeCosto = ItemsClienteService.listaCentrosDeCosto;
            }
            await RefrescarListaDeProyectos();
            cargaInicial = true;
        }

        private async Task RefrescarListaDeProyectos()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ProyectosService.ObtenerListaDeProyectosTerminadosYCancelados(esquema);
            if (ProyectosService.ListaProyectosTerminadosYCancelados != null)
            {
                proyectos = ProyectosService.ListaProyectosTerminadosYCancelados;

            }
            foreach (var proyecto in proyectos)
            {
                string nombreResponsable = listaDeClientes.Where(c => c.CLIENTE == proyecto.codigo_cliente).Select(c => c.CONTACTO).First();
                if (nombreResponsable != null)
                {
                    proyecto.nombre_responsable = nombreResponsable;
                }
                string nombreConsultor = listaUsuariosConsultores.Where(u => u.codigo == proyecto.codigo_consultor).Select(c => c.nombre).First();
                if (nombreConsultor != null)
                {
                    proyecto.nombre_consultor = nombreConsultor;
                }
                string nombreCliente = listaDeClientes.Where(c => c.CLIENTE == proyecto.codigo_cliente).Select(c => c.NOMBRE).First();
                if (nombreCliente != null)
                {
                    proyecto.nombre_cliente = nombreCliente;
                }
                string descripcionCentroCosto = listaCentrosDeCosto.Where(c => c.valor == proyecto.centro_costo).Select(c => c.descripcion).First();
                if (descripcionCentroCosto != null)
                {
                    proyecto.descripcion_centro_costo = descripcionCentroCosto;
                }
            }
        }
        [Parameter]
        public string textoRecibido { get; set; } = string.Empty;

        private Task RecibirTexto(string texto)
        {
            textoRecibido = texto;
            return Task.CompletedTask;
        }


        private async Task SwalEliminarProyecto(string mensajeAlerta, string numeroProyecto)
        {
            if (permisos.Any(p => p.permiso == "Proyectos" && !p.subpermisos.Contains("Eliminar")))
            {
                await AlertasService.SwalAdvertencia("No tienes permisos para eliminar proyectos");
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
                        bool resultadoEliminar = await EliminarProyecto(numeroProyecto);
                        if (resultadoEliminar)
                        {
                            await AlertasService.SwalExito("Se ha eliminado el proyecto #" + numeroProyecto);
                            await RefrescarListaDeProyectos();
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

        private async Task<bool> EliminarProyecto(string numero)
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            bool resultadoEliminar = await ProyectosService.EliminarProyecto(esquema, numero);
            return resultadoEliminar;
        }
    }
}
