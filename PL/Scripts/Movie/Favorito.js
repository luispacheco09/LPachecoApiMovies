function AddFavorito(id, e) {
    var url = '/Movie/AddFavoritas';
    var Favorita = e.checked
    // peticiones del lado del servidor
    $.ajax({
        type: 'POST',
        url: url,
        dataType: 'json',
        data: { id, Favorita },
        success: function (ok) {
            alert('Se añadio correctamente a favoritos');
        }
    });
}


function DeleteFavorito(id, e) {
    var Favorita = e.checked
    var url = '/Movie/AddFavoritas';

    // peticiones del lado del servidor
    $.ajax({
        type: 'POST',
        url: url,
        dataType: 'json',
        data: { id, Favorita },
        success: function (ok) {
            //if (resultUsuario.Correct == true) {
                alert('Se ha eliminado de favoritos')
            //}
            //else {
            //    alert('Se ha actualizado con exito el status' + resultUsuario.ErrorMessage)
            //}
        },
        error: function (ex) {
            alert('Failed.' + ex);
        }
    });
}