using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Usuarios
{
    public class U_UsuariosDeInforme
    {
        public string id { get; set; }
        public string consecutivo_informe { get; set; }
        public string codigo_usuario { get; set; }
        public string recibido { get; set; }



        public U_UsuariosDeInforme(string pId, string pConsecutivo_informe, string pCodigo_usuario, string pRecibido)
        {
            id = pId;
            consecutivo_informe = pConsecutivo_informe;
            codigo_usuario = pCodigo_usuario;
            recibido = pRecibido;
        }
        public U_UsuariosDeInforme() { }
    }
}
