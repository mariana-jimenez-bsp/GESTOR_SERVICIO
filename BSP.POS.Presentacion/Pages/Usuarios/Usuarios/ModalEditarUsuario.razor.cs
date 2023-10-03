using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Permisos;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Services.Clientes;
using BSP.POS.Presentacion.Services.Permisos;
using BSP.POS.Presentacion.Services.Proyectos;
using BSP.POS.Presentacion.Services.Usuarios;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BSP.POS.Presentacion.Pages.Usuarios.Usuarios
{
    public partial class ModalEditarUsuario:ComponentBase
    {
        public string usuarioActual = string.Empty;
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter]
        public string codigo { get; set; } = string.Empty;
        public mUsuariosParaEditar usuario { get; set; } = new mUsuariosParaEditar();
        public string esquema = string.Empty;
        public List<mPermisos> todosLosPermisos { get; set; } = new List<mPermisos>();
        public List<mPermisosAsociados> permisosAsociados { get; set; } = new List<mPermisosAsociados>();
        public List<mClientes> listaClientes = new List<mClientes>();
        bool repetido = false;
        public string correoRepite = string.Empty;
        public string mensajeCorreoRepite = string.Empty;
        public string usuarioRepite = string.Empty;
        public string mensajeUsuarioRepite = string.Empty;
        public string mensajeError;
        [Parameter] public EventCallback<bool> usuarioActualizado { get; set; }
        [Parameter] public EventCallback<bool> usuarioCancelado { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(codigo))
            {
                var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authenticationState.User;
                usuarioActual = user.Identity.Name;
                esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
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
                usuario.clave = string.Empty;
            }
            
        }

        private void OpenModal()
        {
            ActivarModal = true;
        }
        private async Task CancelarCambios()
        {
            await usuarioCancelado.InvokeAsync(true);
            usuario = new mUsuariosParaEditar();
            await CloseModal();
        }
        private async Task CloseModal()
        {
            
            await OnClose.InvokeAsync(false);

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
        private async Task VerificarCorreoYUsuarioExistente()
        {

            usuario.mensajeUsuarioRepite = null;
            usuario.mensajeCorreoRepite = null;

            if (usuario.usuarioOrignal != usuario.usuario)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                usuario.usuarioRepite = await UsuariosService.ValidarUsuarioExistente(usuario.esquema, usuario.usuario);

                if (!string.IsNullOrEmpty(usuario.usuarioRepite))
                {
                    usuario.mensajeUsuarioRepite = "El usuario ya existe";
                    repetido = true;
                }

            }
            else if (usuario.correoOriginal != usuario.correo)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                usuario.correoRepite = await UsuariosService.ValidarCorreoExistente(usuario.esquema, usuario.correo);
                if (!string.IsNullOrEmpty(usuario.correoRepite))
                {
                    usuario.mensajeCorreoRepite = "El correo ya existe";
                    repetido = true;
                }
            }

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
            try
            {
                repetido = false;
                await VerificarCorreoYUsuarioExistente();
                if (!repetido)
                {

                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await UsuariosService.ActualizarUsuario(usuario, esquema, usuarioActual);
                    await ActualizarListaDePermisos();
                    await usuarioActualizado.InvokeAsync(true);
                    usuario = new mUsuariosParaEditar();
                    await CloseModal();

                }
            }
            catch (Exception)
            {

                mensajeError = "Ocurrío un Error vuelva a intentarlo";
            }
            
        }

        private async Task SalirConLaX()
        {
            await OnClose.InvokeAsync(false);
        }
        private bool mostrarClave = false;

        private void CambiarEstadoMostrarClave(bool estado)
        {
            mostrarClave = estado;
        }
    }
}
