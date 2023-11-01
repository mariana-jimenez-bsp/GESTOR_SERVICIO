namespace BSP.POS.Presentacion.Interfaces.Alertas
{
    public interface IAlertasInterface
    {
        Task SwalAviso(string mensajeAlerta);
        Task SwalExito(string mensajeAlerta);
        Task SwalError(string mensajeAlerta);
        Task SwalAdvertencia(string mensajeAlerta);
        Task SwalAvisoCancelado(string mensajeAlerta);
        Task SwalExitoHecho(string mensajeAlerta);
        Task SwalAvisoLogin(string mensajeAlerta);
        Task SwalExitoLogin(string mensajeAlerta);

    }
}
