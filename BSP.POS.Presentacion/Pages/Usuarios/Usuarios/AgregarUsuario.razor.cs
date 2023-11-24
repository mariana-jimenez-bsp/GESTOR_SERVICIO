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
using CurrieTechnologies.Razor.SweetAlert2;
using BSP.POS.Presentacion.Models.CodigoTelefonoPais;
using BSP.POS.Presentacion.Models.Departamentos;
using Newtonsoft.Json;

namespace BSP.POS.Presentacion.Pages.Usuarios.Usuarios
{
    public partial class AgregarUsuario:ComponentBase
    {
        [Parameter] public string codigoCliente { get; set; } = string.Empty;
        public string esquema = string.Empty;
        public mUsuarioParaAgregar usuario = new mUsuarioParaAgregar();
        
        public List<mClientes> listaClientes = new List<mClientes>();
        public List<mUsuariosParaEditar> usuarios = new List<mUsuariosParaEditar>();
        public List<mCodigoTelefonoPais> listaCodigoTelefonoPais = new List<mCodigoTelefonoPais>();
        public List<mDepartamentos> listaDepartamentos = new List<mDepartamentos>();
        public mDatosLicencia licencia = new mDatosLicencia();
       
        bool repetido = false;
        public string correoRepite = string.Empty;
        public string mensajeCorreoRepite = string.Empty;
        public string usuarioRepite = string.Empty;
        public string mensajeUsuarioRepite = string.Empty;
        public string rol = string.Empty;
        List<mObjetoPermiso> permisos = new List<mObjetoPermiso>();
        private bool limiteDeUsuarios = false;
        private bool cargarInicial = false;
        private string mensajeCliente = string.Empty;
        private string mensajeEsquema = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            var PermisosClaim = user.Claims.FirstOrDefault(c => c.Type == "permisos");
            if (PermisosClaim != null)
            {
                permisos = JsonConvert.DeserializeObject<List<mObjetoPermiso>>(PermisosClaim.Value);


            }
            if (await VerificarValidaCodigoCliente()){
                
               
                if (!string.IsNullOrEmpty(codigoCliente))
                {
                    usuario.cod_cliente = codigoCliente;
                }
               
                usuario.paisTelefono = "CRI";
               
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await CodigoTelefonoPaisService.ObtenerDatosCodigoTelefonoPais(esquema);
                if(CodigoTelefonoPaisService.listaDatosCodigoTelefonoPais != null)
                {
                    listaCodigoTelefonoPais = CodigoTelefonoPaisService.listaDatosCodigoTelefonoPais;
                    usuario.IdCodigoTelefono = listaCodigoTelefonoPais.Where(ct => ct.Pais == usuario.paisTelefono).Select(ct => ct.Id).First();
                }
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await DepartamentosService.ObtenerListaDeDepartamentos(esquema);
                if(DepartamentosService.listaDepartamentos != null)
                {
                    listaDepartamentos = DepartamentosService.listaDepartamentos;
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


            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            usuarioRepite = await UsuariosService.ValidarUsuarioExistente(esquema, usuario.usuario);

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
            correoRepite = await UsuariosService.ValidarCorreoExistente(esquema, usuario.correo);
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
           await AlertasService.SwalAvisoCancelado("Se han descartado los cambios");
        }
        private SelectPermisos selectPermisosComponente;
        private SelectEsquemas selectEsquemasComponente;
        private async Task AgregarUsuarioNuevo()
        {
            try
            {
                bool resultadoUsuario = false;
                int resultadoPermisos = 0;
                int ResultadoEsquemas = 0;
                repetido = false;
                mensajeEsquema = null;
                if(selectEsquemasComponente.cantidadEsquemas >= 1)
                {
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
                            resultadoPermisos = await selectPermisosComponente.ActualizarListaDePermisos(usuarioCreado.codigo);
                            ResultadoEsquemas = await selectEsquemasComponente.ActualizarListaDeEsquema(usuarioCreado.codigo);
                            if (resultadoPermisos == 1 && ResultadoEsquemas == 1)
                            {

                                await AlertasService.SwalExitoHecho("Se ha agregado el usuario");

                            }
                            else
                            {
                                await AlertasService.SwalError("Ocurrío un Error vuelva a intentarlo");
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
                    await AlertasService.SwalError(mensajeEsquema);
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
        private async Task InvalidSubmit(EditContext modeloContext)
        {
            await ActivarScrollBarDeErrores();
            var mensajesDeValidaciones = modeloContext.GetValidationMessages();
            string mensaje = mensajesDeValidaciones.First();
            await AlertasService.SwalError(mensaje);
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
                if (!string.IsNullOrEmpty(mensajeCorreoRepite))
                {
                    await AlertasService.SwalError(mensajeCorreoRepite);
                }
                else if (!string.IsNullOrEmpty(mensajeUsuarioRepite))
                {
                    await AlertasService.SwalError(mensajeUsuarioRepite);
                }
            }
        }
    }
}
