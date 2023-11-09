using BSP.POS.Presentacion.Models.Esquemas;
using BSP.POS.Presentacion.Services.Permisos;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BSP.POS.Presentacion.Pages.Usuarios.Usuarios
{
    public partial class SelectEsquemas : ComponentBase
    {
        public List<mEsquemas> listaDeEsquemas = new List<mEsquemas>();
        public List<mDatosEsquemasDeUsuario> listaDeEsquemasDeUsuario = new List<mDatosEsquemasDeUsuario>();
        [Parameter] public string codigo { get; set; } = string.Empty;
        private List<string> esquemasCambiados = new List<string>();
        private bool eventoCambioEsquema = false;
        private bool cargaInicial = false;

        protected override async Task OnInitializedAsync()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await EsquemasService.ObtenerListaDeEsquemas();
            listaDeEsquemas = EsquemasService.ListaEsquemas;
            if (!string.IsNullOrEmpty(codigo))
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await EsquemasService.ObtenerListaDeEsquemasDeUsuario(codigo);
                listaDeEsquemasDeUsuario = EsquemasService.ListaEsquemasDeUsuario;
            }
            
            cargaInicial = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                string jsonData = "";
                if (!string.IsNullOrEmpty(codigo))
                {
                    List<string> listaDeEsquemasAActivar = new List<string>();
                    if (listaDeEsquemasDeUsuario.Any())
                    {
                        foreach (var esquema in listaDeEsquemasDeUsuario)
                        {

                            listaDeEsquemasAActivar.Add(esquema.id_esquema);
                        }
                        jsonData = JsonSerializer.Serialize(listaDeEsquemasAActivar);

                    }
                }

                DotNetObjectReference<SelectEsquemas> objRef = DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync("ActivarSelectMultipleEsquemas", jsonData, objRef);


            }
            catch (Exception ex)
            {

                string error = ex.ToString();
                Console.WriteLine(error);
            }
        }

        [JSInvokable]
        public void CambioDeEsquemas(string[] esquemasSeleccionados)
        {
            esquemasCambiados = esquemasSeleccionados.ToList();
            eventoCambioEsquema = true;
        }

        public async Task<int> ActualizarListaDeEsquema(string codigoEnviado)
        {
            string codigoUsuario = "";
            if (!string.IsNullOrEmpty(codigoEnviado))
            {
                codigoUsuario = codigoEnviado;
            }
            else
            {
                codigoUsuario = codigo;
            }
            bool resultado = false;

            if (eventoCambioEsquema)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                resultado = await EsquemasService.ActualizarListaDeEsquemasDeUsuario(esquemasCambiados, codigoUsuario);

            }
            else
            {
                resultado = true;
            }
            if (resultado)
            {
                if (!string.IsNullOrEmpty(codigoEnviado))
                {
                    return 1;
                }
                else if (eventoCambioEsquema)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 0;
            }

        }
    }
}
