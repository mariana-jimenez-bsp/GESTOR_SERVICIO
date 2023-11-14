using BSP.POS.DATOS.Proyectos;
using BSP.POS.UTILITARIOS.Proyectos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.Proyectos
{
    public class N_Proyectos
    {
        D_Proyectos objProyectos = new D_Proyectos();

        public string ListarProyectosActivos(String pEsquema)
        {
            try
            {
                List<U_ListaProyectos> list = new List<U_ListaProyectos>();

                list = objProyectos.ListarProyectosActivos(pEsquema);

                string proyectos = JsonConvert.SerializeObject(list);
                return proyectos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ObtenerProyecto(string pEsquema, string pNumero)
        {
            try
            {
                U_ListaProyectos proyecto = new U_ListaProyectos();

                proyecto = objProyectos.ObtenerProyecto(pEsquema, pNumero);

                string proyectoJson = JsonConvert.SerializeObject(proyecto);
                return proyectoJson;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ListarDatosProyectosActivos(String pEsquema)
        {
            try
            {
                List<U_DatosProyectos> list = new List<U_DatosProyectos>();

                list = objProyectos.ListarDatosProyectosActivos(pEsquema);

                string proyectos = JsonConvert.SerializeObject(list);
                return proyectos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ListarDatosProyectosActivosDeCliente(String pEsquema, string pCliente)
        {
            try
            {
                List<U_DatosProyectos> list = new List<U_DatosProyectos>();

                list = objProyectos.ListarDatosProyectosActivosDeCliente(pEsquema, pCliente);

                string proyectos = JsonConvert.SerializeObject(list);
                return proyectos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
        public string ListarProyectosTerminadosYCancelados(String pEsquema)
        {
            try
            {
                List<U_ListaProyectos> list = new List<U_ListaProyectos>();

                list = objProyectos.ListarProyectosTerminadosYCancelados(pEsquema);

                string proyectos = JsonConvert.SerializeObject(list);
                return proyectos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string CambiarEstadoProyecto(string pNumero, string pEstado, string pEsquema)
        {
            try
            {
                string mensaje;

                mensaje = objProyectos.CambiarEstadoProyecto(pNumero, pEstado, pEsquema);

                return mensaje;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ActualizarListaDeProyectos(List<U_ListaProyectos> pProyectos, string esquema)
        {
            string mensaje = string.Empty;
            mensaje = objProyectos.ActualizarListaDeProyectos(pProyectos, esquema);
            return mensaje;
        }

        public string AgregarProyecto(U_ListaProyectos pProyecto, string esquema)
        {
            string mensaje = string.Empty;
            mensaje = objProyectos.AgregarProyecto(pProyecto, esquema);
            return mensaje;
        }
    }
}
