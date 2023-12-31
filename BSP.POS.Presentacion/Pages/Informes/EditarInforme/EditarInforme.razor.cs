﻿using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Departamentos;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Observaciones;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Pages.Actividades;
using BSP.POS.Presentacion.Pages.Proyectos;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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
        public mInforme informe { get; set; } = new mInforme();
        public mClienteAsociado ClienteAsociado = new mClienteAsociado();
        public List<mActividades> listaActividades = new List<mActividades>();
        public List<mActividades> listaActividadesDeUsuario = new List<mActividades>();
        
        public List<mActividades> listaActividadesParaAgregar = new List<mActividades>();
        public List<mUsuariosDeCliente> listaDeUsuariosDeCliente = new List<mUsuariosDeCliente>();
        public List<mUsuariosDeCliente> listaDeUsuariosParaAgregar = new List<mUsuariosDeCliente>();
        public List<mDatosUsuariosDeClienteDeInforme> listadeDatosUsuariosDeClienteDeInforme = new List<mDatosUsuariosDeClienteDeInforme>();
        public List<mUsuariosParaEditar> listaTodosLosUsuarios = new List<mUsuariosParaEditar>();
        public List<mDepartamentos> listaDepartamentos = new List<mDepartamentos>();
        public mUsuariosDeInforme usuarioAAgregar = new mUsuariosDeInforme();
        public mActividadAsociadaParaAgregar actividadAAgregar = new mActividadAsociadaParaAgregar();
        public List<mObservaciones> listaDeObservaciones = new List<mObservaciones>();
        public mPerfil perfilActual = new mPerfil();
        public mProyectos proyectoAsociado = new mProyectos();
        public int total_horas_cobradas = 0;
        public int total_horas_no_cobradas = 0;
        public string usuarioActual { get; set; } = string.Empty;
        public string esquema = string.Empty;
        public string rol = string.Empty;
       
        private ElementReference informeButton;

        private bool cargaInicial = false;
        private string mensajeConsecutivo;
        private bool informeGuardado = false;
        private bool activarBotonFinalizar = false;
        private bool informeActualizado = false;
        private bool usuarioAutorizado = true;
        private string[] elementos1 = new string[] { ".el-layout", ".header-col-left", ".div-observaciones", ".btn-agregar-usuario" };
        private string[] elementos2 = new string[] { ".el-layout", ".header-col-right", ".footer-horas", ".footer-col-right" , ".btn-agregar-actividad" };
       
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
                    InformesService.Informe = await InformesService.ObtenerInforme(Consecutivo, esquema);
                    if (InformesService.Informe != null)
                    {
                        informe = InformesService.Informe;
                        await AuthenticationStateProvider.GetAuthenticationStateAsync();
                        await ProyectosService.ObtenerProyecto(esquema, informe.numero_proyecto);
                        if (ProyectosService.ProyectoAsociado != null)
                        {
                            proyectoAsociado = ProyectosService.ProyectoAsociado;
                        }
                        if (VerificarUsuarioAutorizado())
                        {
                            
                            await AuthenticationStateProvider.GetAuthenticationStateAsync();
                            ClientesService.ClienteAsociado = await ClientesService.ObtenerClienteAsociado(proyectoAsociado.codigo_cliente, esquema);
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

    

        private async Task SubmitInforme()
        {
            await JS.InvokeVoidAsync("clickButton", informeButton);
        }

       
        public bool VerificarUsuarioAutorizado()
        {
            
            if(perfilActual.cod_cliente == proyectoAsociado.codigo_cliente || rol == "Admin")
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
                foreach (var actividad in informe.listaActividadesAsociadas)
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
                foreach (var actividad in informe.listaActividadesAsociadas)
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
                foreach (var actividad in informe.listaActividadesAsociadas)
                {
                    if (actividad.Id == actividadId)
                    {
                        actividad.codigo_actividad = e.Value.ToString();
                        actividad.nombre_actividad = listaActividades.Where(a => a.codigo == actividad.codigo_actividad).Select(a => a.Actividad).First();
                    }
                }
            }
        }
        private void CambioFechaActividad(ChangeEventArgs e, string actividadId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                foreach (var actividad in informe.listaActividadesAsociadas)
                {
                    if (actividad.Id == actividadId)
                    {
                        actividad.fecha = e.Value.ToString();
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

       

        private async Task ActualizarInformeAsociado()
        {
            informeGuardado = false;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            bool resultadoActualizar = await InformesService.ActualizarInforme(informe, esquema);
            if (resultadoActualizar)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await InformesService.ObtenerInforme(Consecutivo, esquema);
                informe = InformesService.Informe;
                
                if (!activarBotonFinalizar)
                {
                    await AlertasService.SwalExito("Se han guardado los cambios");
                }
                else
                {
                    await CambiarEstadoInformeGuardado(true);
                }
            }
            else
            {
                if (activarBotonFinalizar)
                {
                    await CambiarEstadoInformeGuardado(true);
                }
            }


        }
       
        private void RefrescarTotalHoras()
        {
            try
            {
                total_horas_cobradas = informe.listaActividadesAsociadas.Sum(act => int.Parse(act.horas_cobradas));
                total_horas_no_cobradas = informe.listaActividadesAsociadas.Sum(act => int.Parse(act.horas_no_cobradas));
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
            informe.listaActividadesAsociadas = ActividadesService.ListaActividadesAsociadas;
            RefrescarTotalHoras();
            if(rol == "Admin")
            {
                listaActividadesParaAgregar = listaActividades.Where(actividad => !informe.listaActividadesAsociadas.Any(actividadAsociada => actividadAsociada.codigo_actividad == actividad.codigo) && actividad.estado == "Activo").ToList();
            }
            else
            {
                listaActividadesParaAgregar = listaActividadesDeUsuario.Where(actividad => !informe.listaActividadesAsociadas.Any(actividadAsociada => actividadAsociada.codigo_actividad == actividad.codigo && actividad.estado == "Activo")).ToList();
            }
            foreach (var actividad in listaActividadesParaAgregar)
            {
                string fecha = actividad.fecha_actualizacion.Replace("  ", " ");
                actividad.FechaActualizacionDateTime = DateTime.ParseExact(fecha, "MMM d yyyy h:mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None);
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
            listaDeUsuariosParaAgregar = listaDeUsuariosDeCliente.Where(usuario => !listadeDatosUsuariosDeClienteDeInforme.Any(usuarioDeInforme => usuarioDeInforme.codigo_usuario == usuario.codigo)).ToList();
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
                await SubmitInforme();
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
                activarBotonFinalizar = false;
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
            foreach (var actividad in informe.listaActividadesAsociadas)
            {
                if(actividad.codigo_actividad == codigoActividad && !string.IsNullOrEmpty(texto))
                {
                    actividad.nombre_actividad = texto;
                }
            }
            StateHasChanged();
            await Task.Delay(100);
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
        private async Task EliminarUsuarioDeClienteDeInforme(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo) && !string.IsNullOrEmpty(esquema))
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                bool resultadoEliminar = await UsuariosService.EliminarUsuarioDeClienteDeInforme(codigo, esquema);
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
                    await AlertasService.SwalError("Ocurrió un Error vuelta a intentarlo");
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

        bool activarModalAgregarUsuario = false;
        bool activarModalAgregarActividad = false;

        async Task ClickHandlerAgregarUsuario(bool activar)
        {
            if (!activar)
            {
                await RefrescarListaDeUsuariosDeInforme();
            }
            activarModalAgregarUsuario = activar;
            StateHasChanged();
        }

        async Task ClickHandlerAgregarActividad(bool activar)
        {
            if (!activar)
            {
                await RefrescarListaDeActividadesAsociadas();
            }
            activarModalAgregarActividad = activar;
            StateHasChanged();
        }

        private async Task InvalidSubmit(EditContext modeloContext)
        {
            activarBotonFinalizar = false;
            await ActivarScrollBarDeErrores();
            var mensajesDeValidaciones = modeloContext.GetValidationMessages();
            string mensaje = mensajesDeValidaciones.First();
            await AlertasService.SwalError(mensaje);
        }
    }
}
