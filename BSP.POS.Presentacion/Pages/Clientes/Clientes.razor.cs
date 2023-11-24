using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Departamentos;
using BSP.POS.Presentacion.Models.ItemsCliente;
using BSP.POS.Presentacion.Models.Licencias;
using BSP.POS.Presentacion.Models.Permisos;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Services.Usuarios;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Clientes
{
    public partial class Clientes: ComponentBase
    {
        public List<mClientes> clientes = new List<mClientes>();
        public mDatosLicencia licencia = new mDatosLicencia();
        public List<mUsuariosParaEditar> usuarios = new List<mUsuariosParaEditar>();
        public List<mDepartamentos> listaDepartamentos = new List<mDepartamentos>();
        public string esquema = string.Empty;
        public bool cargaInicial = false;
        public string rol = string.Empty;
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
            await RefrescarListaClientes();
            await LicenciasService.ObtenerDatosDeLicencia();
            if (LicenciasService.licencia != null)
            {
                licencia = LicenciasService.licencia;

            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await DepartamentosService.ObtenerListaDeDepartamentos(esquema);
            if (DepartamentosService.listaDepartamentos != null)
            {
                listaDepartamentos = DepartamentosService.listaDepartamentos;
            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerListaDeUsuariosParaEditar(esquema);
            if (UsuariosService.ListaDeUsuariosParaEditar != null)
            {
                usuarios = UsuariosService.ListaDeUsuariosParaEditar;
            }
            cargaInicial = true;
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

                // Inicializa los tooltips de Bootstrap
                try
                {
                    if (permisos.Any(p => p.permiso == "Clientes" && p.subpermisos.Contains("Ver Lista") && !p.subpermisos.Contains("Editar")))
                    {
                        await JS.InvokeVoidAsync("DesactivarElementos");
                        await AlertasService.SwalAdvertencia("No tienes permisos de edición, solo puedes visualizar");
                    }
                    await JS.InvokeVoidAsync("initTooltips");
                }
                catch (Exception ex)
                {

                    string error = ex.ToString();
                    Console.WriteLine(error);
                }
                
            
        }
        private async Task RefrescarListaClientes()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ClientesService.ObtenerListaClientes(esquema);
            if (ClientesService.ListaClientes != null)
            {
                clientes = ClientesService.ListaClientes;
                // Asegurar que las listas desplegables y cambioColores tengan la misma cantidad de elementos que la lista de clientes

            }
        }

        private async Task ObtenerUsuariosDeCliente(string clienteObtenido)
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            foreach (var cliente in clientes)
            {
                if (cliente.CLIENTE == clienteObtenido)
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    cliente.listaDeUsuarios = await UsuariosService.ObtenerListaDeUsuariosDeClienteAsociados(esquema, cliente.CLIENTE);
                    cliente.listaDeUsuarios = cliente.listaDeUsuarios.OrderBy(c => c.cod_cliente).ToList();
                    foreach (var usuarioCliente in cliente.listaDeUsuarios)
                    {
                        usuarioCliente.nombre_departamento = listaDepartamentos.Where(d => d.codigo == usuarioCliente.codigo_departamento).Select(d => d.Departamento).First();
                    }
                }
            }
        }

        private async Task ToggleCollapse(string clienteCod)
        {
            foreach (var cliente in clientes)
            {
                if (cliente.CLIENTE == clienteCod)
                {
                    cliente.IsOpen = !cliente.IsOpen;
                    if (cliente.IsOpen)
                    {
                        await AuthenticationStateProvider.GetAuthenticationStateAsync();
                        await ObtenerUsuariosDeCliente(clienteCod);
                    }
                }
                else
                {
                    cliente.IsOpen = false;
                    cliente.listaDeUsuarios = new List<mUsuariosDeCliente>();
                }
            }

        }

        private void CambioNombre(ChangeEventArgs e, string clienteCod)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var cliente in clientes)
                {
                    if (cliente.CLIENTE == clienteCod)
                    {
                        cliente.NOMBRE = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioAlias(ChangeEventArgs e, string clienteCod)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var cliente in clientes)
                {
                    if (cliente.CLIENTE == clienteCod)
                    {
                        cliente.ALIAS = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioContribuyente(ChangeEventArgs e, string clienteCod)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var cliente in clientes)
                {
                    if (cliente.CLIENTE == clienteCod)
                    {
                        cliente.CONTRIBUYENTE = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioTelefono1(ChangeEventArgs e, string clienteCod)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var cliente in clientes)
                {
                    if (cliente.CLIENTE == clienteCod)
                    {
                        cliente.TELEFONO1 = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioTelefono2(ChangeEventArgs e, string clienteCod)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var cliente in clientes)
                {
                    if (cliente.CLIENTE == clienteCod)
                    {
                        cliente.TELEFONO2 = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioCorreo(ChangeEventArgs e, string clienteCod)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var cliente in clientes)
                {
                    if (cliente.CLIENTE == clienteCod)
                    {
                        cliente.E_MAIL = e.Value.ToString();
                    }
                }
            }
        }

        private async Task<bool> ActualizarListaClientes()
        {
            try
            {
                bool resultadoClientes = false;
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                resultadoClientes = await ClientesService.ActualizarListaDeClientes(clientes, esquema);
                if(resultadoClientes) {
                    await RefrescarListaClientes();
                    return true;
                }
                else
                {
                    return false;
                }
                
                
               
            }
            catch (Exception)
            {

                return true;
            }

        }
        private bool esElUltimoUsuario(mClientes cliente, mUsuariosDeCliente usuario)
        {
            // Compara el elemento actual con el último elemento de la lista
            return cliente.listaDeUsuarios.IndexOf(usuario) == cliente.listaDeUsuarios.Count - 1;
        }

        private async Task DescartarCambios()
        {
            await AlertasService.SwalAvisoCancelado("Se han Descartado los cambios");
        }
        

        [Parameter]
        public string textoRecibido { get; set; } = string.Empty;

        private Task RecibirTexto(string texto)
        {
            textoRecibido = texto;
            return Task.CompletedTask;
        }

        private async Task InvalidSubmit(EditContext modeloContext)
        {
            await ActivarScrollBarDeErrores();
            var mensajesDeValidaciones = modeloContext.GetValidationMessages();
            string mensaje = mensajesDeValidaciones.First();
            await AlertasService.SwalError(mensaje);
        }
        private async Task ActivarScrollBarDeErrores()
        {
            StateHasChanged();
            await Task.Delay(100);
            var isValid = await JS.InvokeAsync<bool>("HayErroresValidacion", ".validation-message");

            if (!isValid)
            {
                // Si hay errores de validación, activa el scrollbar
                await JS.InvokeVoidAsync("ActivarScrollViewValidacion", ".validation-message");
            }
        }

        private async Task IrAAgregarUsuario(string cliente)
        {
            if (licencia.CantidadUsuarios <= usuarios.Count)
            {
                await AlertasService.SwalAdvertencia("Límite de Cantidad de Usuarios Alcanzado");
            }
            else
            {
                navigationManager.NavigateTo($"configuraciones/usuario/agregar/{cliente}");
            }

        }

        private void IrAEditarUsuario(string codigo, string cliente)
        {
            navigationManager.NavigateTo($"configuraciones/usuario/editar/{codigo}/{cliente}");
        }

        private void IrAAgregarCliente()
        {
            navigationManager.NavigateTo($"cliente/agregar");
        }
        private async Task SwalActualizandoClientes()
        {
            bool resultadoActualizar = false;
            if (permisos.Any(p => p.permiso == "Clientes" && !p.subpermisos.Contains("Editar")))
            {
                await AlertasService.SwalAdvertencia("No tienes permisos de edición, solo puedes visualizar");
            }
            else
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = " <span class=\"spinner-border spinner-border-sm\" aria-hidden=\"true\"></span>\r\n  <span role=\"status\">Actualizando...</span>",
                    ShowCancelButton = false,
                    ShowConfirmButton = false,
                    AllowOutsideClick = false,
                    AllowEscapeKey = false,
                    DidOpen = new SweetAlertCallback(async () =>
                    {
                        resultadoActualizar = await ActualizarListaClientes();
                        await Swal.CloseAsync();

                    }),
                    WillClose = new SweetAlertCallback(Swal.CloseAsync)

                });

                if (resultadoActualizar)
                {
                    await AlertasService.SwalExito("Clientes Actualizados");
                }
                else
                {
                    await AlertasService.SwalError("Ocurrió un error. Vuelva a intentarlo.");
                }
            }
           

        }

        private async Task SwalEliminarCliente(string mensajeAlerta, string cliente)
        {
            if (permisos.Any(p => p.permiso == "Clientes" && !p.subpermisos.Contains("Eliminar")))
            {
                await AlertasService.SwalAdvertencia("No tienes permisos para eliminar clientes");
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
                        bool resultadoEliminar = await EliminarCliente(cliente);
                        if (resultadoEliminar)
                        {
                            await AlertasService.SwalExito("Se ha eliminado el cliente con el código " + cliente);
                            await RefrescarListaClientes();
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

        private async Task<bool> EliminarCliente(string cliente)
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            bool resultadoEliminar = await ClientesService.EliminarCliente(esquema, cliente);
            return resultadoEliminar;
        }
    }
}
