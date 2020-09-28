﻿// <auto-generated />
using System;
using BCD.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BCD.Repository.Migrations
{
    [DbContext(typeof(BCDContext))]
    partial class BCDContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("BCD.Domain.Entities.Conta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CPF");

                    b.Property<int>("DigitosAgencia");

                    b.Property<int>("DigitosConta");

                    b.Property<string>("NomeConta");

                    b.Property<int>("PessoaId");

                    b.Property<float>("Saldo");

                    b.Property<string>("Senha");

                    b.Property<int>("TipoConta");

                    b.HasKey("Id");

                    b.HasIndex("PessoaId");

                    b.ToTable("Contas");
                });

            modelBuilder.Entity("BCD.Domain.Entities.ContaCadastrada", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ContaId");

                    b.Property<int>("PessoaId");

                    b.HasKey("Id");

                    b.HasIndex("ContaId");

                    b.HasIndex("PessoaId");

                    b.ToTable("ContasCadastradas");
                });

            modelBuilder.Entity("BCD.Domain.Entities.Endereco", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bairro");

                    b.Property<int>("CEP");

                    b.Property<string>("Complemento");

                    b.Property<string>("GIA");

                    b.Property<int>("IBGE");

                    b.Property<string>("Localidade");

                    b.Property<string>("Logradouro");

                    b.Property<int>("UF");

                    b.Property<string>("Unidade");

                    b.HasKey("Id");

                    b.ToTable("Enderecos");
                });

            modelBuilder.Entity("BCD.Domain.Entities.EnderecosPessoas", b =>
                {
                    b.Property<int>("EnderecoId");

                    b.Property<int>("PessoaId");

                    b.Property<DateTime>("DataAtualizacao");

                    b.HasKey("EnderecoId", "PessoaId");

                    b.HasIndex("PessoaId");

                    b.ToTable("EnderecosPessoas");
                });

            modelBuilder.Entity("BCD.Domain.Entities.Historico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DataTransacao");

                    b.Property<string>("DescricaoTransacao");

                    b.Property<int>("DigitosAgencia");

                    b.Property<int>("DigitosAgenciaDestino");

                    b.Property<int>("DigitosConta");

                    b.Property<int>("DigitosContaDestino");

                    b.Property<string>("NomeConta");

                    b.Property<string>("NomeContaDestino");

                    b.Property<int>("Operacao");

                    b.Property<int>("TipoConta");

                    b.Property<float>("Valor");

                    b.HasKey("Id");

                    b.ToTable("Historicos");
                });

            modelBuilder.Entity("BCD.Domain.Entities.HistoricosContas", b =>
                {
                    b.Property<int>("HistoricoId");

                    b.Property<int>("ContaId");

                    b.Property<DateTime>("DataCriacao");

                    b.HasKey("HistoricoId", "ContaId");

                    b.HasIndex("ContaId");

                    b.ToTable("HistoricosContas");
                });

            modelBuilder.Entity("BCD.Domain.Entities.Pessoa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CPF");

                    b.Property<string>("Nome");

                    b.HasKey("Id");

                    b.ToTable("Pessoas");
                });

            modelBuilder.Entity("BCD.Domain.Entities.Conta", b =>
                {
                    b.HasOne("BCD.Domain.Entities.Pessoa", "Pessoa")
                        .WithMany("Contas")
                        .HasForeignKey("PessoaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BCD.Domain.Entities.ContaCadastrada", b =>
                {
                    b.HasOne("BCD.Domain.Entities.Conta", "Conta")
                        .WithMany()
                        .HasForeignKey("ContaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BCD.Domain.Entities.Pessoa", "Pessoa")
                        .WithMany()
                        .HasForeignKey("PessoaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BCD.Domain.Entities.EnderecosPessoas", b =>
                {
                    b.HasOne("BCD.Domain.Entities.Endereco", "Endereco")
                        .WithMany("Pessoas")
                        .HasForeignKey("EnderecoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BCD.Domain.Entities.Pessoa", "Pessoa")
                        .WithMany("Enderecos")
                        .HasForeignKey("PessoaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BCD.Domain.Entities.HistoricosContas", b =>
                {
                    b.HasOne("BCD.Domain.Entities.Conta", "Conta")
                        .WithMany("Extrato")
                        .HasForeignKey("ContaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BCD.Domain.Entities.Historico", "Historico")
                        .WithMany("Contas")
                        .HasForeignKey("HistoricoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
