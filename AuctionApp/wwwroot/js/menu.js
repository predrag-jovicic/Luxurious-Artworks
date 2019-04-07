$(document).ready(function(){
    $("#btnexpand").on("click", function () {
        var wheight = window.innerHeight + "px";
        var lheight = window.innerHeight / 150;
        $(".navbar-nav").css("height", wheight);
        $(".navbar-nav").css("line-height", lheight);
        $(".navbar-nav").slideToggle();
    });
});