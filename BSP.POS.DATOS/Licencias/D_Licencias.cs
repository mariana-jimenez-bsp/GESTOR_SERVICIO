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
        public U_Licencia ObtenerDatosDeLicencia()
        {
            ObtenerDatosDeLicenciaTableAdapter sp = new ObtenerDatosDeLicenciaTableAdapter();
            var response = sp.GetData().ToList();
            U_Licencia licencia = new U_Licencia();
            foreach (var item in response)
            {
                licencia = new U_Licencia(item.FechaInicio, item.FechaFin, item.FechaAviso, item.CantidadUsuarios, item.MacAddress);
                
            }
            return licencia;
        }

        public int EnviarXMLLicencia(string textoXML)
        {
            IngresarXMLTableAdapter sp = new IngresarXMLTableAdapter();

            var response = sp.GetData(textoXML).ToList();
            int resultado = -1;
            foreach (var item in response)
            {
                resultado = item.ResultCode;
            }

            return resultado;
        }
    }
}
