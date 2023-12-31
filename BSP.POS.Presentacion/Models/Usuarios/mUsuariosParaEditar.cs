﻿using BSP.POS.Presentacion.Interfaces.Usuarios;
using BSP.POS.Presentacion.Models.Permisos;
using BSP.POS.Presentacion.Services.Usuarios;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Usuarios
{
    public class mUsuariosParaEditar : IValidatableObject
    {
        public string id { get; set; } = string.Empty;
        public string codigo { get; set; } = string.Empty;
        [StringLength(20, ErrorMessage = "Tamaño máximo de 20 caracteres")]
        [Required(ErrorMessage = "El cliente es requerido")]
        public string cod_cliente { get; set; } = string.Empty;
        [StringLength(25, ErrorMessage = "Tamaño máximo de 25 caracteres")]
        [Required(ErrorMessage = "El Usuario es requerido")]
        public string usuario { get; set; } = string.Empty;
        [StringLength(249, ErrorMessage = "Tamaño máximo de 249 caracteres")]
        [EmailAddress(ErrorMessage = "Ingresa una dirección de correo electrónico válida")]
        [Required(ErrorMessage = "El Correo es requerido")]
        public string correo { get; set; } = string.Empty;
        [Required(ErrorMessage = "La clave es requerida")]
        public string clave { get; set; } = string.Empty;
        [StringLength(100, ErrorMessage = "Tamaño máximo de 100 caracteres")]
        [Required(ErrorMessage = "El Nombre de la empresa es requerido")]
        public string nombre { get; set; } = string.Empty;
        [Required(ErrorMessage = "El rol es requerido")]
        [StringLength(10, ErrorMessage = "Tamaño máximo de 10 caracteres")]
        public string rol { get; set; } = string.Empty;
        [Required(ErrorMessage = "El teléfono es requerido")]
        [StringLength(50, ErrorMessage = "Tamaño máximo de 50 caracteres")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo tiene que ser un teléfono válido")]
        public string telefono { get; set; } = string.Empty;
        [Required(ErrorMessage = "El departamento es requerido")]
        public string codigo_departamento { get; set; } = string.Empty;
        public byte[] imagen { get; set; } = new byte[] { 0x00 };
        public IFormFile? ImagenFile { get; set; }
        public string? claveOriginal { get; set; } = string.Empty;
        public string usuarioOrignal { get; set; } = string.Empty;
        public string correoOriginal { get; set; } = string.Empty;
        public string usuarioRepite { get; set; } = string.Empty;
        public string mensajeUsuarioRepite { get; set; } = string.Empty;
        public string correoRepite { get; set; } = string.Empty;
        public string mensajeCorreoRepite { get; set; } = string.Empty;
        [StringLength(100, MinimumLength = 8, ErrorMessage = "El tamaño debe ser entre 8 a 100 caracteres")]
        [RegularExpression(@"^(?=.*\d)(?=.*[!@#$%^&*()_+\-=?.])(?=.*[A-Z]).+$", ErrorMessage = "La contraseña debe contener al menos un dígito, al menos un carácter de la lista !@#$%^&*()_+-=?. y al menos una letra mayúscula.")]
        public string? claveDesencriptada { get; set; }
        [Required(ErrorMessage = "El pais del télefono es requerido")]
        public string paisTelefono { get; set; } = string.Empty;
        public int IdCodigoTelefono { get; set; } = 0;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ImagenFile != null)
            {
                bool validacion = false;
                if (Path.GetExtension(ImagenFile.FileName) == ".png" || Path.GetExtension(ImagenFile.FileName) == ".jpeg" || Path.GetExtension(ImagenFile.FileName) == ".jpg")
                {
                    validacion = true;
                }
                if (!validacion)
                {
                    yield return new ValidationResult("La imagen debe ser tipo tipo .png o .jpg", new[] { nameof(ImagenFile) });
                }
                
            }
            if (paisTelefono == "CRI")
            {
                if (telefono.Length != 8)
                {
                    yield return new ValidationResult("Un número de télefono de Costa Rica debe tener 8 dígitos", new[] { nameof(telefono) });
                }
            }
            
        }
    }



}
