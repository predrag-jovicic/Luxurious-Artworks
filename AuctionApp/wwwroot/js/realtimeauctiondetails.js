"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/auctionhub").build();

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
    });

connection.on("EndAuctionDetailsPage", function (message,url) {
    alert(message);
    window.location.href = url;
});

connection.on("AddOffer", function (offer) {
    var date = new Date(Date.parse(offer.time));
    var newOffer = `<div class="card"><div class="card-body"><h5 class="card-title">Bidder: XXX</h5><h6>Amount: $${offer.amount}</h6><p class="card-text">Time: ${date}</p></div></div>`;
    $("#nooffersmessage").remove();
    $(newOffer).insertAfter("#latestoffers h4");
});

connection.on("DeleteAuctionDetailsPage", function (message,url) {
    alert(message);
    window.location.href = url;
});