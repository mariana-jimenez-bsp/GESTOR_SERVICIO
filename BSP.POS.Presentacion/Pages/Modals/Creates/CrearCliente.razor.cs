using BSP.POS.Presentacion.Models.Lugares;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Modals.Creates
{
    public partial class CrearCliente : ComponentBase
    {
        public string esquema = string.Empty;   
        public List<mLugares> listaPaises = new List<mLugares>();
        public List<mLugares> listaProvincias = new List<mLugares>();
        public List<mLugares> listaCantones = new List<mLugares>();
        public List<mLugares> listaDistritos = new List<mLugares>();
        public List<mLugares> listaBarrios = new List<mLugares>();
        public string paisActual;
        public string provinciaActual;
        public string cantonActual;
        public string distritoActual;
        public string barrioActual;
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await LugaresService.ObtenerListaDePaises(esquema);
            if(LugaresService.listaDePaises != null)
            {
                listaPaises = LugaresService.listaDePaises;
            }
            
        }

        private async Task CambioPais(ChangeEventArgs e)
        {
            listaProvincias = new List<mLugares>();
            listaCantones = new List<mLugares>();
            listaDistritos = new List<mLugares>();
            listaBarrios = new List<mLugares>();
            provinciaActual = null;
            cantonActual = null;
            distritoActual = null;
            barrioActual = null;
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                paisActual = e.Value.ToString();
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await LugaresService.ObtenerListaDeProvinciasPorPais(esquema, paisActual);
                if(LugaresService.listaDeProvincias != null)
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
            cantonActual = null;
            distritoActual = null;
            barrioActual = null;
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                
                provinciaActual = e.Value.ToString();
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await LugaresService.ObtenerListaDeCantonesPorProvincia(esquema, paisActual, provinciaActual);
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
            
            distritoActual = null;
            barrioActual = null;
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                cantonActual = e.Value.ToString();
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await LugaresService.ObtenerListaDeDistritosPorCanton(esquema, paisActual, provinciaActual, cantonActual);
                if (LugaresService.listaDeDistritos != null)
                {
                    listaDistritos = LugaresService.listaDeDistritos;
                }
            }
        }

        private async Task CambioDistrito(ChangeEventArgs e)
        {

            
            listaBarrios = new List<mLugares>();


            barrioActual = null;
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                distritoActual = e.Value.ToString();
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await LugaresService.ObtenerListaDeBarriosPorDistrito(esquema, paisActual, provinciaActual, cantonActual, distritoActual);
                if (LugaresService.listaDeBarrios != null)
                {
                    listaBarrios = LugaresService.listaDeBarrios;
                }
            }
        }

        private async Task CambioBarrio(ChangeEventArgs e)
        {

            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                barrioActual = e.Value.ToString();
               
            }
        }



    }
}
