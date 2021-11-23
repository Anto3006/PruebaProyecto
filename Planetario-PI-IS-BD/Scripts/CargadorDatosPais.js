window.onload = function () {
    llenarSelect("paises", "#paisSeleccionado");
    llenarSelect("lenguajes", "#listaIdiomas");
};
async function obtenerInformacionJson(nombreArchivo) {
    let url = "../JSON/" + nombreArchivo +".json";
    try {
        let res = await fetch(url);
        return await res.json();
    } catch (error) {
        console.log(error);
    }
}

async function llenarSelect(nombreArchivo, idContenedor) {
    let paises = await obtenerInformacionJson(nombreArchivo);
    paises = paises.Datos;
    for (let indicePais = 0; indicePais < paises.length; indicePais++) {
        $(idContenedor).append('<option value=' + paises[indicePais].replace(" ", "_") + '>' + paises[indicePais] + '</option>');
    }
}

