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

                    b.Property<int>("Situacao");

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

                    b.Property<string>("CEP");

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

            modelBuilder.Entity("BCD.Domain.Entities.Identity.Papel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("BCD.Domain.Entities.Identity.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NomeCompleto")
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("BCD.Domain.Entities.Identity.UsuariosPapeis", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("BCD.Domain.Entities.Pessoa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CPF");

                    b.Property<int>("EnderecoId");

                    b.Property<string>("Nome");

                    b.HasKey("Id");

                    b.HasIndex("EnderecoId");

                    b.ToTable("Pessoas");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
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

            modelBuilder.Entity("BCD.Domain.Entities.Identity.UsuariosPapeis", b =>
                {
                    b.HasOne("BCD.Domain.Entities.Identity.Papel", "Papel")
                        .WithMany("Usuarios")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BCD.Domain.Entities.Identity.Usuario", "Usuario")
                        .WithMany("Papeis")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BCD.Domain.Entities.Pessoa", b =>
                {
                    b.HasOne("BCD.Domain.Entities.Endereco", "Endereco")
                        .WithMany("Pessoas")
                        .HasForeignKey("EnderecoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("BCD.Domain.Entities.Identity.Papel")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("BCD.Domain.Entities.Identity.Usuario")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("BCD.Domain.Entities.Identity.Usuario")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("BCD.Domain.Entities.Identity.Usuario")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
