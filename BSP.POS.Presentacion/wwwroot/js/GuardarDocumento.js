function guardarDocumento(filename, content) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = content;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}