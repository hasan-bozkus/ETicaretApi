using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETicaretApi.Presistence.Migrations
{
    public partial class mig2_add_column_showcase_in_Proudctimagefile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Showcase",
                table: "Files",
                type: "boolean",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Showcase",
                table: "Files");
        }
    }
}
