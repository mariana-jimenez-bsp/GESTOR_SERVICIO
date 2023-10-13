using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Licencias
{
    public class U_ActualizarDatosLicencia
    {
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string FechaAviso { get; set; }
        public string CantidadCajas { get; set; }
        public string CantidadUsuarios { get; set; }
        public string MacAddress { get; set; }
        public byte[] Codigo { get; set; }

        public U_ActualizarDatosLicencia(string pFechaInicio, string pFechaFin, string pFechaAviso, string pCantidadCajas, string pCantidadUsuarios, string pMacAddress, byte[] codigo)
        {
            FechaInicio = pFechaInicio;
            FechaFin = pFechaFin;
            FechaAviso = pFechaAviso;
            CantidadCajas = pCantidadCajas;
            CantidadUsuarios = pCantidadUsuarios;
            MacAddress = pMacAddress;
            Codigo = codigo;
        }

        public U_ActualizarDatosLicencia() { }
    }
}
