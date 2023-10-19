using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Permisos;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using BSP.POS.Presentacion.Models.Licencias;
using System.Security.Claims;
using BSP.POS.Presentacion.Services.Informes;

namespace BSP.POS.Presentacion.Pages.Usuarios.Usuarios
{
    public partial class AgregarUsuario:ComponentBase
    {
        [Parameter] public string codigoCliente { get; set; } = string.Empty;
        public string esquema = string.Empty;
        public mUsuarioParaAgregar usuario = new mUsuarioParaAgregar();
        public List<mPermisos> todosLosPermisos { get; set; } = new List<mPermisos>();
        public List<mPermisosAsociados> permisosAsociados { get; set; } = new List<mPermisosAsociados>();
        public List<mClientes> listaClientes = new List<mClientes>();
        public List<mUsuariosParaEditar> usuarios = new List<mUsuariosParaEditar>();
        public mDatosLicencia licencia = new mDatosLicencia();
        bool repetido = false;
        public string correoRepite = string.Empty;
        public string mensajeCorreoRepite = string.Empty;
        public string usuarioRepite = string.Empty;
        public string mensajeUsuarioRepite = string.Empty;
        public string mensajeError;
        public string rol = string.Empty;
        private bool usuarioAgregado = false;
        private bool descartarCambios = false;
        private bool limiteDeUsuarios = false;
        private bool cargarInicial = false;
        private string mensajeCliente = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            if (await VerificarValidaCodigoCliente()){
                
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await PermisosService.ObtenerListaDePermisos(esquema);
                if (!string.IsNullOrEmpty(codigoCliente))
                {
                    usuario.cod_cliente = codigoCliente;
                }
                if (PermisosService.ListaPermisos != null)
                {
                    todosLosPermisos = PermisosService.ListaPermisos;

                }


            }
            else
            {
                mensajeCliente = "El codigo del cliente no existe";
            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ClientesService.ObtenerListaClientes(esquema);
            if (ClientesService.ListaClientes != null)
            {
                listaClientes = ClientesService.ListaClientes;
            }
            await LicenciasService.ObtenerDatosDeLicencia();
            if (LicenciasService.licencia != null)
            {
                licencia = LicenciasService.licencia;

            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerListaDeUsuariosParaEditar(esquema);
            if (UsuariosService.ListaDeUsuariosParaEditar != null)
            {
                usuarios = UsuariosService.ListaDeUsuariosParaEditar;
            }
            if (licencia.CantidadUsuarios <= usuarios.Count)
            {
                limiteDeUsuarios = true;
            }
            cargarInicial = true;
        }
        public async Task<bool> VerificarValidaCodigoCliente()
        {
            if (string.IsNullOrEmpty(codigoCliente))
            {
                return true;
            }
            if (codigoCliente.Length > 20)
            {
                return false;
            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            string clienteValidar = await ClientesService.ValidarExistenciaDeCliente(esquema, codigoCliente);

            if (!string.IsNullOrEmpty(clienteValidar))
            {
                return true;
            }
            else
            {
                return false;
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
                usuario.claveDesencriptada = e.Value.ToString();
                usuario.clave = UsuariosService.EncriptarClave(usuario.claveDesencriptada);
            }
            else
            {
                usuario.clave = null;
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
            foreach (var item in todosLosPermisos)
            {
                if (item.Id == idPermiso)
                {
                    item.EstadoCheck = (bool)e.Value;
                    permiso = item;
                }
            }


            var permisoEncontrado = permisosAsociados.FirstOrDefault(p => p.id_permiso == permiso.Id);

            if (permiso.EstadoCheck)
            {
                if (permisoEncontrado == null)
                {
                    mPermisosAsociados permisoParaAñadir = new mPermisosAsociados();
                    permisoParaAñadir.id_permiso = permiso.Id;
                    permisosAsociados.Add(permisoParaAñadir);

                }
            }
            else
            {
                if (permisoEncontrado != null)
                {
                    permisosAsociados.Remove(permisoEncontrado);

                }
            }
        }
        private async Task VerificarCorreoYUsuarioExistente()
        {


            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            usuarioRepite = await UsuariosService.ValidarUsuarioExistente(usuario.esquema, usuario.usuario);

            if (!string.IsNullOrEmpty(usuarioRepite))
            {
                mensajeUsuarioRepite = "El usuario ya existe";
                repetido = true;

            }
            else
            {
                mensajeUsuarioRepite = null;
            }



            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            correoRepite = await UsuariosService.ValidarCorreoExistente(usuario.esquema, usuario.correo);
            if (!string.IsNullOrEmpty(correoRepite))
            {
                mensajeCorreoRepite = "El correo ya existe";
                repetido = true;

            }
            else
            {
                mensajeCorreoRepite = null;
            }


        }

        private async Task DescartarCambios()
        {
            descartarCambios = false;
            usuario = new mUsuarioParaAgregar();
            if (!string.IsNullOrEmpty(codigoCliente))
            {
                usuario.cod_cliente = codigoCliente;
            }
            StateHasChanged();
            await Task.Delay(100);
            descartarCambios = true;
        }
        private async Task AgregarUsuarioNuevo()
        {
            mensajeError = null;
            usuarioAgregado = false;
            try
            {
                bool resultadoUsuario = false;
                bool resultadoPermisos = false;
                repetido = false;
                await VerificarCorreoYUsuarioExistente();
                if (!repetido)
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    resultadoUsuario = await UsuariosService.AgregarUsuario(usuario, esquema);
                    if (resultadoUsuario)
                    {
                        await AuthenticationStateProvider.GetAuthenticationStateAsync();
                        await UsuariosService.ObtenerPerfil(usuario.usuario, esquema);
                        mPerfil usuarioCreado = new mPerfil();
                        if (UsuariosService.Perfil != null)
                        {
                            usuarioCreado = UsuariosService.Perfil;
                        }
                        if (permisosAsociados.Any())
                        {
                            await AuthenticationStateProvider.GetAuthenticationStateAsync();
                            resultadoPermisos = await PermisosService.ActualizarListaPermisosAsociados(permisosAsociados, usuarioCreado.id, usuarioCreado.esquema);
                        }
                        else
                        {
                            resultadoPermisos = true;
                        }
                        if(resultadoPermisos)
                        {
                            IrAUsuarios();
                        }
                        else
                        {
                            mensajeError = "Ocurrío un Error vuelva a intentarlo";
                        }

                    }
                    else
                    {
                        mensajeError = "Ocurrío un Error vuelva a intentarlo";
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
