using BSP.POS.Presentacion.Models.Departamentos;

namespace BSP.POS.Presentacion.Interfaces.Departamentos
{
    public interface IDepartamentosInterface
    {
        List<mDepartamentos> listaDepartamentos { get; set; }
        Task ObtenerListaDeDepartamentos(string esquema);
        Task<bool> ActualizarListaDeDepartamentos(List<mDepartamentos> listaDepartamentosActualizar, string esquema);
        Task<bool> AgregarDepartamento(mDepartamentos departamento, string esquema);
        
    }
}
