using BSP.POS.DATOS.POSDataSetTableAdapters;
using BSP.POS.UTILITARIOS.Actividades;
using BSP.POS.UTILITARIOS.Proyectos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.DATOS.Proyectos
{
    public class D_Proyectos
    {

        public List<U_ListaProyectos> ListarProyectos(String pEsquema)
        {
            var LstProyectos = new List<U_ListaProyectos>();

            ListarProyectosTableAdapter sp = new ListarProyectosTableAdapter();

            var response = sp.GetData(pEsquema).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_ListaProyectos proyecto = new U_ListaProyectos(item.Id, item.numero, item.nombre_consultor, item.fecha_inicial, item.fecha_final, item.horas_totales, item.empresa, item.centro_costo, item.nombre_proyecto);

                    LstProyectos.Add(proyecto);
                }
                return LstProyectos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ActualizarListaDeProyectos(List<U_ListaProyectos> pProyectos, string esquema)
        {
            POSDataSet.ActualizarActividadesDataTable bTabla = new POSDataSet.ActualizarActividadesDataTable();
            ActualizarListaProyectosTableAdapter sp = new ActualizarListaProyectosTableAdapter();
            try
            {
                foreach (var proyecto in pProyectos)
                {
                    var response = sp.GetData(proyecto.Id,proyecto.numero, proyecto.nombre_consultor, proyecto.fecha_inicial, proyecto.fecha_final, proyecto.horas_totales, proyecto.empresa, proyecto.centro_costo, proyecto.nombre_proyecto, esquema);

                }
                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }
    }
}
