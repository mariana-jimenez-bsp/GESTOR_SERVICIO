using Blazored.LocalStorage;
using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.CodigoTelefonoPais;
using BSP.POS.Presentacion.Models.Departamentos;
using BSP.POS.Presentacion.Models.Licencias;
using BSP.POS.Presentacion.Models.Permisos;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Services.CodigoTelefonoPais;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Security.Claims;
using BSP.POS.Presentacion.Models.Esquemas;

namespace BSP.POS.Presentacion.Pages.Usuarios.Usuarios
{
    public partial class EditarUsuario: ComponentBase
    {
        [Parameter] public string codigo { get; set; } = string.Empty;
        [Parameter] public string codigoCliente { get; set; } = string.Empty;
        public string esquema = string.Empty;
        public mUsuariosParaEditar usuario = new mUsuariosParaEditar();
        public List<mClientes> listaClientes = new List<mClientes>();
        public List<mCodigoTelefonoPais> listaCodigoTelefonoPais = new List<mCodigoTelefonoPais>();
        public List<mDepartamentos> listaDepartamentos = new List<mDepartamentos>();
        public mCodigoTelefonoPaisUsuarios datosCodigoTelefonoPaisDeUsuario = new mCodigoTelefonoPaisUsuarios();
        public string usuarioActual = string.Empty;
        bool repetido = false;
        public string rol = string.Empty;
        public string correoRepite = string.Empty;
        public string mensajeCorreoRepite = string.Empty;
        public string usuarioRepite = string.Empty;
        public string mensajeUsuarioRepite = string.Empty;
        private bool cargarInicial = false;
        private string mensajeValidacion = string.Empty;
        private string mensajeEsquema = string.Empty;
        List<mObjetoPermiso> permisos = new List<mObjetoPermiso>();
       
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            var PermisosClaim = user.Claims.FirstOrDefault(c => c.Type == "permisos");
            if (PermisosClaim != null)
            {
                permisos = JsonSerializer.Deserialize<List<mObjetoPermiso>>(PermisosClaim.Value);


            }
            usuarioActual = user.Identity.Name;
            if (await VerificarValidaCodigoCliente() && await VerificarValidaCodigoUsuario())
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await UsuariosService.ObtenerElUsuarioParaEditar(esquema, codigo);
                if (UsuariosService.UsuarioParaEditar != null)
                {
                    usuario = UsuariosService.UsuarioParaEditar;
                }
                    usuario.claveOriginal = UsuariosService.DesencriptarClave(usuario.clave);
                    usuario.claveDesencriptada = usuario.claveOriginal;
                
