$(document).ready(function () {
    $("#boolchb1").on("click", function () {
        $("#searchartworkdiv").remove();
        $("#artworkpart").fadeIn(2000);
        $("#searchauthordiv").fadeIn(2000);
        $("#searchcategorydiv").fadeIn(2000);
        $("#searchartwork").val("");
        $("#artworkidchosen").val("");
    });
    var foundData;
    $("#searchartwork").on("keyup", function (event) {
        var inputText = document.getElementById("searchartwork").value.toUpperCase();
        if (inputText.length >= 1 && inputText.length<=3 && !event.shiftKey && inputText !== " ") {
            $.ajax({
                type: "GET",
                url: "/api/SearchArtWork",
                dataType: "json",
                data: {
                    "partialText": inputText
                },
                success: function (data) {
                    foundData = data;
                    let output = '<div class="modal-dialog"><div class="modal-content"><div class="modal-header"><h4 class="modal-title">Results</h4><button type="button" class="close closebtn" data-dismiss="modal">&times;</button></div><div class="modal-body"><ul class="list-group">';
                    if (!data.length) {
                        output += 'No results found';
                    }
                    else {
                        for (let i = 0; i < data.length; i++) {
                            output += '<li class="list-group-item artwork-item"><a href="#" data-artwid="' + data[i].artWorkId + '">' + data[i].name + '</a></li>';
                            if (i === 2)
                                break;
                        }
                    }
                    output += '</ul></div></div></div>';
                    var wheight = window.innerHeight / 3.5;
                    $(".modal").html(output);
                    $(".modal").css({
                        "top": wheight + "px"
                    }).fadeIn(600);
                },
                error: function (xhr) {
                    let output = '<div class="modal-dialog"><div class="modal-content"><div class="modal-header"><h4 class="modal-title">Results</h4><button type="button" class="close closebtn" data-dismiss="modal">&times;</button></div><div class="modal-body">' + xhr.statusText + '</div></div></div>';
                    var wheight = window.innerHeight / 3.5;
                    $(".modal").html(output).css({
                        "top": wheight + "px"
                    }).fadeIn(600);
                }
            });
        }
        else if (inputText.length > 1) {
            if ($(".modal").is(":hidden"))
                $(".modal").fadeIn(600);
            var filtered = [];
            for (let j = 0; j < foundData.length; j++) {
                if (foundData[j].name.toUpperCase().indexOf(inputText) !== -1) {
                    filtered.push(foundData[j]);
                }
            }
            let output = "";
            if (filtered.length) {
                for (let i = 0; i < filtered.length; i++) {
                    output += '<li class="list-group-item artwork-item"><a href="#" data-artwid="' + filtered[i].artWorkId + '">' + filtered[i].name + '</a></li>';
                    if (i === 2)
                        break;
                }
            }
            else {
                output += "No results found";
            }
            $(".list-group").html(output);
        }
        else {
            $(".modal").fadeOut(600);
        }
    });
    $(document).on("click", ".closebtn", function () {
        $(".modal").fadeOut(600);
    });
    $(document).on("click", ".artwork-item", function (e) {
        e.preventDefault();
        $("#artworkidchosen").val($(this).find("a").data("artwid"));
        $("#searchartwork").val($(this).find("a").text());
        $(".modal").fadeOut(600);
    });
});
$(document).on("submit", "form", function () {
    if ($("#categoryidchosen").val() !== '') {
        $("#categorynamepart").remove();
    } else {
        $("#categoryidchosen").remove();
    }
    if ($("#artworkidchosen").val() !== '') {
        $("#artworkpart .form-group").remove();
    }
    else {
        if ($("#ArtWork_ArtWorkName").val() !== '' && $("#ArtWork_Caption").val() !== '' && $("ArtWork.Image").val() !== '')
            $("#artworkidchosen").remove();
        else {
            $("#artworkpart").remove();
            $("#artworkidchosen").remove();
        }
    }
    if ($("#authoridchosen").val() !== '') {
        $("#authorpart").remove();
    }
    else {
        if ($("#Author_FirstName").val() !== '' && $("#Author_LastName").val() !== '' && $("#Author_DateOfBirth").val() !== '') 
            $("#authoridchosen").remove();
        else {
            $("#authorpart").remove();
            $("#authoridchosen").remove();
        }
    }
});