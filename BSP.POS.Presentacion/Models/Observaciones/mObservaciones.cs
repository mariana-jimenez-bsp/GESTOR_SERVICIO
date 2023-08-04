﻿using System.ComponentModel.DataAnnotations;

namespace BSP.POS.Presentacion.Models.Observaciones
{
    public class mObservaciones
    {
        public string Id { get; set; } = string.Empty;
        public string consecutivo_informe { get; set; } = string.Empty;
        public string usuario { get; set; } = string.Empty;
        [StringLength(4000, ErrorMessage = "Tamaño máximo de 4000 caracteres")]
        [Required(ErrorMessage = "Para agregar un comentario no puede estar vacío")]
        public string observacion { get; set; } = string.Empty;

    }
}
