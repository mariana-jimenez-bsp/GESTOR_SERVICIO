using BSP.POS.Presentacion.Models.Lugares;

namespace BSP.POS.Presentacion.Interfaces.Lugares
{
    public interface ILugaresInterface
    {
        Task ObtenerListaDePaises(string esquema);
        Task ObtenerListaDeProvinciasPorPais(string esquema, string pais);
        Task ObtenerListaDeCantonesPorProvincia(string esquema, string pais, string provincia);
        Task ObtenerListaDeDistritosPorCanton(string esquema, string pais, string provincia, string canton);
        Task ObtenerListaDeBarriosPorDistrito(string esquema, string pais, string provincia, string canton, string distrito);

        List<mLugares> listaDePaises { get; set; }
        List<mLugares> listaDeProvincias { get; set; }
        List<mLugares> ListaDeCantones { get; set; }
        List<mLugares> listaDeDistritos { get; set; }
        List<mLugares> listaDeBarrios { get; set; }
    }
}
