using BSP.POS.API.Models.Proyectos;
using BSP.POS.NEGOCIOS.Proyectos;
using BSP.POS.UTILITARIOS.Actividades;
using BSP.POS.UTILITARIOS.Proyectos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProyectosController : ControllerBase
    {
        private N_Proyectos _proyectos;
        public ProyectosController()
        {
            _proyectos = new N_Proyectos();
        }

        [HttpGet("ObtengaLaListaDeProyectos/{esquema}")]
        public string ObtengaLaListaDeProyectos(string esquema)
        {
            try
            {
                string listaProyectosAsociadasJson = _proyectos.ListarProyectos(esquema);
                return listaProyectosAsociadasJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpPost("ActualizaListaDeProyectos")]
        public string ActualizaListaDeActividades([FromBody] List<mProyectos> datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];

                List<U_ListaProyectos> listaProyectos = new List<U_ListaProyectos>();
                foreach (var item in datos)
                {
                    U_ListaProyectos proyecto = new U_ListaProyectos();
                    proyecto.Id = item.Id;
                    proyecto.numero = item.numero;
                    proyecto.nombre_consultor = item.nombre_consultor;
                    proyecto.fecha_inicial = item.fecha_inicial;
                    proyecto.fecha_final = item.fecha_final;
                    proyecto.empresa = item.empresa;
                    proyecto.centro_costo = item.centro_costo;
                    proyecto.horas_totales = item.horas_totales;
                    proyecto.nombre_proyecto = item.nombre_proyecto;


                    listaProyectos.Add(proyecto);
                }

                string mensaje = _proyectos.ActualizarListaDeProyectos(listaProyectos, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpPost("AgregaProyecto")]
        public string AgregaProyecto([FromBody] mProyectos datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];

                U_ListaProyectos proyecto = new U_ListaProyectos();

                    proyecto.nombre_consultor = datos.nombre_consultor;
                    proyecto.fecha_inicial = datos.fecha_inicial;
                    proyecto.fecha_final = datos.fecha_final;
                    proyecto.empresa = datos.empresa;
                    proyecto.centro_costo = datos.centro_costo;
                    proyecto.horas_totales = datos.horas_totales;
                    proyecto.nombre_proyecto = datos.nombre_proyecto;


                

                string mensaje = _proyectos.AgregarProyecto(proyecto, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
