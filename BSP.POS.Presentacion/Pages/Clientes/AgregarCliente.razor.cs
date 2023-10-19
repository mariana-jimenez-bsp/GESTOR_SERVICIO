using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.ItemsCliente;
using BSP.POS.Presentacion.Models.Lugares;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using CurrieTechnologies.Razor.SweetAlert2;

namespace BSP.POS.Presentacion.Pages.Clientes
{
    public partial class AgregarCliente: ComponentBase
    {
        public string esquema = string.Empty;
        public string usuarioActual = string.Empty;
        public List<mLugares> listaPaises = new List<mLugares>();
        public List<mLugares> listaProvincias = new List<mLugares>();
        public List<mLugares> listaCantones = new List<mLugares>();
        public List<mLugares> listaDistritos = new List<mLugares>();
        public List<mLugares> listaBarrios = new List<mLugares>();
        public List<mItemsCliente> listaCondicionesPago = new List<mItemsCliente>();
        public List<mItemsCliente> listaNivelesPrecio = new List<mItemsCliente>();
        public List<mItemsCliente> listaTiposImpuestos = new List<mItemsCliente>();
        public List<mItemsCliente> listaTiposNit = new List<mItemsCliente>();
        public List<mTarifa> listaTiposTarifasImpuesto = new List<mTarifa>();
        public List<mClienteContado> listaClientesCorporaciones = new List<mClienteContado>();
        public mAgregarCliente clienteNuevo = new mAgregarCliente();
        public string mensajeError;
        public string monedaActual;
        private bool cargaInicial = false;
        List<string> permisos;
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            usuarioActual = user.Identity.Name;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            permisos = user.Claims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();
            await RefresacarListas();
            await CargaInciialPaisCliente();
            cargaInicial = true;
        }
        public async Task CargaInciialPaisCliente() {
            clienteNuevo.PAIS = "CRI";
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await LugaresService.ObtenerListaDeProvinciasPorPais(esquema, clienteNuevo.PAIS);
            if (LugaresService.listaDeProvincias != null)
            {
                listaProvincias = LugaresService.listaDeProvincias;
            }

        }
        public async Task RefresacarListas()
        {
            
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await LugaresService.ObtenerListaDePaises(esquema);
            if (LugaresService.listaDePaises != null)
            {
                listaPaises = LugaresService.listaDePaises;
            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ItemsClienteService.ObtenerLaListaDeCondicionesDePago(esquema);
            if (ItemsClienteService.listaCondicionesDePago != null)
            {
                listaCondicionesPago = ItemsClienteService.listaCondicionesDePago;
            }

            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ItemsClienteService.ObtenerLosTiposDeImpuestos(esquema);
            if (ItemsClienteService.listaTiposDeImpuestos != null)
            {
                listaTiposImpuestos = ItemsClienteService.listaTiposDeImpuestos;
            }

            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ItemsClienteService.ObtenerLosTiposDeNit(esquema);
            if (ItemsClienteService.listaTiposDeNit != null)
            {
                listaTiposNit = ItemsClienteService.listaTiposDeNit;
            }
        }
       
        

        private async Task CambioPais(ChangeEventArgs e)
        {
            listaProvincias = new List<mLugares>();
            listaCantones = new List<mLugares>();
            listaDistritos = new List<mLugares>();
            listaBarrios = new List<mLugares>();
            clienteNuevo.DIVISION_GEOGRAFICA1 = null;
            clienteNuevo.DIVISION_GEOGRAFICA2 = null;
            clienteNuevo.DIVISION_GEOGRAFICA3 = null;
            clienteNuevo.DIVISION_GEOGRAFICA4 = null;
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                clienteNuevo.PAIS = e.Value.ToString();
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await LugaresService.ObtenerListaDeProvinciasPorPais(esquema, clienteNuevo.PAIS);
                if (LugaresService.listaDeProvincias != null)
                {
                    listaProvincias = LugaresService.listaDeProvincias;
                }
            }
        }

