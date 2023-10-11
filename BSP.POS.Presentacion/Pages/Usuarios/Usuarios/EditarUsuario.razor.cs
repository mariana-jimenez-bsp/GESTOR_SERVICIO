using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Licencias;
using BSP.POS.Presentacion.Models.Permisos;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Usuarios.Usuarios
{
    public partial class EditarUsuario: ComponentBase
    {
        [Parameter] public string codigo { get; set; } = string.Empty;
        [Parameter] public string codigoCliente { get; set; } = string.Empty;
        public string esquema = string.Empty;
        public mUsuariosParaEditar usuario = new mUsuariosParaEditar();
        public List<mPermisos> todosLosPermisos { get; set; } = new List<mPermisos>();
        public List<mPermisosAsociados> permisosAsociados { get; set; } = new List<mPermisosAsociados>();
        public List<mClientes> listaClientes = new List<mClientes>();
        public string usuarioActual = string.Empty;
        bool repetido = false;
        public string rol = string.Empty;
        public string correoRepite = string.Empty;
        public string mensajeCorreoRepite = string.Empty;
        public string usuarioRepite = string.Empty;
        public string mensajeUsuarioRepite = string.Empty;
        public string mensajeError;
        private bool usuarioActualizado = false;
        private bool descartarCambios = false;
        private bool cargarInicial = false;

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerElUsuarioParaEditar(esquema, codigo);
            if (UsuariosService.UsuarioParaEditar != null)
            {
                usuario = UsuariosService.UsuarioParaEditar;
                await RefrescarPermisos();
            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ClientesService.ObtenerListaClientes(esquema);
            if (ClientesService.ListaClientes != null)
            {
                listaClientes = ClientesService.ListaClientes;
            }
            if (usuarioActual == usuario.usuario)
            {
                usuario.claveOriginal = usuario.clave;
            }
            usuario.usuarioOrignal = usuario.usuario;
            usuario.correoOriginal = usuario.correo;
            usuario.clave = null;
            cargarInicial = true;
        }

        private async Task RefrescarPermisos()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await PermisosService.ObtenerListaDePermisos(esquema);
            usuario.listaTodosLosPermisos = PermisosService.ListaPermisos;
            PermisosService.ListaPermisosAsociadados = new List<mPermisosAsociados>();
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await PermisosService.ObtenerListaDePermisosAsociados(esquema, usuario.id);
            if (PermisosService.ListaPermisosAsociadados != null)
            {
                usuario.listaPermisosAsociados = PermisosService.ListaPermisosAsociadados;
                foreach (var item in usuario.listaTodosLosPermisos)
                {
                    if (usuario.listaPermisosAsociados.Any(elPermiso => elPermiso.id_permiso == item.Id))
                    {
                        item.EstadoCheck = true;
                    }
                }
            }
            
        }
        private void CambioCliente(ChangeEventArgs e, string usuarioId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
                usuario.cod_cliente = e.Value.ToString();
        }


        private void CambioUsuario(ChangeEventArgs e, string usuarioId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.usuario = e.Value.ToString();
            }
        }

        private void CambioCorreo(ChangeEventArgs e, string usuarioId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.correo = e.Value.ToString();
            }
        }

        private void CambioClave(ChangeEventArgs e, string usuarioId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.clave = e.Value.ToString();
            }
        }

        private void CambioRol(ChangeEventArgs e, string usuarioId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.rol = e.Value.ToString();
            }
        }
        private void CambioEsquema(ChangeEventArgs e, string usuarioId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.esquema = e.Value.ToString();
            }
        }

        private void CambioNombre(ChangeEventArgs e, string usuarioId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.nombre = e.Value.ToString();
            }
        }

        private void CambioTelefono(ChangeEventArgs e, string usuarioId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.telefono = e.Value.ToString();
            }
        }


        private void CambioDepartamento(ChangeEventArgs e, string usuarioId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.departamento = e.Value.ToString();
            }
        }
        private async Task CambioImagen(InputFileChangeEventArgs e)
        {
            var archivo = e.File;

            if (archivo != null)
            {
                usuario.ImagenFile = new FormFile(archivo.OpenReadStream(archivo.Size), 0, archivo.Size, "name", archivo.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = archivo.ContentType
                };
                using (var memoryStream = new MemoryStream())
                {
                    await archivo.OpenReadStream().CopyToAsync(memoryStream);
                    byte[] bytes = memoryStream.ToArray();
                    usuario.imagen = bytes;

                }
            }

        }
        private void HandleCheckCambiado(ChangeEventArgs e, string idPermiso)
        {
            var permiso = new mPermisos();
            foreach (var item in usuario.listaTodosLosPermisos)
            {
                if (item.Id == idPermiso)
                {
                    item.EstadoCheck = (bool)e.Value;
                    permiso = item;
                }
            }


            var permisoEncontrado = usuario.listaPermisosAsociados.FirstOrDefault(p => p.id_permiso == permiso.Id);

            if (permiso.EstadoCheck)
            {
                if (permisoEncontrado == null)
                {
                    mPermisosAsociados permisoParaAñadir = new mPermisosAsociados();
                    permisoParaAñadir.id_permiso = permiso.Id;
                    usuario.listaPermisosAsociados.Add(permisoParaAñadir);

                }
            }
            else
            {
                if (permisoEncontrado != null)
                {
                    usuario.listaPermisosAsociados.Remove(permisoEncontrado);

                }
            }
        }
        private async Task VerificarCorreoYUsuarioExistente()
        {

            if (usuario.usuarioOrignal != usuario.usuario)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                usuario.usuarioRepite = await UsuariosService.ValidarUsuarioExistente(usuario.esquema, usuario.usuario);

                if (!string.IsNullOrEmpty(usuario.usuarioRepite))
                {
                    mensajeUsuarioRepite = "El usuario ya existe";

                    repetido = true;
                }
                else
                {
                    mensajeUsuarioRepite = null;
                }

            }
            else
            {
                mensajeUsuarioRepite = null;
            }
            if (usuario.correoOriginal != usuario.correo)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                usuario.correoRepite = await UsuariosService.ValidarCorreoExistente(usuario.esquema, usuario.correo);
                if (!string.IsNullOrEmpty(usuario.correoRepite))
                {
                    mensajeCorreoRepite = "El correo ya existe";

                    repetido = true;
                }
                else
                {
                    mensajeCorreoRepite = null;
                }
            }
            else
            {
                mensajeCorreoRepite = null;
            }

        }

        private async Task DescartarCambios()
        {
            descartarCambios = false;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerElUsuarioParaEditar(esquema, codigo);
            if (UsuariosService.UsuarioParaEditar != null)
            {
                usuario = UsuariosService.UsuarioParaEditar;
                await RefrescarPermisos();
            }
            StateHasChanged();
            await Task.Delay(100);
            descartarCambios = true;
        }

        private async Task ActualizarListaDePermisos()
        {

            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await PermisosService.ObtenerListaDePermisosAsociados(usuario.esquema, usuario.id);
            bool sonIguales = PermisosService.ListaPermisosAsociadados.Count == usuario.listaPermisosAsociados.Count && PermisosService.ListaPermisosAsociadados.All(usuario.listaPermisosAsociados.Contains);
            if (!sonIguales)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await PermisosService.ActualizarListaPermisosAsociados(usuario.listaPermisosAsociados, usuario.id, usuario.esquema);

            }

        }
        private async Task ActualizarUsuario()
        {
            mensajeError = null;
            usuarioActualizado = false;
            try
            {
                repetido = false;
                await VerificarCorreoYUsuarioExistente();
                if (!repetido)
                {

                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await UsuariosService.ActualizarUsuario(usuario, esquema, usuarioActual);
                    await ActualizarListaDePermisos();
                    usuarioActualizado = true;
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await UsuariosService.ObtenerElUsuarioParaEditar(esquema, codigo);
                    if (UsuariosService.UsuarioParaEditar != null)
                    {
                        usuario = UsuariosService.UsuarioParaEditar;
                        await RefrescarPermisos();
                    }
                }
                else
                {
                    await ActivarScrollBarErroresRepite();
                }
            }
            catch (Exception)
            {

                mensajeError = "Ocurrío un Error vuelva a intentarlo";
            }

        }


        private bool mostrarClave = false;

        private void CambiarEstadoMostrarClave(bool estado)
        {
            mostrarClave = estado;
        }

        private async Task ActivarScrollBarDeErrores()
        {
            mensajeUsuarioRepite = null;
            mensajeCorreoRepite = null;
            StateHasChanged();
            await Task.Delay(100);
            var isValid = await JSRuntime.InvokeAsync<bool>("HayErroresValidacion", ".validation-message");

            if (!isValid)
            {
                // Si hay errores de validación, activa el scrollbar
                await JSRuntime.InvokeVoidAsync("ActivarScrollViewValidacion", ".validation-message");
            }

        }

        private async Task ActivarScrollBarErroresRepite()
        {
            if (!string.IsNullOrEmpty(mensajeCorreoRepite) || !string.IsNullOrEmpty(mensajeUsuarioRepite))
            {
                StateHasChanged();
                await Task.Delay(100);


                var isValid = await JSRuntime.InvokeAsync<bool>("HayErroresValidacion", ".mensaje-repite");


                // Si hay errores de validación, activa el scrollbar
                await JSRuntime.InvokeVoidAsync("ActivarScrollViewValidacion", ".mensaje-repite");

            }
        }

        private void IrAUsuarios()
        {
            navigationManager.NavigateTo($"configuraciones/usuarios");
        }
    }
}
