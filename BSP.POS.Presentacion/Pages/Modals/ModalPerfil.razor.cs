using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Models.Permisos;
using System;

namespace BSP.POS.Presentacion.Pages.Modals
{
    public partial class ModalPerfil : ComponentBase
    {


        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string esquema { get; set; } = string.Empty;
        public mPerfil perfil { get; set; } = new mPerfil();
        public List<mPermisos> todosLosPermisos { get; set; } = new List<mPermisos>();
        public List<mPermisosAsociados> permisosAsociados { get; set; } = new List<mPermisosAsociados>();
        public string usuarioOriginal = string.Empty;
        public string claveOriginal = string.Empty;
        public string correoOriginal = string.Empty;


        public string tipo { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {

            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            Usuario = user.Identity.Name;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            if (!string.IsNullOrEmpty(Usuario) && !string.IsNullOrEmpty(esquema))
            {
                await UsuariosService.ObtenerPerfil(Usuario, esquema);
            }
           
            if (UsuariosService.Perfil != null)
            {
                perfil = UsuariosService.Perfil;
                claveOriginal = perfil.clave;
                correoOriginal = perfil.correo;
                perfil.clave = string.Empty;
                usuarioOriginal = perfil.usuario;
               
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await PermisosService.ObtenerListaDePermisos(perfil.esquema);
                if (PermisosService.ListaPermisos != null)
                {
                    todosLosPermisos = PermisosService.ListaPermisos;
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await PermisosService.ObtenerListaDePermisosAsociados(perfil.esquema, perfil.id);
                    if (PermisosService.ListaPermisosAsociadados != null)
                    {
                        permisosAsociados = PermisosService.ListaPermisosAsociadados;
                        foreach (var item in todosLosPermisos)
                        {
                            if (permisosAsociados.Any(elPermiso => elPermiso.id_permiso == item.Id))
                            {
                                item.EstadoCheck = true;
                            }
                         }

                    }
                }
               
            }


        }
        private void HandleCheckCambiado(ChangeEventArgs e, string idPermiso)
        {
            var permiso = new mPermisos();
            foreach (var item in todosLosPermisos)
            {
                if(item.Id == idPermiso)
                {
                    item.EstadoCheck = (bool)e.Value;
                    permiso = item;
                }
            }
          

            var permisoEncontrado = permisosAsociados.FirstOrDefault(p => p.id_permiso == permiso.Id);

            if (permiso.EstadoCheck)
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
            if (!sonIguales)
            {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await PermisosService.ActualizarListaPermisosAsociados(permisosAsociados, perfil.id, perfil.esquema);
                
            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ActualizarPefil(perfil, usuarioOriginal, claveOriginal, correoOriginal);
            await CloseModal();
        }
        private void OpenModal()
        {
            ActivarModal = true;
        }

        private async Task CloseModal()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();

            if (!string.IsNullOrEmpty(Usuario) && !string.IsNullOrEmpty(esquema))
            {
                await UsuariosService.ObtenerPerfil(Usuario, esquema);
            }
            perfil = UsuariosService.Perfil;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await PermisosService.ObtenerListaDePermisos(perfil.esquema);
            todosLosPermisos = PermisosService.ListaPermisos;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await PermisosService.ObtenerListaDePermisosAsociados(perfil.esquema, perfil.id);
            permisosAsociados = PermisosService.ListaPermisosAsociadados;
            if (PermisosService.ListaPermisosAsociadados != null)
            {
                permisosAsociados = PermisosService.ListaPermisosAsociadados;
                foreach (var item in todosLosPermisos)
                {
                    if (permisosAsociados.Any(elPermiso => elPermiso.id_permiso == item.Id))
                    {
                        item.EstadoCheck = true;
                    }
                }

            }
            claveOriginal = perfil.clave;
            correoOriginal = perfil.correo;
            perfil.clave = string.Empty;
            usuarioOriginal = perfil.usuario;
            await OnClose.InvokeAsync(false);

        }

    }
}
