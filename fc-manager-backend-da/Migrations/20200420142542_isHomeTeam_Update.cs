using Microsoft.EntityFrameworkCore.Migrations;

namespace fc_manager_backend_da.Migrations
{
    public partial class isHomeTeam_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isHomeTeam",
                table: "MatchRecords",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isHomeTeam",
                table: "MatchRecords");
        }
    }
}
