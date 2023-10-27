namespace BSP.POS.Presentacion.Interfaces.Alertas
{
    public interface IAlertasInterface
    {
        Task SwalAviso(string mensajeAlerta);
        Task SwalExito(string mensajeAlerta);
        Task SwalError(string mensajeAlerta);
        Task SwalAdvertencia(string mensajeAlerta);
    }
}
