$(document).ready(function () {
    var currhigh;
    var startpr;
    var id;
    $(".closebtn").on("click", function () {
        $("#biddingModal").fadeOut(500);
    });
    $("#bid").on("click", function (event) {
        currhigh = $("#bid").attr("data-currhigh").toString();
        startpr = $("#bid").attr("data-startpr");
        currhigh = parseFloat(currhigh.split("<span>")[1].split("</span>")[0].split("$")[1]);
        id = $("#bid").attr("data-id");
        event.preventDefault();
        var wheight = window.innerHeight / 3.5;
        $("#biddingModal").css({
            "top": wheight + "px"
        }).fadeIn(1100);
    });
    $(".bidbtn").on("click", function () {
        var userNum = parseFloat($("#bidamount").val());
        var dataToSend = {
            "Amount": Number(userNum),
            "AuctionId": id
        };
        if (userNum <= currhigh || userNum <= startpr) {
            $("#errmodal").text("Your amount is not in range");
        }
        else {
            $(".modal-body").html("Bidding...");
            $.ajax({
                type: "POST",
                url: "/api/Bidding",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(dataToSend),
                success: function () {
                    var newPrice = "<span>$" + userNum + "</span>";
                    $("#bid").attr("data-currhigh", newPrice);
                    $(".modal-body").html("You've just made a bid");
                    setTimeout(function () {
                        $("#biddingModal").hide();
                        $(".modal-body").html('<input type="number" id="bidamount" class="form-control"/><span id="errmodal"></span>');
                    }, 2000);
                },
                error: function (xhr) {
                    $(".modal-body").html(xhr.responseText, xhr.statusText);
                    setTimeout(function () {
                        $("#biddingModal").hide();
                        $(".modal-body").html('<input type="number" id="bidamount" class="form-control"/><span id="errmodal"></span>');
                    }, 2000);
                }
            });
        }
    });
});
