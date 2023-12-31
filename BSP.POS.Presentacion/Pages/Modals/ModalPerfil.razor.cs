﻿using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Models.Permisos;
using System;
using System.Security.Claims;
using BSP.POS.Presentacion.Models.Clientes;
using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.JSInterop;
using BSP.POS.Presentacion.Pages.Usuarios.Usuarios;
using System.Text.Json;

namespace BSP.POS.Presentacion.Pages.Modals
{
    public partial class ModalPerfil : ComponentBase
    {


        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string esquema { get; set; } = string.Empty;
        public string rol { get; set; } = string.Empty;
        public mPerfil perfil { get; set; } = new mPerfil();
        
        public string usuarioOriginal = string.Empty;
        public string claveOriginal = string.Empty;
        public string correoOriginal = string.Empty;
        bool repetido = false;
        public string correoRepite = string.Empty;
        public string mensajeCorreoRepite = string.Empty;
        public string usuarioRepite = string.Empty;
        public string mensajeUsuarioRepite = string.Empty;
        public mClienteAsociado clienteAsociado = new mClienteAsociado();
        public List<mClientes> listaDeClientes = new List<mClientes>();
        public string tipo { get; set; } = string.Empty;
        public string mensajeError;
        private string mensajeEsquema = string.Empty;
        [Parameter] public EventCallback<bool> perfilActualizado { get; set; }
        [Parameter] public EventCallback<bool> perfilDescartado { get; set; }
        private bool cargaInicial = false;
        protected override async Task OnInitializedAsync()
        {

            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            Usuario = user.Identity.Name;
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            if (!string.IsNullOrEmpty(Usuario) && !string.IsNullOrEmpty(esquema))
            {
                await UsuariosService.ObtenerPerfil(Usuario, esquema);
            }
           
            if (UsuariosService.Perfil != null)
            {
                perfil = UsuariosService.Perfil;
                claveOriginal = UsuariosService.DesencriptarClave(perfil.clave);
                perfil.claveDesencriptada = claveOriginal;
                correoOriginal = perfil.correo;
                usuarioOriginal = perfil.usuario;
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ClientesService.ObtenerListaClientes(esquema);
                if(ClientesService.ListaClientes != null)
                {
                    listaDeClientes = ClientesService.ListaClientes;
                }
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                clienteAsociado = await ClientesService.ObtenerClienteAsociado(perfil.cod_cliente, esquema);

    
            }
            cargaInicial = true;

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
                perfil.claveDesencriptada = e.Value.ToString();
                perfil.clave = UsuariosService.EncriptarClave(perfil.claveDesencriptada);
            }
            else
            {
                perfil.clave = null;
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
        private void CambioCodigoCliente(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                perfil.cod_cliente = e.Value.ToString();
            }
        }

