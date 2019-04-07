"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/auctionhub").build();

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("EndAuction", function (message, id) {
    $("#auct-" + id).html(message);
});
connection.on("AddAuctionAuthenticated", function (auction) {
    $("#noauctionsmessage").remove();
    var date = new Date(Date.parse(auction.postedOn));
    var newAuction =
        `<div class="card" id="auct-${auction.auctionId}">
        <div class="card-body">
            <h5 class="card-title">${auction.artWorkName} (${auction.author})</h5>
            <h6><strong>Starting price: $${auction.startingPrice}</strong></h6>
            <p class="card-text">${auction.caption}</p>
            <p class="card-text">Posted on: ${date} by ${auction.user}</p>
            <p class="highestbid">Currently the highest bid: <strong>No bids yet</strong></p>
            <a href="${auction.readMoreLink}" class="card-link">Read more...</a>
            <a href="#" data-currhigh="<span>No bids yet</span>" data-id="${auction.auctionId}" class="card-link bid">Make a bid</a>
            </div>
        </div>`;
    $("#auctions").prepend(newAuction);
});
connection.on("AddAuctionUnAuthenticated", function (auction) {
    $("#noauctionsmessage").remove();
    var date = new Date(Date.parse(auction.postedOn));
    var newAuction =
        `<div class="card" id="auct-${auction.auctionId}">
        <div class="card-body">
            <h5 class="card-title">${auction.artWorkName} (${auction.author})</h5>
            <h6><strong>Starting price: $${auction.startingPrice}</strong></h6>
            <p class="card-text">${auction.caption}</p>
            <p class="card-text">Posted on: ${date} by ${auction.user}</p>
            <p class="highestbid">Currently the highest bid: <strong>No bids yet</strong></p>
            <a href="${auction.readMoreLink}" class="card-link">Read more...</a>
            </div>
        </div>`;
    $("#auctions").prepend(newAuction);
});
connection.on("DeleteAuction", function (message, id) {
    $("#auct-" + id).html(message);
});
connection.on("NewBid", function (message, id) {
    $("#auct-" + id).find(".highestbid").html(message);
    $("#auct-" + id).find(".highestbid").animate({ letterSpacing: "+=2px" }, 1500);
    setTimeout(function () {
        $("#auct-" + id).find(".highestbid").animate({ letterSpacing: "-=2px" }, 1500);
    }, 2500);
});