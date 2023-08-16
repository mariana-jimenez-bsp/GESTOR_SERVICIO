
using BSP.POS.UTILITARIOS.Correos;
using BSP.POS.UTILITARIOS.CorreosModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.CorreosService
{
    public interface ICorreosInterface
    {
        void EnviarCorreoRecuperarClave(U_Correo datos, string token, string esquema);
        void EnviarCorreoAprobarInforme(U_Correo datos, mObjetosParaCorreoAprobacion objetosParaAprobacion);
        void ReenvioDeInforme(U_Correo datos, mObjetosParaCorreoAprobacion objetosParaAprobacion);
    }
}
