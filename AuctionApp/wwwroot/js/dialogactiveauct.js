$(document).ready(function () {
    var currhigh;
    var startpr;
    var id;
    var highestbidelementtext;
    $(document).on("click", ".closebtn", function () {
        $("#biddingModal").fadeOut(500);
    });
    $(document).on("click", ".bid", function (event) {
        currhigh = $(this).data("currhigh").toString();
        startpr = $(this).data("startpr");
        currhigh = currhigh.split("<span>")[1].split("</span>")[0].split("$")[1];
        highestbidelementtext = $(this).siblings(".highestbid").children("strong").children("span");
        id = $(this).data("id");
        event.preventDefault();
        var wheight = window.innerHeight / 3.5;
        $("#biddingModal").html('<div class="modal-dialog"><div class="modal-content"><div class="modal-header"><h4 class="modal-title">Your offer</h4><button type="button" class="close closebtn" data-dismiss="modal">&times;</button></div><div class="modal-body"><input type="number" id="bidamount" class="form-control" /><span id="errmodal"></span></div><div class="modal-footer"><button type="button" class="btn closebtn" data-dismiss="modal">Close</button><button type="button" class="btn bidbtn" data-dismiss="modal">Bid</button></div></div></div>');
        $("#biddingModal").css({
            "top": wheight + "px"
        }).fadeIn(1100);
    });
    $(document).on("click", ".bidbtn", function () {
        var userNum = $("#bidamount").val();
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
                    $(".modal-body").html("You've just made a bid");
                    highestbidelementtext.text("$" + dataToSend.Amount);
                    setTimeout(function () {
                        $("#biddingModal").fadeOut(500);
                        $("#bid").fadeOut(500);
                    },2000);
                },
                error: function (xhr) {
                    $(".modal-body").html(xhr.statusText, xhr.statusText);
                    setTimeout(function () {
                        $("#biddingModal").fadeOut(500);
                        $("#bid").fadeOut(500);
                    }, 2000);
                }
            });
        }
    });
    $(".deleteAuctionbtn").on("click", function () {
        id = $(this).data("id");
        event.preventDefault();
        var output = '<div class="modal-dialog"><div class="modal-content"><div class="modal-header"><h4 class="modal-title">Deleting</h4><button type="button" class="close closebtn" data-dismiss="modal">&times;</button></div><div class="modal-body"><p>Are you sure you want to delete this auction?</p><div class="modal-footer"><button type="button" class="btn closebtn" data-dismiss="modal">No</button><button type="button" id="yesdelbtn" class="btn yesbtn" data-dismiss="modal">Yes</button></div></div></div>';
        $(".modal").html(output);
        var wheight = window.innerHeight / 3.5;
        $("#biddingModal").css({
            "top": wheight + "px"
        }).fadeIn(1100);
    });
    $(".endAuctionbtn").on("click", function () {
        id = $(this).data("id");
        event.preventDefault();
        var output = '<div class="modal-dialog"><div class="modal-content"><div class="modal-header"><h4 class="modal-title">Ending auction</h4><button type="button" class="close closebtn" data-dismiss="modal">&times;</button></div><div class="modal-body"><p>Do you want to finish this auction?</p><div class="modal-footer"><button type="button" class="btn closebtn" data-dismiss="modal">No</button><button type="button" class="btn yesbtn" id="yesendbtn" data-dismiss="modal">Yes</button></div></div></div>';
        $(".modal").html(output);
        var wheight = window.innerHeight / 3.5;
        $("#biddingModal").css({
            "top": wheight + "px"
        }).fadeIn(1100);
    });
    $(document).on("click", "#yesdelbtn", function () {
        $.ajax({
            type: "DELETE",
            url: "/api/AuctionApi",
            data: {
                "auctId" : id
            },
            success: function () {
                $(".modal-body").html("The auction has been deleted");
                $("#auct-" + id).fadeOut();
            },
            error: function (xhr) {
                $(".modal-body").html(xhr.responseText, xhr.statusText);
            }

        });
    });
    $(document).on("click", "#yesendbtn", function () {
        $.ajax({
            type: "PATCH",
            url: "/api/AuctionApi",
            data: {
                "auctId": id
            },
            success: function () {
                $(".modal-body").html("The auction has been terminated");
                $("#auct-" + id).fadeOut();
            },
            error: function (xhr) {
                $(".modal-body").html(xhr.responseText, xhr.statusText);
            }
        });
    });
});
