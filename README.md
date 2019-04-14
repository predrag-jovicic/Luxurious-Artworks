# Documentation for Luxurious Artworks Website

The main goal of this website is to enable users to create auctions for various luxurious artworks which can be bidden in real-time by registered users. When creating an auction, a user has to choose an artwork he/she wants to sell, enter an amount which will be a starting price. An auction can last forever. It all depends how much a creator of an auction wants to last. The creator can end an auction only when there is at least one bid placed. The creator can delete an auction whenever he wants. A bidder can place a bid which value is higher than a currently highest bid.

Front-end technologies used: HTML, CSS, jQuery, JavaScript.

Back-end technologies used: ASP.NET Core, SignalR Core.

###### Pages:

1. The whole **home page** is filled with the trending information. On the left side is the space for the hottest auction - that is an auction which is currently running with the most bids placed. On the right side is the space for the top selling artists - that is an artist which has the highest number of artworks sold.

2. **Active Auctions** page contains a list of active auctions which is being updated in real-time. Every time an auction finishes/is deleted a message is shown on the screen. Every time someone makes a new bid successfully, the amount of the highest bid gets updated. Every time someone makes a new auction it gets added to the list. Number of options which are available to the users who visit this page varies. A user who made an auction before has options: End auction, Delete auction. Registered users can make a bid and unregistered or unlogged users can only proceed to the page which shows details about the auction.

3. **Auction Details** page contains detailed information about an auction. It also has options which are available to users depending on their role. On the bottom of the page there are latest offers. When the auction ends/gets deleted or when a new bid is made the page automatically updates. All of these updates in real-time are conducted using SignalR library.

###### Pages that are visible only to logged users which haven't got administrative privileges.
4. **Create an auction** page enables a user to search for an existing artwork, enter an amount and then to create an auction.

5. **My auctions** is a page which contains a list of auctions which have been made by a currently logged user. There are finished and unfinished user's auctions. Every item of the list (an auction) has a link which points to the page for viewing all offers which have been made by users who participated. The page has a pagination.

6. **My bids** is a page which contains a list of bids which have been made by a currently logged user. The page has a pagination.

7. **My profile** is a page which contains information about a currently logged user. There's a button which points to a page for editing user information, a password and a photo change. The page also contains 3 latest user's auctions and 3 latest user's bids.

8. **Edit profile** is a page which enables a user to change basic information about them, change a password, change a profile photo. This page is available to an administrator as well.

###### Pages that are visible only to an administrator

9. **Artworks Index/Edit/Create/Delete/Update** are pages for manipulating artworks in database.

10. **Categories Index/Edit/Create/Delete/Update** are pages for manipulating categories in database.

11. **Authors Index/Edit/Create/Delete/Update** are pages for manipulating authors in database.

###### Pages that are visible only to anonymous user

12. **Sign in**

13. **Register**

> This is my basic implementation of a website which is auction themed. Of course, this project can be updated and new features can be added. This is just one of many simple starting solutions.
