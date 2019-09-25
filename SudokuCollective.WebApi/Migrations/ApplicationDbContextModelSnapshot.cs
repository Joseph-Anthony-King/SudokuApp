﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SudokuCollective.WebApi.Models.DataModel;

namespace SudokuCollective.WebApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("SudokuCollective.Models.App", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("DevUrl");

                    b.Property<string>("License");

                    b.Property<string>("LiveUrl");

                    b.Property<string>("Name");

                    b.Property<int>("OwnerId");

                    b.HasKey("Id");

                    b.ToTable("Apps");
                });

            modelBuilder.Entity("SudokuCollective.Models.Difficulty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DifficultyLevel");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Difficulties");
                });

            modelBuilder.Entity("SudokuCollective.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("ContinueGame");

                    b.Property<DateTime>("DateCompleted");

                    b.Property<DateTime>("DateCreated");

                    b.Property<int>("SudokuMatrixId");

                    b.Property<int>("SudokuSolutionId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("SudokuMatrixId")
                        .IsUnique();

                    b.HasIndex("SudokuSolutionId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("SudokuCollective.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("RoleLevel");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("SudokuCollective.Models.SudokuCell", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Column");

                    b.Property<int>("DisplayValue");

                    b.Property<int>("Index");

                    b.Property<bool>("Obscured");

                    b.Property<int>("Region");

                    b.Property<int>("Row");

                    b.Property<int>("SudokuMatrixId");

                    b.Property<int>("Value");

                    b.HasKey("Id");

                    b.HasIndex("SudokuMatrixId");

                    b.ToTable("SudokuCells");
                });

            modelBuilder.Entity("SudokuCollective.Models.SudokuMatrix", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DifficultyId");

                    b.HasKey("Id");

                    b.HasIndex("DifficultyId");

                    b.ToTable("SudokuMatrices");
                });

            modelBuilder.Entity("SudokuCollective.Models.SudokuSolution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SolutionList");

                    b.HasKey("Id");

                    b.ToTable("SudokuSolutions");
                });

            modelBuilder.Entity("SudokuCollective.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("NickName");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SudokuCollective.Models.UserApp", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("AppId");

                    b.HasKey("UserId", "AppId");

                    b.HasIndex("AppId");

                    b.ToTable("UsersApps");
                });

            modelBuilder.Entity("SudokuCollective.Models.UserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UsersRoles");
                });

            modelBuilder.Entity("SudokuCollective.Models.Game", b =>
                {
                    b.HasOne("SudokuCollective.Models.SudokuMatrix", "SudokuMatrix")
                        .WithOne("Game")
                        .HasForeignKey("SudokuCollective.Models.Game", "SudokuMatrixId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SudokuCollective.Models.SudokuSolution", "SudokuSolution")
                        .WithOne("Game")
                        .HasForeignKey("SudokuCollective.Models.Game", "SudokuSolutionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SudokuCollective.Models.User", "User")
                        .WithMany("Games")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SudokuCollective.Models.SudokuCell", b =>
                {
                    b.HasOne("SudokuCollective.Models.SudokuMatrix", "SudokuMatrix")
                        .WithMany("SudokuCells")
                        .HasForeignKey("SudokuMatrixId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SudokuCollective.Models.SudokuMatrix", b =>
                {
                    b.HasOne("SudokuCollective.Models.Difficulty", "Difficulty")
                        .WithMany("Matrices")
                        .HasForeignKey("DifficultyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SudokuCollective.Models.UserApp", b =>
                {
                    b.HasOne("SudokuCollective.Models.App", "App")
                        .WithMany("Users")
                        .HasForeignKey("AppId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SudokuCollective.Models.User", "User")
                        .WithMany("Apps")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SudokuCollective.Models.UserRole", b =>
                {
                    b.HasOne("SudokuCollective.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SudokuCollective.Models.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
