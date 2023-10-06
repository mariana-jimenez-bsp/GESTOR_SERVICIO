using BSP.POS.UTILITARIOS.Correos;
using BSP.POS.UTILITARIOS.CorreosModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.WhatsappService
{
    public interface IWhatsappInterface
    {
        Task EnviarWhatsappAprobarInforme(mObjetosParaCorreoAprobacion objetosParaAprobacion, string token, string idTelefono, string tipoInicio);
    }
}