                usuario.usuarioOrignal = usuario.usuario;
                usuario.correoOriginal = usuario.correo;
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await CodigoTelefonoPaisService.ObtenerDatosCodigoTelefonoPaisDeUsuarioPorUsuario(esquema, usuario.codigo);
                if(CodigoTelefonoPaisService.datosCodigoTelefonoPaisDeUsuario != null)
                {
                    datosCodigoTelefonoPaisDeUsuario = CodigoTelefonoPaisService.datosCodigoTelefonoPaisDeUsuario;
                    usuario.IdCodigoTelefono = datosCodigoTelefonoPaisDeUsuario.IdCodigoTelefono;
                    usuario.paisTelefono = datosCodigoTelefonoPaisDeUsuario.Pais;
                }
               
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await CodigoTelefonoPaisService.ObtenerDatosCodigoTelefonoPais(esquema);
                if (CodigoTelefonoPaisService.listaDatosCodigoTelefonoPais != null)
                {
                    listaCodigoTelefonoPais = CodigoTelefonoPaisService.listaDatosCodigoTelefonoPais;
                    
                }
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await DepartamentosService.ObtenerListaDeDepartamentos(esquema);
                if (DepartamentosService.listaDepartamentos != null)
                {
                    listaDepartamentos = DepartamentosService.listaDepartamentos;
                }
            }
            else
            {
                mensajeValidacion = "Codigo de cliente o de usuario inválidos";
            }
            
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ClientesService.ObtenerListaClientes(esquema);
            if (ClientesService.ListaClientes != null)
            {
                listaClientes = ClientesService.ListaClientes;
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
        public async Task<bool> VerificarValidaCodigoUsuario()
        {
            if (string.IsNullOrEmpty(codigo))
            {
                return true;
            }
            if (codigo.Length > 6)
            {
                return false;
            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            string codigoValidar = await UsuariosService.ValidarExistenciaDeCodigoUsuario(esquema, codigo);

            if (!string.IsNullOrEmpty(codigoValidar))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

      
        private void CambioCliente(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
                usuario.cod_cliente = e.Value.ToString();
        }


        private void CambioUsuario(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.usuario = e.Value.ToString();
            }
        }

        private void CambioCorreo(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.correo = e.Value.ToString();
            }
        }

        private void CambioClave(ChangeEventArgs e)
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

        private void CambioRol(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.rol = e.Value.ToString();
            }
        }
        private void CambioNombre(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.nombre = e.Value.ToString();
            }
        }

        private void CambioTelefono(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.telefono = e.Value.ToString();
            }
        }
        private void CambioPaisTelefono(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.paisTelefono = e.Value.ToString();
                usuario.IdCodigoTelefono = listaCodigoTelefonoPais.Where(ct => ct.Pais == usuario.paisTelefono).Select(ct => ct.Id).First();
            }
        }

        private void CambioDepartamento(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuario.codigo_departamento = e.Value.ToString();
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
        private async Task VerificarCorreoYUsuarioExistente()
        {

            if (usuario.usuarioOrignal.ToLower() != usuario.usuario.ToLower())
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                usuario.usuarioRepite = await UsuariosService.ValidarUsuarioExistente(esquema, usuario.usuario);

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
            if (usuario.correoOriginal.ToLower() != usuario.correo.ToLower())
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                usuario.correoRepite = await UsuariosService.ValidarCorreoExistente(esquema, usuario.correo);
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
            await AlertasService.SwalAvisoCancelado("Se han descartado los cambios");
            
        }

        
        private SelectPermisos selectPermisosComponente;
        private SelectEsquemas selectEsquemasComponente;
        private async Task ActualizarUsuario()
        {
            try
            {
                bool ResultadoUsuario = false;
                int ResultadoPermisos = 0;
                int ResultadoEsquemas = 0;
                repetido = false;
                mensajeEsquema = null;
                if(selectEsquemasComponente.cantidadEsquemas >= 1)
                {
                    await VerificarCorreoYUsuarioExistente();
                    if (!repetido)
                    {

                        await AuthenticationStateProvider.GetAuthenticationStateAsync();
                        ResultadoUsuario = await UsuariosService.ActualizarUsuario(usuario, esquema, usuarioActual);
                        ResultadoPermisos = await selectPermisosComponente.ActualizarListaDePermisos("");
                        ResultadoEsquemas = await selectEsquemasComponente.ActualizarListaDeEsquema("");
                        if (ResultadoUsuario && ResultadoPermisos != 0 && ResultadoEsquemas != 0)
                        {
                            if (usuarioActual.ToLower() == usuario.usuarioOrignal.ToLower())
                            {
                                if (usuario.usuarioOrignal.ToLower() != usuario.usuario.ToLower() || usuario.correoOriginal.ToLower() != usuario.correo.ToLower() || usuario.claveOriginal != usuario.claveDesencriptada || ResultadoPermisos == 2 || ResultadoEsquemas == 2)
                                {
                                    await AlertasService.SwalExitoLogin("Se ha actualizado el usuario");

                                }
                                else
                                {
                                    await AlertasService.SwalExitoHecho("Se ha actualizado el usuario");

                                }
                            }
                            else
                            {
                                await AlertasService.SwalExitoHecho("Se ha actualizado el usuario");

                            }


                        }
                        else
                        {
                            await AlertasService.SwalError("Ocurrío un Error vuelva a intentarlo");
                        }
                    }
                    else
                    {
                        await ActivarScrollBarErroresRepite();
                    }
                }
                else
                {
                    mensajeEsquema = "Debe seleccionar al menos 1 esquema";
                }
                
            }
            catch (Exception)
            {

                await AlertasService.SwalError("Ocurrío un Error vuelva a intentarlo");
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

        

        
    }
}
