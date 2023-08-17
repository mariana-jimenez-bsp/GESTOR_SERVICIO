using BSP.POS.DATOS.POSDataSetTableAdapters;
using BSP.POS.UTILITARIOS.Licencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.DATOS.Licencias
{
    public class D_Licencias
    {
        public U_Licencia ObtenerEstadoDeLicencia()
        {
            ObtenerEstadoDeLicenciaTableAdapter sp = new ObtenerEstadoDeLicenciaTableAdapter();
            var response = sp.GetData().ToList();
            U_Licencia licencia = new U_Licencia();
            foreach (var item in response)
            {
                licencia = new U_Licencia(item.Id, item.esquema, item.estado, item.fecha_vencimiento);
                
            }
            return licencia;
        }
    }
}
