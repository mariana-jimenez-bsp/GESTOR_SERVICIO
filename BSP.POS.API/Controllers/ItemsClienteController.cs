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
        public IActionResult ObtenegaLaListaDeCondicionesDePago()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string listaJson = _items.ObtenerCondicionesDePago(esquema);
                if(string.IsNullOrEmpty(listaJson))
                {
                    return NotFound();
                }
                return Ok(listaJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("ObtengaLaListaDeNivelesDePrecio")]
        public IActionResult ObtengaLaListaDeNivelesDePrecio()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string moneda = Request.Headers["X-Moneda"];
                string listaJson = _items.ObtenerNivelesPrecio(esquema, moneda);
                if(string.IsNullOrEmpty(listaJson))
                {
                    return NotFound();
                }
                return Ok(listaJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("ObtengaLosTiposDeImpuestos")]
        public IActionResult ObtengaLosTiposDeImpuestos()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string listaJson = _items.ObtenerTiposDeImpuestos(esquema);
                if(string.IsNullOrEmpty(listaJson))
                {
                    return NotFound();
                }
                return Ok(listaJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("ObtengaLosTiposDeTarifasDeImpuesto")]
        public IActionResult ObtengaLosTiposDeTarifasDeImpuesto()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string impuesto = Request.Headers["X-Impuesto"];
                string listaJson = _items.ObtenerTiposDeTarifasDeImpuesto(esquema, impuesto);
                if (string.IsNullOrEmpty(listaJson))
                {
                    return NotFound();
                }
                return Ok(listaJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("ObtengaElPorcentajeDeTarifa")]
        public IActionResult ObtengaElPorcentajeDeTarifa()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string impuesto = Request.Headers["X-Impuesto"];
                string tipoTarifa = Request.Headers["X-TipoTarifa"];
                decimal porcentaje = _items.ObtenerPorcentajeTarifa(esquema, impuesto, tipoTarifa);
                if(string.IsNullOrEmpty(porcentaje.ToString()))
                {
                    return NotFound();
                }
                return Ok(porcentaje.ToString());
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("ObtengaElSiguienteCodigoDeCliente")]
        public IActionResult ObtengaElSiguienteCodigoDeCliente()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string letra = Request.Headers["X-Letra"];
                string codigo = _items.ObtenerSiguienteCodigoDeCliente(esquema, letra);
                if (string.IsNullOrEmpty(codigo))
                {
                    return NotFound();
                }
                return Ok(codigo);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet("ObtengaLosTiposDeNit")]
        public IActionResult ObtengaLosTiposDeNit()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string listaJson = _items.ObtenerTiposDeNit(esquema);
                if (string.IsNullOrEmpty(listaJson))
                {
                    return NotFound();
                }
                return Ok(listaJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("ObtengaLaListaDeCentrosDeCosto")]
        public IActionResult ObtengaLaListaDeCentrosDeCosto()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string listaJson = _items.ObtenerListaDeCentrosDeCosto(esquema);
                if (string.IsNullOrEmpty(listaJson))
                {
                    return NotFound();
                }
                return Ok(listaJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
