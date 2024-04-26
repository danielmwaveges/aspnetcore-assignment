using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QueueManagementSystem.MVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOnDeleteReferentialActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "ServiceProvider",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.DropForeignKey(
                name:"FK_ServicePoint_Service_ServiceId",
                table:"ServicePoint"
            );

            migrationBuilder.AddForeignKey(
                name:"FK_ServicePoint_Service_ServiceId",
                table:"ServicePoint",
                column:"ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.DropForeignKey(
                name:"FK_ServiceProvider_ServicePoint_ServicePointId",
                table:"ServiceProvider"
            );

            migrationBuilder.AddForeignKey(
                name:"FK_ServiceProvider_ServicePoint_ServicePointId",
                table:"ServiceProvider",
                column:"ServicePointId",
                principalTable: "ServicePoint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict
            );

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "ServiceProvider",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.DropForeignKey(
                name:"FK_ServicePoint_Service_ServiceId",
                table:"ServicePoint"
            );

            migrationBuilder.AddForeignKey(
                name:"FK_ServicePoint_Service_ServiceId",
                table:"ServicePoint",
                column:"ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.DropForeignKey(
                name:"FK_ServiceProvider_ServicePoint_ServicePointId",
                table:"ServiceProvider"
            );

            migrationBuilder.AddForeignKey(
                name:"FK_ServiceProvider_ServicePoint_ServicePointId",
                table:"ServiceProvider",
                column:"ServicePointId",
                principalTable: "ServicePoint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
