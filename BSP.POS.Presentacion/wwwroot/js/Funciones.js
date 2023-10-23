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
function ScriptMaxHeightExtra(contenido, elementosJson) {
    const elementos = JSON.parse(elementosJson);
    const elements = elementos.map(elemento => document.querySelector(elemento));
    const content = document.querySelector(contenido);

    if (elements.includes(null) || !content) {
        return;
    }

    const totalHeight = elements.reduce((acc, elemento) => acc + elemento.offsetHeight, 0);
    const windowHeight = window.innerHeight;
    const availableHeight = windowHeight - totalHeight - 50;

    content.style.maxHeight = availableHeight + 'px';
    content.style.minHeight = availableHeight + 'px';
}

function ScriptMaxHeightExtraContainer(contenido, contenedor, elementosJson) {
    const elementos = JSON.parse(elementosJson);
    const elements = elementos.map(elemento => document.querySelector(elemento));
    const content = document.querySelector(contenido);
    const container = document.querySelector(contenedor);

    if (elements.includes(null) || !content || !container) {
        return;
    }

    const totalHeight = elements.reduce((acc, elemento) => acc + elemento.offsetHeight, 0);
    const windowHeight = window.innerHeight;
    const availableHeight = windowHeight - totalHeight - 50;
    const HeightContent = availableHeight - 100;
    container.style.maxHeight = availableHeight + 'px';
    container.style.minHeight = availableHeight + 'px';
    content.style.maxHeight = HeightContent + 'px';
}
window.HacerSelectEditable = function (objRef, selectId) {
    var select = document.getElementById("select-actividad-" + selectId);
    var text = select.options[select.selectedIndex].text;
    var input = document.createElement("input");
    input.value = text;
    input.id = "inputEditable";
    input.classList.add("border-0", "bg-transparent", "text-light", "text-center");

    select.parentNode.replaceChild(input, select);

    input.addEventListener("blur", function () {
        var updatedText = input.value;
        input.parentNode.replaceChild(select, input);
        select.options[select.selectedIndex].text = updatedText;

        // Llama a la función de Blazor para enviar el texto editado.
        objRef.invokeMethodAsync('ActualizarTextoEditable', updatedText, selectId);
    });
};