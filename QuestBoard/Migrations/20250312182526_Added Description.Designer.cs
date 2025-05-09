﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuestBoard.Data;

#nullable disable

namespace QuestBoard.Migrations
{
    [DbContext(typeof(QuestboardDbContext))]
    [Migration("20250312182526_Added Description")]
    partial class AddedDescription
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AppUserJobTask", b =>
                {
                    b.Property<Guid>("JobTasksId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("JobTasksId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("AppUserJobTask");
                });

            modelBuilder.Entity("AppUserProjects", b =>
                {
                    b.Property<Guid>("ProjectsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ProjectsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("AppUserProjects");
                });

            modelBuilder.Entity("JobTaskTag", b =>
                {
                    b.Property<Guid>("TagsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TasksId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TagsId", "TasksId");

                    b.HasIndex("TasksId");

                    b.ToTable("JobTaskTag");
                });

            modelBuilder.Entity("QuestBoard.Models.Domain.AppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("QuestBoard.Models.Domain.JobTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.PrimitiveCollection<string>("AdminUserRights")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("JobsAndTasks");
                });

            modelBuilder.Entity("QuestBoard.Models.Domain.Projects", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.PrimitiveCollection<string>("AdminUserRights")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("shortDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("QuestBoard.Models.Domain.Subtask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<Guid?>("JobTaskId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("JobTaskId");

                    b.ToTable("Subtask");
                });

            modelBuilder.Entity("QuestBoard.Models.Domain.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("AppUserJobTask", b =>
                {
                    b.HasOne("QuestBoard.Models.Domain.JobTask", null)
                        .WithMany()
                        .HasForeignKey("JobTasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuestBoard.Models.Domain.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AppUserProjects", b =>
                {
                    b.HasOne("QuestBoard.Models.Domain.Projects", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuestBoard.Models.Domain.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("JobTaskTag", b =>
                {
                    b.HasOne("QuestBoard.Models.Domain.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuestBoard.Models.Domain.JobTask", null)
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("QuestBoard.Models.Domain.JobTask", b =>
                {
                    b.HasOne("QuestBoard.Models.Domain.Projects", "Project")
                        .WithMany("JobTasks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("QuestBoard.Models.Domain.Subtask", b =>
                {
                    b.HasOne("QuestBoard.Models.Domain.JobTask", null)
                        .WithMany("Subtasks")
                        .HasForeignKey("JobTaskId");
                });

            modelBuilder.Entity("QuestBoard.Models.Domain.JobTask", b =>
                {
                    b.Navigation("Subtasks");
                });

            modelBuilder.Entity("QuestBoard.Models.Domain.Projects", b =>
                {
                    b.Navigation("JobTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
