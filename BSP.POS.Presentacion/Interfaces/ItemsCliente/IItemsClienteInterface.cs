using BSP.POS.Presentacion.Models.ItemsCliente;

namespace BSP.POS.Presentacion.Interfaces.ItemsCliente
{
    public interface IItemsClienteInterface
    {
        Task ObtenerLaListaDeCondicionesDePago(string esquema);

        List<mItemsCliente> listaCondicionesDePago { get; set; }

        Task ObtenerLaListaDeNivelesDePrecio(string esquema, string moneda);

        List<mItemsCliente> listaNivelesDePrecio { get; set; }

        Task ObtenerLosTiposDeImpuestos(string esquema);

        List<mItemsCliente> listaTiposDeImpuestos { get; set; }

        Task ObtenerLosTiposDeTarifasDeImpuesto(string esquema, string impuesto);

        List<mTarifa> listaDeTarifasDeImpuesto { get; set; }

        Task<decimal> ObtenerElPorcentajeDeTarifa(string esquema, string impuesto, string tipoTarifa);
        Task<string> ObtenerElSiguienteCodigoCliente(string esquema, string letra);
        Task ObtenerLosTiposDeNit(string esquema);

        List<mItemsCliente> listaTiposDeNit { get; set; }

        Task ObtenerListaDeCentrosDeCosto(string esquema);

        List<mItemsCliente> listaCentrosDeCosto { get; set; }

    }
}
