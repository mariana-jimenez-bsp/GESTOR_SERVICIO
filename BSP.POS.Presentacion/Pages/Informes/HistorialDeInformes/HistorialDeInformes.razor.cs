using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Departamentos;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Models.Reportes;
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


        public List<mInformesFinalizados> listaDeInformesFinalizados = new List<mInformesFinalizados>();
        public mInformesFinalizados informeAsociadoSeleccionado = new mInformesFinalizados();

        public List<mActividades> listaDeActividades = new List<mActividades>();
       
        public List<mDepartamentos> listaDepartamentos = new List<mDepartamentos>();
        public List<string> listaCorreosExtras { get; set; } = new List<string>();
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
                    if (datosUsuario.rol == "Admin")
                    {
                        await AuthenticationStateProvider.GetAuthenticationStateAsync();
                        await InformesService.ObtenerListaDeInformesFinalizados(esquema);
                    }
                    else
                    {
                        await AuthenticationStateProvider.GetAuthenticationStateAsync();
                        await InformesService.ObtenerListaDeInformesDeClienteFinalizados(datosUsuario.cod_cliente, esquema);
                    }
                    if (InformesService.ListaInformesFinalizados != null)
                    {
                        listaDeInformesFinalizados = InformesService.ListaInformesFinalizados;

                        if (listaDeInformesFinalizados.Any())
                        {
                            ObtenerFechaMasAltaInformes();
                            ObtenerFechaMasBajaInformes();
                        }


                    }
                    
                }
            }

        }
        
        public async Task cambioSeleccion(string consecutivo)
        {
            foreach (var informe in listaDeInformesFinalizados)
            {
                if (informe.consecutivo == consecutivo)
                {
                    informe.informeSeleccionado = "informe-hover";
                    informe.imagenSeleccionada = "imagen-hover";

                    informeAsociadoSeleccionado = informe;
                    
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
            mObjetoReporte objetoReporte = new mObjetoReporte();
            objetoReporte.reporte = reporte;
            objetoReporte.listaCorreosExtras = listaCorreosExtras;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            bool validar = await InformesService.EnviarCorreoDeReporteDeInforme(esquema, informeAsociadoSeleccionado.consecutivo, objetoReporte);
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
            DateTime fechaTemporal = listaDeInformesFinalizados.OrderBy(i => i.FechaConsultoriaDateTime).Select(i => i.FechaConsultoriaDateTime).First();
            fechaMin = fechaTemporal.ToString("yyyy-MM-dd");
            fechaInicioDateTime = fechaTemporal;
        }

        private void ObtenerFechaMasAltaInformes()
        {
            DateTime fechaTemporal = listaDeInformesFinalizados.OrderByDescending(i => i.FechaConsultoriaDateTime).Select(i => i.FechaConsultoriaDateTime).First();
            fechaMax = fechaTemporal.ToString("yyyy-MM-dd");
            fechaFinalDateTime = fechaTemporal;
        }
        bool activarModalEnviarCorreo = false;
        async Task ClickHandleEnviarCorreo(bool activar)
        {
            if (!string.IsNullOrEmpty(informeAsociadoSeleccionado.consecutivo))
            {
                activarModalEnviarCorreo = activar;
            }
            else
            {
                await AlertasService.SwalAdvertencia("Debe seleccionar un Informe");
            }

        }
        private async Task RecibirListaCorreosExtras(List<string> lista)
        {
            listaCorreosExtras = lista;
            await SwalEnviandoCorreo();
        }
        private async Task SwalEnviandoCorreo()
        {
            bool resultadoCorreo = false;
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = " <span class=\"spinner-border spinner-border-sm\" aria-hidden=\"true\"></span>\r\n  <span role=\"status\">Enviando...</span>",
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

        private async Task SwalDescargandoReporte()
        {


            if (!string.IsNullOrEmpty(informeAsociadoSeleccionado.consecutivo))
            {

                bool resultadoDescargar = false;
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = " <span class=\"spinner-border spinner-border-sm\" aria-hidden=\"true\"></span>\r\n  <span role=\"status\">Descargando...</span>",
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

       
    }
}
