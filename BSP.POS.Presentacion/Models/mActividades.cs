﻿
using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models
{
    public class mActividades
    {
        public string Id { get; set; } = string.Empty;
        public string codigo { get; set; } = string.Empty;
        [Required(ErrorMessage ="El campo del nombre es requerido")]
        public string Actividad { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del CI-Referencia es requerido")]
        public string CI_referencia { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de Horas es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de horas debe ser un número entero válido.")]
        public string horas { get; set; } = string.Empty;
    }
}
