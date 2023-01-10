using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdentityForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserRefId",
                table: "TinyUrls",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_TinyUrls_UserRefId",
                table: "TinyUrls",
                column: "UserRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_TinyUrls_UserIdentities_UserRefId",
                table: "TinyUrls",
                column: "UserRefId",
                principalTable: "UserIdentities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TinyUrls_UserIdentities_UserRefId",
                table: "TinyUrls");

            migrationBuilder.DropIndex(
                name: "IX_TinyUrls_UserRefId",
                table: "TinyUrls");

            migrationBuilder.DropColumn(
                name: "UserRefId",
                table: "TinyUrls");
        }
    }
}
