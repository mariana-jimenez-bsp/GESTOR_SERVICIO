using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Usuarios;

namespace BSP.POS.Presentacion.Models.Informes
{
    public class mEditarInforme
    {
        public mInformeAsociado informeAsociado { get; set; } = new mInformeAsociado();
        public List<mActividadesAsociadas> actividadesAsociadas { get; set; } = new List<mActividadesAsociadas>();
        public List<mUsuariosDeClienteDeInforme> usuariosDeClienteDeInformes { get; set; } = new List<mUsuariosDeClienteDeInforme>();

    }
}
