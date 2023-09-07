function AlertaDeError(mensaje) {
    Swal.fire({
        title: 'Error!',
        text: mensaje,
        icon: 'error',
        confirmButtonText: 'Aceptar'
    })
}

function AlertaDeExito(mensaje) {
    Swal.fire({
        title: 'Éxito!',
        text: mensaje,
        icon: 'success',
        confirmButtonText: 'Aceptar'
    })
}

function AlertaDeAdvertencia(mensaje) {
    Swal.fire({
        title: 'Advertencia!',
        text: mensaje,
        icon: 'warning',
        confirmButtonText: 'Aceptar'
    })
}