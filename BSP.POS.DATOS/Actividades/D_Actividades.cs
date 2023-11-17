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
                    U_ListaActividades actividad = new U_ListaActividades(item.Id, item.codigo, item.codigo_usuario, item.Actividad, item.CI_referencia, item.horas, item.RecordDate);

                    LstActividades.Add(actividad);
                }
                return LstActividades;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public List<U_ListaActividades> ListaActividadesPorUsuario(String pEsquema, string pCodigo)
        {
            var LstActividades = new List<U_ListaActividades>();

            ListarActividadesPorUsuarioTableAdapter sp = new ListarActividadesPorUsuarioTableAdapter();

            var response = sp.GetData(pEsquema, pCodigo).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_ListaActividades actividad = new U_ListaActividades(item.Id, item.codigo, item.codigo_usuario, item.Actividad, item.CI_referencia, item.horas, item.RecordDate);

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
                    var response = sp.GetData(actividad.Id, actividad.codigo, actividad.codigo_usuario, actividad.Actividad, actividad.CI_referencia, actividad.horas, esquema);

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

        public List<U_DatosActividadesAsociadas> ListaDatosActividadesAsociadas(String pEsquema, String pConsecutivo)
        {
            var LstActividades = new List<U_DatosActividadesAsociadas>();

            ListarDatosActividadesAsociadasTableAdapter sp = new ListarDatosActividadesAsociadasTableAdapter();

            var response = sp.GetData(pEsquema, pConsecutivo).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_DatosActividadesAsociadas actividad = new U_DatosActividadesAsociadas(item.Id, item.consecutivo_informe, item.codigo_actividad, item.horas_cobradas, item.horas_no_cobradas, item.nombre_actividad);

                    LstActividades.Add(actividad);
                }
                return LstActividades;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ActualizarListaDeActividadesAsociadas(List<U_ListaActividadesAsociadas> pActividades, string esquema)
        {
            POSDataSet.ActualizarActividades_InformesDataTable bTabla = new POSDataSet.ActualizarActividades_InformesDataTable();
            ActualizarActividades_InformesTableAdapter sp = new ActualizarActividades_InformesTableAdapter();
            try
            {
                foreach (var actividad in pActividades)
                {
                    var response = sp.GetData(actividad.Id, actividad.consecutivo_informe, actividad.codigo_actividad, int.Parse(actividad.horas_cobradas), int.Parse(actividad.horas_no_cobradas), esquema);

                }
                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }

        public string AgregarActividadDeInforme(U_ListaActividadesAsociadas pActividad, string esquema)
        {
            POSDataSet.AgregarActividadDeInformeDataTable bTabla = new POSDataSet.AgregarActividadDeInformeDataTable();
            AgregarActividadDeInformeTableAdapter sp = new AgregarActividadDeInformeTableAdapter();
            try
            {
                var response = sp.GetData(esquema, pActividad.consecutivo_informe, pActividad.codigo_actividad, int.Parse(pActividad.horas_cobradas));


                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }

        public string EliminarActividadDeInforme(string pIdActividad, string esquema)
        {
            POSDataSet.EliminarActividadAsociadaDataTable bTabla = new POSDataSet.EliminarActividadAsociadaDataTable();
            EliminarActividadAsociadaTableAdapter sp = new EliminarActividadAsociadaTableAdapter();
            try
            {
                var response = sp.GetData(pIdActividad, esquema);

                return "Exito";
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error: ", ex.InnerException);
            }



        }

        public string AgregarActividad(U_ListaActividades pActividad, string esquema)
        {
            POSDataSet.AgregarActividadDataTable bTabla = new POSDataSet.AgregarActividadDataTable();
            AgregarActividadTableAdapter sp = new AgregarActividadTableAdapter();
            try
            {
                var response = sp.GetData(pActividad.codigo_usuario, pActividad.Actividad, pActividad.CI_referencia, int.Parse(pActividad.horas), esquema);


                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }

        public void EliminarActividad(string esquema, string codigo)
        {
            try
            {
                EliminarActividadTableAdapter sp = new EliminarActividadTableAdapter();
                var response = sp.GetData(codigo, esquema).ToList();
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
    }
}
