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
                licencia = new U_Licencia(item.FechaInicio, item.FechaFin, item.FechaAviso, item.CantidadCajas, item.CantidadUsuarios, item.MacAddress);
                
            }
            return licencia;
        }

        public bool ActualizarDatosLicencia(U_ActualizarDatosLicencia datosLicencia)
        {
            ActualizarDatosLicenciaTableAdapter sp = new ActualizarDatosLicenciaTableAdapter();
            var response = sp.GetData(datosLicencia.Codigo, datosLicencia.FechaInicio, datosLicencia.FechaFin, datosLicencia.FechaAviso, datosLicencia.CantidadCajas, datosLicencia.CantidadUsuarios, datosLicencia.MacAddress).ToList();
            bool resultado = false;
            foreach (var item in response)
            {

                resultado = item.Resultado;
            }
            return resultado;
        }

        public U_CodigoDeLicencia ObtenerCodigoDeLicencia()
        {
            ObtenerCodigoDeLicenciaTableAdapter sp = new ObtenerCodigoDeLicenciaTableAdapter();
            var response = sp.GetData().ToList();
            U_CodigoDeLicencia licencia = new U_CodigoDeLicencia();
            foreach (var item in response)
            {
                licencia.codigo_licencia = item.Codigo;

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
