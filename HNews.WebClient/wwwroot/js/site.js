// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
  // Mostrar el loader durante las solicitudes AJAX
$(document).ajaxStart(function () {
    console.log("se ha iniciado")
    $("#overlay").css("display", "flex");
});

$(document).ajaxStop(function () {
    console.log("se ha detenido")
    $("#overlay").hide();
});

function DisplayLoading() {
    console.log("se ha iniciado")
    $("#overlay").css("display", "flex");
}