using BSP.POS.Presentacion.Models.Licencias;

namespace BSP.POS.Presentacion.Interfaces.Licencias
{
    public interface ILicenciasInterface
    {
        Task ObtenerDatosDeLicencia();
        mDatosLicencia licencia { get; set; }

        Task ObtenerCodigoDeLicenciaYProducto();
        Task<string> ObtenerCodigoDeLicenciaDesencriptado();

        mCodigoLicenciaYProducto codigoLicenciaYProducto { get; set; }

        Task<bool> ActualizarDatosLicencia(mLicencia datosLicencia, byte[] codigoLicencia);
    }

}
