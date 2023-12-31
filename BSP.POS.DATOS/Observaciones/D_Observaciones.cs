﻿using BSP.POS.DATOS.POSDataSetTableAdapters;
using BSP.POS.UTILITARIOS.Observaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.DATOS.Observaciones
{
    public class D_Observaciones
    {
        
        public List<U_DatosObservaciones> ListarDatosObservacionesDeInforme(string pConsecutivo, string pEsquema)
        {
            var LstObservaciones = new List<U_DatosObservaciones>();
            ObtenerDatosObservacionesDeInformeTableAdapter sp = new ObtenerDatosObservacionesDeInformeTableAdapter();
            var response = sp.GetData(pEsquema, pConsecutivo).ToList();

            try
            {
                foreach (var item in response)
                {
                    U_DatosObservaciones observacion = new U_DatosObservaciones(item.Id, item.consecutivo_informe, item.codigo_usuario, item.observacion, item.nombre_usuario);

                    LstObservaciones.Add(observacion);
                }
                return LstObservaciones;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string AgregarObservacionDeInforme(U_Observaciones pObservacion, string esquema)
        {
            POSDataSet.AgregarObservacionDeInformeDataTable bTabla = new POSDataSet.AgregarObservacionDeInformeDataTable();
            AgregarObservacionDeInformeTableAdapter sp = new AgregarObservacionDeInformeTableAdapter();
            try
            {
                var response = sp.GetData(esquema, pObservacion.consecutivo_informe, pObservacion.codigo_usuario, pObservacion.observacion);
                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }
    }
}
