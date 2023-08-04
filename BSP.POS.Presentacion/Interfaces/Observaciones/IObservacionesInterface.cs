

using BSP.POS.Presentacion.Models.Observaciones;

namespace BSP.POS.Presentacion.Interfaces.Observaciones
{
    public interface IObservacionesInterface
    {
        List<mObservaciones> ListaDeObservacionesDeInforme { get; set; }
        Task ObtenerListaDeObservacionesDeInforme(string consecutivo, string esquema);
        Task AgregarObservacionDeInforme(mObservaciones observacion, string esquema);
    }
}
