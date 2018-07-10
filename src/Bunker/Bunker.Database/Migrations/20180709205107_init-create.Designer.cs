﻿// <auto-generated />
using System;
using Bunker.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Bunker.Database.Migrations
{
    [DbContext(typeof(BunkerDbContext))]
    [Migration("20180709205107_init-create")]
    partial class initcreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Bunker.Database.Entities.Challange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompanyId");

                    b.Property<string>("Desciprion")
                        .IsRequired()
                        .HasMaxLength(10000);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("PlayerOwnerId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("PlayerOwnerId");

                    b.ToTable("Challanges");
                });

            modelBuilder.Entity("Bunker.Database.Entities.ChallangeTeam", b =>
                {
                    b.Property<int>("ChallangeId");

                    b.Property<int>("TeamId");

                    b.Property<DateTime>("JoinTime");

                    b.HasKey("ChallangeId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("ChallangeTeams");
                });

            modelBuilder.Entity("Bunker.Database.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Desciprion")
                        .HasMaxLength(1000);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Bunker.Database.Entities.CompanyJoinInfo", b =>
                {
                    b.Property<int>("CompanyId");

                    b.Property<string>("Key")
                        .IsRequired();

                    b.HasKey("CompanyId");

                    b.ToTable("CompanyJoinInfos");
                });

            modelBuilder.Entity("Bunker.Database.Entities.CompanyPlayer", b =>
                {
                    b.Property<int>("CompanyId");

                    b.Property<int>("PlayerId");

                    b.Property<bool>("IsOwner");

                    b.HasKey("CompanyId", "PlayerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("CompanyPlayers");
                });

            modelBuilder.Entity("Bunker.Database.Entities.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(16);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(16);

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasMaxLength(16);

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired();

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Bunker.Database.Entities.PlayerRole", b =>
                {
                    b.Property<int>("PlayerId");

                    b.Property<int>("RoleId");

                    b.HasKey("PlayerId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("PlayerRoles");
                });

            modelBuilder.Entity("Bunker.Database.Entities.PlayerTask", b =>
                {
                    b.Property<int>("PlayerId");

                    b.Property<int>("TaskId");

                    b.HasKey("PlayerId", "TaskId");

                    b.HasIndex("TaskId");

                    b.ToTable("PlayerTasks");
                });

            modelBuilder.Entity("Bunker.Database.Entities.PlayerTeam", b =>
                {
                    b.Property<int>("PlayerId");

                    b.Property<int>("TeamId");

                    b.Property<bool>("IsOwner");

                    b.HasKey("PlayerId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("PlayerTeams");
                });

            modelBuilder.Entity("Bunker.Database.Entities.Role", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Bunker.Database.Entities.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer")
                        .HasMaxLength(200);

                    b.Property<int>("ChallangeId");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int?>("PlayerId");

                    b.Property<int>("Score");

                    b.HasKey("Id");

                    b.HasIndex("ChallangeId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Bunker.Database.Entities.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompanyId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Bunker.Database.Entities.TeamJoinInfo", b =>
                {
                    b.Property<int>("TeamId");

                    b.Property<string>("Key")
                        .IsRequired();

                    b.HasKey("TeamId");

                    b.ToTable("TeamJoinInfos");
                });

            modelBuilder.Entity("Bunker.Database.Entities.Challange", b =>
                {
                    b.HasOne("Bunker.Database.Entities.Company", "Company")
                        .WithMany("Challanges")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Bunker.Database.Entities.Player", "PlayerOwner")
                        .WithMany()
                        .HasForeignKey("PlayerOwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Bunker.Database.Entities.ChallangeTeam", b =>
                {
                    b.HasOne("Bunker.Database.Entities.Challange", "Challange")
                        .WithMany("Teams")
                        .HasForeignKey("ChallangeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Bunker.Database.Entities.Team", "Team")
                        .WithMany("Challanges")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Bunker.Database.Entities.CompanyJoinInfo", b =>
                {
                    b.HasOne("Bunker.Database.Entities.Company", "Company")
                        .WithOne("CompanyJoinInfo")
                        .HasForeignKey("Bunker.Database.Entities.CompanyJoinInfo", "CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Bunker.Database.Entities.CompanyPlayer", b =>
                {
                    b.HasOne("Bunker.Database.Entities.Company", "Company")
                        .WithMany("Players")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Bunker.Database.Entities.Player", "Player")
                        .WithMany("Companies")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Bunker.Database.Entities.PlayerRole", b =>
                {
                    b.HasOne("Bunker.Database.Entities.Player", "Player")
                        .WithMany("Roles")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Bunker.Database.Entities.Role", "Role")
                        .WithMany("Players")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Bunker.Database.Entities.PlayerTask", b =>
                {
                    b.HasOne("Bunker.Database.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Bunker.Database.Entities.Task", "Task")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Bunker.Database.Entities.PlayerTeam", b =>
                {
                    b.HasOne("Bunker.Database.Entities.Player", "Player")
                        .WithMany("Teams")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Bunker.Database.Entities.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Bunker.Database.Entities.Task", b =>
                {
                    b.HasOne("Bunker.Database.Entities.Challange", "Challange")
                        .WithMany("Tasks")
                        .HasForeignKey("ChallangeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Bunker.Database.Entities.Player")
                        .WithMany("Tasks")
                        .HasForeignKey("PlayerId");
                });

            modelBuilder.Entity("Bunker.Database.Entities.Team", b =>
                {
                    b.HasOne("Bunker.Database.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Bunker.Database.Entities.TeamJoinInfo", b =>
                {
                    b.HasOne("Bunker.Database.Entities.Team", "Team")
                        .WithOne("TeamJoinInfo")
                        .HasForeignKey("Bunker.Database.Entities.TeamJoinInfo", "TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}