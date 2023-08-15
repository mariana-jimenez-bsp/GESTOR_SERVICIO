using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Pages.Home;
using Microsoft.AspNetCore.Components;
using System.Security.Cryptography.X509Certificates;

namespace BSP.POS.Presentacion.Pages.Informes.MisInformes
{
    public partial class MisInformes : ComponentBase
    {
        public string usuarioActual { get; set; } = string.Empty;
        public string esquema = string.Empty;
        public mPerfil datosUsuario = new mPerfil();
        public List<mUsuariosDeClienteDeInforme> informesDeUsuario = new List<mUsuariosDeClienteDeInforme>();
        public List<mInformes> informesAsociados = new List<mInformes>();
        public mClienteAsociado clienteAsociado = new mClienteAsociado();
        public mInformeAsociado informeAsociadoSeleccionado = new mInformeAsociado();
        public mUsuariosDeClienteDeInforme informeDeUsuarioAsociado = new mUsuariosDeClienteDeInforme();
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

            if(UsuariosService.Perfil != null)
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
                            foreach (var informe in informesDeUsuario)
                            {
                                informe.fecha_consultoria = informesAsociados.Where(i => i.consecutivo == informe.consecutivo_informe).Select(c => c.fecha_consultoria).First();
                            }
                        }

                    }
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    ClientesService.ClienteAsociado = await ClientesService.ObtenerClienteAsociado(datosUsuario.cod_cliente, esquema);
                    if(ClientesService.ClienteAsociado != null)
                    {
                        clienteAsociado = ClientesService.ClienteAsociado;
                    }
                }
            }

        }

        public async Task cambioSeleccion(string consecutivo)
        {
            foreach (var informe in informesDeUsuario)
            {
                if(informe.consecutivo_informe == consecutivo)
                {
                    informe.informeSeleccionado = "informe-hover";
                    informe.imagenSeleccionada = "imagen-hover";
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
        private async Task EnviarCorreosAClientes()
        {
            mObjetosParaCorreoAprobacion objetoParaCorreo = new mObjetosParaCorreoAprobacion();
            
            objetoParaCorreo.informe = informeAsociadoSeleccionado;
            //objetoParaCorreo.total_horas_cobradas = total_horas_cobradas;
            //objetoParaCorreo.total_horas_no_cobradas = total_horas_no_cobradas;
            //objetoParaCorreo.listaActividadesAsociadas = listaActividadesAsociadas;
            //foreach (var actividad in objetoParaCorreo.listaActividadesAsociadas)
            //{
            //    actividad.nombre_actividad = listaActividades.Where(a => a.codigo == actividad.codigo_actividad).Select(c => c.Actividad).First();
            //}
            //objetoParaCorreo.listadeUsuariosDeClienteDeInforme = listadeUsuariosDeClienteDeInforme;
            //objetoParaCorreo.ClienteAsociado = ClienteAsociado;
            //objetoParaCorreo.esquema = esquema;
            //objetoParaCorreo.listaDeObservaciones = listaDeObservaciones;
            //await AuthenticationStateProvider.GetAuthenticationStateAsync();
            //bool validar = await InformesService.EnviarCorreoDeAprobacionDeInforme(objetoParaCorreo);
        }
    }
}
