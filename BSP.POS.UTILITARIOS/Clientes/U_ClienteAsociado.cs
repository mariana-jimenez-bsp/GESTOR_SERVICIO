using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Clientes
{
    public class U_ClienteAsociado
    {
        public string CLIENTE { get; set; }
        public string NOMBRE { get; set; }

        public string CONTACTO { get; set; }
        public string CARGO { get; set; }



        public U_ClienteAsociado(string pCLIENTE, string pNOMBRE, string pCONTACTO, string pCARGO)
        {
            CLIENTE = pCLIENTE;
            NOMBRE = pNOMBRE;
            CARGO = pCARGO;
            CONTACTO = pCONTACTO;

        }
        public U_ClienteAsociado() { }
    }
}