        private async Task CambioProvincia(ChangeEventArgs e)
        {
            listaCantones = new List<mLugares>();
            listaDistritos = new List<mLugares>();
            listaBarrios = new List<mLugares>();
            clienteNuevo.DIVISION_GEOGRAFICA2 = null;
            clienteNuevo.DIVISION_GEOGRAFICA3 = null;
            clienteNuevo.DIVISION_GEOGRAFICA4 = null;
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.DIVISION_GEOGRAFICA1 = e.Value.ToString();
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await LugaresService.ObtenerListaDeCantonesPorProvincia(esquema, clienteNuevo.PAIS, clienteNuevo.DIVISION_GEOGRAFICA1);
                if (LugaresService.ListaDeCantones != null)
                {
                    listaCantones = LugaresService.ListaDeCantones;
                }
            }
        }

        private async Task CambioCanton(ChangeEventArgs e)
        {

            listaDistritos = new List<mLugares>();
            listaBarrios = new List<mLugares>();

            clienteNuevo.DIVISION_GEOGRAFICA3 = null;
            clienteNuevo.DIVISION_GEOGRAFICA4 = null;
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.DIVISION_GEOGRAFICA2 = e.Value.ToString();
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await LugaresService.ObtenerListaDeDistritosPorCanton(esquema, clienteNuevo.PAIS, clienteNuevo.DIVISION_GEOGRAFICA1, clienteNuevo.DIVISION_GEOGRAFICA2);
                if (LugaresService.listaDeDistritos != null)
                {
                    listaDistritos = LugaresService.listaDeDistritos;
                }
            }
        }

        private async Task CambioDistrito(ChangeEventArgs e)
        {


            listaBarrios = new List<mLugares>();


            clienteNuevo.DIVISION_GEOGRAFICA4 = null;
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.DIVISION_GEOGRAFICA3 = e.Value.ToString();
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await LugaresService.ObtenerListaDeBarriosPorDistrito(esquema, clienteNuevo.PAIS, clienteNuevo.DIVISION_GEOGRAFICA1, clienteNuevo.DIVISION_GEOGRAFICA2, clienteNuevo.DIVISION_GEOGRAFICA3);
                if (LugaresService.listaDeBarrios != null)
                {
                    listaBarrios = LugaresService.listaDeBarrios;
                }
            }
        }

        private void CambioBarrio(ChangeEventArgs e)
        {

            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.DIVISION_GEOGRAFICA4 = e.Value.ToString();

            }
        }

        private void CambioNombre(ChangeEventArgs e)
        {

            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.NOMBRE = e.Value.ToString();

            }
        }

        private void CambioAlias(ChangeEventArgs e)
        {

            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.ALIAS = e.Value.ToString();

            }
        }

        private void CambioContacto(ChangeEventArgs e)
        {

            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.CONTACTO = e.Value.ToString();

            }
        }

        private void CambioTelefono1(ChangeEventArgs e)
        {

            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.TELEFONO1 = e.Value.ToString();

            }
        }

        private void CambioTelefono2(ChangeEventArgs e)
        {

            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.TELEFONO2 = e.Value.ToString();

            }
        }

        private void CambioCorreo(ChangeEventArgs e)
        {

            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.E_MAIL = e.Value.ToString();

            }
        }

        private void CambioContribuyente(ChangeEventArgs e)
        {

            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.CONTRIBUYENTE = e.Value.ToString();

            }
        }

        private async Task CambioMoneda(ChangeEventArgs e)
        {
            listaNivelesPrecio = new List<mItemsCliente>();

            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.MONEDA = e.Value.ToString();
                if (clienteNuevo.MONEDA == "CRC")
                {
                    monedaActual = "L";
                }
                else
                {
                    monedaActual = "D";
                }
                if (!string.IsNullOrEmpty(monedaActual))
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await ItemsClienteService.ObtenerLaListaDeNivelesDePrecio(esquema, monedaActual);
                    if (ItemsClienteService.listaNivelesDePrecio != null)
                    {
                        listaNivelesPrecio = ItemsClienteService.listaNivelesDePrecio;
                    }
                }

            }
            else
            {
                monedaActual = null;
            }
        }
        private void CambioCondicionPago(ChangeEventArgs e)
        {

            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.CONDICION_PAGO = e.Value.ToString();

            }
        }
        private void CambioOtrasSeñas(ChangeEventArgs e)
        {

            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.OTRAS_SENAS = e.Value.ToString();

            }
        }

        private void CambioDocAGenerar(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.DOC_A_GENERAR = e.Value.ToString();

            }

        }

        private void CambioExentoImpuesto(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.EXENTO_IMPUESTOS = e.Value.ToString();

            }
            if (clienteNuevo.EXENTO_IMPUESTOS == "S")
            {
                clienteNuevo.EXENCION_IMP1 = "100";
                clienteNuevo.EXENCION_IMP2 = "100";
                clienteNuevo.readonlyExento = false;
            }
            else if (clienteNuevo.EXENTO_IMPUESTOS == "N")
            {
                clienteNuevo.EXENCION_IMP1 = "0";
                clienteNuevo.EXENCION_IMP2 = "0";
                clienteNuevo.readonlyExento = true;
            }
        }

        private void CambioNivelPrecio(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.NIVEL_PRECIO = e.Value.ToString();

            }

        }

        private void CambioDescuento(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.DESCUENTO = e.Value.ToString();

            }

        }
        private async Task CambioEsCorporacion(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.ES_CORPORACION = e.Value.ToString();

            }
            if (clienteNuevo.ES_CORPORACION == "S")
            {
                listaClientesCorporaciones = new List<mClienteContado>();
                clienteNuevo.CLI_CORPORAC_ASOC = string.Empty;
                clienteNuevo.readonlyCorporacion = true;
            }
            else if (clienteNuevo.ES_CORPORACION == "N")
            {
                clienteNuevo.readonlyCorporacion = false;
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ClientesService.ObtenerListaClientesCorporaciones(esquema);
                if (ClientesService.ListaClientesCorporaciones != null)
                {
                    listaClientesCorporaciones = ClientesService.ListaClientesCorporaciones;
                }

            }
        }
        private void CambioCorporacionAsociada(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.CLI_CORPORAC_ASOC = e.Value.ToString();

            }

        }

        private async Task CambioTipoImpuesto(ChangeEventArgs e)
        {
            listaTiposTarifasImpuesto = new List<mTarifa>();
            clienteNuevo.TIPO_TARIFA = "";
            clienteNuevo.PORC_TARIFA = "";
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.TIPO_IMPUESTO = e.Value.ToString();
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ItemsClienteService.ObtenerLosTiposDeTarifasDeImpuesto(esquema, clienteNuevo.TIPO_IMPUESTO);
                if (ItemsClienteService.listaDeTarifasDeImpuesto != null)
                {
                    listaTiposTarifasImpuesto = ItemsClienteService.listaDeTarifasDeImpuesto;
                }

            }

        }

        private async Task CambioTipoTarifa(ChangeEventArgs e)
        {
            clienteNuevo.PORC_TARIFA = "";
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                clienteNuevo.TIPO_TARIFA = e.Value.ToString();
                decimal porcentaje = await ItemsClienteService.ObtenerElPorcentajeDeTarifa(esquema, clienteNuevo.TIPO_IMPUESTO, clienteNuevo.TIPO_TARIFA);
                clienteNuevo.PORC_TARIFA = porcentaje.ToString();
            }
        }

        private void CambioTipoCliente(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.TIPIFICACION_CLIENTE = e.Value.ToString();

            }

        }

        private void CambioAfectacionIva(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.AFECTACION_IVA = e.Value.ToString();

            }

        }

        private void CambioTipoNit(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                clienteNuevo.TIPO_NIT = e.Value.ToString();

            }

        }

        private async Task CambioImagen(InputFileChangeEventArgs e)
        {
            var archivo = e.File;

            if (archivo != null)
            {
                clienteNuevo.ImagenFile = new FormFile(archivo.OpenReadStream(archivo.Size), 0, archivo.Size, "name", archivo.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = archivo.ContentType
                };
                using (var memoryStream = new MemoryStream())
                {
                    await archivo.OpenReadStream().CopyToAsync(memoryStream);
                    byte[] bytes = memoryStream.ToArray();
                    clienteNuevo.IMAGEN = bytes;

                }
            }

        }
        private async Task DescartarCambios()
        {
            await SwalAviso("Se han cancelado los cambios");
        }
        private async Task AgregarClienteNuevo()
        {
            mensajeError = null;
            try
            {
                clienteNuevo.CARGO = "ND";
                clienteNuevo.DIRECCION = "ND";
                clienteNuevo.ZONA = "ND";
                if (string.IsNullOrEmpty(clienteNuevo.DESCUENTO))
                {
                    clienteNuevo.DESCUENTO = "0";
                }
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                clienteNuevo.CLIENTE = await ItemsClienteService.ObtenerElSiguienteCodigoCliente(esquema, clienteNuevo.NOMBRE.Substring(0, 1));
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ClientesService.AgregarCliente(clienteNuevo, esquema, usuarioActual);
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ClientesService.ObtenerClienteAsociado(clienteNuevo.CLIENTE, esquema);
                if (ClientesService.ClienteAsociado != null)
                {
                    await SwalExito("Se ha agregado el cliente");
                }
                else
                {
                    mensajeError = "Error al agregar el cliente";
                    clienteNuevo = new mAgregarCliente();
                }

            }
            catch (Exception)
            {
                clienteNuevo = new mAgregarCliente();
                mensajeError = "Ocurrío un Error vuelva a intentarlo";
            }

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

        private void IrAClientes()
        {
            navigationManager.NavigateTo($"clientes");
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
            }).ContinueWith(swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (result.IsConfirmed || result.IsDismissed)
                {
                           IrAClientes();
                }
            });
        }

        private async Task SwalAviso(string mensajeAlerta)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Aviso!",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Info,
                ShowCancelButton = false,
                ConfirmButtonText = "Ok"
            }).ContinueWith(swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (result.IsConfirmed || result.IsDismissed)
                {
                    IrAClientes();   
                }
            });
        }
    }
}
