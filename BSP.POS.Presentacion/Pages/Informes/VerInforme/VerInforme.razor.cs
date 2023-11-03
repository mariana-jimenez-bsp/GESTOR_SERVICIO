using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Departamentos;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Observaciones;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Pages.Usuarios.Usuarios;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Informes.VerInforme
{
    public partial class VerInforme: ComponentBase
    {
        [Parameter]
        public string Consecutivo { get; set; } = string.Empty;
        public mInformeAsociado informe { get; set; } = new mInformeAsociado();
        public mClienteAsociado ClienteAsociado = new mClienteAsociado();
        public List<mActividades> listaActividades = new List<mActividades>();
        public List<mActividadesAsociadas> listaActividadesAsociadas = new List<mActividadesAsociadas>();
        public List<mActividades> listaActividadesParaAgregar = new List<mActividades>();
        public List<mUsuariosDeCliente> listaDeUsuariosDeCliente = new List<mUsuariosDeCliente>();
        public List<mUsuariosDeCliente> listaDeUsuariosParaAgregar = new List<mUsuariosDeCliente>();
        public List<mDatosUsuariosDeClienteDeInforme> listadeDatosUsuariosDeClienteDeInforme = new List<mDatosUsuariosDeClienteDeInforme>();
        public mPerfil perfilActual = new mPerfil();
        public List<mDepartamentos> listaDepartamentos = new List<mDepartamentos>();
        public mUsuariosDeClienteDeInforme usuarioAAgregar = new mUsuariosDeClienteDeInforme();
        public mActividadAsociadaParaAgregar actividadAAgregar = new mActividadAsociadaParaAgregar();
        public List<mObservaciones> listaDeObservaciones = new List<mObservaciones>();
        public int total_horas_cobradas = 0;
        public int total_horas_no_cobradas = 0;
        private string[] elementos1 = new string[] { ".el-layout", ".header-col-left", ".div-observaciones" };
        private string[] elementos2 = new string[] { ".el-layout", ".header-col-right", ".footer-horas", ".footer-col-right" };
        public string rol = string.Empty;
        public string usuarioActual { get; set; } = string.Empty;
        public string esquema = string.Empty;
        private bool cargaInicial = false;
        private string mensajeConsecutivo;
        private bool usuarioAutorizado = true;
        protected override async Task OnInitializedAsync()
        {

            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            usuarioActual = user.Identity.Name;
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            if (await VerificarValidezDeConsecutivo())
            {
                if (!string.IsNullOrEmpty(Consecutivo))
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await UsuariosService.ObtenerPerfil(usuarioActual, esquema);
                    if (UsuariosService.Perfil != null)
                    {
                        perfilActual = UsuariosService.Perfil;
                    }
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    InformesService.InformeAsociado = await InformesService.ObtenerInformeAsociado(Consecutivo, esquema);
                    if (InformesService.InformeAsociado != null)
                    {
                        informe = InformesService.InformeAsociado;
                        if (VerificarUsuarioAutorizado())
                        {
                            await AuthenticationStateProvider.GetAuthenticationStateAsync();
                            ClientesService.ClienteAsociado = await ClientesService.ObtenerClienteAsociado(informe.cliente, esquema);
                            if (ClientesService.ClienteAsociado != null)
                            {
                                ClienteAsociado = ClientesService.ClienteAsociado;
                            }
                            await AuthenticationStateProvider.GetAuthenticationStateAsync();
                            await ActividadesService.ObtenerListaDeActividades(esquema);
                            if (ActividadesService.ListaActividades != null)
                            {
                                listaActividades = ActividadesService.ListaActividades;

                            }
                            await AuthenticationStateProvider.GetAuthenticationStateAsync();
                            await DepartamentosService.ObtenerListaDeDepartamentos(esquema);
                            if (DepartamentosService.listaDepartamentos != null)
                            {
                                listaDepartamentos = DepartamentosService.listaDepartamentos;
                            }
                            await RefrescarListaDeActividadesAsociadas();

                            await AuthenticationStateProvider.GetAuthenticationStateAsync();
                            listaDeUsuariosDeCliente = await UsuariosService.ObtenerListaDeUsuariosDeClienteAsociados(esquema, ClienteAsociado.CLIENTE);
                            await RefrescarListaDeUsuariosDeInforme();
                            await RefrescarLaListaDeObservaciones(Consecutivo);
                        }
                        else
                        {
                            usuarioAutorizado = false;
                        }
                        
                    }
                }
            }
            else
            {
                mensajeConsecutivo = "El consecutivo no existe o no es válido";
            }
            cargaInicial = true;
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            // Inicializa los tooltips de Bootstrap
            try
            {
                await JS.InvokeVoidAsync("initTooltips");
            }
            catch (Exception ex)
            {

                string error = ex.ToString();
                Console.WriteLine(error);
            }


        }
        private async Task<bool> VerificarAprobacionesUsuarios()
        {
            bool aprobado = false;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerDatosListaUsuariosDeClienteDeInforme(Consecutivo, esquema);
            if (UsuariosService.ListaDatosUsuariosDeClienteDeInforme.Any())
            {
                foreach (var usuario in UsuariosService.ListaDatosUsuariosDeClienteDeInforme)
                {
                    if (usuario.recibido == "1")
                    {
                        aprobado = true;
                    }
                    else
                    {
                        aprobado = false;
                        break;
                    }
                }
            }
            else
            {
                aprobado = true;
            }
            return aprobado;
        }
        public bool VerificarUsuarioAutorizado()
        {

            if (perfilActual.cod_cliente == informe.cliente || rol == "Admin")
            {
                return true;
            }
            return false;
        }
        public async Task<bool> VerificarValidezDeConsecutivo()
        {
            if (Consecutivo.Length > 5)
            {
                return false;
            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            string consecutivoValidar = await InformesService.ValidarExistenciaConsecutivoInforme(esquema, Consecutivo);

            if (!string.IsNullOrEmpty(consecutivoValidar))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task RefrescarLaListaDeObservaciones(string consecutivo)
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ObservacionesService.ObtenerListaDatosDeObservacionesDeInforme(consecutivo, esquema);
            if (ObservacionesService.ListaDatosDeObservacionesDeInforme != null)
            {
                listaDeObservaciones = ObservacionesService.ListaDatosDeObservacionesDeInforme;
            }
        }
        private async Task RefrescarListaDeActividadesAsociadas()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ActividadesService.ObtenerListaDeActividadesAsociadas(Consecutivo, esquema);
            listaActividadesAsociadas = ActividadesService.ListaActividadesAsociadas;
            try
            {
                total_horas_cobradas = listaActividadesAsociadas.Sum(act => int.Parse(act.horas_cobradas));
                total_horas_no_cobradas = listaActividadesAsociadas.Sum(act => int.Parse(act.horas_no_cobradas));
            }
            catch
            {
                total_horas_cobradas = 0;
                total_horas_no_cobradas = 0;
            }
            listaActividadesParaAgregar = listaActividades.Where(actividad => !listaActividadesAsociadas.Any(actividadAsociada => actividadAsociada.codigo_actividad == actividad.codigo)).ToList();
        }
        private async Task RefrescarListaDeUsuariosDeInforme()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerDatosListaUsuariosDeClienteDeInforme(informe.consecutivo, esquema);
            if (UsuariosService.ListaDatosUsuariosDeClienteDeInforme != null)
            {
                listadeDatosUsuariosDeClienteDeInforme = UsuariosService.ListaDatosUsuariosDeClienteDeInforme;

            }
            listaDeUsuariosParaAgregar = listaDeUsuariosDeCliente.Where(usuario => !listadeDatosUsuariosDeClienteDeInforme.Any(usuarioDeInforme => usuarioDeInforme.codigo_usuario_cliente == usuario.codigo)).ToList();
        }

        bool activarModalObservaciones = false;
      
        private async Task<bool> EnviarCorreosAClientes()
        { 
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            bool validar = await InformesService.EnviarCorreoDeReporteDeInforme(esquema, Consecutivo);
            if (validar)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


      

        async Task ClickHandlerObservaciones(bool activar)
        {
            activarModalObservaciones = activar;
            StateHasChanged();
            if (!activar)
            {
                await RefrescarLaListaDeObservaciones(Consecutivo);
            }
           
        }
      
        

        private bool EsLaPrimeraObservacion(mObservaciones observacion)
        {

            if (listaDeObservaciones.IndexOf(observacion) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool EsLaUltmaObservacion(mObservaciones observacion)
        {

            if (listaDeObservaciones.IndexOf(observacion) == listaDeObservaciones.Count - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task ObservacionNuevaAgregada()
        {
            await AlertasService.SwalExito("Se ha agregado la observación");

        }
        public async Task ObservacionNuevaCancelada()
        {
            await AlertasService.SwalAviso("Se han descartado los cambios");
        }


        private async Task ActivarAdvertenciaFinalizar()
        {
            await AlertasService.SwalAdvertencia("El informe ya fue finalizado antes");
        }
        private async Task ActivarAdvertenciaGuardar()
        {
            await AlertasService.SwalAdvertencia("El informe fue finalizado y no se puede editar");
        }
        private async Task SwalAdvertenciaInforme(string mensajeAlerta, string accion, string identificador)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Advertencia!",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Aceptar",
                CancelButtonText = "Cancelar"
            }).ContinueWith(async swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (result.IsConfirmed)
                {
                    if (accion == "Informe")
                    {
                        await EliminarInforme(identificador);
                    }
                }
                
            });
        }

        private async Task EliminarInforme(string consecutivo)
        {
     
            if (!string.IsNullOrEmpty(consecutivo) && !string.IsNullOrEmpty(esquema))
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                bool resultadoEliminar = await InformesService.EliminarInforme(consecutivo, esquema);
                if (resultadoEliminar)
                {
                    await SwalAvisoInforme("Se ha eliminado el informe", "Informe");
                }
                else
                {
                    await AlertasService.SwalError("Ocurrió un error vuelva a intentarlo");
                }
                
            }
        }

        private async Task SwalAvisoInforme(string mensajeAlerta, string accion)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Aviso!",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Info,
                ShowCancelButton = false,
                ConfirmButtonText = "Ok"
            }).ContinueWith(swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (result.IsConfirmed || result.IsDismissed)
                {
                    if (accion == "Informe")
                    {
                        navigationManager.NavigateTo($"index", forceLoad: true);
                    }
                }
            });
        }

        private async Task SwalEnviandoCorreo()
        {
            bool verificarAprobacion = false;
            verificarAprobacion = await VerificarAprobacionesUsuarios();
            if (!verificarAprobacion)
            {
                bool resultadoCorreo = false;
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Icon = SweetAlertIcon.Info,
                    Title = "Enviando...",
                    ShowCancelButton = false,
                    ShowConfirmButton = false,
                    AllowOutsideClick = false,
                    AllowEscapeKey = false,
                    DidOpen = new SweetAlertCallback(async () =>
                    {
                        resultadoCorreo = await EnviarCorreosAClientes();
                        await Swal.CloseAsync();
                        
                    }),
                    WillClose = new SweetAlertCallback(Swal.CloseAsync)

                });

                if (resultadoCorreo)
                {
                    await AlertasService.SwalExito("El correo ha sido enviado");
                }
                else
                {
                    await AlertasService.SwalError("Ocurrió un error. Vuelva a intentarlo.");
                }
            }
            else
            {
                await AlertasService.SwalAviso("Todos los usuarios ya aprobaron el informe");
            }
            
        }

        
    }
}
