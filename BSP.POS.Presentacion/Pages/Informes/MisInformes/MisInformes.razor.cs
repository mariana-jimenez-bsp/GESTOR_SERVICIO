﻿using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Departamentos;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Pages.Home;
using BSP.POS.Presentacion.Pages.Proyectos;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace BSP.POS.Presentacion.Pages.Informes.MisInformes
{
    public partial class MisInformes : ComponentBase
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
        public List<mDepartamentos> listaDepartamentos = new List<mDepartamentos>();
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
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await DepartamentosService.ObtenerListaDeDepartamentos(esquema);
            if (DepartamentosService.listaDepartamentos != null)
            {
                listaDepartamentos = DepartamentosService.listaDepartamentos;
            }
            if (UsuariosService.Perfil != null)
            {
                datosUsuario = UsuariosService.Perfil;
                if (!string.IsNullOrEmpty(datosUsuario.codigo))
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await UsuariosService.ObtenerListaDeInformesDeUsuario(datosUsuario.codigo, esquema);
                    if(UsuariosService.ListaDeInformesDeUsuarioAsociados != null)
                    {
                        informesDeUsuario = UsuariosService.ListaDeInformesDeUsuarioAsociados;
                        await AuthenticationStateProvider.GetAuthenticationStateAsync();
                        await InformesService.ObtenerListaDeInformesAsociados(datosUsuario.cod_cliente, esquema);
                        if(InformesService.ListaInformesAsociados != null)
                        {
                            informesAsociados = InformesService.ListaInformesAsociados;
                            informesDeUsuarioFinalizados = UsuariosService.ListaDeInformesDeUsuarioAsociados
                            .Where(usuario =>
                                informesAsociados.Any(informe =>
                                    usuario.consecutivo_informe == informe.consecutivo && informe.estado == "Finalizado"))
                            .ToList();
                            foreach (var informe in informesDeUsuarioFinalizados)
                            {
                                informe.fecha_consultoria = informesAsociados.Where(i => i.consecutivo == informe.consecutivo_informe).Select(c => c.fecha_consultoria).First();
                                informe.FechaConsultoriaDateTime = DateTime.ParseExact(informe.fecha_consultoria, "yyyy-MM-dd", CultureInfo.InvariantCulture);
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
                    if(ClientesService.ClienteAsociado != null)
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
            await UsuariosService.ObtenerDatosListaUsuariosDeClienteDeInforme(informeAsociadoSeleccionado.consecutivo, esquema);
            if (UsuariosService.ListaDatosUsuariosDeClienteDeInforme.Any())
            {
                foreach (var usuario in UsuariosService.ListaDatosUsuariosDeClienteDeInforme)
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
                if(informe.consecutivo_informe == consecutivo)
                {
                    informe.informeSeleccionado = "informe-hover";
                    informe.imagenSeleccionada = "imagen-hover";
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    informeAsociadoSeleccionado = await InformesService.ObtenerInformeAsociado(consecutivo, esquema);
                    if(informeAsociadoSeleccionado != null)
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
            correoEnviado = null;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (!string.IsNullOrEmpty(informeAsociadoSeleccionado.consecutivo))
            {
                verificarAprobacion = await VerificarAprobacionesUsuarios();
                if (!verificarAprobacion)
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    bool validar = await InformesService.EnviarCorreoDeAprobacionDeInforme(esquema, informeAsociadoSeleccionado.consecutivo);
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
        [Parameter]
        public string textoRecibido { get; set; } = string.Empty;

        private Task RecibirTexto(string texto)
        {
            textoRecibido = texto;
            return Task.CompletedTask;
        }

        private void IrAHistorial()
        {

            navigationManager.NavigateTo($"Informes/HistorialDeInformes");
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

                    var fileName = "ReporteInforme_"+ informeAsociadoSeleccionado.consecutivo + ".pdf";
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
