using BSP.POS.DATOS.Proyectos;
using BSP.POS.UTILITARIOS.Actividades;
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

        public string ListarProyectosIniciados(String pEsquema)
        {
            try
            {
                List<U_ListaProyectos> list = new List<U_ListaProyectos>();

                list = objProyectos.ListarProyectosIniciados(pEsquema);

                string proyectos = JsonConvert.SerializeObject(list);
                return proyectos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
        public string ListarProyectosTerminados(String pEsquema)
        {
            try
            {
                List<U_ListaProyectos> list = new List<U_ListaProyectos>();

                list = objProyectos.ListarProyectosTerminados(pEsquema);

                string proyectos = JsonConvert.SerializeObject(list);
                return proyectos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string TerminarProyecto(string pNumero, string pEsquema)
        {
            try
            {
                string mensaje;

                mensaje = objProyectos.TerminarProyecto(pNumero, pEsquema);

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
