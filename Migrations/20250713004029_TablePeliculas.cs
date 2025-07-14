using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ApiRestPeliculas.Migrations
{
    /// <inheritdoc />
    public partial class TablePeliculas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Peliculas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    imagen = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Duracion = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Clasificacion = table.Column<int>(type: "integer", nullable: false),
                    categoriaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peliculas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Peliculas_Categorias_categoriaId",
                        column: x => x.categoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Peliculas_categoriaId",
                table: "Peliculas",
                column: "categoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Peliculas");
        }
    }
}
