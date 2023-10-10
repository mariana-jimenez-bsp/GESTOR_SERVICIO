﻿namespace BSP.POS.Presentacion.Models.Licencias
{
    public class mLicencia
    {
        public DateTime FechaInicio { get; set; } = DateTime.Now;
        public DateTime FechaFin { get; set; } = DateTime.Now;
        public DateTime FechaAviso { get; set; } = DateTime.Now;
        public int CantidadUsuarios { get; set; } = 0;
        public string MacAddress { get; set; } = string.Empty;
        public string MacAddressActual { get; set; } = string.Empty;
    }
}
