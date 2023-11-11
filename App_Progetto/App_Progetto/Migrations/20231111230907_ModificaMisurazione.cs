using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Progetto.Migrations
{
    /// <inheritdoc />
    public partial class ModificaMisurazione : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Misurazione",
                table: "Misurazione");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Misurazione",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Misurazione",
                table: "Misurazione",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Misurazione",
                table: "Misurazione");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Misurazione");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Misurazione",
                table: "Misurazione",
                columns: new[] { "DataOra", "CodiceSensore" });
        }
    }
}
