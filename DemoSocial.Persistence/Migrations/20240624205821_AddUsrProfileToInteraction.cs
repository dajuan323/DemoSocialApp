using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoSocial.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUsrProfileToInteraction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "interactionType",
                table: "PostInteraction",
                newName: "InteractionType");

            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileId",
                table: "PostInteraction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostInteraction_UserProfileId",
                table: "PostInteraction",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostInteraction_UserProfiles_UserProfileId",
                table: "PostInteraction",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostInteraction_UserProfiles_UserProfileId",
                table: "PostInteraction");

            migrationBuilder.DropIndex(
                name: "IX_PostInteraction_UserProfileId",
                table: "PostInteraction");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "PostInteraction");

            migrationBuilder.RenameColumn(
                name: "InteractionType",
                table: "PostInteraction",
                newName: "interactionType");
        }
    }
}
