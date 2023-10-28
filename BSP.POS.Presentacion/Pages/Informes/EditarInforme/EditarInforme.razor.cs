using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Departamentos;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Observaciones;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Pages.Clientes;
using BSP.POS.Presentacion.Pages.Usuarios.Usuarios;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Globalization;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Informes.EditarInforme
{
    public partial class EditarInforme : ComponentBase
    {
        [Parameter]
        public string Consecutivo { get; set; } = string.Empty;
        public mInformeAsociado informe { get; set; } = new mInformeAsociado();
        public mClienteAsociado ClienteAsociado = new mClienteAsociado();
        public List<mActividades> listaActividades = new List<mActividades>();
        public List<mActividades> listaActividadesDeUsuario = new List<mActividades>();
        public List<mActividadesAsociadas> listaActividadesAsociadas = new List<mActividadesAsociadas>();
        public List<mActividades> listaActividadesParaAgregar = new List<mActividades>();
        public List<mUsuariosDeCliente> listaDeUsuariosDeCliente = new List<mUsuariosDeCliente>();
        public List<mUsuariosDeCliente> listaDeUsuariosParaAgregar = new List<mUsuariosDeCliente>();
        public List<mDatosUsuariosDeClienteDeInforme> listadeDatosUsuariosDeClienteDeInforme = new List<mDatosUsuariosDeClienteDeInforme>();
        public List<mUsuariosParaEditar> listaTodosLosUsuarios = new List<mUsuariosParaEditar>();
        public List<mDepartamentos> listaDepartamentos = new List<mDepartamentos>();
        public mUsuariosDeClienteDeInforme usuarioAAgregar = new mUsuariosDeClienteDeInforme();
        public mActividadAsociadaParaAgregar actividadAAgregar = new mActividadAsociadaParaAgregar();
        public List<mObservaciones> listaDeObservaciones = new List<mObservaciones>();
        public mPerfil perfilActual = new mPerfil();
        public int total_horas_cobradas = 0;
        public int total_horas_no_cobradas = 0;
        public string usuarioActual { get; set; } = string.Empty;
        public string esquema = string.Empty;
        public string rol = string.Empty;
        private ElementReference actividadesButton;
        private ElementReference informeButton;

        private bool cargaInicial = false;
        private string mensajeConsecutivo;
        private bool informeGuardado = false;
        private bool activarBotonFinalizar = false;
        private bool informeActualizado = false;
        private string fechaInicio = string.Empty;
        private string fechaFinal = string.Empty;
        private DateTime fechaInicioDateTime = DateTime.MinValue;
        private DateTime fechaFinalDateTime = DateTime.MinValue;
        private string usuarioFiltro = string.Empty;
        private bool usuarioAutorizado = true;
        private string[] elementos1 = new string[] { ".el-layout", ".header-col-left", ".div-observaciones", ".div-agregar-usuario" };
        private string[] elementos2 = new string[] { ".el-layout", ".header-col-right", ".footer-horas", ".footer-col-right" , ".div-agregar-actividad" };

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
                            await RefrescarListaActividades();
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
                                listaTodosLosUsuarios = UsuariosService.ListaDeUsuariosParaEditar;
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
        private async Task RefrescarListaActividades()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ActividadesService.ObtenerListaDeActividades(esquema);
            if (ActividadesService.ListaActividades != null)
            {
                listaActividades = ActividadesService.ListaActividades;

            }
            if (rol != "Admin")
            {
                
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await ActividadesService.ObtenerListaDeActividadesPorUsuario(esquema, perfilActual.codigo);
                    if (ActividadesService.ListaActividades != null)
                    {
                        listaActividadesDeUsuario = ActividadesService.ListaActividadesDeUsuario;

                    }
                
            }
            
        }
        private async Task SubmitActividades()
        {
            await JS.InvokeVoidAsync("clickButton", actividadesButton);
        }

        private async Task SubmitInforme()
        {
            await JS.InvokeVoidAsync("clickButton", informeButton);
        }

        private async Task TodosLosBotonesSubmit()
        {
            informeActualizado = false;
            try
            {
                await SubmitInforme();
                StateHasChanged();
                await Task.Delay(100);
                await SubmitActividades();
               
            }
            catch (Exception)
            {

                await AlertasService.SwalError("Ocurrió un Error vuelva a intentarlo");
            }
            

        }
        public bool VerificarUsuarioAutorizado()
        {
            
            if(perfilActual.cod_cliente == informe.cliente || rol == "Admin")
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
        private void CambioHorasCobradas(ChangeEventArgs e, string actividadId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var actividad in listaActividadesAsociadas)
                {
                    if (actividad.Id == actividadId)
                    {
                        actividad.horas_cobradas = e.Value.ToString();
                        RefrescarTotalHoras();
                    }
                }
            }
        }
       
        private void CambioHorasNoCobradas(ChangeEventArgs e, string actividadId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var actividad in listaActividadesAsociadas)
                {
                    if (actividad.Id == actividadId)
                    {
                        actividad.horas_no_cobradas = e.Value.ToString();
                        RefrescarTotalHoras();
                    }
                }
            }
        }

        private void CambioActividad(ChangeEventArgs e, string actividadId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var actividad in listaActividadesAsociadas)
                {
                    if (actividad.Id == actividadId)
                    {
                        actividad.codigo_actividad = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioFechaConsultoria(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                informe.fecha_consultoria = e.Value.ToString();

            }
        }

        private void CambioHoraInicio(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                informe.hora_inicio = e.Value.ToString();

            }
        }

        private void CambioHoraFinal(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                informe.hora_final = e.Value.ToString();


            }
        }

        private async Task ActualizarActividadesAsociadas()
        {
            informeGuardado = false;
            if (informeActualizado)
            {
                bool resultadoActividad = false;
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                resultadoActividad = await ActividadesService.ActualizarListaDeActividadesAsociadas(listaActividadesAsociadas, esquema);
                if (resultadoActividad)
                {
                    await RefrescarListaDeActividadesAsociadas();

                    if (!activarBotonFinalizar)
                    {
                        await AlertasService.SwalExito("Se han guardado los cambios");
                    }
                    await CambiarEstadoInformeGuardado(true);
                }
                else
                {
                    await AlertasService.SwalError("Ocurrió un Error vuelva a intentarlo");
                }
            }
            else
            {
                await AlertasService.SwalError("Ocurrió un Error vuelva a intentarlo");
            }
            
            
        }

        private async Task ActualizarInformeAsociado()
        {
            
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            bool resultadoActualizar = await InformesService.ActualizarInformeAsociado(informe, esquema);
            if (resultadoActualizar)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await InformesService.ObtenerInformeAsociado(Consecutivo, esquema);
                informe = InformesService.InformeAsociado;
                informeActualizado = true;
            }
            else
            {
                informeActualizado = false;
            }


        }
        private void CambioCodigoDeUsuario(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuarioAAgregar.codigo_usuario_cliente = e.Value.ToString();

            }
        }

        private void CambioCodigoActividad(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                actividadAAgregar.codigo_actividad = e.Value.ToString();

            }
        }
        private void RefrescarTotalHoras()
        {
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
        }
        private async Task RefrescarListaDeActividadesAsociadas()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ActividadesService.ObtenerListaDeActividadesAsociadas(Consecutivo, esquema);
            listaActividadesAsociadas = ActividadesService.ListaActividadesAsociadas;
            RefrescarTotalHoras();
            if(rol == "Admin")
            {
                listaActividadesParaAgregar = listaActividades.Where(actividad => !listaActividadesAsociadas.Any(actividadAsociada => actividadAsociada.codigo_actividad == actividad.codigo)).ToList();
            }
            else
            {
                listaActividadesParaAgregar = listaActividadesDeUsuario.Where(actividad => !listaActividadesAsociadas.Any(actividadAsociada => actividadAsociada.codigo_actividad == actividad.codigo)).ToList();
            }
            foreach (var actividad in listaActividadesParaAgregar)
            {
                string fecha = actividad.fecha_actualizacion.Replace("  ", " ");
                actividad.FechaActualizacionDateTime = DateTime.ParseExact(fecha, "MMM dd yyyy h:mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            if (listaActividadesParaAgregar.Any())
            {
                fechaInicioDateTime = listaActividadesParaAgregar.OrderBy(i => i.FechaActualizacionDateTime).Select(i => i.FechaActualizacionDateTime).First();
                fechaFinalDateTime = listaActividadesParaAgregar.OrderByDescending(i => i.FechaActualizacionDateTime).Select(i => i.FechaActualizacionDateTime).First();
            }
            
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
        private async Task AgregarUsuarioDeClienteDeInforme()
        {

            if (usuarioAAgregar.codigo_usuario_cliente != null)
            {
                usuarioAAgregar.consecutivo_informe = Consecutivo;
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                bool resultadoAgregar = await UsuariosService.AgregarUsuarioDeClienteDeInforme(usuarioAAgregar, esquema);
                if (resultadoAgregar)
                {
                    usuarioAAgregar = new mUsuariosDeClienteDeInforme();
                    await RefrescarListaDeUsuariosDeInforme();
                }
                else
                {
                    await AlertasService.SwalError("Ocurrió un Error vuelva a intentarlo");
                }
               
            }
        }

        private async Task AgregarActividadDeInforme()
        {
            if (actividadAAgregar.codigo_actividad != null)
            {
                string horas = listaActividadesParaAgregar.Where(actividad => actividad.codigo == actividadAAgregar.codigo_actividad).First().horas;
                actividadAAgregar.consecutivo_informe = Consecutivo;
                actividadAAgregar.horas_cobradas = horas;
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ActividadesService.AgregarActividadDeInforme(actividadAAgregar, esquema);
                actividadAAgregar = new mActividadAsociadaParaAgregar();
                await RefrescarListaDeActividadesAsociadas();
            }
        }

        
        bool activarModalObservaciones = false;
        async Task ClickHandlerObservaciones(bool activar)
        {
            activarModalObservaciones = activar;
            StateHasChanged();
            if (!activar)
            {
                await RefrescarLaListaDeObservaciones(Consecutivo);
            }
        }
        async Task ClickHandlerFinalizarInforme(bool activar)
        {
                activarBotonFinalizar = true;
                await TodosLosBotonesSubmit();
        }

        private async Task ActivarAdvertenciaEnviar()
        {
            await AlertasService.SwalAdvertencia("El informe debe estar Finalizado para realizar el envío al cliente");
        }

        private bool EsLaPrimeraObservacion(mObservaciones observacion)
        {

            if(listaDeObservaciones.IndexOf(observacion) == 0)
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

        public async Task CambiarEstadoInformeGuardado(bool estado)
        {
            informeGuardado = estado;
            if (informeGuardado && activarBotonFinalizar)
            {
                await SwalAccionPregunta("¿Está seguro de finalizar el informe?", "Finalizar", Consecutivo);
            }
        }

        private async Task HacerSelectEditable(string codigoActividad)
        {
            if(rol == "Admin" || listaActividadesDeUsuario.Any(a => a.codigo == codigoActividad))
            {
                DotNetObjectReference<EditarInforme> objRef = DotNetObjectReference.Create(this);
                await JS.InvokeVoidAsync("HacerSelectEditable", objRef, codigoActividad);
            }
            
        }
        [JSInvokable]
        public async Task ActualizarTextoEditable(string texto, string codigoActividad)
        {
            foreach (var actividad in listaActividades)
            {
                if(actividad.codigo == codigoActividad && !string.IsNullOrEmpty(texto))
                {
                    actividad.Actividad = texto;
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await ActividadesService.ActualizarListaDeActividades(listaActividades, esquema);
                    await RefrescarListaActividades();
                }
            }
            StateHasChanged(); 
        }
        private void CambioFechaInicio(ChangeEventArgs e)
        {
            fechaInicio = e.Value.ToString();
            if (!string.IsNullOrEmpty(fechaInicio))
            {
                fechaInicioDateTime = DateTime.ParseExact(fechaInicio, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (fechaInicioDateTime > fechaFinalDateTime)
                {
                    fechaFinal = fechaInicio;
                    fechaFinalDateTime = fechaInicioDateTime;
                }
            }
        }

        private void CambioFechaFinal(ChangeEventArgs e)
        {
            fechaFinal = e.Value.ToString();
            if (!string.IsNullOrEmpty(fechaFinal))
            {
                fechaFinalDateTime = DateTime.ParseExact(fechaFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (fechaFinalDateTime < fechaInicioDateTime)
                {
                    fechaInicio = fechaFinal;
                    fechaInicioDateTime = fechaFinalDateTime;
                }
            }
        }
        private void CambioUsuarioFiltro(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuarioFiltro = e.Value.ToString();
            }
            else
            {
                usuarioFiltro = string.Empty;
            }
        }

        private async Task SwalAccionPregunta(string mensajeAlerta, string accion, string identificador)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = mensajeAlerta,
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
                ConfirmButtonText = "Aceptar",
                CancelButtonText = "Cancelar"
            }).ContinueWith(async swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (result.IsConfirmed)
                {
                    if(accion == "Actividad")
                    {
                        await EliminarActividadDeInforme(identificador);
                    }else if(accion == "Informe")
                    {
                        await EliminarInforme(identificador);
                    }else if(accion == "Usuario")
                    {
                        await EliminarUsuarioDeClienteDeInforme(identificador);
                    }else if(accion == "Finalizar")
                    {
                        await FinalizarInforme(identificador);
                    }
                }else if(result.IsDismissed && accion == "Finalizar")
                {
                    activarBotonFinalizar = false;
                    StateHasChanged();
                }
            });
        }

        private async Task EliminarActividadDeInforme(string idActividad)
        {
            if (!string.IsNullOrEmpty(idActividad) && !string.IsNullOrEmpty(esquema))
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                bool resultadoEliminar = await ActividadesService.EliminarActividadDeInforme(idActividad, esquema);
                if (resultadoEliminar)
                {
                    await RefrescarListaDeActividadesAsociadas();
                }
                else
                {
                    await AlertasService.SwalError("Ocurrió un Error vuelva a intentarlo");
                }
               
               
                StateHasChanged();
            }
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
                    await AlertasService.SwalError("Ocurrió un Error vuelva a intentarlo");
                }
                
            }
        }
        private async Task EliminarUsuarioDeClienteDeInforme(string idUsuario)
        {
            if (!string.IsNullOrEmpty(idUsuario) && !string.IsNullOrEmpty(esquema))
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                bool resultadoEliminar = await UsuariosService.EliminarUsuarioDeClienteDeInforme(idUsuario, esquema);
                if (resultadoEliminar)
                {
                    await RefrescarListaDeUsuariosDeInforme();
                }
                else
                {
                    await AlertasService.SwalError("Ocurrió un Error vuelva a intentarlo");
                }
                StateHasChanged();
            }
        }
        private async Task FinalizarInforme(string consecutivo)
        {
            if (!string.IsNullOrEmpty(consecutivo) && !string.IsNullOrEmpty(esquema))
            {
                mInformeEstado informeEstado = new mInformeEstado();
                informeEstado.consecutivo = consecutivo;
                informeEstado.estado = "Finalizado";
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                bool resultadoEstado = await InformesService.CambiarEstadoDeInforme(informeEstado, esquema);
                if (resultadoEstado)
                {
                    await SwalAvisoInforme("El informe ha sido finalizado", "Finalizar");
                }
                else
                {
                    await AlertasService.SwalExito("Ocurrió un Error vuelta a intentarlo");
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
                    if(accion == "Finalizar")
                    {
                        navigationManager.NavigateTo($"Informe/VerInforme/" + Consecutivo, forceLoad: true);
                    }
                    else if(accion == "Informe")
                    {
                        navigationManager.NavigateTo($"index", forceLoad: true);
                    }
                }
            });
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
    }
}
