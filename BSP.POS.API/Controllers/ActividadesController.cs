﻿using BSP.POS.NEGOCIOS.Actividades;
using BSP.POS.UTILITARIOS.Actividades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActividadesController : ControllerBase
    {
        private N_Actividades actividades;

        public ActividadesController()
        {
            actividades = new N_Actividades();

        }

        [HttpGet("ObtengaLaListaDeActividadesAsociadas/{consecutivo}/{esquema}")]
        public IActionResult ObtengaLaListaDeActividadesAsociadas(string consecutivo, string esquema)
        {
            try
            {
                string listaActividadesAsociadasJson = actividades.ListarActividadesAsociadas(esquema, consecutivo);
                if (string.IsNullOrEmpty(listaActividadesAsociadasJson))
                {
                    return NotFound();
                }
                return Ok(listaActividadesAsociadasJson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("ObtengaLaListaDeActividades/{esquema}")]
        public IActionResult ObtengaLaListaDeActividades(string esquema)
        {
            try
            {
                string listaActividadesJson = actividades.ListarActividades(esquema);
                if (string.IsNullOrEmpty(listaActividadesJson))
                {
                    return NotFound();
                }
                return Ok(listaActividadesJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("ObtengaLaListaDeActividadesActivas/{esquema}")]
        public IActionResult ObtengaLaListaDeActividadesActivas(string esquema)
        {
            try
            {
                string listaActividadesJson = actividades.ListarActividadesActivas(esquema);
                if (string.IsNullOrEmpty(listaActividadesJson))
                {
                    return NotFound();
                }
                return Ok(listaActividadesJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet("ObtengaLaListaDeActividadesPorUsuario")]
        public IActionResult ObtengaLaListaDeActividadesPorUsuario()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string codigo = Request.Headers["X-CodigoUsuario"];
                string listaActividadesJson = actividades.ListarActividadesPorUsuario(esquema, codigo);
                if (string.IsNullOrEmpty(listaActividadesJson))
                {
                    return NotFound();
                }
                return Ok(listaActividadesJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("ObtengaLaListaDeActividadesActivasPorUsuario")]
        public IActionResult ObtengaLaListaDeActividadesActivasPorUsuario()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string codigo = Request.Headers["X-CodigoUsuario"];
                string listaActividadesJson = actividades.ListarActividadesActivasPorUsuario(esquema, codigo);
                if (string.IsNullOrEmpty(listaActividadesJson))
                {
                    return NotFound();
                }
                return Ok(listaActividadesJson);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("ActualizaListaDeActividades")]
        public IActionResult ActualizaListaDeActividades([FromBody] List<U_ListaActividades> datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = actividades.ActualizarListaDeActividades(datos, esquema);
                if (string.IsNullOrEmpty(mensaje) || mensaje == "Error")
                {
                    return BadRequest();
                }
                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("ActualizaListaDeActividadesAsociadas")]
        public IActionResult ActualizaListaDeActividadesAsociadas([FromBody] List<U_ListaActividadesAsociadas> datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = actividades.ActualizarListaDeActividadesAsociadas(datos, esquema);
                if (string.IsNullOrEmpty(mensaje) || mensaje == "Error")
                {
                    return BadRequest();
                }
                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("AgregaActividadDeInforme")]
        public IActionResult AgregaActividadDeInforme([FromBody] U_ListaActividadesAsociadas datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = actividades.AgregarActividadDeInforme(datos, esquema);
                if (string.IsNullOrEmpty(mensaje) || mensaje == "Error")
                {
                    return BadRequest();
                }
                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpDelete("EliminaActividadDeInforme")]
        public IActionResult EliminaActividadDeInforme()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string idActividad = Request.Headers["X-IdActividad"];


                string mensaje = actividades.EliminarActividadDeInforme(idActividad, esquema);
                if (string.IsNullOrEmpty(mensaje) || mensaje == "Error")
                {
                    return BadRequest();
                }
                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("AgregaActividad")]
        public IActionResult AgregaActividad([FromBody] U_ListaActividades datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string mensaje = actividades.AgregarActividad(datos, esquema);
                if (string.IsNullOrEmpty(mensaje) || mensaje == "Error")
                {
                    return BadRequest();
                }
                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpDelete("EliminaLaActividad")]
        public IActionResult EliminaLaActividad()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string codigo = Request.Headers["X-Codigo"];
                actividades.EliminarActividad(esquema, codigo);
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("CambiaEstadoActividad")]
        public IActionResult CambiaEstadoActividad()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string codigo = Request.Headers["X-Codigo"];
                string estado = Request.Headers["X-Estado"];
                actividades.CambiarEstadoActividad(esquema, codigo, estado);
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("ValidaActividadAsociadaInforme")]
        public IActionResult ValidaActividadAsociadaInforme()
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string codigo = Request.Headers["X-Codigo"];
                string resultado = actividades.ValidarActivadAsociadaInforme(esquema, codigo);
                return Ok(resultado);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
    }
}
