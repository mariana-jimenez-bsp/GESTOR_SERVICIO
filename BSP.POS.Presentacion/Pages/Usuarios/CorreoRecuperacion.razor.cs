﻿using BSP.POS.Presentacion.Models.Licencias;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Services.Usuarios;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BSP.POS.Presentacion.Pages.Usuarios
{
    public partial class CorreoRecuperacion: ComponentBase
    {
        public mDatosLicencia licencia = new mDatosLicencia();
        private bool cargaInicial = false;
        private bool licenciaActiva = false;
        private bool mismaMacAdress = true;
        protected override async Task OnInitializedAsync()
        {
            await ValidarLicencia();
            cargaInicial = true;

        }

        private async Task ValidarLicencia()
        {
            licenciaActiva = false;
            mismaMacAdress = true;
            await LicenciasService.ObtenerDatosDeLicencia();
            if (LicenciasService.licencia != null)
            {
                licencia = LicenciasService.licencia;
                if (licencia.FechaFin > DateTime.Now)
                {
                    licenciaActiva = true;
                    StateHasChanged();
                    if (licencia.FechaAviso < DateTime.Now)
                    {
                        await AlertasService.SwalAdvertencia("Licencia Próxima a vencer");
                    }
                    if (!licencia.MacAddressIguales)
                    {
                        mismaMacAdress = false;
                        StateHasChanged();
                        await AlertasService.SwalError("La MacAddress no es la misma registrada");
                    }
                }
                else
                {
                    await AlertasService.SwalError("Licencia no activa, debe renovarla");
                }
            }
        }
        public mTokenRecuperacion tokenRecuperacion { get; set; } = new mTokenRecuperacion();
        public string mensaje { get; set; } = string.Empty;
        public bool CorreoEnviado = false; 
        private void ValorCorreo(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                tokenRecuperacion.correo = e.Value.ToString();
            }
        }

        private void ValorEsquema(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                tokenRecuperacion.esquema = e.Value.ToString();
            }
        }

        private async Task<bool> EnviarCorreo()
        {    
            bool validar = await LoginService.EnviarCorreoRecuperarClave(tokenRecuperacion);
            if (validar)
            {

                return true;
            }
            return false;
        }
        private async Task IrAtras()
        {
            await JSRuntime.InvokeVoidAsync("history.back");
        }
        private async Task SwalEnviandoCorreo()
        {
            mensaje = string.Empty;
            if (licenciaActiva && mismaMacAdress)
            {
                string verificar = await LoginService.ValidarCorreoCambioClave(tokenRecuperacion.esquema, tokenRecuperacion.correo);
                if (verificar != null)
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
                            resultadoCorreo = await EnviarCorreo();
                            await Swal.CloseAsync();

                        }),
                        WillClose = new SweetAlertCallback(Swal.CloseAsync)

                    });

                    if (resultadoCorreo)
                    {
                        CorreoEnviado = true;
                    }
                }
                else
                {
                    mensaje = "El correo no existe";
                }
                

            }
            else
            {
                await ValidarLicencia();
            }


        }
    }
}
