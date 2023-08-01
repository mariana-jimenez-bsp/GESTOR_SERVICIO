using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSP.POS.UTILITARIOS.Actividades;
using BSP.POS.DATOS.POSDataSetTableAdapters;


namespace BSP.POS.DATOS.Actividades
{
    public class D_Actividades
    {

        public List<U_ListaActividades> ListaActividades(String pEsquema)
        {
            var LstActividades = new List<U_ListaActividades>();

            ListarActividadesTableAdapter sp = new ListarActividadesTableAdapter();

            var response = sp.GetData(pEsquema).ToList();
            try
            {
                foreach (var item in response)
                { 
                    U_ListaActividades actividad = new U_ListaActividades(item.Id, item.codigo, item.Actividad, item.CI_referencia, item.horas);

                    LstActividades.Add(actividad);
                }
                return LstActividades;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ActualizarListaDeActividades(List<U_ListaActividades> pActividades, string esquema)
        {
            POSDataSet.ActualizarActividadesDataTable bTabla = new POSDataSet.ActualizarActividadesDataTable();
            ActualizarActividadesTableAdapter sp = new ActualizarActividadesTableAdapter();
            try
            {
                foreach (var actividad in pActividades)
                {
                    var response = sp.GetData(actividad.Id, actividad.codigo, actividad.Actividad, actividad.CI_referencia, actividad.horas, esquema);

                }
                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }

        public List<U_ListaActividadesAsociadas> ListaActividadesAsociadas(String pEsquema, String pConsecutivo)
        {
            var LstActividades = new List<U_ListaActividadesAsociadas>();

            ListarActividadesAsociadasTableAdapter sp = new ListarActividadesAsociadasTableAdapter();

            var response = sp.GetData(pEsquema, pConsecutivo).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_ListaActividadesAsociadas actividad = new U_ListaActividadesAsociadas(item.Id, item.consecutivo_informe, item.codigo_actividad, item.horas_cobradas, item.horas_no_cobradas);

                    LstActividades.Add(actividad);
                }
                return LstActividades;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
    }
}
