using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Progetto.Migrations
{
    /// <inheritdoc />
    public partial class SetTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Terreno",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Mappale = table.Column<int>(type: "INTEGER", nullable: false),
                    Foglio = table.Column<int>(type: "INTEGER", nullable: false),
                    Ettari = table.Column<int>(type: "INTEGER", nullable: false),
                    CittaTerreno = table.Column<string>(type: "TEXT", nullable: false),
                    TipoColtura = table.Column<string>(type: "TEXT", nullable: false),
                    TipoTerreno = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terreno", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attuatore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TipoAttuatore = table.Column<string>(type: "TEXT", nullable: false),
                    Standby = table.Column<bool>(type: "INTEGER", nullable: false),
                    Attivazione = table.Column<bool>(type: "INTEGER", nullable: false),
                    TerrenoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attuatore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attuatore_Terreno_TerrenoId",
                        column: x => x.TerrenoId,
                        principalTable: "Terreno",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gestione",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Ruolo = table.Column<string>(type: "TEXT", nullable: false),
                    TerrenoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gestione", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gestione_Terreno_TerrenoId",
                        column: x => x.TerrenoId,
                        principalTable: "Terreno",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sensore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StatoSensore = table.Column<bool>(type: "INTEGER", nullable: false),
                    TipoSensore = table.Column<string>(type: "TEXT", nullable: false),
                    TerrenoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensore_Terreno_TerrenoId",
                        column: x => x.TerrenoId,
                        principalTable: "Terreno",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Piano",
                columns: table => new
                {
                    CodicePiano = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrarioAttivazione = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    OrarioDisattivazione = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    OrarioAttDefault = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    OrarioDisattDefault = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    CondAttivazione = table.Column<string>(type: "TEXT", nullable: false),
                    CondDisattivazione = table.Column<string>(type: "TEXT", nullable: false),
                    CodAtt = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Piano", x => x.CodicePiano);
                    table.ForeignKey(
                        name: "FK_Piano_Attuatore_CodAtt",
                        column: x => x.CodAtt,
                        principalTable: "Attuatore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Misurazione",
                columns: table => new
                {
                    DataOra = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CodiceSensore = table.Column<int>(type: "INTEGER", nullable: false),
                    Valore = table.Column<float>(type: "REAL", nullable: false),
                    TipoMisurazione = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Misurazione", x => new { x.DataOra, x.CodiceSensore });
                    table.ForeignKey(
                        name: "FK_Misurazione_Sensore_CodiceSensore",
                        column: x => x.CodiceSensore,
                        principalTable: "Sensore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attuatore_TerrenoId",
                table: "Attuatore",
                column: "TerrenoId");

            migrationBuilder.CreateIndex(
                name: "IX_Gestione_TerrenoId",
                table: "Gestione",
                column: "TerrenoId");

            migrationBuilder.CreateIndex(
                name: "IX_Misurazione_CodiceSensore",
                table: "Misurazione",
                column: "CodiceSensore");

            migrationBuilder.CreateIndex(
                name: "IX_Piano_CodAtt",
                table: "Piano",
                column: "CodAtt",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sensore_TerrenoId",
                table: "Sensore",
                column: "TerrenoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gestione");

            migrationBuilder.DropTable(
                name: "Misurazione");

            migrationBuilder.DropTable(
                name: "Piano");

            migrationBuilder.DropTable(
                name: "Sensore");

            migrationBuilder.DropTable(
                name: "Attuatore");

            migrationBuilder.DropTable(
                name: "Terreno");
        }
    }
}
