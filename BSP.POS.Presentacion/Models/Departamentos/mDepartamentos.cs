using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Departamentos
{
    public class mDepartamentos
    {
        public int Id { get; set; } = 0;
        public string codigo { get; set; } = string.Empty;
        [Required(ErrorMessage = "El nombre del departamento es requerido")]
        [StringLength(100, ErrorMessage = "Tamaño máximo de 100 caracteres")]
        public string Departamento { get; set; } = string.Empty;
    }
}
