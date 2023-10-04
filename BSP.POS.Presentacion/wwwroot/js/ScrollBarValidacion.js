function ActivarScrollViewValidacion() {
    var firstErrorElement = document.querySelector(".validation-message");

    if (firstErrorElement != null) {
        // Desplaza el scrollbar al primer elemento con error de validación
        firstErrorElement.scrollIntoView();
    }

}
