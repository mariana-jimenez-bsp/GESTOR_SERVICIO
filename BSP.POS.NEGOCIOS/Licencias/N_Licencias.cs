using BSP.POS.DATOS.Licencias;
using BSP.POS.UTILITARIOS.Licencias;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.Licencias
{
    public class N_Licencias
    {
        D_Licencias licencias = new D_Licencias();

        public string ObtenerEstadoDeLicencia()
        {
            try
            {
                   U_Licencia licencia = new U_Licencia();
                licencia = licencias.ObtenerEstadoDeLicencia();
                string licenciaJson = JsonConvert.SerializeObject(licencia);
                return licenciaJson;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
