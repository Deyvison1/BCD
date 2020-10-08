using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BCD.Repository.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enderecos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CEP = table.Column<string>(nullable: true),
                    Logradouro = table.Column<string>(nullable: true),
                    Complemento = table.Column<string>(nullable: true),
                    Bairro = table.Column<string>(nullable: true),
                    Localidade = table.Column<string>(nullable: true),
                    UF = table.Column<int>(nullable: false),
                    Unidade = table.Column<string>(nullable: true),
                    IBGE = table.Column<int>(nullable: false),
                    GIA = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enderecos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Historicos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataTransacao = table.Column<DateTime>(nullable: false),
                    Valor = table.Column<float>(nullable: false),
                    DescricaoTransacao = table.Column<string>(nullable: true),
                    TipoConta = table.Column<int>(nullable: false),
                    NomeContaDestino = table.Column<string>(nullable: true),
                    Operacao = table.Column<int>(nullable: false),
                    NomeConta = table.Column<string>(nullable: true),
                    DigitosConta = table.Column<int>(nullable: false),
                    DigitosAgencia = table.Column<int>(nullable: false),
                    DigitosAgenciaDestino = table.Column<int>(nullable: false),
                    DigitosContaDestino = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(nullable: true),
                    CPF = table.Column<string>(nullable: true),
                    Situacao = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DigitosConta = table.Column<int>(nullable: false),
                    DigitosAgencia = table.Column<int>(nullable: false),
                    TipoConta = table.Column<int>(nullable: false),
                    NomeConta = table.Column<string>(nullable: true),
                    Saldo = table.Column<float>(nullable: false),
                    CPF = table.Column<string>(nullable: true),
                    Senha = table.Column<string>(nullable: true),
                    Situacao = table.Column<int>(nullable: false),
                    PessoaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contas_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PessoasEnderecos",
                columns: table => new
                {
                    PessoaId = table.Column<int>(nullable: false),
                    EnderecoId = table.Column<int>(nullable: false),
                    Criacao = table.Column<DateTime>(nullable: false),
                    Modificado = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PessoasEnderecos", x => new { x.PessoaId, x.EnderecoId });
                    table.ForeignKey(
                        name: "FK_PessoasEnderecos_Enderecos_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Enderecos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PessoasEnderecos_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContasCadastradas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContaId = table.Column<int>(nullable: false),
                    PessoaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContasCadastradas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContasCadastradas_Contas_ContaId",
                        column: x => x.ContaId,
                        principalTable: "Contas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContasCadastradas_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoricosContas",
                columns: table => new
                {
                    HistoricoId = table.Column<int>(nullable: false),
                    ContaId = table.Column<int>(nullable: false),
                    DataCriacao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricosContas", x => new { x.HistoricoId, x.ContaId });
                    table.ForeignKey(
                        name: "FK_HistoricosContas_Contas_ContaId",
                        column: x => x.ContaId,
                        principalTable: "Contas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoricosContas_Historicos_HistoricoId",
                        column: x => x.HistoricoId,
                        principalTable: "Historicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contas_PessoaId",
                table: "Contas",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_ContasCadastradas_ContaId",
                table: "ContasCadastradas",
                column: "ContaId");

            migrationBuilder.CreateIndex(
                name: "IX_ContasCadastradas_PessoaId",
                table: "ContasCadastradas",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricosContas_ContaId",
                table: "HistoricosContas",
                column: "ContaId");

            migrationBuilder.CreateIndex(
                name: "IX_PessoasEnderecos_EnderecoId",
                table: "PessoasEnderecos",
                column: "EnderecoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContasCadastradas");

            migrationBuilder.DropTable(
                name: "HistoricosContas");

            migrationBuilder.DropTable(
                name: "PessoasEnderecos");

            migrationBuilder.DropTable(
                name: "Contas");

            migrationBuilder.DropTable(
                name: "Historicos");

            migrationBuilder.DropTable(
                name: "Enderecos");

            migrationBuilder.DropTable(
                name: "Pessoas");
        }
    }
}
