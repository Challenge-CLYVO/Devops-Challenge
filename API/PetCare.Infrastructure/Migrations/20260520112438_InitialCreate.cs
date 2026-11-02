using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clinica",
                columns: table => new
                {
                    id_clinica = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    endereco = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    telefone = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinica", x => x.id_clinica);
                });

            migrationBuilder.CreateTable(
                name: "Tutor",
                columns: table => new
                {
                    id_tutor = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    telefone = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutor", x => x.id_tutor);
                });

            migrationBuilder.CreateTable(
                name: "Vacina",
                columns: table => new
                {
                    id_vacina = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    descricao = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacina", x => x.id_vacina);
                });

            migrationBuilder.CreateTable(
                name: "Pet",
                columns: table => new
                {
                    id_pet = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    idade = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    especie = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    raca = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    id_tutor = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pet", x => x.id_pet);
                    table.ForeignKey(
                        name: "FK_Pet_Tutor_id_tutor",
                        column: x => x.id_tutor,
                        principalTable: "Tutor",
                        principalColumn: "id_tutor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Aplicacao_vacina",
                columns: table => new
                {
                    id_aplicacao = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    data_aplicacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    id_vacina = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    id_pet = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aplicacao_vacina", x => x.id_aplicacao);
                    table.ForeignKey(
                        name: "FK_Aplicacao_vacina_Pet_id_pet",
                        column: x => x.id_pet,
                        principalTable: "Pet",
                        principalColumn: "id_pet",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Aplicacao_vacina_Vacina_id_vacina",
                        column: x => x.id_vacina,
                        principalTable: "Vacina",
                        principalColumn: "id_vacina",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Consulta",
                columns: table => new
                {
                    id_consulta = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    data_consulta = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    descricao = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    id_pet = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    id_clinica = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consulta", x => x.id_consulta);
                    table.ForeignKey(
                        name: "FK_Consulta_Clinica_id_clinica",
                        column: x => x.id_clinica,
                        principalTable: "Clinica",
                        principalColumn: "id_clinica",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Consulta_Pet_id_pet",
                        column: x => x.id_pet,
                        principalTable: "Pet",
                        principalColumn: "id_pet",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Historico_saude",
                columns: table => new
                {
                    id_historico = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    descricao = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    data_registro = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    id_pet = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historico_saude", x => x.id_historico);
                    table.ForeignKey(
                        name: "FK_Historico_saude_Pet_id_pet",
                        column: x => x.id_pet,
                        principalTable: "Pet",
                        principalColumn: "id_pet",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aplicacao_vacina_id_pet",
                table: "Aplicacao_vacina",
                column: "id_pet");

            migrationBuilder.CreateIndex(
                name: "IX_Aplicacao_vacina_id_vacina",
                table: "Aplicacao_vacina",
                column: "id_vacina");

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_id_clinica",
                table: "Consulta",
                column: "id_clinica");

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_id_pet",
                table: "Consulta",
                column: "id_pet");

            migrationBuilder.CreateIndex(
                name: "IX_Historico_saude_id_pet",
                table: "Historico_saude",
                column: "id_pet");

            migrationBuilder.CreateIndex(
                name: "IX_Pet_id_tutor",
                table: "Pet",
                column: "id_tutor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aplicacao_vacina");

            migrationBuilder.DropTable(
                name: "Consulta");

            migrationBuilder.DropTable(
                name: "Historico_saude");

            migrationBuilder.DropTable(
                name: "Vacina");

            migrationBuilder.DropTable(
                name: "Clinica");

            migrationBuilder.DropTable(
                name: "Pet");

            migrationBuilder.DropTable(
                name: "Tutor");
        }
    }
}
