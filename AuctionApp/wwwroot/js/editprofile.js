$(document).ready(function(){
    $("#browse").on("change", function(){
        $("#thickSpan").html('<i class="fas fa-check"></i>');
    });
    $("#passwdchb").on("change", function () {
        if (document.getElementById("passwdchb").checked) {
            $(".chpswd").fadeIn();
        }
        else {
            $(".chpswd").fadeOut();
        }
    });
});