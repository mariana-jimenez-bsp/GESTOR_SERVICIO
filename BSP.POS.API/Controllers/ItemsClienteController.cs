using BSP.POS.NEGOCIOS.ItemsCliente;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemsClienteController : ControllerBase
    {
        private readonly N_ItemsCliente _items;
        
        public ItemsClienteController() {
            _items = new N_ItemsCliente();
        }

        [HttpGet("ObtengaLaListaDeCondicionesDePago")]
        public string ObtenegaLaListaDeCondicionesDePago()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string listaJson = _items.ObtenerCondicionesDePago(esquema);
                return listaJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpGet("ObtengaLaListaDeNivelesDePrecio")]
        public string ObtengaLaListaDeNivelesDePrecio()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string moneda = Request.Headers["X-Moneda"];
                string listaJson = _items.ObtenerNivelesPrecio(esquema, moneda);
                return listaJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpGet("ObtengaLosTiposDeImpuestos")]
        public string ObtengaLosTiposDeImpuestos()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string listaJson = _items.ObtenerTiposDeImpuestos(esquema);
                return listaJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpGet("ObtengaLosTiposDeTarifasDeImpuesto")]
        public string ObtengaLosTiposDeTarifasDeImpuesto()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string impuesto = Request.Headers["X-Impuesto"];
                string listaJson = _items.ObtenerTiposDeTarifasDeImpuesto(esquema, impuesto);
                return listaJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpGet("ObtengaElPorcentajeDeTarifa")]
        public string ObtengaElPorcentajeDeTarifa()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string impuesto = Request.Headers["X-Impuesto"];
                string tipoTarifa = Request.Headers["X-TipoTarifa"];
                decimal porcentaje = _items.ObtenerPorcentajeTarifa(esquema, impuesto, tipoTarifa);
                return porcentaje.ToString();
            }

            catch (Exception)
            {
                return "0";
            }

        }

        [HttpGet("ObtengaElSiguienteCodigoDeCliente")]
        public string ObtengaElSiguienteCodigoDeCliente()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string letra = Request.Headers["X-Letra"];
                string codigo = _items.ObtenerSiguienteCodigoDeCliente(esquema, letra);
                return codigo;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        [HttpGet("ObtengaLosTiposDeNit")]
        public string ObtengaLosTiposDeNit()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string listaJson = _items.ObtenerTiposDeNit(esquema);
                return listaJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpGet("ObtengaLaListaDeCentrosDeCosto")]
        public string ObtengaLaListaDeCentrosDeCosto()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string listaJson = _items.ObtenerListaDeCentrosDeCosto(esquema);
                return listaJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
