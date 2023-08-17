using BSP.POS.Presentacion.Models.Licencias;

namespace BSP.POS.Presentacion.Interfaces.Licencias
{
    public interface ILicenciasInterface
    {
        Task ObtenerEstadoDeLicencia();
        mLicencia licencia { get; set; }
    }
}
