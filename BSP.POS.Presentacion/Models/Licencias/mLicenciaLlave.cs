using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Licencias
{
    public class mLicenciaLlave : IValidatableObject
    {
        public IFormFile? archivo_llave { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (archivo_llave != null && Path.GetExtension(archivo_llave.FileName) != ".txt")
            {
                yield return new ValidationResult("El archivo debe ser tipo tipo .txt", new[] { nameof(archivo_llave) });
            }
        }
    }
}
