using BSP.POS.Presentacion.Models.Licencias;

namespace BSP.POS.Presentacion.Interfaces.Licencias
{
    public interface ILicenciasInterface
    {
        Task ObtenerDatosDeLicencia();
        mDatosLicencia licencia { get; set; }

        Task ObtenerCodigoDeLicencia();

        mCodigoLicencia codigoLicencia { get; set; }

        Task<bool> ActualizarDatosLicencia(mActualizarDatosLicencia datosLicencia);
    }

}
