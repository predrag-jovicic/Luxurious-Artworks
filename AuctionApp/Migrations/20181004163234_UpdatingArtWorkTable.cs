using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AuctionApp.Migrations
{
    public partial class UpdatingArtWorkTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Auctions_ArtWorkId",
                table: "Auctions");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_ArtWorkId",
                table: "Auctions",
                column: "ArtWorkId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Auctions_ArtWorkId",
                table: "Auctions");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_ArtWorkId",
                table: "Auctions",
                column: "ArtWorkId");
        }
    }
}
