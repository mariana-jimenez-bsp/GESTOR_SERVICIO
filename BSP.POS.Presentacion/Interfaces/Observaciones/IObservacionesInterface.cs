

using BSP.POS.Presentacion.Models.Observaciones;

namespace BSP.POS.Presentacion.Interfaces.Observaciones
{
    public interface IObservacionesInterface
    {
        List<mObservaciones> ListaDatosDeObservacionesDeInforme { get; set; }
        Task ObtenerListaDatosDeObservacionesDeInforme(string consecutivo, string esquema);
        Task<bool> AgregarObservacionDeInforme(mObservaciones observacion, string esquema);
    }
}
