window.clickButton = (element) => {
    element.click();
}

window.clickInput = (element) => {
    element.querySelector('input').click();
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
function ScriptMaxHeight(contenido, elemento1,elemento2, elemento3) {
    
    const headerElement = document.querySelector(elemento1);
    const footerElement = document.querySelector(elemento2);
    const headersContentElement = document.querySelector(elemento3);
    const content = document.querySelector(contenido);
    if (!headerElement || !footerElement || !headersContentElement || !content) {
        return;
    }

    const headerHeight = document.querySelector(elemento1).offsetHeight;
    const footerHeight = document.querySelector(elemento2).offsetHeight;
    const headersContentHeight = document.querySelector(elemento3).offsetHeight;
    const windowHeight = window.innerHeight;
    const availableHeight = windowHeight - headerHeight - footerHeight - headersContentHeight - 50;
    content.style.maxHeight = availableHeight + 'px';
}