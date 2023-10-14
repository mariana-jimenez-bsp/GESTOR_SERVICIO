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
        public string CantidadCajas { get; set; }
        public string CantidadUsuarios { get; set; }
        public string MacAddress { get; set; }
        public string Pais { get; set; } 
        public string CedulaJuridica { get; set; } 
        public string NombreCliente { get; set; } 

        public U_Licencia(string pFechaInicio, string pFechaFin, string pFechaAviso, string pCantidadCajas,  string pCantidadUsuarios, string pMacAddress, string pais, string cedulaJuridica, string nombreCliente) {
            FechaInicio = pFechaInicio;
            FechaFin = pFechaFin;
            FechaAviso = pFechaAviso;
            CantidadCajas = pCantidadCajas;
            CantidadUsuarios = pCantidadUsuarios;
            MacAddress = pMacAddress;
            Pais = pais;
            CedulaJuridica = cedulaJuridica;
            NombreCliente = nombreCliente;

        
        }

        public U_Licencia() { }
    }
}
