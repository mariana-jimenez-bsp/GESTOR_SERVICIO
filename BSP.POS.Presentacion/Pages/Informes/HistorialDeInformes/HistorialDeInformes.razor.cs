using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Pages.Home;
using BSP.POS.Presentacion.Services.Reportes;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
            if (!string.IsNullOrEmpty(informeAsociadoSeleccionado.consecutivo))
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
                EsConsecutivoNull = true;
            }
        }
        private byte[] pdfContent;

        private async Task DescargarReporte()
        {
            mensajeError = null;
            EsConsecutivoNull = false;
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
    }
}
