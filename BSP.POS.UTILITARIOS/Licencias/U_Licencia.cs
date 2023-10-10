using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Licencias
{
    public class U_Licencia
    {
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string FechaAviso { get; set; }
        public string CantidadUsuarios { get; set; }
        public string MacAddress { get; set; }

        public U_Licencia(string pFechaInicio, string pFechaFin, string pFechaAviso, string pCantidadUsuarios, string pMacAddress) {
            FechaInicio = pFechaInicio;
            FechaFin = pFechaFin;
            FechaAviso = pFechaAviso;
            CantidadUsuarios = pCantidadUsuarios;
            MacAddress = pMacAddress;
        
        }

        public U_Licencia() { }
    }
}
