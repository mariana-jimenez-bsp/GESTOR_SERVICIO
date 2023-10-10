using BSP.POS.Presentacion.Models.Licencias;

namespace BSP.POS.Presentacion.Interfaces.Licencias
{
    public interface ILicenciasInterface
    {
        Task ObtenerDatosDeLicencia();
        mLicencia licencia { get; set; }

       
    }

}
