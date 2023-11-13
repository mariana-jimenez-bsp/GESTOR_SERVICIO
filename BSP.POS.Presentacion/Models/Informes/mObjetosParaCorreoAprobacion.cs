using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Observaciones;
using BSP.POS.Presentacion.Models.Usuarios;

namespace BSP.POS.Presentacion.Models.Informes
{
    public class mObjetosParaCorreoAprobacion
    {
        public mInforme informe { get; set; } = new mInforme();
        public string esquema { get; set; } = string.Empty;
        public mClienteAsociado ClienteAsociado { get; set; } = new mClienteAsociado();
        public List<mActividadesAsociadas> listaActividadesAsociadas { get; set; } = new List<mActividadesAsociadas>();
        public List<mUsuariosDeInforme> listadeUsuariosDeClienteDeInforme { get; set; } = new List<mUsuariosDeInforme>();
        public int total_horas_cobradas { get; set; } = 0;
        public int total_horas_no_cobradas { get; set; } = 0;

        public List<mObservaciones> listaDeObservaciones { get; set; } = new List<mObservaciones>();
    }
}
