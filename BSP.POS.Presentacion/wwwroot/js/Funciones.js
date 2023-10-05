window.clickButton = (element) => {
    element.click();
}

function guardarDocumento(filename, content) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = content;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function initTooltips() {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })
}

function ActivarScrollViewValidacion(clase) {
    var elementosDeError = document.querySelectorAll(clase);
    var hayErrores = elementosDeError.length > 0;

    if (hayErrores) {
        // Desplaza el scrollbar al primer elemento con error de validación
        var firstErrorElement = elementosDeError[0];
        firstErrorElement.scrollIntoView();
    }

}

function HayErroresValidacion(clase) {
    // Realiza la validación aquí, por ejemplo, verifica si hay elementos con la clase "error"
    var elementosDeError = document.querySelectorAll(clase);
    var hayErrores = elementosDeError.length > 0;

    if (hayErrores) {
        // Desplaza el scrollbar al primer elemento con error de validación
        var firstErrorElement = elementosDeError[0];
        firstErrorElement.scrollIntoView();
    }

    // Devuelve true si hay errores o false si no los hay
    return hayErrores;
}
