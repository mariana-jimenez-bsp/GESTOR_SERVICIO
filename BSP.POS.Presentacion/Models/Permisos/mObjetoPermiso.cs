namespace BSP.POS.Presentacion.Models.Permisos
{
    public class mObjetoPermiso
    {
        public string permiso { get; set; } = string.Empty;
        public List<string> subpermisos { get; set; } = new List<string>();
    }
}
