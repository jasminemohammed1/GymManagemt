using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYMProject.Migrations
{
    /// <inheritdoc />
    public partial class addrelationshipbetweenmemberhealthrecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HealthRecordMemberId",
                table: "HealthRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_HealthRecordMemberId",
                table: "HealthRecords",
                column: "HealthRecordMemberId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecords_Members_HealthRecordMemberId",
                table: "HealthRecords",
                column: "HealthRecordMemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecords_Members_HealthRecordMemberId",
                table: "HealthRecords");

            migrationBuilder.DropIndex(
                name: "IX_HealthRecords_HealthRecordMemberId",
                table: "HealthRecords");

            migrationBuilder.DropColumn(
                name: "HealthRecordMemberId",
                table: "HealthRecords");
        }
    }
}
