function imprimirDocumento(content, nombre) {
    var win = window.open(content, "_blank");
    win.document.write('<embed width="100%" height="100%" src="' + content + '" type="application/pdf">');
    win.document.close();
    win.print(); // Intenta iniciar la impresión.

}