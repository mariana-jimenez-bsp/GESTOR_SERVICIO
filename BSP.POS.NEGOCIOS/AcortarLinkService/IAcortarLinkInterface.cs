using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.AcortarLinkService
{
    public interface IAcortarLinkInterface
    {
        Task<string> AcortarLink(string token);
    }
}
