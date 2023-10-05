﻿using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Pages.Home;
using BSP.POS.Presentacion.Services.Reportes;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace BSP.POS.Presentacion.Pages.Informes.HistorialDeInformes
{
    public partial class HistorialDeInformes : ComponentBase
    {
        public string usuarioActual { get; set; } = string.Empty;
        public string esquema = string.Empty;
        public mPerfil datosUsuario = new mPerfil();
        public List<mUsuariosDeClienteDeInforme> informesDeUsuario = new List<mUsuariosDeClienteDeInforme>();
        public List<mUsuariosDeClienteDeInforme> informesDeUsuarioFinalizados = new List<mUsuariosDeClienteDeInforme>();
        public List<mInformes> informesAsociados = new List<mInformes>();
        public mClienteAsociado clienteAsociado = new mClienteAsociado();
        public mInformeAsociado informeAsociadoSeleccionado = new mInformeAsociado();
        public mUsuariosDeClienteDeInforme informeDeUsuarioAsociado = new mUsuariosDeClienteDeInforme();
        public List<mActividades> listaDeActividades = new List<mActividades>();
        public List<mUsuariosDeCliente> listaDeUsuariosDeCliente = new List<mUsuariosDeCliente>();
        private string correoEnviado;
        public string mensajeError;
        private bool EsConsecutivoNull = false;
        private bool todosLosUsuariosAprobados = false;
        private DateTime fechaInicioDateTime = DateTime.MinValue;
        private DateTime fechaFinalDateTime = DateTime.MinValue;
        private string fechaInicio = string.Empty;
        private string fechaFinal = string.Empty;
        private string fechaMax = DateTime.MinValue.ToString("yyyy-MM-dd");
        private string fechaMin = DateTime.MinValue.ToString("yyyy-MM-dd");
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            usuarioActual = user.Identity.Name;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();

            if (!string.IsNullOrEmpty(usuarioActual) && !string.IsNullOrEmpty(esquema))
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await UsuariosService.ObtenerPerfil(usuarioActual, esquema);
            }

            if (UsuariosService.Perfil != null)
            {
                datosUsuario = UsuariosService.Perfil;
                if (!string.IsNullOrEmpty(datosUsuario.codigo))
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await UsuariosService.ObtenerListaDeInformesDeUsuario(datosUsuario.codigo, esquema);
                    if (UsuariosService.ListaDeInformesDeUsuarioAsociados != null)
                    {
                        informesDeUsuario = UsuariosService.ListaDeInformesDeUsuarioAsociados;
                        await AuthenticationStateProvider.GetAuthenticationStateAsync();
                        await InformesService.ObtenerListaDeInformesAsociados(datosUsuario.cod_cliente, esquema);
                        if (InformesService.ListaInformesAsociados != null)
                        {
                            informesAsociados = InformesService.ListaInformesAsociados;
                            informesDeUsuarioFinalizados = UsuariosService.ListaDeInformesDeUsuarioAsociados
                            .Where(usuario =>
                                informesAsociados.Any(informe => informe.estado == "Finalizado"))
                            .ToList();
                            foreach (var informe in informesDeUsuarioFinalizados)
                            {
                                informe.fecha_consultoria = informesAsociados.Where(i => i.consecutivo == informe.consecutivo_informe).Select(c => c.fecha_consultoria).First();
                            }
                            if (informesDeUsuarioFinalizados.Any())
                            {
                                ObtenerFechaMasAltaInformes();
                                ObtenerFechaMasBajaInformes();
                            }
                        }

                    }
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    ClientesService.ClienteAsociado = await ClientesService.ObtenerClienteAsociado(datosUsuario.cod_cliente, esquema);
                    if (ClientesService.ClienteAsociado != null)
                    {
                        clienteAsociado = ClientesService.ClienteAsociado;
                        await AuthenticationStateProvider.GetAuthenticationStateAsync();
                        listaDeUsuariosDeCliente = await UsuariosService.ObtenerListaDeUsuariosDeClienteAsociados(esquema, clienteAsociado.CLIENTE);
                    }
                }
            }

        }
        private async Task<bool> VerificarAprobacionesUsuarios()
        {
            bool aprobado = false;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerListaUsuariosDeClienteDeInforme(informeAsociadoSeleccionado.consecutivo, esquema);
            if (UsuariosService.ListaUsuariosDeClienteDeInforme.Any())
            {
                foreach (var usuario in UsuariosService.ListaUsuariosDeClienteDeInforme)
                {
                    if (usuario.aceptacion == "1")
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

        public async Task cambioSeleccion(string consecutivo)
        {
            foreach (var informe in informesDeUsuarioFinalizados)
            {
                if (informe.consecutivo_informe == consecutivo)
                {
                    informe.informeSeleccionado = "informe-hover";
                    informe.imagenSeleccionada = "imagen-hover";
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    informeAsociadoSeleccionado = await InformesService.ObtenerInformeAsociado(consecutivo, esquema);
                    if (informeAsociadoSeleccionado != null)
                    {
                        informeDeUsuarioAsociado = informe;

                    }
                }
                else
                {
                    informe.informeSeleccionado = "";
                    informe.imagenSeleccionada = "eyelash-background";
                }
            }
        }
        private async Task ReenviarCorreo()
        {
            EsConsecutivoNull = false;
            bool verificarAprobacion = false;
            todosLosUsuariosAprobados = false;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (!string.IsNullOrEmpty(informeAsociadoSeleccionado.consecutivo))
            {
                verificarAprobacion = await VerificarAprobacionesUsuarios();
                if (!verificarAprobacion)
                {
                    correoEnviado = null;

                    mObjetosParaCorreoAprobacion objetoParaCorreo = new mObjetosParaCorreoAprobacion();

                    objetoParaCorreo.informe = informeAsociadoSeleccionado;
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await ActividadesService.ObtenerListaDeActividadesAsociadas(informeAsociadoSeleccionado.consecutivo, esquema);
                    if (ActividadesService.ListaActividadesAsociadas != null)
                    {
                        objetoParaCorreo.listaActividadesAsociadas = ActividadesService.ListaActividadesAsociadas;
                    }
                    try
                    {
                        objetoParaCorreo.total_horas_cobradas = objetoParaCorreo.listaActividadesAsociadas.Sum(act => int.Parse(act.horas_cobradas));
                        objetoParaCorreo.total_horas_no_cobradas = objetoParaCorreo.listaActividadesAsociadas.Sum(act => int.Parse(act.horas_no_cobradas));
                    }
                    catch
                    {
                        objetoParaCorreo.total_horas_cobradas = 0;
                        objetoParaCorreo.total_horas_no_cobradas = 0;
                    }
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await ActividadesService.ObtenerListaDeActividades(esquema);
                    if (ActividadesService.ListaActividades != null)
                    {
                        listaDeActividades = ActividadesService.ListaActividades;

                    }
                    foreach (var actividad in objetoParaCorreo.listaActividadesAsociadas)
                    {
                        actividad.nombre_actividad = listaDeActividades.Where(a => a.codigo == actividad.codigo_actividad).Select(c => c.Actividad).First();
                    }
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await UsuariosService.ObtenerListaUsuariosDeClienteDeInforme(informeAsociadoSeleccionado.consecutivo, esquema);
                    if (UsuariosService.ListaUsuariosDeClienteDeInforme != null)
                    {
                        objetoParaCorreo.listadeUsuariosDeClienteDeInforme = UsuariosService.ListaUsuariosDeClienteDeInforme;
                        foreach (var usuario in objetoParaCorreo.listadeUsuariosDeClienteDeInforme)
                        {
                            usuario.nombre_usuario = listaDeUsuariosDeCliente.Where(u => u.codigo == usuario.codigo_usuario_cliente).Select(c => c.usuario).First();
                            usuario.departamento_usuario = listaDeUsuariosDeCliente.Where(u => u.codigo == usuario.codigo_usuario_cliente).Select(c => c.departamento).First();
                        }
                    }
                    objetoParaCorreo.ClienteAsociado = clienteAsociado;
                    objetoParaCorreo.esquema = esquema;
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await ObservacionesService.ObtenerListaDeObservacionesDeInforme(informeAsociadoSeleccionado.consecutivo, esquema);
                    if (ObservacionesService.ListaDeObservacionesDeInforme != null)
                    {
                        objetoParaCorreo.listaDeObservaciones = ObservacionesService.ListaDeObservacionesDeInforme;
                        foreach (var observacion in objetoParaCorreo.listaDeObservaciones)
                        {
                            await AuthenticationStateProvider.GetAuthenticationStateAsync();
                            await UsuariosService.ObtenerElUsuarioParaEditar(esquema, observacion.codigo_usuario);
                            if (UsuariosService.UsuarioParaEditar != null)
                            {
                                observacion.nombre_usuario = UsuariosService.UsuarioParaEditar.nombre;
                            }
                        }
                    }
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    bool validar = await InformesService.EnviarCorreoDeAprobacionDeInforme(objetoParaCorreo);
                    if (validar)
                    {
                        correoEnviado = "Correo Enviado";
                    }
                    else
                    {
                        correoEnviado = "Error";
                    }
                }
                else
                {
                    todosLosUsuariosAprobados = true;
                }
               
            }
            else
            {
                EsConsecutivoNull = true;
            }
        }
        private byte[] pdfContent;

        private async Task DescargarReporte()
        {
            mensajeError = null;
            EsConsecutivoNull = false;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            try
            {
                if (!string.IsNullOrEmpty(informeAsociadoSeleccionado.consecutivo))
                {
                    pdfContent = await ReportesService.GenerarReporteDeInforme(esquema, informeAsociadoSeleccionado.consecutivo);

                    var fileName = "ReporteInforme_" + informeAsociadoSeleccionado.consecutivo + ".pdf";
                    var data = Convert.ToBase64String(pdfContent);
                    var url = $"data:application/pdf;base64,{data}";

                    await JSRuntime.InvokeVoidAsync("guardarDocumento", fileName, url);
                    //await JSRuntime.InvokeVoidAsync("imprimirDocumento", url, fileName);
                }
                else
                {
                    EsConsecutivoNull = true;
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                mensajeError = "Ocurrió un Error vuelva a intentarlo";
            }

        }
        [Parameter]
        public string textoRecibido { get; set; } = string.Empty;

        private Task RecibirTexto(string texto)
        {
            textoRecibido = texto;
            return Task.CompletedTask;
        }

        private void IrAMisInformes()
        {

            navigationManager.NavigateTo($"Informes/MisInformes");
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

        private void ObtenerFechaMasBajaInformes()
        {
            DateTime fechaTemporal = informesDeUsuarioFinalizados.OrderBy(i => i.FechaConsultoriaDateTime).Select(i => i.FechaConsultoriaDateTime).First();
            fechaMin = fechaTemporal.ToString("yyyy-MM-dd");
            fechaInicioDateTime = fechaTemporal;
        }

        private void ObtenerFechaMasAltaInformes()
        {
            DateTime fechaTemporal = informesDeUsuarioFinalizados.OrderByDescending(i => i.FechaConsultoriaDateTime).Select(i => i.FechaConsultoriaDateTime).First();
            fechaMax = fechaTemporal.ToString("yyyy-MM-dd");
            fechaFinalDateTime = fechaTemporal;
        }
    }
}
