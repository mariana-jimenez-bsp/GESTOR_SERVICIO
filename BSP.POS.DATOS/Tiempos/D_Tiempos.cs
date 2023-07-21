using BSP.POS.DATOS.POSDataSetTableAdapters;
using BSP.POS.UTILITARIOS.Tiempos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.DATOS.Tiempos
{
    public class D_Tiempos
    {
        public List<U_ListaTiempos> ListaTiempos(String pEsquema)
        {
            var LstTiempos = new List<U_ListaTiempos>();

            ListarTiemposTableAdapter sp = new ListarTiemposTableAdapter();

            var response = sp.GetData(pEsquema).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_ListaTiempos cliente = new U_ListaTiempos(item.Id, item.nombre_servicio, item.horas);

                    LstTiempos.Add(cliente);
                }
                return LstTiempos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ActualizarListaDeTiempos(List<U_ListaTiempos> pTiempos)
        {
            POSDataSet.ActualizarListaDeTiemposDataTable bTabla = new POSDataSet.ActualizarListaDeTiemposDataTable();
            ActualizarListaDeTiemposTableAdapter sp = new ActualizarListaDeTiemposTableAdapter();
            try
            {
                foreach (var tiempo in pTiempos)
            {
                var response = sp.GetData(tiempo.Id, tiempo.nombre_servicio, tiempo.horas);

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
