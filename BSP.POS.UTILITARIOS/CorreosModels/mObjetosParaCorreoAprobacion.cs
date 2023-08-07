using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSP.POS.UTILITARIOS.CorreosModels.Models;

namespace BSP.POS.UTILITARIOS.CorreosModels
{
    public class mObjetosParaCorreoAprobacion
    {
        public mInformesAsociado informe { get; set; } = new mInformesAsociado();
        public string esquema { get; set; } = string.Empty;
        public mClientesAsociado ClienteAsociado { get; set; } = new mClientesAsociado();
        public List<mLasActividadesAsociadas> listaActividadesAsociadas { get; set; } = new List<mLasActividadesAsociadas>();
        public List<mUsuariosDeClientesDeInforme> listadeUsuariosDeClienteDeInforme { get; set; } = new List<mUsuariosDeClientesDeInforme>();
        public int total_horas_cobradas { get; set; } = 0;
        public int total_horas_no_cobradas { get; set; } = 0;
        public List<mLasObservaciones> listaDeObservaciones { get; set; } = new List<mLasObservaciones>();
    }
}
