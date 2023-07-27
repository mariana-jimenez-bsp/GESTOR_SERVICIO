using Microsoft.AspNetCore.Components;
using BSP.POS.Presentacion.Models;
using System.Collections.Generic;


namespace BSP.POS.Presentacion.Pages.Modals
{
    public partial class ModalPerfil : ComponentBase
    {


        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public mPerfil perfil { get; set; } = new mPerfil();
        public List<mPermisos> todosLosPermisos { get; set; } = new List<mPermisos>();
        public List<mPermisosAsociados> permisosAsociados { get; set; } = new List<mPermisosAsociados>();
        public string usuarioOriginal = string.Empty;
        public string claveOriginal = string.Empty;

        public string tipo { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {

            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            Usuario = user.Identity.Name;
            await UsuariosService.ObtenerPerfil(Usuario);
            if (UsuariosService.Perfil != null)
            {
                perfil = UsuariosService.Perfil;
                claveOriginal = perfil.clave;
                perfil.clave = string.Empty;
                usuarioOriginal = perfil.usuario;
               
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await PermisosService.ObtenerListaDePermisos(perfil.esquema);
                if (PermisosService.ListaPermisos != null)
                {
                    todosLosPermisos = PermisosService.ListaPermisos;
                    EstadosDeChecks = todosLosPermisos.Select(permiso => new EstadoCheck()).ToList();
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await PermisosService.ObtenerListaDePermisosAsociados(perfil.esquema, perfil.id);
                    if (PermisosService.ListaPermisosAsociadados != null)
                    {
                        permisosAsociados = PermisosService.ListaPermisosAsociadados;
                        for (int i = 0; i < EstadosDeChecks.Count; i++)
                        {
                            if (permisosAsociados.Any(elPermiso => elPermiso.id_permiso == todosLosPermisos[i].Id))
                            {
                                EstadosDeChecks[i].Check = true;
                            }
                         }

                    }
                }
               
            }


        }
        public class EstadoCheck
        {
            public bool Check { get; set; } = false;
            // Agrega aquí las otras propiedades necesarias para tu desplegable
        }

        private List<EstadoCheck> EstadosDeChecks = new List<EstadoCheck>();

        private void HandleCheckCambiado(ChangeEventArgs e, int index)
        {
            EstadosDeChecks[index].Check = (bool)e.Value;

            var permiso = todosLosPermisos[index];
            var permisoEncontrado = permisosAsociados.FirstOrDefault(p => p.id_permiso == permiso.Id);

            if (EstadosDeChecks[index].Check)
            {
                if (permisoEncontrado == null)
                {
                    mPermisosAsociados permisoParaAñadir = new mPermisosAsociados();
                    permisoParaAñadir.id_permiso = permiso.Id;
                    permisosAsociados.Add(permisoParaAñadir);

                }
            }
            else
            {
                if (permisoEncontrado != null)
                {
                    permisosAsociados.Remove(permisoEncontrado);

                }
            }
        }


        private void CambioUsuario(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                perfil.usuario = e.Value.ToString();
            }
        }
        private void CambioClave(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                perfil.clave = e.Value.ToString();
            }
            else
            {
                perfil.clave = string.Empty;
            }
        }
        private void CambioCorreo(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                perfil.correo = e.Value.ToString();
            }
        }
        private void CambioRol(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                perfil.rol = e.Value.ToString();
            }
        }
        private void CambioNombreEmpresa(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                perfil.nombre = e.Value.ToString();
            }
        }

        private void CambioTelefono(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                perfil.telefono = e.Value.ToString();
            }
        }
        private void CambioEsquema(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                perfil.esquema = e.Value.ToString();
            }
        }

        private async Task ActualizarPerfil()
        {

            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await PermisosService.ObtenerListaDePermisosAsociados(perfil.esquema, perfil.id);
            bool sonIguales = PermisosService.ListaPermisosAsociadados.Count == permisosAsociados.Count && PermisosService.ListaPermisosAsociadados.All(permisosAsociados.Contains);
            foreach (var item in PermisosService.ListaPermisosAsociadados)
            {
                Console.WriteLine(item.id_permiso);
            }
            if (!sonIguales)
            {
                try
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await PermisosService.ActualizarListaPermisosAsociados(permisosAsociados, perfil.id, perfil.esquema);
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await PermisosService.ObtenerListaDePermisosAsociados(perfil.esquema, perfil.id);
                    if(PermisosService.ListaPermisosAsociadados != null)
                    {
                        permisosAsociados = PermisosService.ListaPermisosAsociadados;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                
            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ActualizarPefil(perfil, usuarioOriginal, claveOriginal);
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerPerfil(perfil.usuario);
            if (UsuariosService.Perfil != null)
            {
                perfil = UsuariosService.Perfil;
            }
            await CloseModal();
        }
        private void OpenModal()
        {
            ActivarModal = true;
        }

        private async Task CloseModal()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerPerfil(Usuario);
            perfil = UsuariosService.Perfil;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await PermisosService.ObtenerListaDePermisosAsociados(perfil.esquema, perfil.id);
            permisosAsociados = PermisosService.ListaPermisosAsociadados;
            if (PermisosService.ListaPermisosAsociadados != null)
            {
                permisosAsociados = PermisosService.ListaPermisosAsociadados;
                for (int i = 0; i < EstadosDeChecks.Count; i++)
                {
                    if (permisosAsociados.Any(elPermiso => elPermiso.id_permiso == todosLosPermisos[i].Id))
                    {
                        EstadosDeChecks[i].Check = true;
                    }
                }

            }
            perfil.clave = string.Empty;
            await OnClose.InvokeAsync(false);

        }

    }
}
