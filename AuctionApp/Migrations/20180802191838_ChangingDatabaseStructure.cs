using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AuctionApp.Migrations
{
    public partial class ChangingDatabaseStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Auctions_ArtWorkId",
                table: "Auctions");

            migrationBuilder.DropIndex(
                name: "IX_ArtWorks_AuthorId",
                table: "ArtWorks");

            migrationBuilder.DropIndex(
                name: "IX_ArtWorks_CategoryId",
                table: "ArtWorks");

            migrationBuilder.AddColumn<bool>(
                name: "Sold",
                table: "ArtWorks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_ArtWorkId",
                table: "Auctions",
                column: "ArtWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtWorks_AuthorId",
                table: "ArtWorks",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtWorks_CategoryId",
                table: "ArtWorks",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Auctions_ArtWorkId",
                table: "Auctions");

            migrationBuilder.DropIndex(
                name: "IX_ArtWorks_AuthorId",
                table: "ArtWorks");

            migrationBuilder.DropIndex(
                name: "IX_ArtWorks_CategoryId",
                table: "ArtWorks");

            migrationBuilder.DropColumn(
                name: "Sold",
                table: "ArtWorks");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_ArtWorkId",
                table: "Auctions",
                column: "ArtWorkId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtWorks_AuthorId",
                table: "ArtWorks",
                column: "AuthorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtWorks_CategoryId",
                table: "ArtWorks",
                column: "CategoryId",
                unique: true);
        }
    }
}
