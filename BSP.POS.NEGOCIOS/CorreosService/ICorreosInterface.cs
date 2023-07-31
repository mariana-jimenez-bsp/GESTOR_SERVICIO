using BSP.POS.UTILITARIOS.Correos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.CorreosService
{
    public interface ICorreosInterface
    {
        void EnviarCorreo(U_Correo datos, string token, string esquema);
    }
}
