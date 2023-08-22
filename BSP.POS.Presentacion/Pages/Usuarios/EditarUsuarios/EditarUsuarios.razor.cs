using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Permisos;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Pages.Home;
using BSP.POS.Presentacion.Services.Actividades;
using BSP.POS.Presentacion.Services.Permisos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Usuarios.EditarUsuarios
{
    public partial class EditarUsuarios : ComponentBase
    {
        public string esquema = string.Empty;
        public List<mUsuariosParaEditar> usuarios = new List<mUsuariosParaEditar>();
        public List<mClientes> listaClientes = new List<mClientes>();
        public bool cargaInicial = false;
        public string rol = string.Empty;
        public string usuarioActual = string.Empty;
        public string mensajeAcualizar;
        bool repetido = false;
        protected override async Task OnInitializedAsync()
        {
            cargaInicial = false;
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            usuarioActual = user.Identity.Name;
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await UsuariosService.ObtenerListaDeUsuariosParaEditar(esquema);
            if (UsuariosService.ListaDeUsuariosParaEditar != null)
            {
                foreach (var usuario in UsuariosService.ListaDeUsuariosParaEditar)
                {
                    if(usuarioActual == usuario.usuario)
                    {
                        usuario.claveOriginal = usuario.clave;
                    }
                    usuario.usuarioOrignal = usuario.usuario;
                    usuario.correoOriginal = usuario.correo;
                    usuario.clave = string.Empty;
                }
                usuarios = UsuariosService.ListaDeUsuariosParaEditar;
                await RefrescarPermisos();
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ClientesService.ObtenerListaClientes(esquema);
                if(ClientesService.ListaClientes != null) { 
                listaClientes = ClientesService.ListaClientes;
                }
                cargaInicial = true;
            }

        }

        private async Task RefrescarPermisos()
        {
           
            foreach (var usuario in usuarios)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await PermisosService.ObtenerListaDePermisos(esquema);
                usuario.listaTodosLosPermisos = PermisosService.ListaPermisos;
                PermisosService.ListaPermisosAsociadados = new List<mPermisosAsociados>();
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await PermisosService.ObtenerListaDePermisosAsociados(esquema, usuario.id);
                if(PermisosService.ListaPermisosAsociadados != null)
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
        }


        private void CambioCliente(ChangeEventArgs e, string usuarioId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var usuario in usuarios)
                {
                    if (usuario.id == usuarioId)
                    {
                        usuario.cod_cliente = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioUsuario(ChangeEventArgs e, string usuarioId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var usuario in usuarios)
                {
                    if (usuario.id == usuarioId)
                    {
                        usuario.usuario = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioCorreo(ChangeEventArgs e, string usuarioId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var usuario in usuarios)
                {
                    if (usuario.id == usuarioId)
                    {
                        usuario.correo = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioClave(ChangeEventArgs e, string usuarioId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var usuario in usuarios)
                {
                    if (usuario.id == usuarioId)
                    {
                        usuario.clave = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioRol(ChangeEventArgs e, string usuarioId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var usuario in usuarios)
                {
                    if (usuario.id == usuarioId)
                    {
                        usuario.rol = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioNombre(ChangeEventArgs e, string usuarioId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var usuario in usuarios)
                {
                    if (usuario.id == usuarioId)
                    {
                        usuario.nombre = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioTelefono(ChangeEventArgs e, string usuarioId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var usuario in usuarios)
                {
                    if (usuario.id == usuarioId)
                    {
                        usuario.telefono = e.Value.ToString();
                    }
                }
            }
        }


        private void CambioDepartamento(ChangeEventArgs e, string usuarioId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var usuario in usuarios)
                {
                    if (usuario.id == usuarioId)
                    {
                        usuario.departamento = e.Value.ToString();
                    }
                }
            }
        }
        private async Task CambioImagen(InputFileChangeEventArgs e, string usuarioId)
        {
            var archivo = e.File;

            if (archivo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await archivo.OpenReadStream().CopyToAsync(memoryStream);
                    byte[] bytes = memoryStream.ToArray();

                    foreach (var usuario in usuarios)
                    {
                        if (usuario.id == usuarioId)
                        {
                            usuario.imagen = bytes;
                        }
                    }
                }
            }

        }

        private void HandleCheckCambiado(ChangeEventArgs e, string idPermiso, string idUsuario)
        {
            var permiso = new mPermisos();
            foreach (var usuario in usuarios)
            {
                if(usuario.id == idUsuario)
                {
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
            }

           
        }
        private void VolverAlHome()
        {

            navigationManager.NavigateTo($"Index", forceLoad: true);

        }
        private async Task ValidarUsuarioCorreoYExistente()
        {
            foreach (var usuario in usuarios)
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
                        break;
                    }
                    
                }
                if (usuario.correoOriginal != usuario.correo)
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    usuario.correoRepite = await UsuariosService.ValidarCorreoExistente(usuario.esquema, usuario.correo);
                    if (!string.IsNullOrEmpty(usuario.correoRepite))
                    {
                        usuario.mensajeCorreoRepite = "El correo ya existe";
                        repetido = true;
                        break;
                    }
                }
            }
        }
        private async Task ActualizarListaUsuarios()
        {
            repetido = false;
            await ValidarUsuarioCorreoYExistente();
            if (!repetido)
            {
           
            mensajeAcualizar = null;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ActualizarListaDeUsuarios(usuarios, esquema, usuarioActual);
            await ActualizarListaDePermisos();
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerListaDeUsuariosParaEditar(esquema);
            if (UsuariosService.ListaDeUsuariosParaEditar != null)
            {
                foreach (var usuario in UsuariosService.ListaDeUsuariosParaEditar)
                {
                    if (usuarioActual == usuario.usuario)
                    {
                       
                        usuario.claveOriginal = usuario.clave;
                       
                    }
                    usuario.usuarioOrignal = usuario.usuario;
                    usuario.correoOriginal = usuario.correo;
                    usuario.clave = string.Empty;
                }
                usuarios = UsuariosService.ListaDeUsuariosParaEditar;
            }
            await RefrescarPermisos();
            mensajeAcualizar = "Usuarios Actualizados";
            }
        }
        private async Task ActualizarListaDePermisos()
        {
            foreach (var usuario in usuarios)
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
        }

        [Parameter]
        public string textoRecibido { get; set; } = string.Empty;

        private Task RecibirTexto(string texto)
        {
            textoRecibido = texto;
            return Task.CompletedTask;
        }

        bool actividadModalAgregarProyecto = false;

        //async Task ClickHandlerAgregarProyecto(bool activar)
        //{
        //    actividadModalAgregarProyecto = activar;
        //    if (!activar)
        //    {
        //        await AuthenticationStateProvider.GetAuthenticationStateAsync();
        //        await ProyectosService.ObtenerListaDeProyectos(esquema);
        //        if (ProyectosService.ListaProyectos != null)
        //        {
        //            proyectos = ProyectosService.ListaProyectos;
        //        }
        //    }
        //    StateHasChanged();
        //}
    }
}