        private void CambioTelefono(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                perfil.telefono = e.Value.ToString();
            }
        }
        private async Task VerificarCorreoYUsuarioExistente()
        {

                mensajeUsuarioRepite = null;
                mensajeCorreoRepite = null;
                if (usuarioOriginal.ToLower() != perfil.usuario.ToLower())
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    usuarioRepite = await UsuariosService.ValidarUsuarioExistente(esquema, perfil.usuario);

                    if (!string.IsNullOrEmpty(usuarioRepite))
                    {
                        mensajeUsuarioRepite = "El usuario ya existe";
                        repetido = true;
       
                    }

                }
                if (correoOriginal.ToLower() != perfil.correo.ToLower())
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    correoRepite = await UsuariosService.ValidarCorreoExistente(esquema, perfil.correo);
                    if (!string.IsNullOrEmpty(correoRepite))
                    {
                        mensajeCorreoRepite = "El correo ya existe";
                        repetido = true;
              
                    }
                }
            
        }
        private SelectPermisos selectPermisosComponente;
        private SelectEsquemas selectEsquemasComponente;
        private async Task ActualizarPerfil()
        {
            mensajeError = null;
            mensajeEsquema = null;
            try
            {
                int resultadoPermisos = 0;
                int ResultadoEsquemas = 0;
                bool resultaPerfil = false;
                repetido = false;
                if(selectEsquemasComponente.cantidadEsquemas >= 1)
                {
                    await VerificarCorreoYUsuarioExistente();
                    if (!repetido)
                    {

                        await AuthenticationStateProvider.GetAuthenticationStateAsync();
                        if (rol == "Admin")
                        {
                            resultadoPermisos = await selectPermisosComponente.ActualizarListaDePermisos("");
                            ResultadoEsquemas = await selectEsquemasComponente.ActualizarListaDeEsquema("");
                        }
                        else
                        {
                            resultadoPermisos = 1;
                            ResultadoEsquemas = 1;
                        }

                        string claveDesencriptada = perfil.claveDesencriptada;
                        await AuthenticationStateProvider.GetAuthenticationStateAsync();
                        resultaPerfil = await UsuariosService.ActualizarPefil(perfil, usuarioOriginal, claveOriginal, correoOriginal, esquema);
                        if (resultaPerfil && resultadoPermisos != 0 && ResultadoEsquemas != 0)
                        {
                            if (usuarioOriginal.ToLower() != perfil.usuario.ToLower() || correoOriginal.ToLower() != perfil.correo.ToLower() || claveOriginal != claveDesencriptada || resultadoPermisos == 2 || ResultadoEsquemas == 2)
                            {
                                await CloseModal();
                                StateHasChanged();
                                await SwalExito("Se ha actualizado el perfil");
                            }
                            else
                            {
                                await CloseModal();
                                StateHasChanged();
                                await perfilActualizado.InvokeAsync(true);
                            }
                        }
                        else
                        {
                            mensajeError = "Ocurrío un Error vuelva a intentarlo";
                        }

                    }
                    else
                    {
                        await ActivarScrollBarErroresRepite();
                    }
                }
                else
                {
                    mensajeEsquema = "Debe seleccionar al menos 1 esquema";
                }

            }
            catch (Exception)
            {

                mensajeError = "Ocurrío un Error vuelva a intentarlo";
            }
            
        }
        private void OpenModal()
        {
            ActivarModal = true;
        }
        private async Task DescartarCambios()
        {
            await CloseModal();
            await perfilDescartado.InvokeAsync(true);
        }
        private async Task CloseModal()
        {
            await OnClose.InvokeAsync(false);

        }

        private async Task SalirConLaX()
        {
            await OnClose.InvokeAsync(false);
        }
        private bool mostrarClave = false;

        private void CambiarEstadoMostrarClave(bool estado)
        {
            mostrarClave = estado;
        }
        private async Task ActivarScrollBarDeErrores()
        {
            StateHasChanged();
            await Task.Delay(100);
            var isValid = await JSRuntime.InvokeAsync<bool>("HayErroresValidacion", ".validation-message");

            if (!isValid)
            {
                // Si hay errores de validación, activa el scrollbar
                await JSRuntime.InvokeVoidAsync("ActivarScrollViewValidacion", ".validation-message");
            }
        }
        private async Task ActivarScrollBarErroresRepite()
        {
            if (!string.IsNullOrEmpty(mensajeCorreoRepite) || !string.IsNullOrEmpty(mensajeUsuarioRepite))
            {
                StateHasChanged();
                await Task.Delay(100);


                var isValid = await JSRuntime.InvokeAsync<bool>("HayErroresValidacion", ".mensaje-repite");


                // Si hay errores de validación, activa el scrollbar
                await JSRuntime.InvokeVoidAsync("ActivarScrollViewValidacion", ".mensaje-repite");

            }
        }
        private async Task SwalExito(string mensajeAlerta)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Éxito",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Success,
                ShowCancelButton = false,
                ConfirmButtonText = "Ok"
            }).ContinueWith(async swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (result.IsConfirmed || result.IsDismissed)
                {
                    
                    navigationManager.NavigateTo($"login", forceLoad: true);
                    await localStorageService.RemoveItemAsync("token");
                }
            });
        }

    }
}
