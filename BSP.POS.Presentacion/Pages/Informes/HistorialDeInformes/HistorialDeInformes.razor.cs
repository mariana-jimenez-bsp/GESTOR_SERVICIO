using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Departamentos;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Pages.Home;
using BSP.POS.Presentacion.Services.Reportes;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;

namespace BSP.POS.Presentacion.Pages.Informes.HistorialDeInformes
{
    public partial class HistorialDeInformes : ComponentBase
    {
        public string usuarioActual { get; set; } = string.Empty;
        public string esquema = string.Empty;
        public mPerfil datosUsuario = new mPerfil();
        public List<mUsuariosDeInforme> informesDeUsuario = new List<mUsuariosDeInforme>();
        public List<mUsuariosDeInforme> informesDeUsuarioFinalizados = new List<mUsuariosDeInforme>();
        public List<mInformesDeCliente> informesDeCliente = new List<mInformesDeCliente>();
        public mClienteAsociado clienteAsociado = new mClienteAsociado();
        public mInforme informeAsociadoSeleccionado = new mInforme();
        public mUsuariosDeInforme informeDeUsuarioAsociado = new mUsuariosDeInforme();
        public List<mActividades> listaDeActividades = new List<mActividades>();
        public List<mUsuariosDeCliente> listaDeUsuariosDeCliente = new List<mUsuariosDeCliente>();
        public List<mDepartamentos> listaDepartamentos = new List<mDepartamentos>();
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
                    await UsuariosService.ObtenerListaDeInformesDeUsuarioDeCliente(datosUsuario.cod_cliente, esquema);
                    if (UsuariosService.ListaUsuariosDeInformeDeCliente != null)
                    {
                        informesDeUsuario = UsuariosService.ListaUsuariosDeInformeDeCliente;
                        await AuthenticationStateProvider.GetAuthenticationStateAsync();
                        await InformesService.ObtenerListaDeInformesDeCliente(datosUsuario.cod_cliente, esquema);
                        if (InformesService.ListaInformesDeCliente != null)
                        {
                            informesDeCliente = InformesService.ListaInformesDeCliente;
                            informesDeUsuarioFinalizados = UsuariosService.ListaUsuariosDeInformeDeCliente
                            .Where(usuario =>
                                informesDeCliente.Any(informe => informe.estado == "Finalizado"))
                            .ToList();
                            foreach (var informe in informesDeUsuarioFinalizados)
                            {
                                informe.fecha_consultoria = informesDeCliente.Where(i => i.consecutivo == informe.consecutivo_informe).Select(c => c.fecha_consultoria).First();
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
                    if (ClientesService.ClienteAsociado != null)
                    {
                        clienteAsociado = ClientesService.ClienteAsociado;
                        await AuthenticationStateProvider.GetAuthenticationStateAsync();
                        listaDeUsuariosDeCliente = await UsuariosService.ObtenerListaDeUsuariosDeClienteAsociados(esquema, clienteAsociado.CLIENTE);
                    }
                }
            }

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
                    informeAsociadoSeleccionado = await InformesService.ObtenerInforme(consecutivo, esquema);
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
        private async Task<bool> ReenviarCorreo()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            byte[] reporte = await ReportesService.GenerarReporteDeInforme(esquema, informeAsociadoSeleccionado.consecutivo);
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            bool validar = await InformesService.EnviarCorreoDeReporteDeInforme(esquema, informeAsociadoSeleccionado.consecutivo, reporte);
            if (validar)
            {
                return true;
            }
            else
            {
                return false;
            }
               
        }
        private byte[] pdfContent;

        private async Task<bool> DescargarReporte()
        {
            
            try
            {
                    pdfContent = await ReportesService.GenerarReporteDeInforme(esquema, informeAsociadoSeleccionado.consecutivo);

                    var fileName = "ReporteInforme_" + informeAsociadoSeleccionado.consecutivo + ".pdf";
                    var data = Convert.ToBase64String(pdfContent);
                    var url = $"data:application/pdf;base64,{data}";

                    await JSRuntime.InvokeVoidAsync("guardarDocumento", fileName, url);
                    return true;
                
            }
            catch (Exception)
            {
                return false;
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

        private async Task SwalEnviandoCorreo()
        {

           

            if (!string.IsNullOrEmpty(informeAsociadoSeleccionado.consecutivo))
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
                            resultadoCorreo = await ReenviarCorreo();
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
                    await AlertasService.SwalAviso("Todos los usuarios ya aprobaron el informe seleccionado");
                }


        }

        private async Task SwalDescargandoReporte()
        {


            if (!string.IsNullOrEmpty(informeAsociadoSeleccionado.consecutivo))
            {

                bool resultadoDescargar = false;
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Icon = SweetAlertIcon.Info,
                    Title = "Descargando...",
                    ShowCancelButton = false,
                    ShowConfirmButton = false,
                    AllowOutsideClick = false,
                    AllowEscapeKey = false,
                    DidOpen = new SweetAlertCallback(async () =>
                    {
                        resultadoDescargar = await DescargarReporte();
                        await Swal.CloseAsync();

                    }),
                    WillClose = new SweetAlertCallback(Swal.CloseAsync)

                });

                if (resultadoDescargar)
                {
                    await AlertasService.SwalExito("El reporte se ha descargado");
                }
                else
                {
                    await AlertasService.SwalError("Ocurrió un error. Vuelva a intentarlo.");
                }

            }
            else
            {
                await AlertasService.SwalAdvertencia("Debe seleccionar un Informe");
            }


        }

        private void VerInforme(string consecutivo)
        {

            navigationManager.NavigateTo($"Informe/VerInforme/{consecutivo}");
        }
    }
}
