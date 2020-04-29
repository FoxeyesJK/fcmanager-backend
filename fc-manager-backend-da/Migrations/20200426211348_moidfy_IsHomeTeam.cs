using Microsoft.EntityFrameworkCore.Migrations;

namespace fc_manager_backend_da.Migrations
{
    public partial class moidfy_IsHomeTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isHomeTeam",
                table: "MatchRecords",
                newName: "IsHomeTeam");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsHomeTeam",
                table: "MatchRecords",
                newName: "isHomeTeam");
        }
    }
}
